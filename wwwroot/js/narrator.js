/**
 * narrator.js
 * Noir-style narration engine. Exposes window.Narrator with speak()/stop()
 * plus voice selection (listVoices/setVoice), persisted in localStorage so
 * the same chosen voice is used everywhere in the app.
 */
(function () {
    const synth = window.speechSynthesis;
    const STORAGE_KEY = 'dda-narrator-voice';
    const FALLBACK_NAMES = ['Moira'];
    const RATE = 0.5;
    const PITCH = 0.9;

    let cachedVoice = null;
    let voicesLoaded = false;

    function loadVoices() {
        if (!synth) return;
        const voices = synth.getVoices();
        if (voices.length === 0) return;
        voicesLoaded = true;

        const savedName = localStorage.getItem(STORAGE_KEY);
        cachedVoice =
            (savedName && voices.find(v => v.name === savedName)) ||
            voices.find(v => FALLBACK_NAMES.includes(v.name)) ||
            voices.find(v => v.lang.startsWith('en') && /male/i.test(v.name)) ||
            voices.find(v => v.lang.startsWith('en')) ||
            voices[0] ||
            null;

        document.dispatchEvent(new CustomEvent('dda-voices-ready'));
    }

    if (synth) {
        loadVoices();
        if (synth.onvoiceschanged !== undefined) {
            synth.onvoiceschanged = () => {
                if (voicesLoaded) return; // only process the first successful load
                loadVoices();
            };
        }
    }

    function listVoices() {
        if (!synth) return [];
        return synth.getVoices().filter(v => v.lang.startsWith('en'));
    }

    function setVoice(name) {
        if (!synth) return;
        const voice = synth.getVoices().find(v => v.name === name);
        if (!voice) return;
        cachedVoice = voice;
        localStorage.setItem(STORAGE_KEY, name);
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
        listVoices,
        setVoice,
        isSupported: !!synth,
        get currentVoiceName() { return cachedVoice ? cachedVoice.name : null; },
        get voicesLoaded() { return voicesLoaded; }
    };

    // Safety net: never let speech bleed into whatever page loads next.
    window.addEventListener('beforeunload', () => {
        if (synth) synth.cancel();
    });
    window.addEventListener('pagehide', () => {
        if (synth) synth.cancel();
    });
})();
