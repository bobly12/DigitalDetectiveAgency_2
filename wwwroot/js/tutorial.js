document.addEventListener("DOMContentLoaded", () => {

    const dialogueBox = document.getElementById("dialogue-box");
    const nextButton = document.getElementById("nextButton");
    const skipButton = document.getElementById("skipButton");

    if (!dialogueBox || !nextButton)
        return;

    const dialogue = window.tutorialDialogue || [];
    const firstCaseId = window.firstCaseId || 1;

    let currentStep = 0;
    let typing = false;

    //--------------------------------------------------
    // Typewriter Effect
    //--------------------------------------------------

    function typeWriter(text) {

        dialogueBox.innerHTML = "";
        typing = true;

        let i = 0;

        function type() {

            if (!typing) {
                dialogueBox.innerHTML = text;
                return;
            }

            if (i < text.length) {

                dialogueBox.innerHTML += text.charAt(i);

                i++;

                setTimeout(type, 18);

            } else {

                typing = false;

            }

        }

        type();

    }

    //--------------------------------------------------
    // Show Current Dialogue
    //--------------------------------------------------

    function showStep() {

        typeWriter(dialogue[currentStep]);

        if (currentStep === dialogue.length - 1) {

            nextButton.textContent = "Begin Investigation";

        }
        else {

            nextButton.textContent = "Continue";

        }

    }

    //--------------------------------------------------
    // Continue Button
    //--------------------------------------------------

    nextButton.addEventListener("click", () => {

        // Skip typing animation
        if (typing) {

            typing = false;
            dialogueBox.innerHTML = dialogue[currentStep];
            return;

        }

        currentStep++;

        if (currentStep >= dialogue.length) {

            completeTutorial();
            return;

        }

        showStep();

    });

    //--------------------------------------------------
    // Skip Briefing Button
    //--------------------------------------------------

    if (skipButton) {

        skipButton.addEventListener("click", async () => {

            skipButton.disabled = true;
            skipButton.textContent = "Loading...";

            try {

                const response = await fetch("/Tutorial/Complete", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    }
                });

                if (!response.ok)
                    throw new Error();

                const data = await response.json();

                window.location.href = data.redirect;

            }
            catch {

                window.location.href = `/Board/Index/${firstCaseId}`;

            }

        });

    }

    //--------------------------------------------------
    // Save Tutorial Completion
    //--------------------------------------------------

    async function completeTutorial() {

        nextButton.disabled = true;
        nextButton.textContent = "Loading...";

        try {

            const response = await fetch("/Tutorial/Complete", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok)
                throw new Error();

            const data = await response.json();

            document.body.style.transition = "opacity .8s ease";
            document.body.style.opacity = "0";

            setTimeout(() => {

                window.location.href = data.redirect;

            }, 800);

        }
        catch {

            window.location.href = `/Board/Index/${firstCaseId}`;

        }

    }

    //--------------------------------------------------
    // Keyboard Support
    //--------------------------------------------------

    document.addEventListener("keydown", e => {

        if (e.code === "Enter" || e.code === "Space") {

            e.preventDefault();

            nextButton.click();

        }

        if (e.code === "Escape" && skipButton) {

            e.preventDefault();

            skipButton.click();

        }

    });

    //--------------------------------------------------
    // Start Tutorial
    //--------------------------------------------------

    showStep();

});