// file-modal.js — generic "pop-out" card reader
// Works off data-* attributes on any element with class "dossier-item-card is-clickable"

(function () {
    // Build the modal shell once, inject it into the page
    const overlay = document.createElement('div');
    overlay.className = 'file-modal-overlay';
    overlay.innerHTML = `
        <div class="file-modal" role="dialog" aria-modal="true">
            <button class="file-modal__close" aria-label="Close">&times;</button>
            <img class="file-modal__image" src="" alt="" style="display:none;" />
            <div class="file-modal__label" data-modal-label style="display:none;"></div>
            <h2 class="file-modal__title" data-modal-title></h2>
            <div class="file-modal__body" data-modal-body></div>
        </div>
    `;
    document.body.appendChild(overlay);

    const modalImg = overlay.querySelector('.file-modal__image');
    const modalLabel = overlay.querySelector('[data-modal-label]');
    const modalTitle = overlay.querySelector('[data-modal-title]');
    const modalBody = overlay.querySelector('[data-modal-body]');
    const closeBtn = overlay.querySelector('.file-modal__close');

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

        overlay.classList.add('is-open');
    }

    function closeModal() {
        overlay.classList.remove('is-open');
    }

    document.querySelectorAll('.dossier-item-card.is-clickable').forEach(card => {
        card.addEventListener('click', () => openModal(card));
    });

    closeBtn.addEventListener('click', closeModal);
    overlay.addEventListener('click', (e) => {
        if (e.target === overlay) closeModal(); // click outside the card closes it
    });
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') closeModal();
    });
})();