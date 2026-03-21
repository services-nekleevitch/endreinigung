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

    // Simple Carousel
    initCarousel('gallery-carousel', { slidesPerView: { base: 1, md: 2, lg: 3 } });
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
    var currentIndex = 0;

    function getSlidesPerView() {
        var w = window.innerWidth;
        if (options.slidesPerView.lg && w >= 1024) return options.slidesPerView.lg;
        if (options.slidesPerView.md && w >= 768) return options.slidesPerView.md;
        return options.slidesPerView.base || 1;
    }

    function updatePosition() {
        var spv = getSlidesPerView();
        var maxIndex = Math.max(0, slides.length - spv);
        if (currentIndex > maxIndex) currentIndex = maxIndex;
        var pct = (currentIndex * 100) / slides.length;
        track.style.transform = 'translateX(-' + pct + '%)';
    }

    function next() {
        var spv = getSlidesPerView();
        var maxIndex = Math.max(0, slides.length - spv);
        currentIndex = currentIndex >= maxIndex ? 0 : currentIndex + 1;
        updatePosition();
    }

    function prev() {
        var spv = getSlidesPerView();
        var maxIndex = Math.max(0, slides.length - spv);
        currentIndex = currentIndex <= 0 ? maxIndex : currentIndex - 1;
        updatePosition();
    }

    if (prevBtn) prevBtn.addEventListener('click', prev);
    if (nextBtn) nextBtn.addEventListener('click', next);

    // Set slide widths
    slides.forEach(function (slide) {
        slide.style.flex = '0 0 ' + (100 / slides.length) + '%';
    });

    // Responsive update
    function updateSlideWidths() {
        var spv = getSlidesPerView();
        slides.forEach(function (slide) {
            slide.style.flex = '0 0 ' + (100 / spv) + '%';
        });
        track.style.width = (slides.length / spv * 100) + '%';
        updatePosition();
    }

    updateSlideWidths();
    window.addEventListener('resize', updateSlideWidths);

    // Autoplay
    if (options.autoplay) {
        var interval = setInterval(next, options.autoplay);
        container.addEventListener('mouseenter', function () { clearInterval(interval); });
        container.addEventListener('mouseleave', function () { interval = setInterval(next, options.autoplay); });
    }
}
