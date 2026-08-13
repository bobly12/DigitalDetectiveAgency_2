// wwwroot/js/audio-system.js
// Shared audio controller: one AudioContext, one master gain node that
// every sound (board connections, nav clicks, ambient hum) routes through.
// Muting here mutes everything, everywhere, in one place.
(function () {
    let ctx = null;
    let masterGain = null;
    let ambientNodes = null;
    let isMuted = localStorage.getItem('dda-audio-muted') === 'true';

    function getCtx() {
        if (!ctx) {
            ctx = new (window.AudioContext || window.webkitAudioContext)();
            masterGain = ctx.createGain();
            masterGain.gain.value = isMuted ? 0 : 1;
            masterGain.connect(ctx.destination);
        }
        if (ctx.state === 'suspended') ctx.resume();
        return ctx;
    }

    function getMasterGain() {
        getCtx(); // ensures masterGain exists
        return masterGain;
    }

    function setMuted(muted) {
        isMuted = muted;
        localStorage.setItem('dda-audio-muted', String(muted));
        if (masterGain) {
            masterGain.gain.setTargetAtTime(muted ? 0 : 1, getCtx().currentTime, 0.05);
        }
        document.dispatchEvent(new CustomEvent('dda-mute-changed', { detail: { muted } }));
    }

    // ---------- Simple UI sounds (click, paper shuffle) ----------
    function tone(freq, duration, type = 'sine', gainPeak = 0.12, delay = 0) {
        const c = getCtx();
        const osc = c.createOscillator();
        const gain = c.createGain();
        osc.type = type;
        osc.frequency.value = freq;
        const start = c.currentTime + delay;
        gain.gain.setValueAtTime(0, start);
        gain.gain.linearRampToValueAtTime(gainPeak, start + 0.01);
        gain.gain.exponentialRampToValueAtTime(0.0001, start + duration);
        osc.connect(gain).connect(getMasterGain());
        osc.start(start);
        osc.stop(start + duration + 0.02);
    }

    function playNavClick() {
        try {
            tone(900, 0.04, 'square', 0.08);
            tone(500, 0.05, 'triangle', 0.05, 0.015);
        } catch { /* AudioContext not ready yet - ignore */ }
    }

    function playPaperShuffle() {
        try {
            const c = getCtx();
            const bufferSize = c.sampleRate * 0.12;
            const buffer = c.createBuffer(1, bufferSize, c.sampleRate);
            const data = buffer.getChannelData(0);
            for (let i = 0; i < bufferSize; i++) {
                data[i] = (Math.random() * 2 - 1) * (1 - i / bufferSize);
            }
            const noise = c.createBufferSource();
            noise.buffer = buffer;
            const filter = c.createBiquadFilter();
            filter.type = 'bandpass';
            filter.frequency.setValueAtTime(1800, c.currentTime);
            filter.frequency.exponentialRampToValueAtTime(300, c.currentTime + 0.12);
            const gain = c.createGain();
            gain.gain.setValueAtTime(0.15, c.currentTime);
            gain.gain.exponentialRampToValueAtTime(0.0001, c.currentTime + 0.12);
            noise.connect(filter).connect(gain).connect(getMasterGain());
            noise.start();
        } catch { /* ignore */ }
    }

    // ---------- Ambient office hum (rain-ish noise bed + faint tick) ----------
    function startAmbient() {
        if (ambientNodes) return; // already running
        try {
            const c = getCtx();

            // Soft filtered noise bed = distant rain / room tone
            const bufferSize = c.sampleRate * 2;
            const buffer = c.createBuffer(1, bufferSize, c.sampleRate);
            const data = buffer.getChannelData(0);
            for (let i = 0; i < bufferSize; i++) {
                data[i] = Math.random() * 2 - 1;
            }
            const noise = c.createBufferSource();
            noise.buffer = buffer;
            noise.loop = true;

            const noiseFilter = c.createBiquadFilter();
            noiseFilter.type = 'lowpass';
            noiseFilter.frequency.value = 600;

            const noiseGain = c.createGain();
            noiseGain.gain.value = 0.025; // deliberately very quiet - ambience, not a soundtrack

            noise.connect(noiseFilter).connect(noiseGain).connect(getMasterGain());
            noise.start();

            // Faint clock tick every ~1.1s
            let tickTimer = setInterval(() => {
                tone(2200, 0.02, 'square', 0.02);
            }, 1100);

            ambientNodes = { noise, tickTimer };
        } catch { /* ignore */ }
    }

    function stopAmbient() {
        if (!ambientNodes) return;
        try {
            ambientNodes.noise.stop();
            clearInterval(ambientNodes.tickTimer);
        } catch { /* ignore */ }
        ambientNodes = null;
    }

    let musicEl = null;
    let musicStarted = false;

    function initMusic() {
        musicEl = document.getElementById('bg-music');
        if (!musicEl) return;

        const c = getCtx();
        const source = c.createMediaElementSource(musicEl);
        const musicGain = c.createGain();
        musicGain.gain.value = 0.25;
        source.connect(musicGain).connect(getMasterGain());
    }

    function startMusic() {
        if (musicStarted) return;
        if (!musicEl) initMusic();
        if (!musicEl) return;
        musicEl.play().catch(() => { /* blocked until user gesture - fine */ });
        musicStarted = true;
    }

    function stopMusic() {
        if (musicEl) musicEl.pause();
        musicStarted = false;
    }

    window.GameAudio = {
        getCtx,
        getMasterGain,
        isMuted: () => isMuted,
        setMuted,
        playNavClick,
        playPaperShuffle,
        startAmbient,
        stopAmbient,
        startMusic,
        stopMusic
    };

    // Apply saved mute state immediately once the context exists (lazily -
    // browsers block audio until a user gesture, so this just pre-sets the
    // gain value for whenever the first sound actually plays).
})();
