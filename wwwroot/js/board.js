// wwwroot/js/board.js
(function () {
    const svg = document.getElementById('connection-svg');
    let selectedCard = null;
    let connections = [...window.initialConnections];

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

        // Sag the string like real corkboard twine — bow the midpoint downward,
        // more sag for longer threads, capped so it doesn't get silly on wide boards.
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

        path.addEventListener('click', () => removeConnection(path, conn.id || conn.Id));

        svg.appendChild(path);
    }

    function redrawAll() {
        svg.querySelectorAll('.conn-line').forEach(el => el.remove());
        connections.forEach(drawLine);
    }

    async function removeConnection(lineEl, connectionId) {
        if (!confirm('Remove this connection?')) return;

        const res = await fetch('/Board/DeleteConnection', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ connectionId })
        });

        if (res.ok) {
            connections = connections.filter(c => (c.id || c.Id) !== connectionId);
            redrawAll();
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
    // The pin is the only click target for starting/completing a connection.
    // Clicking elsewhere on a card (e.g. the photo) is handled separately by file-modal.js.
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
                connections.push({ fromType, fromId, toType, toId, id: Date.now() });
                redrawAll();
                location.reload();
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
                card.classList.toggle('is-eliminated');
            } else {
                alert('Could not toggle elimination.');
            }
        });
    });

    redrawAll();
})();