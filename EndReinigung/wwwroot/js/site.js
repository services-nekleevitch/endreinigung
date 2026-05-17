// Module-level viewport-width cache. Multiple carousels each call
// getSlidesPerView() in their own layout(); without this, the second carousel's
// window.innerWidth read happens after the first carousel's style writes,
// forcing a synchronous layout recalculation (layout thrashing across carousels).
var _vpWidth = null;
function getViewportWidth() {
    if (_vpWidth !== null) return _vpWidth;
    _vpWidth = window.innerWidth;
    return _vpWidth;
}
window.addEventListener('resize', function () { _vpWidth = null; });

// Mobile Navigation Toggle
document.addEventListener('DOMContentLoaded', function () {
    const menuBtn = document.getElementById('mobile-menu-btn');
    const menuIcon = document.getElementById('menu-icon');
    const closeIcon = document.getElementById('close-icon');
    const mobileMenu = document.getElementById('mobile-menu');

    if (menuBtn && mobileMenu) {
        menuBtn.addEventListener('click', function () {
            const isOpen = mobileMenu.classList.toggle('hidden');
            if (menuIcon && closeIcon) {
                menuIcon.classList.toggle('hidden');
                closeIcon.classList.toggle('hidden');
            }
        });
    }

    // FAQ Accordion
    document.querySelectorAll('.faq-item .faq-trigger').forEach(function (trigger) {
        trigger.addEventListener('click', function () {
            const item = this.closest('.faq-item');
            const wasOpen = item.classList.contains('open');
            // Close all
            document.querySelectorAll('.faq-item').forEach(function (i) {
                i.classList.remove('open');
            });
            // Toggle clicked
            if (!wasOpen) {
                item.classList.add('open');
            }
        });
    });

    // Hero Form
    const heroForm = document.getElementById('hero-form');
    if (heroForm) {
        heroForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const roomSize = document.getElementById('hero-room-size').value;
            const zipCode = document.getElementById('hero-zip-code').value;
            const errorEl = document.getElementById('hero-error');
            const roomField = document.getElementById('hero-room-size');
            const zipField = document.getElementById('hero-zip-code');

            // Reset errors
            roomField.classList.remove('border-booking-orange', 'border-[3px]');
            zipField.classList.remove('border-booking-orange');
            if (errorEl) errorEl.textContent = '';

            if (!roomSize && !zipCode) {
                if (errorEl) errorEl.textContent = 'Bitte wählen Sie die Zimmeranzahl und die Postleitzahl aus.';
                roomField.classList.add('border-booking-orange', 'border-[3px]');
                zipField.classList.add('border-booking-orange');
                return;
            }
            if (!roomSize) {
                if (errorEl) errorEl.textContent = 'Bitte wählen Sie die Zimmeranzahl aus.';
                roomField.classList.add('border-booking-orange', 'border-[3px]');
                return;
            }
            if (!zipCode) {
                if (errorEl) errorEl.textContent = 'Bitte geben Sie Ihre Postleitzahl ein.';
                zipField.classList.add('border-booking-orange');
                return;
            }
            if (!isValidZurichPlz(zipCode)) {
                if (errorEl) errorEl.textContent = 'Diese Postleitzahl liegt nicht in unserem Servicegebiet (Kanton Zürich).';
                zipField.classList.add('border-booking-orange');
                return;
            }

            window.location.href = '/calculator?zip=' + encodeURIComponent(zipCode) + '&room=' + encodeURIComponent(roomSize);
        });

        // Only allow digits in PLZ field
        const zipInput = document.getElementById('hero-zip-code');
        if (zipInput) {
            zipInput.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, '').slice(0, 4);
            });
        }
    }

    // Gallery Carousel — 1 on mobile, 2 on tablet, 3 on desktop, infinite loop
    initCarousel('gallery-carousel', { slidesPerView: { base: 1, md: 2, lg: 3 } });

    // Reviews Carousel
    initCarousel('reviews-carousel', { slidesPerView: { base: 1 }, autoplay: 6000 });
});

// Zurich PLZ validation
var ZURICH_PLZ = new Set([
    8000,8001,8002,8003,8004,8005,8006,8008,8010,8012,8032,8037,8038,8041,8044,8045,8046,8047,8048,8049,
    8050,8051,8052,8053,8055,8057,8064,8102,8103,8104,8105,8106,8107,8108,8109,8112,8113,8114,8115,8117,
    8118,8121,8122,8123,8124,8125,8126,8127,8132,8133,8134,8135,8136,8142,8143,8152,8153,8154,8155,8156,
    8157,8158,8162,8164,8165,8166,8172,8173,8174,8175,8180,8181,8182,8184,8185,8187,8192,8193,8194,8195,
    8196,8197,8302,8303,8304,8305,8306,8307,8308,8309,8310,8311,8312,8314,8315,8317,8320,8322,8330,8331,
    8332,8335,8340,8342,8344,8345,8400,8403,8404,8405,8406,8408,8409,8412,8413,8414,8415,8416,8418,8421,
    8422,8424,8425,8426,8427,8428,8442,8444,8447,8450,8451,8452,8453,8457,8458,8459,8460,8461,8462,8463,
    8465,8466,8467,8468,8471,8472,8474,8475,8476,8477,8478,8479,8482,8483,8484,8486,8487,8488,8489,8492,
    8493,8494,8496,8497,8498,8499,8523,8525,8542,8543,8544,8545,8546,8548,8600,8602,8603,8604,8605,8606,
    8607,8608,8610,8614,8615,8616,8617,8618,8620,8623,8624,8625,8626,8627,8630,8632,8633,8634,8635,8636,
    8637,8700,8702,8703,8704,8706,8707,8708,8712,8713,8800,8802,8803,8804,8810,8815,8816,8820,8824,8825,
    8833,8902,8903,8904,8906,8907,8908,8909,8910,8911,8912,8913,8914,8915,8951,8952,8953,8954,8955
]);

