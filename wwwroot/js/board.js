// wwwroot/js/board.js
(function () {
    const svg = document.getElementById('connection-svg');
    let selectedCard = null;
    let connections = [...window.initialConnections];

    // ===== Tried-wrong pair tracking =====
    function pairKey(fromType, fromId, toType, toId) {
        const a = `${fromType}${fromId}`;
        const b = `${toType}${toId}`;
        return a <= b ? `${a}|${b}` : `${b}|${a}`;
    }

    const triedWrongPairs = new Set(
        (window.triedWrongPairs || []).map(p =>
            pairKey(p.fromType || p.FromType, p.fromId || p.FromId, p.toType || p.ToType, p.toId || p.ToId)
        )
    );

    // ---------- Audio ----------
    const sounds = {
        pin: new Audio('/audio/pin.mp3'),
        paper: new Audio('/audio/paper.mp3'),
        unlock: new Audio('/audio/unlock.mp3')
    };

    // Plays a sound if available; never lets a missing/broken audio file
    // break the calling function. Safe to call even with no audio files present.
    function playSound(audio) {
        try {
            audio.currentTime = 0;
            audio.play().catch(() => {
                // Ignore playback errors (missing file, autoplay policy, etc.)
            });
        } catch {
            // Ignore any synchronous errors too
        }
    }

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

        const focusHintEl = document.getElementById('focus-hint');
        const hint = progress.nextFocusHint ?? progress.NextFocusHint ?? '';
        if (focusHintEl) {
            if (canAccuse || !hint) {
                focusHintEl.remove();
            } else {
                focusHintEl.textContent = `🔎 ${hint}`;
            }
        } else if (!canAccuse && hint) {
            const newHint = document.createElement('div');
            newHint.className = 'confidence-gauge__focus-hint';
            newHint.id = 'focus-hint';
            newHint.textContent = `🔎 ${hint}`;
            detailEl?.insertAdjacentElement('afterend', newHint);
        }

        const wrongAttemptsEl = document.getElementById('wrong-attempts-count');
        if (wrongAttemptsEl) {
            wrongAttemptsEl.textContent = progress.wrongAttempts ?? progress.WrongAttempts ?? 0;
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

    // ===== Connection Feedback Helpers =====
    function showRejectionNote(fromType, fromId, toType, toId, message) {
        [findCard(fromType, fromId), findCard(toType, toId)].forEach(card => {
            if (!card) return;
            card.animate([
                { transform: "translateX(0)" },
                { transform: "translateX(-6px)" },
                { transform: "translateX(6px)" },
                { transform: "translateX(0)" }
            ], { duration: 300, easing: "ease-in-out" });
        });

        const note = document.createElement('div');
        note.className = 'board-rejection-note';
        note.textContent = message || "No link found between these.";
        document.getElementById('board-container').appendChild(note);

        requestAnimationFrame(() => note.classList.add('is-visible'));
        setTimeout(() => {
            note.classList.remove('is-visible');
            setTimeout(() => note.remove(), 300);
        }, 1400);
    }

    function showConfirmationNote(fromType, fromId, toType, toId, note) {
        [findCard(fromType, fromId), findCard(toType, toId)].forEach(card => {
            if (!card) return;
            card.animate([
                { boxShadow: "0 0 0 rgba(255,215,0,0)" },
                { boxShadow: "0 0 20px rgba(255,215,0,.7)" },
                { boxShadow: "0 0 0 rgba(255,215,0,0)" }
            ], { duration: 900, easing: "ease-out" });
        });

        if (!note) return;

        const banner = document.createElement('div');
        banner.className = 'board-confirmation-note';
        banner.textContent = note;
        document.getElementById('board-container').appendChild(banner);

        requestAnimationFrame(() => banner.classList.add('is-visible'));
        setTimeout(() => {
            banner.classList.remove('is-visible');
            setTimeout(() => banner.remove(), 300);
        }, 2200);
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

        clearSelection();

        const key = pairKey(fromType, fromId, toType, toId);
        if (triedWrongPairs.has(key)) {
            showRejectionNote(fromType, fromId, toType, toId, "Already tried — no link found here.");
            return;
        }

        const res = await fetch('/Board/SaveConnection', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ caseId: window.boardCaseId, fromType, fromId, toType, toId })
        });

        if (!res.ok) {
            alert('Something went wrong saving that connection.');
            return;
        }

        const data = await res.json();

        if (data.rejected) {
            triedWrongPairs.add(key);
            showRejectionNote(fromType, fromId, toType, toId);
            if (data.progress) applyProgress(data.progress);
            return;
        }

        if (data.connected) {
            playSound(sounds.pin);
            connections.push({ id: data.connectionId, fromType, fromId, toType, toId });
            redrawAll();
            applyProgress(data.progress);
            showConfirmationNote(fromType, fromId, toType, toId, data.note);
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

    // ===== Cancel an in-progress connection or active modals =====
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            if (selectedCard) clearSelection();
            if (corkView && corkView.style.display !== 'none') {
                corkView.style.display = 'none';
                if (corkBackdrop) corkBackdrop.style.display = 'none';
            }
        }
    });

    document.getElementById('board-container').addEventListener('click', (e) => {
        if (!selectedCard) return;
        if (e.target.closest('.board-card')) return;
        clearSelection();
    });

    // ===== Corkboard Modal View =====
    const corkBackdrop = document.getElementById('corkboard-backdrop');
    const closeCorkBtn = document.getElementById('close-corkboard-btn');
    const corkView = document.getElementById('board-corkboard-view');
    const corkSvg = document.getElementById('corkboard-svg');
    const toggleBtn = document.getElementById('toggle-corkboard-btn');

    // Deterministic pseudo-random number from a card's type+id, so layout
    // stays stable across renders instead of reshuffling every time.
    function seededRandom(seedStr) {
        let hash = 0;
        for (let i = 0; i < seedStr.length; i++) {
            hash = (hash << 5) - hash + seedStr.charCodeAt(i);
            hash |= 0;
        }
        const x = Math.sin(hash) * 10000;
        return x - Math.floor(x);
    }

    function buildCorkboard() {
        const corkCanvas = document.getElementById('corkboard-canvas');
        if (!corkView || !corkSvg || !corkCanvas) return;

        corkCanvas.querySelectorAll('.cork-card').forEach(el => el.remove());
        corkCanvas.querySelectorAll('.cork-victim').forEach(el => el.remove());
        corkSvg.innerHTML = '';

        const allCards = Array.from(document.querySelectorAll('#board-grid-view .board-card'));
        const positions = {};

        const boardWidth = corkView.clientWidth || 1400;
        const centerX = boardWidth / 2;

        // ===== Victim card at the very top, centered =====
        const victimY = 40;
        const victimEl = document.createElement('div');
        victimEl.className = 'cork-card cork-victim';
        victimEl.style.left = (centerX - 90) + 'px';
        victimEl.style.top = victimY + 'px';
        victimEl.style.transform = 'rotate(-2deg)';
        victimEl.innerHTML = `
            <span class="cork-card__pin"></span>
            <div class="cork-card__victim-photo">💀</div>
            <span class="cork-card__label">${window.caseVictimName || 'The Victim'}</span>
        `;
        corkCanvas.appendChild(victimEl);

        // ===== Pyramid rows below the victim =====
        // Row sizes grow as you go down: 1 connects to victim conceptually,
        // then widen out row by row like an inverted triangle of evidence.
        const cardW = 160;
        const cardH = 190;
        const rowGapY = 210;
        const colGapX = 190;

        let remaining = allCards.length;
        let rowIndex = 0;
        let cardIndex = 0;
        const rowSizes = [];

        // Build row sizes: start small, grow, e.g. 2, 3, 4, 5, ... until cards run out
        let rowSize = 2;
        while (remaining > 0) {
            const take = Math.min(rowSize, remaining);
            rowSizes.push(take);
            remaining -= take;
            rowSize += 1;
        }

        allCards.forEach((card) => {
            if (cardIndex >= rowSizes[rowIndex]) {
                rowIndex++;
                cardIndex = 0;
            }

            const thisRowCount = rowSizes[rowIndex];
            const rowWidth = thisRowCount * colGapX;
            const rowStartX = centerX - rowWidth / 2 + colGapX / 2;

            const type = card.dataset.type;
            const id = card.dataset.id;
            const seed = `${type}${id}`;
            const rand1 = seededRandom(seed);
            const rand2 = seededRandom(seed + 'y');
            const rand3 = seededRandom(seed + 'r');

            const jitterX = (rand1 - 0.5) * 24;
            const jitterY = (rand2 - 0.5) * 24;
            const rotation = (rand3 - 0.5) * 14;

            const x = rowStartX + cardIndex * colGapX - cardW / 2 + jitterX;
            const y = victimY + 160 + rowIndex * rowGapY + jitterY;

            positions[`${type}-${id}`] = { x: x + cardW / 2, y: y + 20 };

            const isLocked = card.classList.contains('board-card--undiscovered') ||
                card.querySelector('[data-suspect-lock]') !== null;

            const img = card.querySelector('img');
            const nameEl = card.querySelector('strong');
            const imgSrc = img ? img.src : '';
            const name = nameEl ? nameEl.textContent : (card.dataset.name || '???');

            const el = document.createElement('div');
            el.className = 'cork-card' + (isLocked ? ' cork-card--locked' : '');
            el.style.left = x + 'px';
            el.style.top = y + 'px';
            el.style.transform = `rotate(${rotation}deg)`;
            el.dataset.type = type;
            el.dataset.id = id;

            el.innerHTML = `
                <span class="cork-card__pin"></span>
                ${isLocked
                ? `<div class="cork-card__locked-icon">🔒</div>`
                : `<img src="${imgSrc}" alt="${name}" />`}
                <span class="cork-card__label">${isLocked ? '???' : name}</span>
            `;

            if (!isLocked) {
                el.addEventListener('click', () => window.openFileModal?.(card));
            }

            corkCanvas.appendChild(el);
            cardIndex++;
        });

        // ===== Draw connection strings =====
        connections.forEach(conn => {
            const fromKey = `${conn.fromType || conn.FromType}-${conn.fromId || conn.FromId}`;
            const toKey = `${conn.toType || conn.ToType}-${conn.toId || conn.ToId}`;
            const from = positions[fromKey];
            const to = positions[toKey];
            if (!from || !to) return;

            const line = document.createElementNS('http://www.w3.org/2000/svg', 'line');
            line.setAttribute('x1', from.x);
            line.setAttribute('y1', from.y);
            line.setAttribute('x2', to.x);
            line.setAttribute('y2', to.y);
            line.classList.add('cork-line');
            corkSvg.appendChild(line);
        });

        const totalHeight = victimY + 160 + rowSizes.length * rowGapY + 100;
        const totalWidth = boardWidth;
        corkCanvas.style.width = totalWidth + 'px';
        corkCanvas.style.height = totalHeight + 'px';

        // Safe dimension fallbacks for viewport calculations
        const availableWidth = Math.max(corkView.clientWidth - 40, 300);
        const availableHeight = Math.max(corkView.clientHeight - 40, 300);
        const scale = Math.min(1, availableWidth / totalWidth, availableHeight / totalHeight);
        corkCanvas.style.transform = `scale(${Math.max(scale, 0.1)})`;
    }

    if (toggleBtn) {
        toggleBtn.addEventListener('click', () => {
            corkView.style.display = 'block';
            if (corkBackdrop) corkBackdrop.style.display = 'block';
            buildCorkboard();
        });
    }

    if (closeCorkBtn) {
        closeCorkBtn.addEventListener('click', () => {
            corkView.style.display = 'none';
            if (corkBackdrop) corkBackdrop.style.display = 'none';
        });
    }

    corkBackdrop?.addEventListener('click', () => {
        corkView.style.display = 'none';
        corkBackdrop.style.display = 'none';
    });

    redrawAll();
})();