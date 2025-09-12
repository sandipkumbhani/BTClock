(function () {
    document.addEventListener("DOMContentLoaded", function () {
        var openPanel = openPanel;
        console.log(openPanel)
        if (openPanel) {
            var btn = document.getElementById("btn-" + openPanel);
            if (btn) btn.click(); // simulate opening the correct one
        }
    });
    const toggles = document.querySelectorAll('.accordion-toggle');
    const singleOpen = true; // set to true if you want only one open at a time

    toggles.forEach(btn => {
        const panel = document.getElementById(btn.getAttribute('aria-controls'));

        // click toggling
        btn.addEventListener('click', () => toggle(btn, panel));

        // keyboard: Enter or Space should toggle
        btn.addEventListener('keydown', (e) => {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                btn.click();
            }
        });
    });

    function toggle(btn, panel) {
        const isOpen = btn.getAttribute('aria-expanded') === 'true';

        if (isOpen) {
            // close it
            btn.setAttribute('aria-expanded', 'false');
            collapse(panel);
        } else {
            // open (and optionally close others)
            if (singleOpen) closeAllExcept(panel);
            btn.setAttribute('aria-expanded', 'true');
            expand(panel);
        }
    }

    function expand(panel) {
        // set explicit max-height to enable smooth transition to the measured height
        panel.classList.add('open');
        const fullHeight = panel.scrollHeight; // measured content height
        panel.style.maxHeight = fullHeight + 'px';
        panel.style.opacity = '1';
        // after transition ends, clear inline maxHeight to allow natural resizing
        panel.addEventListener('transitionend', function handler(e) {
            if (e.propertyName === 'max-height') {
                // remove maxHeight so content can expand if child changes height later
                panel.style.maxHeight = panel.scrollHeight + 'px';
                panel.removeEventListener('transitionend', handler);
            }
        });
    }

    function collapse(panel) {
        // set maxHeight to current to ensure starting point then animate to 0
        panel.style.maxHeight = panel.scrollHeight + 'px';
        // force layout so the browser registers the starting maxHeight
        panel.getBoundingClientRect();
        requestAnimationFrame(() => {
            panel.style.maxHeight = '0px';
            panel.style.opacity = '0';
        });

        // remove 'open' class after transition completes
        panel.addEventListener('transitionend', function handler(e) {
            if (e.propertyName === 'max-height' && panel.style.maxHeight === '0px') {
                panel.classList.remove('open');
                panel.removeEventListener('transitionend', handler);
            }
        });
    }

    function closeAllExcept(exceptPanel) {
        toggles.forEach(btn => {
            const p = document.getElementById(btn.getAttribute('aria-controls'));
            if (p !== exceptPanel && btn.getAttribute('aria-expanded') === 'true') {
                btn.setAttribute('aria-expanded', 'false');
                collapse(p);
            }
        });
    }

    // Keep open panels height updated on window resize
    window.addEventListener('resize', () => {
        document.querySelectorAll('.accordion-panel.open').forEach(p => {
            p.style.maxHeight = p.scrollHeight + 'px';
        });
    });
})();