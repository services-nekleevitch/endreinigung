// Zürich Endreinigung — Baureinigung multi-step calculator (vanilla JS state machine).
// Keep pricing in sync with EndReinigung/Models/BaureinigungPricing.cs and Services/PriceCalculator.cs.

(function () {
    'use strict';

    const ROOT = document.getElementById('baureinigung-calculator');
    if (!ROOT) return;

    // ===== Price data (mirror of BaureinigungPricing.cs) =====
    const APARTMENT_PRICES = {
        '1': 435, '1.5': 449, '2': 565, '2.5': 595, '3': 725, '3.5': 745,
        '4': 895, '4.5': 945, '5': 1095, '5.5': 1150
    };
    const HOUSE_PRICES = {
        '3': 1289, '3.5': 1389, '4': 1589, '4.5': 1689, '5': 1889, '5.5': 1989
    };
    const TOGGLE_PRICES = { bauschutt: 120, fassade: 180 };
    const COUNTABLE_PRICES = { balcony: 45, bath: 65, wc: 40, garage_pressure: 65 };
    const TOGGLE_LABELS = { bauschutt: 'Bauschutt-Entsorgung', fassade: 'Fassadenreinigung' };
    const COUNTABLE_LABELS = {
        balcony: 'Extra Balkon / Terrasse',
        bath: 'Anzahl Badezimmer',
        wc: 'Separates WC',
        garage_pressure: 'Garage / Carport Hochdruckreinigung'
    };
    // C# property names for hidden inputs
    const COUNTABLE_FIELD = {
        balcony: 'Balcony', bath: 'Bath', wc: 'Wc', garage_pressure: 'GaragePressure'
    };

    // ===== State =====
    const state = {
        step: 1,
        propertyType: 'apartment',
        selectedRoom: '',
        zipCode: '',
        selectedAddOns: new Set(), // 'bauschutt' / 'fassade'
        extraCounts: { balcony: 0, bath: 0, wc: 0, garage_pressure: 0 },
        cleaningDate: '',
        paymentMethod: '',
        sameAsCustomer: false
    };

    const $ = (sel, ctx) => (ctx || ROOT).querySelector(sel);
    const $$ = (sel, ctx) => Array.from((ctx || ROOT).querySelectorAll(sel));

    function getActivePrices() {
        return state.propertyType === 'house' ? HOUSE_PRICES : APARTMENT_PRICES;
    }

    function getRoomLabel(value) {
        if (!value) return '';
        return value.endsWith('.5') ? `${value} Zimmer` : `${parseInt(value, 10)} Zimmer`;
    }

    function computeTotal() {
        const prices = getActivePrices();
        const base = prices[state.selectedRoom] || 0;
        let extras = 0;
        state.selectedAddOns.forEach((id) => { extras += TOGGLE_PRICES[id] || 0; });
        Object.keys(state.extraCounts).forEach((key) => {
            extras += (state.extraCounts[key] || 0) * (COUNTABLE_PRICES[key] || 0);
        });
        return base + extras;
    }

    // Mirror of React getDefaultBathroomMapping
    function defaultBathMapping(roomValue) {
        const n = parseFloat(roomValue || '0');
        if (n <= 0) return { bath: 0, wc: 0 };
        if (n <= 2.5) return { bath: 1, wc: 0 };
        if (n <= 3.5) return { bath: 1, wc: 0 };
        if (n <= 4.5) return { bath: 1, wc: 1 };
        if (n <= 6.5) return { bath: 2, wc: 1 };
        return { bath: 3, wc: 1 };
    }

    function formatDateDE(isoDate) {
        if (!isoDate) return '';
        const d = new Date(isoDate);
        if (isNaN(d.getTime())) return '';
        return d.toLocaleDateString('de-CH', { day: '2-digit', month: 'long', year: 'numeric' });
    }

    // ===== Rendering =====
    function renderStep() {
        $$('[data-step]').forEach((el) => {
            const s = parseInt(el.getAttribute('data-step'), 10);
            el.classList.toggle('hidden', s !== state.step);
        });
        renderProgress();
    }

    function renderProgress() {
        for (let i = 1; i <= 4; i++) {
            const btn = $(`[data-progress-step="${i}"]`);
            const label = $(`[data-progress-label="${i}"]`);
            if (!btn) continue;
            const isDone = i < state.step;
            const isCurrent = i === state.step;
            const isClickable = i < state.step && state.step <= 4;

            btn.classList.remove('bg-primary', 'text-primary-foreground', 'bg-border', 'text-muted-foreground', 'cursor-pointer', 'cursor-default', 'hover:scale-110', 'hover:shadow-md');
            if (isDone || isCurrent) {
                btn.classList.add('bg-primary', 'text-primary-foreground');
            } else {
                btn.classList.add('bg-border', 'text-muted-foreground');
            }
            if (isClickable) {
                btn.classList.add('cursor-pointer', 'hover:scale-110', 'hover:shadow-md');
                btn.disabled = false;
            } else {
                btn.classList.add('cursor-default');
                btn.disabled = true;
            }
            btn.innerHTML = isDone
                ? '<svg class="w-5 h-5" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" viewBox="0 0 24 24"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/></svg>'
                : String(i);

            if (label) {
                label.classList.toggle('text-primary', isDone || isCurrent);
                label.classList.toggle('font-medium', isDone || isCurrent);
                label.classList.toggle('text-muted-foreground', !(isDone || isCurrent));
                label.classList.toggle('cursor-pointer', isClickable);
                label.classList.toggle('hover:underline', isClickable);
                label.disabled = !isClickable;
            }

            if (i < 4) {
                const bar = $(`[data-progress-bar="${i}"]`);
                if (bar) {
                    bar.classList.toggle('bg-primary', i < state.step);
                    bar.classList.toggle('bg-border', i >= state.step);
                }
            }
        }
    }

    function renderRoomGrid() {
        $$('[data-room-grid]').forEach((grid) => {
            const type = grid.getAttribute('data-room-grid');
            const show = type === state.propertyType;
            grid.classList.toggle('hidden', !show);
            grid.classList.toggle('grid', show);
        });

        $$('[data-room]').forEach((btn) => {
            const value = btn.getAttribute('data-room');
            const isSelected = value === state.selectedRoom;
            btn.classList.toggle('border-primary', isSelected && state.propertyType === 'apartment');
            btn.classList.toggle('bg-primary/10', isSelected && state.propertyType === 'apartment');
            btn.classList.toggle('border-booking-orange', isSelected && state.propertyType === 'house');
            btn.classList.toggle('bg-booking-orange/10', isSelected && state.propertyType === 'house');
            btn.classList.toggle('border-border', !isSelected);
            btn.classList.toggle('bg-card', !isSelected);
        });

        $$('[data-property-type]').forEach((btn) => {
            const type = btn.getAttribute('data-property-type');
            const isActive = type === state.propertyType;
            btn.classList.remove('border-primary', 'bg-primary/10', 'text-primary', 'border-booking-orange', 'bg-booking-orange/10', 'text-booking-orange', 'border-border', 'bg-card', 'text-muted-foreground');
            if (isActive && type === 'apartment') {
                btn.classList.add('border-primary', 'bg-primary/10', 'text-primary');
            } else if (isActive && type === 'house') {
                btn.classList.add('border-booking-orange', 'bg-booking-orange/10', 'text-booking-orange');
            } else {
                btn.classList.add('border-border', 'bg-card', 'text-muted-foreground');
            }
        });

        const title = $('[data-label-size-title]');
        if (title) title.textContent = state.propertyType === 'house' ? 'Hausgrösse wählen' : 'Wohnungsgrösse wählen';
    }

    function renderExtras() {
        $$('[data-extra-toggle]').forEach((card) => {
            const id = card.getAttribute('data-extra-toggle');
            const isActive = state.selectedAddOns.has(id);
            card.classList.toggle('border-booking-orange', isActive);
            card.classList.toggle('bg-booking-orange/10', isActive);
            card.classList.toggle('border-border', !isActive);
            card.classList.toggle('bg-card', !isActive);
            const check = $('[data-check]', card);
            if (check) {
                check.classList.toggle('bg-orange-500', isActive);
                check.classList.toggle('border-orange-500', isActive);
                check.classList.toggle('border-slate-300', !isActive);
                check.innerHTML = isActive ? '<svg class="w-3 h-3 text-white" fill="none" stroke="currentColor" stroke-width="3" viewBox="0 0 24 24"><polyline points="20 6 9 17 4 12"/></svg>' : '';
            }
        });

        $$('[data-extra-countable]').forEach((card) => {
            const key = card.getAttribute('data-extra-countable');
            const count = state.extraCounts[key] || 0;
            const isActive = count > 0;
            const price = parseInt(card.getAttribute('data-extra-price'), 10);

            card.classList.toggle('border-orange-500', isActive);
            card.classList.toggle('bg-orange-50', isActive);
            card.classList.toggle('border-slate-200', !isActive);
            card.classList.toggle('bg-white', !isActive);

            const check = $('[data-check]', card);
            if (check) {
                check.classList.toggle('bg-orange-500', isActive);
                check.classList.toggle('border-orange-500', isActive);
                check.classList.toggle('border-slate-300', !isActive);
                check.innerHTML = isActive ? '<svg class="w-3 h-3 text-white" fill="none" stroke="currentColor" stroke-width="3" viewBox="0 0 24 24"><polyline points="20 6 9 17 4 12"/></svg>' : '';
            }

            const counter = $('[data-extra-counter]', card);
            if (counter) {
                counter.classList.toggle('hidden', !isActive);
                counter.classList.toggle('flex', isActive);
            }

            $$('[data-count]', card).forEach((btn) => {
                const n = parseInt(btn.getAttribute('data-count'), 10);
                const isSel = n === count;
                btn.classList.toggle('bg-orange-500', isSel);
                btn.classList.toggle('text-white', isSel);
                btn.classList.toggle('bg-slate-100', !isSel);
                btn.classList.toggle('text-slate-700', !isSel);
                btn.classList.toggle('hover:bg-slate-200', !isSel);
            });

            const totalLbl = $('[data-extra-total]', card);
            if (totalLbl) totalLbl.textContent = `+CHF ${count * price}`;
        });
    }

    function renderStep2Summary() {
        const prices = getActivePrices();
        const price = prices[state.selectedRoom] || 0;
        const typeLabel = state.propertyType === 'house' ? 'Haus' : 'Wohnung';
        const roomLabel = getRoomLabel(state.selectedRoom);

        const line = $('[data-summary-line]');
        if (line) line.textContent = `${typeLabel} • ${roomLabel} • CHF ${price}`;

        const apIcon = $('[data-summary-icon="apartment"]');
        const hsIcon = $('[data-summary-icon="house"]');
        if (apIcon) apIcon.classList.toggle('hidden', state.propertyType !== 'apartment');
        if (hsIcon) hsIcon.classList.toggle('hidden', state.propertyType !== 'house');

        const incl = $('[data-summary-incl]');
        if (incl) {
            const baths = 1 + (state.extraCounts.bath || 0);
            const wcs = state.extraCounts.wc || 0;
            let html = `Inkl. <span class="font-bold text-booking-orange">${baths} Bad</span>`;
            if (wcs > 0) html += `, <span class="font-bold text-booking-orange">${wcs} WC</span>`;
            incl.innerHTML = html;
        }
    }

    function renderStep3() {
        const total = computeTotal();
        const totalEl = $('[data-price-total]');
        if (totalEl) totalEl.textContent = String(total);

        const list = $('[data-summary-list]');
        if (!list) return;

        const prices = getActivePrices();
        const base = prices[state.selectedRoom] || 0;
        const typeLabel = state.propertyType === 'house' ? 'Haus' : 'Wohnung';
        let html = '';
        html += `<div class="flex justify-between"><span class="text-muted-foreground">Baureinigung ${typeLabel}</span><span class="font-medium">${getRoomLabel(state.selectedRoom)}</span></div>`;
        html += `<div class="flex justify-between"><span class="text-muted-foreground">Grundpreis</span><span class="font-medium">CHF ${base}</span></div>`;
        html += `<div class="flex justify-between"><span class="text-muted-foreground">PLZ</span><span class="font-medium">${state.zipCode}</span></div>`;

        const hasExtras = state.selectedAddOns.size > 0 || Object.values(state.extraCounts).some((c) => c > 0);
        if (hasExtras) {
            html += `<div class="pt-2 border-t border-border"><span class="text-muted-foreground">Extras</span><ul class="mt-1 space-y-1">`;
            state.selectedAddOns.forEach((id) => {
                html += `<li class="flex justify-between"><span class="text-muted-foreground">${TOGGLE_LABELS[id]}</span><span class="font-medium">+CHF ${TOGGLE_PRICES[id]}</span></li>`;
            });
            Object.keys(state.extraCounts).forEach((key) => {
                const count = state.extraCounts[key];
                if (!count) return;
                const p = COUNTABLE_PRICES[key];
                html += `<li class="flex justify-between"><span class="text-muted-foreground">${COUNTABLE_LABELS[key]} (${count}×)</span><span class="font-medium">+CHF ${count * p}</span></li>`;
            });
            html += `</ul></div>`;
        }
        html += `<div class="pt-2 border-t border-border flex justify-between font-semibold text-base"><span>Total</span><span class="text-cleaning-green">CHF ${total}</span></div>`;
        list.innerHTML = html;
    }

    function renderStep4Summary() {
        const total = computeTotal();
        const typeLabel = state.propertyType === 'house' ? 'Haus' : 'Wohnung';
        const room = $('[data-booking-room]');
        if (room) room.textContent = `Baureinigung ${typeLabel} • ${getRoomLabel(state.selectedRoom)}`;
        const dateEl = $('[data-booking-date]');
        if (dateEl) dateEl.textContent = state.cleaningDate ? formatDateDE(state.cleaningDate) : '—';
        const zipEl = $('[data-booking-zip]');
        if (zipEl) zipEl.textContent = state.zipCode;
        const totalEl = $('[data-booking-total]');
        if (totalEl) totalEl.textContent = String(total);
    }

    function renderAll() {
        renderStep();
        renderRoomGrid();
        renderExtras();
        if (state.step === 2) renderStep2Summary();
        if (state.step === 3) renderStep3();
        if (state.step === 4) renderStep4Summary();
    }

    // ===== Step navigation =====
    function goToStep(n) {
        state.step = n;
        renderAll();
        scrollToCalculator();
    }

    function scrollToCalculator() {
        const top = ROOT.getBoundingClientRect().top + window.pageYOffset - 80;
        window.scrollTo({ top, behavior: 'smooth' });
    }

    function validateStep1() {
        let ok = true;
        const roomErr = $('[data-room-error]');
        const zipErr = $('[data-zip-error]');

        if (!state.selectedRoom) {
            if (roomErr) { roomErr.textContent = 'Bitte wählen Sie die Zimmeranzahl.'; roomErr.classList.remove('hidden'); }
            ok = false;
        } else if (roomErr) {
            roomErr.classList.add('hidden');
        }

        const zip = state.zipCode;
        if (!zip || zip.length !== 4) {
            if (zipErr) { zipErr.textContent = 'Bitte geben Sie Ihre 4-stellige Postleitzahl ein.'; zipErr.classList.remove('hidden'); }
            ok = false;
        } else if (typeof window.isValidZurichPlz === 'function' && !window.isValidZurichPlz(zip)) {
            if (zipErr) { zipErr.textContent = 'Diese Postleitzahl liegt nicht in unserem Servicegebiet (Kanton Zürich).'; zipErr.classList.remove('hidden'); }
            ok = false;
        } else if (zipErr) {
            zipErr.classList.add('hidden');
        }
        return ok;
    }

    function validateStep3() {
        const cleaningErr = $('[data-cleaning-date-error]');
        if (!state.cleaningDate) {
            if (cleaningErr) { cleaningErr.textContent = 'Bitte wählen Sie einen Reinigungstermin.'; cleaningErr.classList.remove('hidden'); }
            return false;
        }
        if (cleaningErr) cleaningErr.classList.add('hidden');
        return true;
    }

    // ===== Event wiring =====
    function wireStep1() {
        $$('[data-property-type]').forEach((btn) => {
            btn.addEventListener('click', () => {
                state.propertyType = btn.getAttribute('data-property-type');
                state.selectedRoom = '';
                renderRoomGrid();
            });
        });

        $$('[data-room]').forEach((btn) => {
            btn.addEventListener('click', () => {
                state.selectedRoom = btn.getAttribute('data-room');
                const defaults = defaultBathMapping(state.selectedRoom);
                state.extraCounts.bath = defaults.bath > 1 ? defaults.bath - 1 : 0;
                state.extraCounts.wc = defaults.wc;
                const err = $('[data-room-error]');
                if (err) err.classList.add('hidden');
                renderRoomGrid();
                renderExtras();
            });
        });

        const zipInput = $('#bau-calc-zip');
        if (zipInput) {
            zipInput.addEventListener('input', () => {
                zipInput.value = zipInput.value.replace(/\D/g, '').slice(0, 4);
                state.zipCode = zipInput.value;
                const err = $('[data-zip-error]');
                if (err) err.classList.add('hidden');
            });
        }

        const next1 = $('[data-action="next-1"]');
        if (next1) next1.addEventListener('click', () => { if (validateStep1()) goToStep(2); });
    }

    function wireStep2() {
        $$('[data-extra-toggle]').forEach((card) => {
            const id = card.getAttribute('data-extra-toggle');
            card.addEventListener('click', () => {
                if (state.selectedAddOns.has(id)) state.selectedAddOns.delete(id);
                else state.selectedAddOns.add(id);
                renderExtras();
            });
        });

        $$('[data-extra-countable]').forEach((card) => {
            const key = card.getAttribute('data-extra-countable');

            const counter = $('[data-extra-counter]', card);
            if (counter) {
                counter.addEventListener('click', (e) => {
                    const countBtn = e.target.closest('[data-count]');
                    if (countBtn) {
                        state.extraCounts[key] = parseInt(countBtn.getAttribute('data-count'), 10);
                        renderExtras();
                    }
                    e.stopPropagation();
                });
            }

            card.addEventListener('click', () => {
                const current = state.extraCounts[key] || 0;
                state.extraCounts[key] = current > 0 ? 0 : 1;
                renderExtras();
            });
        });

        const back2 = $('[data-action="back-2"]');
        if (back2) back2.addEventListener('click', () => goToStep(1));
        const next2 = $('[data-action="next-2"]');
        if (next2) next2.addEventListener('click', () => goToStep(3));
    }

    function wireStep3() {
        const cleaning = $('#bau-calc-cleaning-date');
        const today = new Date().toISOString().split('T')[0];
        if (cleaning) {
            cleaning.min = today;
            cleaning.addEventListener('change', () => {
                state.cleaningDate = cleaning.value;
                const err = $('[data-cleaning-date-error]');
                if (err) err.classList.add('hidden');
            });
        }
        const back3 = $('[data-action="back-3"]');
        if (back3) back3.addEventListener('click', () => goToStep(2));
        const next3 = $('[data-action="next-3"]');
        if (next3) next3.addEventListener('click', () => { if (validateStep3()) goToStep(4); });
    }

    function wireStep4() {
        $$('[data-payment]').forEach((btn) => {
            btn.addEventListener('click', () => {
                state.paymentMethod = btn.getAttribute('data-payment');
                $$('[data-payment]').forEach((b) => {
                    const sel = b.getAttribute('data-payment') === state.paymentMethod;
                    b.classList.toggle('border-booking-orange', sel);
                    b.classList.toggle('bg-booking-orange/10', sel);
                    b.classList.toggle('border-border', !sel);
                    b.classList.toggle('bg-card', !sel);
                });
                const err = $('[data-payment-error]');
                if (err) err.classList.add('hidden');
            });
        });

        const sameChk = $('#bau-bk-same-address');
        const objBlock = $('[data-object-address]');
        if (sameChk) {
            sameChk.addEventListener('change', () => {
                state.sameAsCustomer = sameChk.checked;
                if (objBlock) objBlock.classList.toggle('hidden', state.sameAsCustomer);
                ['bau-bk-ostreet', 'bau-bk-oplz', 'bau-bk-ocity'].forEach((id) => {
                    const el = document.getElementById(id);
                    if (el) el.required = !state.sameAsCustomer;
                });
            });
        }

        const back4 = $('[data-action="back-4"]');
        if (back4) back4.addEventListener('click', () => goToStep(3));

        const form = $('#bau-calc-booking-form');
        if (form) form.addEventListener('submit', handleBookingSubmit);
    }

    function wireProgressClicks() {
        $$('[data-progress-step], [data-progress-label]').forEach((btn) => {
            btn.addEventListener('click', () => {
                const s = parseInt(btn.getAttribute('data-progress-step') || btn.getAttribute('data-progress-label'), 10);
                if (s < state.step && state.step <= 4) goToStep(s);
            });
        });
    }

    // ===== Submission =====
    function populateHiddenInputs() {
        const setHidden = (name, value) => {
            const el = $(`[data-hidden="${name}"]`);
            if (el) el.value = value;
        };
        setHidden('PropertyType', state.propertyType);
        setHidden('RoomSize', state.selectedRoom);
        setHidden('ZipCode', state.zipCode);
        setHidden('Bauschutt', state.selectedAddOns.has('bauschutt') ? 'true' : 'false');
        setHidden('Fassade', state.selectedAddOns.has('fassade') ? 'true' : 'false');
        Object.keys(state.extraCounts).forEach((key) => {
            setHidden(COUNTABLE_FIELD[key], String(state.extraCounts[key] || 0));
        });
        setHidden('CleaningDate', state.cleaningDate || '');
        setHidden('QuotedTotal', String(computeTotal()));
        setHidden('PaymentMethod', state.paymentMethod);
        setHidden('SameAsCustomer', state.sameAsCustomer ? 'true' : 'false');
    }

    async function handleBookingSubmit(e) {
        e.preventDefault();
        const errEl = $('[data-submit-error]');
        if (errEl) errEl.classList.add('hidden');

        if (!state.paymentMethod) {
            const pe = $('[data-payment-error]');
            if (pe) pe.classList.remove('hidden');
            return;
        }

        const form = e.target;
        const submitBtn = form.querySelector('[data-action="submit"]');

        populateHiddenInputs();

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        if (submitBtn) {
            submitBtn.disabled = true;
            submitBtn.textContent = 'Wird gesendet …';
        }

        try {
            const fd = new FormData(form);
            const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            const headers = {};
            if (tokenInput) headers['RequestVerificationToken'] = tokenInput.value;

            const res = await fetch(form.action, { method: 'POST', body: fd, headers });
            const data = await res.json().catch(() => null);

            if (!res.ok || !data || !data.ok) {
                const msg = (data && data.error) ? data.error : 'Buchung konnte nicht gesendet werden. Bitte versuchen Sie es erneut oder rufen Sie uns an.';
                if (errEl) { errEl.textContent = msg; errEl.classList.remove('hidden'); }
                if (submitBtn) { submitBtn.disabled = false; submitBtn.textContent = 'Buchung bestätigen ✓'; }
                return;
            }

            const idEl = $('[data-success-id]');
            const dateEl = $('[data-success-date]');
            const totalEl = $('[data-success-total]');
            if (idEl) idEl.textContent = data.bookingId || '';
            if (dateEl) dateEl.textContent = data.cleaningDate ? formatDateDE(data.cleaningDate) : '';
            if (totalEl) totalEl.textContent = String(data.total || computeTotal());
            state.step = 5;
            renderAll();
        } catch (err) {
            if (errEl) {
                errEl.textContent = 'Netzwerkfehler. Bitte versuchen Sie es erneut oder rufen Sie uns an.';
                errEl.classList.remove('hidden');
            }
            if (submitBtn) { submitBtn.disabled = false; submitBtn.textContent = 'Buchung bestätigen ✓'; }
        }
    }

    // ===== Bootstrap =====
    function bootstrap() {
        // Pre-fill from URL (?zip=X&room=Y) — matches the React initialZipCode/initialRoomSize behavior
        const params = new URLSearchParams(window.location.search);
        const zipFromUrl = (params.get('zip') || '').replace(/\D/g, '').slice(0, 4);
        const roomFromUrl = params.get('room') || '';

        if (zipFromUrl) {
            state.zipCode = zipFromUrl;
            const zipInput = $('#bau-calc-zip');
            if (zipInput) zipInput.value = zipFromUrl;
        }

        if (roomFromUrl && APARTMENT_PRICES[roomFromUrl]) {
            state.selectedRoom = roomFromUrl;
            const defaults = defaultBathMapping(roomFromUrl);
            state.extraCounts.bath = defaults.bath > 1 ? defaults.bath - 1 : 0;
            state.extraCounts.wc = defaults.wc;
        }

        wireStep1();
        wireStep2();
        wireStep3();
        wireStep4();
        wireProgressClicks();

        const zipValid = state.zipCode.length === 4 &&
            (typeof window.isValidZurichPlz !== 'function' || window.isValidZurichPlz(state.zipCode));
        if (zipValid && state.selectedRoom) {
            state.step = 2;
        }

        renderAll();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', bootstrap);
    } else {
        bootstrap();
    }
})();
