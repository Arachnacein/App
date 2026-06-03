// Cursor glow — śledzi pozycję myszy przez CSS custom properties
document.addEventListener('mousemove', (e) => {
    document.documentElement.style.setProperty('--cursor-x', e.clientX + 'px');
    document.documentElement.style.setProperty('--cursor-y', e.clientY + 'px');
});

// Fade-in przy scroll — IntersectionObserver + MutationObserver dla Blazor Server re-renderów
(function () {
    const fadeObs = new IntersectionObserver((entries) => {
        entries.forEach(e => e.target.classList.toggle('visible', e.isIntersecting));
    }, { threshold: 0.08 });

    function observeFadeElements() {
        document.querySelectorAll('.fade-in:not(.fade-observed)').forEach(el => {
            el.classList.add('fade-observed');
            fadeObs.observe(el);
        });
    }

    observeFadeElements();
    new MutationObserver(observeFadeElements)
        .observe(document.body, { childList: true, subtree: true });
})();

// Count-up — animacja liczby na elementach z atrybutem data-count
(function () {
    function countUp(el) {
        const target = parseFloat(el.dataset.count) || 0;
        const suffix = el.dataset.suffix || '';
        const decimals = el.dataset.decimals ? parseInt(el.dataset.decimals) : 0;
        const duration = 900;
        const start = performance.now();

        const update = (time) => {
            const progress = Math.min((time - start) / duration, 1);
            const ease = 1 - Math.pow(1 - progress, 3);
            const value = ease * target;
            el.textContent = value.toFixed(decimals) + suffix;
            if (progress < 1) requestAnimationFrame(update);
        };
        requestAnimationFrame(update);
    }

    const countObs = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting && !e.target.dataset.animated) {
                e.target.dataset.animated = 'true';
                countUp(e.target);
            }
        });
    }, { threshold: 0.5 });

    function observeCountElements() {
        document.querySelectorAll('[data-count]:not([data-count-observed])').forEach(el => {
            el.dataset.countObserved = 'true';
            countObs.observe(el);
        });
    }

    observeCountElements();
    new MutationObserver(observeCountElements)
        .observe(document.body, { childList: true, subtree: true });
})();
