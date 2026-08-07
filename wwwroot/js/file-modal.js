// file-modal.js — generic "pop-out" card reader
(function () {
    const overlay = document.createElement('div');
    overlay.className = 'file-modal-overlay';
    overlay.innerHTML = `
        <div class="file-modal" role="dialog" aria-modal="true">
            <button class="file-modal__close" aria-label="Close">&times;</button>
            <img class="file-modal__image" src="" alt="" style="display:none;" />
            <div class="file-modal__label" data-modal-label style="display:none;"></div>
            <h2 class="file-modal__title" data-modal-title></h2>
            <button class="listen-btn" data-modal-listen>🔊 Listen</button>
            <div class="file-modal__body" data-modal-body></div>
        </div>
    `;
    document.body.appendChild(overlay);

    const modalImg = overlay.querySelector('.file-modal__image');
    const modalLabel = overlay.querySelector('[data-modal-label]');
    const modalTitle = overlay.querySelector('[data-modal-title]');
    const modalBody = overlay.querySelector('[data-modal-body]');
    const closeBtn = overlay.querySelector('.file-modal__close');
    const listenBtn = overlay.querySelector('[data-modal-listen]');

    // Hide the Listen button entirely if speechSynthesis isn't supported.
    if (window.Narrator && !window.Narrator.isSupported) {
        listenBtn.style.display = 'none';
    }

    function buildNarrationText(name, label, fields) {
        // Concatenate everything a player would want read aloud, in reading order.
        const parts = [];
        if (label) parts.push(label);
        if (name) parts.push(name);
        fields.forEach(f => {
            if (f.heading) parts.push(f.heading);
            if (f.text) parts.push(f.text);
        });
        return parts.join('. ');
    }

    function openModal(card) {
        const name = card.dataset.name || '';
        const image = card.dataset.image || '';
        const label = card.dataset.label || '';
        const fields = JSON.parse(card.dataset.fields || '[]');

        modalTitle.textContent = name;

        if (image) {
            modalImg.src = image;
            modalImg.alt = name;
            modalImg.style.display = 'block';
        } else {
            modalImg.style.display = 'none';
        }

        if (label) {
            modalLabel.textContent = label;
            modalLabel.style.display = 'block';
        } else {
            modalLabel.style.display = 'none';
        }

        modalBody.innerHTML = fields.map(f =>
            f.heading
                ? `<div class="file-modal__label">${f.heading}</div><p>${f.text}</p>`
                : `<p>${f.text}</p>`
        ).join('');

        // Store the narration text on the button itself so we don't
        // recompute it on every click.
        listenBtn.dataset.narrateText = buildNarrationText(name, label, fields);

        overlay.classList.add('is-open');
    }

    function closeModal() {
        overlay.classList.remove('is-open');
        if (window.Narrator) window.Narrator.stop(listenBtn);
    }

    // Attach click handlers to every card's image specifically
    document.querySelectorAll('.board-card__img').forEach(img => {
        img.addEventListener('click', (e) => {
            e.stopPropagation();
            openModal(img.closest('.board-card'));
        });
    });

    listenBtn.addEventListener('click', () => {
        if (!window.Narrator) return;

        if (listenBtn.classList.contains('is-speaking')) {
            window.Narrator.stop(listenBtn);
        } else {
            window.Narrator.speak(listenBtn.dataset.narrateText || '', listenBtn);
        }
    });

    closeBtn.addEventListener('click', closeModal);
    overlay.addEventListener('click', (e) => {
        if (e.target === overlay) closeModal();
    });
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') closeModal();
    });
})();