// wwwroot/js/board.js
(function () {
    const svg = document.getElementById('connection-svg');
    const cards = document.querySelectorAll('.board-card');
    let selectedCard = null;
    let connections = [...window.initialConnections];

    function getCardCenter(card) {
        const rect = card.getBoundingClientRect();
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

        const from = getCardCenter(fromCard);
        const to = getCardCenter(toCard);

        const line = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        line.setAttribute('x1', from.x);
        line.setAttribute('y1', from.y);
        line.setAttribute('x2', to.x);
        line.setAttribute('y2', to.y);
        line.setAttribute('stroke', '#c0392b');
        line.setAttribute('stroke-width', '2');
        line.dataset.connectionId = conn.id || conn.Id;
        line.style.pointerEvents = 'stroke';
        line.style.cursor = 'pointer';

        line.addEventListener('click', () => removeConnection(line, conn.id || conn.Id));

        svg.appendChild(line);
    }

    function redrawAll() {
        svg.innerHTML = '';
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

    // ===== Card-to-card connection behavior =====
    cards.forEach(card => {
        card.style.cursor = 'pointer';
        card.addEventListener('click', async (e) => {
            // If the click was on the photo itself, let file-modal.js handle it instead
            if (e.target.classList.contains('board-card__img')) {
                return;
            }

            if (!selectedCard) {
                selectedCard = card;
                card.classList.add('is-selected');
                return;
            }

            if (selectedCard === card) {
                selectedCard.classList.remove('is-selected');
                selectedCard = null;
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

            selectedCard.classList.remove('is-selected');
            selectedCard = null;

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