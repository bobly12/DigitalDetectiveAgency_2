// wwwroot/js/board.js
(function () {
    const svg = document.getElementById('connection-svg');
    let selectedCard = null;
    let connections = [...window.initialConnections];

    // Tracks which suspects we already know are unlocked, so we only
    // fetch a suspect's real file once, right when they first unlock.
    const knownUnlockedSuspectIds = new Set(
        Array.from(document.querySelectorAll('.board-card[data-type="Suspect"]'))
            .filter(card => !card.querySelector('[data-suspect-lock]'))
            .map(card => parseInt(card.dataset.id))
    );

    function getPinPosition(card) {
        const pin = card.querySelector('[data-connect-pin]');
        const target = pin || card;
        const rect = target.getBoundingClientRect();
        const containerRect = document.getElementById('board-container').getBoundingClientRect();
        return {
            x: rect.left - containerRect.left + rect.width / 2,
            y: rect.top - containerRect.top + rect.height / 2
        };
    }

    function findCard(type, id) {
        return document.querySelector(`.board-card[data-type="${type}"][data-id="${id}"]`);
    }

    function drawLine(conn) {
        const fromCard = findCard(conn.fromType || conn.FromType, conn.fromId || conn.FromId);
        const toCard = findCard(conn.toType || conn.ToType, conn.toId || conn.ToId);
        if (!fromCard || !toCard) return;

        const from = getPinPosition(fromCard);
        const to = getPinPosition(toCard);

        const dist = Math.hypot(to.x - from.x, to.y - from.y);
        const sag = Math.min(50, dist * 0.15);
        const midX = (from.x + to.x) / 2;
        const midY = (from.y + to.y) / 2 + sag;

        const path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        path.setAttribute('d', `M ${from.x} ${from.y} Q ${midX} ${midY} ${to.x} ${to.y}`);
        path.setAttribute('fill', 'none');
        path.setAttribute('stroke', '#b8342a');
        path.setAttribute('stroke-width', '2');
        path.setAttribute('filter', 'url(#stringWobble)');
        path.classList.add('conn-line');
        path.dataset.connectionId = conn.id || conn.Id;
        path.style.pointerEvents = 'stroke';
        path.style.cursor = 'pointer';

        path.addEventListener('click', () => removeConnection(conn.id || conn.Id));

        svg.appendChild(path);
    }

    function redrawAll() {
        svg.querySelectorAll('.conn-line').forEach(el => el.remove());
        connections.forEach(drawLine);
    }

    // ===== Apply a progress snapshot returned by the server to the DOM =====
    // No page reload — this is the whole point of this rewrite.
    function applyProgress(progress) {
        if (!progress) return;

        const fillEl = document.getElementById('confidence-fill');
        const valueEl = document.getElementById('confidence-value');
        const detailEl = document.getElementById('confidence-detail');
        const accuseBtn = document.getElementById('accuse-btn');
        const accuseLabel = document.getElementById('accuse-btn-label');

        if (fillEl) {
            fillEl.style.width = progress.confidence + '%';
            fillEl.classList.remove('confidence-gauge__fill--high', 'confidence-gauge__fill--mid', 'confidence-gauge__fill--low');
            fillEl.classList.add(
                progress.confidence >= 75 ? 'confidence-gauge__fill--high' :
                    progress.confidence >= 40 ? 'confidence-gauge__fill--mid' :
                        'confidence-gauge__fill--low'
            );
        }
        if (valueEl) valueEl.textContent = progress.confidence + '%';
        if (detailEl) {
            detailEl.textContent =
                `${progress.correctConnections} / ${progress.totalRequiredConnections} connections  •  ` +
                `${progress.correctEliminatedSuspects} / ${progress.totalInnocentSuspects} suspects cleared`;
        }

        if (accuseBtn && accuseLabel) {
            accuseBtn.dataset.canAccuse = progress.canAccuse;
            if (progress.canAccuse) {
                accuseBtn.classList.remove('btn-stamp--disabled');
                accuseBtn.removeAttribute('aria-disabled');
                accuseBtn.removeAttribute('title');
                accuseLabel.textContent = 'Make Accusation';
            } else {
                accuseBtn.classList.add('btn-stamp--disabled');
                accuseBtn.setAttribute('aria-disabled', 'true');
                accuseBtn.setAttribute('title', 'Reach 75% confidence to unlock');
                accuseLabel.textContent = `Make Accusation (${progress.remainingConfidence}% more needed)`;
            }
        }

        // Reveal any newly-unlocked suspects' files.
        (progress.unlockedSuspectIds || []).forEach(suspectId => {
            if (knownUnlockedSuspectIds.has(suspectId)) return;
            knownUnlockedSuspectIds.add(suspectId);
            unlockSuspectCard(suspectId);
        });
    }

    // Fetches a suspect's real Motive/Alibi (server re-checks the unlock —
    // this call fails harmlessly if something's out of sync) and patches
    // the card's stored fields + removes the lock icon.
    async function unlockSuspectCard(suspectId) {
        const card = findCard('Suspect', suspectId);
        if (!card) return;

        try {
            const res = await fetch(`/Board/GetSuspectFile?caseId=${window.boardCaseId}&suspectId=${suspectId}`);
            if (!res.ok) return;
            const data = await res.json();

            const existingFields = JSON.parse(card.dataset.fields);
            const updatedFields = existingFields.map(f => {
                if (f.heading === 'Motive') return { ...f, text: data.motive };
                if (f.heading === 'Alibi') return { ...f, text: data.alibi };
                return f;
            });
            card.dataset.fields = JSON.stringify(updatedFields);

            const lockEl = card.querySelector('[data-suspect-lock]');
            if (lockEl) lockEl.remove();
        } catch {
            // Non-fatal — worst case, the file stays locked-looking until the next full page load.
        }
    }

    async function removeConnection(connectionId) {
        if (!confirm('Remove this connection?')) return;

        const res = await fetch('/Board/DeleteConnection', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ connectionId, caseId: window.boardCaseId })
        });

        if (res.ok) {
            const data = await res.json();
            connections = connections.filter(c => (c.id || c.Id) !== connectionId);
            redrawAll();
            applyProgress(data.progress);
        } else {
            alert('Could not remove connection.');
        }
    }

    function clearSelection() {
        if (!selectedCard) return;
        selectedCard.querySelector('[data-connect-pin]')?.classList.remove('is-active-pin');
        selectedCard.classList.remove('is-selected');
        selectedCard = null;
    }

    // ===== Pin-to-pin connection behavior =====
    document.querySelectorAll('[data-connect-pin]').forEach(pin => {
        pin.addEventListener('click', async (e) => {
            e.stopPropagation();
            const card = pin.closest('.board-card');

            if (!selectedCard) {
                selectedCard = card;
                pin.classList.add('is-active-pin');
                card.classList.add('is-selected');
                return;
            }

            if (selectedCard === card) {
                clearSelection();
                return;
            }

            const fromType = selectedCard.dataset.type;
            const fromId = parseInt(selectedCard.dataset.id);
            const toType = card.dataset.type;
            const toId = parseInt(card.dataset.id);

            const res = await fetch('/Board/SaveConnection', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ caseId: window.boardCaseId, fromType, fromId, toType, toId })
            });

            clearSelection();

            if (res.ok) {
                const data = await res.json();
                connections.push({ id: data.connectionId, fromType, fromId, toType, toId });
                redrawAll();
                applyProgress(data.progress);
            } else {
                const data = await res.json();
                alert(data.message || 'Could not save connection.');
            }
        });
    });

    // ===== Suspect elimination behavior =====
    document.querySelectorAll('[data-eliminate-suspect]').forEach(btn => {
        btn.addEventListener('click', async (e) => {
            e.stopPropagation();
            const suspectId = parseInt(btn.dataset.eliminateSuspect);
            const card = btn.closest('.board-card');

            const res = await fetch('/Board/ToggleElimination', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ caseId: window.boardCaseId, suspectId })
            });

            if (res.ok) {
                const data = await res.json();
                card.classList.toggle('is-eliminated');
                applyProgress(data.progress);
            } else {
                alert('Could not toggle elimination.');
            }
        });
    });

    redrawAll();
})();