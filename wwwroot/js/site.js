// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ===== Global click SFX =====
// One delegated listener covers every button/link on every page, current
// and future, without needing to wire each one individually. Elements that
// already trigger their own distinct sound (board pins, corkboard toggle,
// file-modal open/close) are explicitly skipped here so they don't double-fire.
document.addEventListener('click', function (e) {
    if (!window.GameAudio) return;

    const target = e.target.closest(
        'button, a.btn-stamp, .dossier-nav__menu-trigger, .dossier-nav__dropdown a, .dossier-nav__dropdown button'
    );
    if (!target) return;

    // Already-wired elements elsewhere in the app — skip to avoid double sound.
    if (target.closest('#toggle-corkboard-btn, #close-corkboard-btn, .board-card__img, [data-modal-listen], [data-connect-pin], .file-modal-overlay')) {
        return;
    }

    window.GameAudio.playNavClick();
});