function isValidZurichPlz(plz) {
    return plz.length === 4 && ZURICH_PLZ.has(parseInt(plz, 10));
}

// Generic carousel initializer
function initCarousel(id, options) {
    var container = document.getElementById(id);
    if (!container) return;

    var track = container.querySelector('.carousel-track');
    var slides = container.querySelectorAll('.carousel-slide');
    var prevBtn = container.querySelector('.carousel-prev');
    var nextBtn = container.querySelector('.carousel-next');
    var dotsContainer = document.getElementById(id + '-dots');
    var currentIndex = 0;
    var totalSlides = slides.length;

    function getSlidesPerView() {
        var w = getViewportWidth();
        if (options.slidesPerView.lg && w >= 1024) return options.slidesPerView.lg;
        if (options.slidesPerView.md && w >= 768) return options.slidesPerView.md;
        return options.slidesPerView.base || 1;
    }

    function getMaxIndex() {
        return Math.max(0, totalSlides - getSlidesPerView());
    }

    // Dot indicators
    function buildDots() {
        if (!dotsContainer) return;
        dotsContainer.innerHTML = '';
        var count = getMaxIndex() + 1;
        for (var i = 0; i < count; i++) {
            var dot = document.createElement('button');
            dot.className = i === currentIndex
                ? 'w-6 h-2 rounded-full bg-primary transition-all duration-300'
                : 'w-2 h-2 rounded-full bg-gray-300 transition-all duration-300';
            dot.setAttribute('aria-label', 'Slide ' + (i + 1));
            dot.dataset.index = i;
            dot.addEventListener('click', function () {
                currentIndex = parseInt(this.dataset.index);
                updatePosition();
            });
            dotsContainer.appendChild(dot);
        }
    }

    function updateDots() {
        if (!dotsContainer) return;
        var dots = dotsContainer.children;
        for (var i = 0; i < dots.length; i++) {
            dots[i].className = i === currentIndex
                ? 'w-6 h-2 rounded-full bg-primary transition-all duration-300'
                : 'w-2 h-2 rounded-full bg-gray-300 transition-all duration-300';
        }
    }

    function updatePosition() {
        var maxIdx = getMaxIndex();
        if (currentIndex > maxIdx) currentIndex = maxIdx;
        if (currentIndex < 0) currentIndex = 0;
        // Each slide is (100 / totalSlides)% of track width
        var offset = currentIndex * (100 / totalSlides);
        track.style.transform = 'translateX(-' + offset + '%)';
        updateDots();
    }

    function next() {
        currentIndex = currentIndex >= getMaxIndex() ? 0 : currentIndex + 1;
        updatePosition();
    }

    function prev() {
        currentIndex = currentIndex <= 0 ? getMaxIndex() : currentIndex - 1;
        updatePosition();
    }

    if (prevBtn) prevBtn.addEventListener('click', prev);
    if (nextBtn) nextBtn.addEventListener('click', next);

    // Touch / swipe
    var touchStartX = 0;
    var touchEndX = 0;
    var swiping = false;

    track.addEventListener('touchstart', function (e) {
        touchStartX = e.changedTouches[0].screenX;
        touchEndX = touchStartX;
        swiping = true;
        track.style.transition = 'none';
    }, { passive: true });

    track.addEventListener('touchmove', function (e) {
        if (!swiping) return;
        touchEndX = e.changedTouches[0].screenX;
    }, { passive: true });

    track.addEventListener('touchend', function () {
        if (!swiping) return;
        swiping = false;
        track.style.transition = 'transform 0.4s ease';
        var diff = touchStartX - touchEndX;
        if (Math.abs(diff) > 40) {
            if (diff > 0) next(); else prev();
        }
    });

    // Layout: track is wide, each slide is sized for slidesPerView
    function layout() {
        var spv = getSlidesPerView();
        // Track width = (totalSlides / spv) * 100% of container
        track.style.width = (totalSlides / spv * 100) + '%';
        // Each slide = equal fraction of track
        for (var i = 0; i < slides.length; i++) {
            slides[i].style.width = (100 / totalSlides) + '%';
            slides[i].style.flex = 'none';
        }
        buildDots();
        updatePosition();
    }

    layout();
    window.addEventListener('resize', layout);

    // Autoplay
    if (options.autoplay) {
        var iv = setInterval(next, options.autoplay);
        container.addEventListener('mouseenter', function () { clearInterval(iv); });
        container.addEventListener('mouseleave', function () { iv = setInterval(next, options.autoplay); });
    }
}
