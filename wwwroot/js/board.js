// wwwroot/js/board.js
(function () {
    const svg = document.getElementById('connection-svg');
    let selectedCard = null;
    let connections = [...window.initialConnections];

    // ---------- Audio ----------
    const sounds = {
        pin: new Audio('/audio/pin.mp3'),
        paper: new Audio('/audio/paper.mp3'),
        unlock: new Audio('/audio/unlock.mp3')
    };

    Object.values(sounds).forEach(sound => {
        sound.preload = "auto";
    });

    function playSound(sound) {
        if (!sound) return;
        sound.pause();
        sound.currentTime = 0;
        sound.play().catch(() => {
            // Browser blocked playback due to autoplay policy.
        });
    }

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

    // ===== Thread Drawing with Animation =====
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

        // Thread stroke animation setup
        const len = path.getTotalLength();
        path.style.strokeDasharray = len;
        path.style.strokeDashoffset = len;

        requestAnimationFrame(() => {
            path.style.transition = "stroke-dashoffset .45s ease-out, filter .3s ease";
            path.style.strokeDashoffset = 0;
        });
    }

    function redrawAll() {
        svg.querySelectorAll('.conn-line').forEach(el => el.remove());
        connections.forEach(drawLine);
    }

    // ===== Apply progress snapshot & dynamic counter animation =====
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

        // Animated Confidence Percentage Increment & Pulse
        if (valueEl) {
            const start = parseInt(valueEl.textContent) || 0;
            const end = progress.confidence;
            const duration = 500;
            const startTime = performance.now();

            function animate(now) {
                const p = Math.min((now - startTime) / duration, 1);
                const value = Math.round(start + (end - start) * p);
                valueEl.textContent = value + '%';

                if (p < 1) {
                    requestAnimationFrame(animate);
                }
            }

            requestAnimationFrame(animate);

            valueEl.animate([
                { transform: "scale(1)" },
                { transform: "scale(1.18)" },
                { transform: "scale(1)" }
            ], {
                duration: 350
            });
        }

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

    // ===== Unlock suspect card with flip & highlight animations =====
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
            if (lockEl) {
                playSound(sounds.unlock);

                lockEl.animate([
                    { transform: "rotateY(0deg)", opacity: 1 },
                    { transform: "rotateY(180deg)", opacity: 0 }
                ], {
                    duration: 600,
                    easing: "ease-out"
                });

                setTimeout(() => {
                    lockEl.remove();
                }, 580);
            }

            // Card highlight & paper sound
            card.animate([
                { transform: "scale(.95)", boxShadow: "0 0 0 rgba(255,215,0,0)" },
                { transform: "scale(1.05)", boxShadow: "0 0 25px rgba(255,215,0,.8)" },
                { transform: "scale(1)" }
            ], {
                duration: 700
            });

            playSound(sounds.paper);

        } catch {
            // Non-fatal fallback
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

                // Sound and pin pop animation on first selection
                playSound(sounds.pin);
                pin.animate([
                    { transform: "translateX(-50%) scale(1)" },
                    { transform: "translateX(-50%) scale(1.45)" },
                    { transform: "translateX(-50%) scale(1.2)" }
                ], {
                    duration: 220,
                    easing: "ease-out"
                });

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

                playSound(sounds.pin);
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