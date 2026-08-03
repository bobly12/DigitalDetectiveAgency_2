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

   // Object.values(sounds).forEach(sound => sound.preload = "auto");//

   // function playSound(sound) {
  //      if (!sound) return;
    //    sound.pause();
      //  sound.currentTime = 0;
        //sound.play().catch(() => {});
  //  }//

    // Tracks which suspects/nodes are already unlocked in state.
    const knownUnlockedSuspectIds = new Set(
        Array.from(document.querySelectorAll('.board-card[data-type="Suspect"]'))
            .filter(card => !card.querySelector('[data-suspect-lock]'))
            .map(card => parseInt(card.dataset.id, 10))
    );

    const knownUnlockedEvidenceIds = new Set(
        Array.from(document.querySelectorAll('.board-card[data-type="Evidence"]'))
            .filter(card => !card.classList.contains('board-card--undiscovered'))
            .map(card => parseInt(card.dataset.id, 10))
    );

    const knownUnlockedWitnessIds = new Set(
        Array.from(document.querySelectorAll('.board-card[data-type="Witness"]'))
            .filter(card => !card.classList.contains('board-card--undiscovered'))
            .map(card => parseInt(card.dataset.id, 10))
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
        // String conversion ensures matching regardless of string/number type passing
        return document.querySelector(`.board-card[data-type="${type}"][data-id="${String(id)}"]`);
    }

    // ===== Thread Drawing =====
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

    // ===== Apply progress snapshot =====
    function applyProgress(progress) {
        if (!progress) return;

        const confidence = progress.confidence ?? progress.Confidence ?? 0;
        const correctConn = progress.correctConnections ?? progress.CorrectConnections ?? 0;
        const totalConn = progress.totalRequiredConnections ?? progress.TotalRequiredConnections ?? 0;
        const correctElim = progress.correctEliminatedSuspects ?? progress.CorrectEliminatedSuspects ?? 0;
        const totalInnocent = progress.totalInnocentSuspects ?? progress.TotalInnocentSuspects ?? 0;
        const canAccuse = progress.canAccuse ?? progress.CanAccuse ?? false;
        const remConf = progress.remainingConfidence ?? progress.RemainingConfidence ?? 0;

        const fillEl = document.getElementById('confidence-fill');
        const valueEl = document.getElementById('confidence-value');
        const detailEl = document.getElementById('confidence-detail');
        const accuseBtn = document.getElementById('accuse-btn');
        const accuseLabel = document.getElementById('accuse-btn-label');

        if (fillEl) {
            fillEl.style.width = confidence + '%';
            fillEl.classList.remove('confidence-gauge__fill--high', 'confidence-gauge__fill--mid', 'confidence-gauge__fill--low');
            fillEl.classList.add(
                confidence >= 75 ? 'confidence-gauge__fill--high' :
                    confidence >= 40 ? 'confidence-gauge__fill--mid' :
                        'confidence-gauge__fill--low'
            );
        }

        if (valueEl) {
            const start = parseInt(valueEl.textContent, 10) || 0;
            const duration = 500;
            const startTime = performance.now();

            function animate(now) {
                const p = Math.min((now - startTime) / duration, 1);
                valueEl.textContent = Math.round(start + (confidence - start) * p) + '%';
                if (p < 1) requestAnimationFrame(animate);
            }

            requestAnimationFrame(animate);
            valueEl.animate([
                { transform: "scale(1)" },
                { transform: "scale(1.18)" },
                { transform: "scale(1)" }
            ], { duration: 350 });
        }

        if (detailEl) {
            detailEl.textContent = `${correctConn} / ${totalConn} connections  •  ${correctElim} / ${totalInnocent} suspects cleared`;
        }

        if (accuseBtn && accuseLabel) {
            accuseBtn.dataset.canAccuse = canAccuse;
            if (canAccuse) {
                accuseBtn.classList.remove('btn-stamp--disabled');
                accuseBtn.removeAttribute('aria-disabled');
                accuseBtn.removeAttribute('title');
                accuseLabel.textContent = 'Make Accusation';
            } else {
                accuseBtn.classList.add('btn-stamp--disabled');
                accuseBtn.setAttribute('aria-disabled', 'true');
                accuseBtn.setAttribute('title', 'Reach 75% confidence to unlock');
                accuseLabel.textContent = `Make Accusation (${remConf}% more needed)`;
            }
        }

        // Handle property name casing differences (camelCase vs PascalCase)
        const unlockedSuspects = progress.unlockedSuspectIds || progress.UnlockedSuspectIds || [];
        const unlockedEvidence = progress.unlockedEvidenceIds || progress.UnlockedEvidenceIds || [];
        const unlockedWitnesses = progress.unlockedWitnessIds || progress.UnlockedWitnessIds || [];

        unlockedSuspects.forEach(suspectId => {
            const idNum = parseInt(suspectId, 10);
            if (knownUnlockedSuspectIds.has(idNum)) return;
            unlockSuspectCard(idNum);
        });

        unlockedEvidence.forEach(evidenceId => {
            const idNum = parseInt(evidenceId, 10);
            if (knownUnlockedEvidenceIds.has(idNum)) return;
            revealNode('Evidence', idNum);
        });

        unlockedWitnesses.forEach(witnessId => {
            const idNum = parseInt(witnessId, 10);
            if (knownUnlockedWitnessIds.has(idNum)) return;
            revealNode('Witness', idNum);
        });
    }

    // ===== Unlock suspect card =====
    async function unlockSuspectCard(suspectId) {
        const card = findCard('Suspect', suspectId);
        if (!card) return;

        try {
            const res = await fetch(`/Board/GetSuspectFile?caseId=${window.boardCaseId}&suspectId=${suspectId}`);
            if (!res.ok) {
                console.error(`[Board] Failed to fetch suspect ${suspectId}: HTTP ${res.status}`);
                return;
            }
            const data = await res.json();

            knownUnlockedSuspectIds.add(parseInt(suspectId, 10));

            const existingFields = JSON.parse(card.dataset.fields || '[]');
            const updatedFields = existingFields.map(f => {
                if (f.heading === 'Motive') return { ...f, text: data.motive || data.Motive };
                if (f.heading === 'Alibi') return { ...f, text: data.alibi || data.Alibi };
                return f;
            });
            card.dataset.fields = JSON.stringify(updatedFields);

            const lockEl = card.querySelector('[data-suspect-lock]');
            if (lockEl) {
                playSound(sounds.unlock);
                lockEl.animate([
                    { transform: "rotateY(0deg)", opacity: 1 },
                    { transform: "rotateY(180deg)", opacity: 0 }
                ], { duration: 600, easing: "ease-out" });
                setTimeout(() => lockEl.remove(), 580);
            }

            card.animate([
                { transform: "scale(.95)", boxShadow: "0 0 0 rgba(255,215,0,0)" },
                { transform: "scale(1.05)", boxShadow: "0 0 25px rgba(255,215,0,.8)" },
                { transform: "scale(1)" }
            ], { duration: 700 });

            playSound(sounds.paper);
        } catch (err) {
            console.error(`[Board] Error unlocking suspect ${suspectId}:`, err);
        }
    }

    // ===== Reveal Evidence/Witness cards =====
    async function revealNode(type, id) {
        const placeholder = findCard(type, id);

        if (!placeholder) {
            console.warn(`[Board] Node card not found in DOM: Type=${type}, ID=${id}`);
            return;
        }

        if (!placeholder.classList.contains('board-card--undiscovered')) {
            if (type === 'Evidence') knownUnlockedEvidenceIds.add(parseInt(id, 10));
            if (type === 'Witness') knownUnlockedWitnessIds.add(parseInt(id, 10));
            return;
        }

        const endpoint = type === 'Evidence' ? '/Board/GetEvidenceFile' : '/Board/GetWitnessFile';
        const idParam = type === 'Evidence' ? 'evidenceId' : 'witnessId';

        try {
            const res = await fetch(`${endpoint}?caseId=${window.boardCaseId}&${idParam}=${id}`);
            if (!res.ok) {
                console.error(`[Board] Failed to fetch ${type} ${id}: HTTP ${res.status}`);
                return;
            }
            const data = await res.json();

            // Support both camelCase and PascalCase backend models
            const name = data.name || data.Name || '';
            const imageUrl = data.imageUrl || data.ImageUrl || '';
            const description = data.description || data.Description || '';

            if (type === 'Evidence') knownUnlockedEvidenceIds.add(parseInt(id, 10));
            if (type === 'Witness') knownUnlockedWitnessIds.add(parseInt(id, 10));

            const fields = type === 'Evidence'
                ? [{ heading: null, text: description }]
                : [{ heading: null, text: `"${description}"` }];

            placeholder.classList.remove('board-card--undiscovered');
            placeholder.dataset.name = name;
            placeholder.dataset.image = imageUrl;
            placeholder.dataset.label = type;
            placeholder.dataset.fields = JSON.stringify(fields);

            placeholder.innerHTML = `
                <span class="board-card__pin" data-connect-pin title="Click to start or complete a connection"></span>
                <img src="${imageUrl}" alt="${name}" class="board-card__img" />
                <strong>${name}</strong>
            `;

            placeholder.querySelector('[data-connect-pin]').addEventListener('click', handlePinClick);
            placeholder.querySelector('.board-card__img').addEventListener('click', (e) => {
                e.stopPropagation();
                window.openFileModal?.(placeholder);
            });

            playSound(sounds.paper);
            placeholder.animate([
                { opacity: 0.4, transform: "scale(.96)" },
                { opacity: 1, transform: "scale(1)" }
            ], { duration: 400 });

        } catch (err) {
            console.error(`[Board] Error revealing ${type} ${id}:`, err);
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
            applyProgress(data.progress || data.Progress);
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
    async function handlePinClick(e) {
        e.stopPropagation();
        const pin = e.currentTarget;
        const card = pin.closest('.board-card');

        if (!selectedCard) {
            selectedCard = card;
            pin.classList.add('is-active-pin');
            card.classList.add('is-selected');

            playSound(sounds.pin);
            pin.animate([
                { transform: "translateX(-50%) scale(1)" },
                { transform: "translateX(-50%) scale(1.45)" },
                { transform: "translateX(-50%) scale(1.2)" }
            ], { duration: 220, easing: "ease-out" });

            return;
        }

        if (selectedCard === card) {
            clearSelection();
            return;
        }

        const fromType = selectedCard.dataset.type;
        const fromId = parseInt(selectedCard.dataset.id, 10);
        const toType = card.dataset.type;
        const toId = parseInt(card.dataset.id, 10);

        const res = await fetch('/Board/SaveConnection', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ caseId: window.boardCaseId, fromType, fromId, toType, toId })
        });

        clearSelection();

        if (res.ok) {
            const data = await res.json();
            playSound(sounds.pin);
            connections.push({ id: data.connectionId || data.ConnectionId, fromType, fromId, toType, toId });
            redrawAll();
            applyProgress(data.progress || data.Progress);
        } else {
            const data = await res.json();
            alert(data.message || data.Message || 'Could not save connection.');
        }
    }

    document.querySelectorAll('[data-connect-pin]').forEach(pin => {
        pin.addEventListener('click', handlePinClick);
    });

    // ===== Suspect elimination behavior =====
    document.querySelectorAll('[data-eliminate-suspect]').forEach(btn => {
        btn.addEventListener('click', async (e) => {
            e.stopPropagation();
            const suspectId = parseInt(btn.dataset.eliminateSuspect, 10);
            const card = btn.closest('.board-card');

            const res = await fetch('/Board/ToggleElimination', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ caseId: window.boardCaseId, suspectId })
            });

            if (res.ok) {
                const data = await res.json();
                card.classList.toggle('is-eliminated');
                applyProgress(data.progress || data.Progress);
            } else {
                alert('Could not toggle elimination.');
            }
        });
    });

    redrawAll();
})();