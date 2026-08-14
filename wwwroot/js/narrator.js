/**
 * narrator.js
 * Noir-style narration engine. Exposes window.Narrator with speak()/stop().
 * file-modal.js calls Narrator.speak(text, buttonEl) directly since the modal
 * is a single reused node, not per-card buttons.
 *
 * Voice tuned via prototype testing: Moira (macOS/Safari local voice),
 * rate 0.5, pitch 0.9. Falls back gracefully elsewhere — this is a known,
 * accepted limitation, not a bug.
 */
(function () {
    const synth = window.speechSynthesis;
    const PREFERRED_VOICE_NAMES = ['Moira'];
    const RATE = 1.0;
    const PITCH = 0.8;
    let cachedVoice = null;

    function loadVoices() {
        if (!synth) return;
        const voices = synth.getVoices();
        if (voices.length === 0) return;

        cachedVoice =
            voices.find(v => PREFERRED_VOICE_NAMES.includes(v.name)) ||
            voices.find(v => v.lang.startsWith('en') && /male/i.test(v.name)) ||
            voices.find(v => v.lang.startsWith('en')) ||
            voices[0] ||
            null;
    }

    if (synth) {
        loadVoices();
        if (synth.onvoiceschanged !== undefined) synth.onvoiceschanged = loadVoices;
    }

    function setButtonState(button, speaking) {
        if (!button) return;
        button.classList.toggle('is-speaking', speaking);
        button.textContent = speaking ? '⏹ Stop' : '🔊 Listen';
    }

    function speak(text, button) {
        if (!synth || !text) return;

        synth.cancel();

        const utter = new SpeechSynthesisUtterance(text);
        utter.rate = RATE;
        utter.pitch = PITCH;
        if (cachedVoice) utter.voice = cachedVoice;

        utter.onstart = () => setButtonState(button, true);
        utter.onend = () => setButtonState(button, false);
        utter.onerror = () => setButtonState(button, false);

        synth.speak(utter);
    }

    function stop(button) {
        if (!synth) return;
        synth.cancel();
        setButtonState(button, false);
    }

    window.Narrator = {
        speak,
        stop,
        isSupported: !!synth
    };
})();