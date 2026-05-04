// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// ======================================================
// REVEAL ON SCROLL (IntersectionObserver)
// ======================================================
(function () {
    const obs = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting) {
                e.target.classList.add('is-visible');
                obs.unobserve(e.target);
            }
        });
    }, { threshold: 0.15 });

    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('.reveal').forEach(el => obs.observe(el));
    });
})();

// ======================================================
// IMÁGENES (lazy + decode + fade-in)
// ======================================================
(function () {
    document.addEventListener('DOMContentLoaded', () => {
        const imgs = document.querySelectorAll('img[loading="lazy"]');
        imgs.forEach(img => {
            img.style.opacity = 0;
            const onReady = () => {
                img.style.transition = 'opacity 300ms ease';
                img.style.opacity = 1;
                img.classList.remove('img-skeleton');
            };
            if (img.complete) onReady();
            else {
                img.addEventListener('load', onReady, { once: true });
                img.addEventListener('error', () => img.classList.remove('img-skeleton'), { once: true });
            }
            img.classList.add('img-skeleton');
            img.decoding = 'async';
        });
    });
})();

// ======================================================
// CARRITO (feedback + contador)
// ======================================================
window.cart = {
    add(product) {
        const carrito = JSON.parse(sessionStorage.getItem('carrito')) || [];
        carrito.push(product);
        sessionStorage.setItem('carrito', JSON.stringify(carrito));
        window.cart.updateBadge();
        window.cart.toast(`Añadido: ${product.nombre}`);
    },
    updateBadge() {
        const carrito = JSON.parse(sessionStorage.getItem('carrito')) || [];
        const el = document.getElementById('contadorCarrito');
        if (!el) return;
        el.innerText = carrito.length;
        el.style.display = carrito.length ? 'inline-block' : 'none';
    },
    toast(msg) {
        let t = document.getElementById('cartToast');
        if (!t) {
            t = document.createElement('div');
            t.id = 'cartToast';
            t.className = 'cart-toast';
            document.body.appendChild(t);
        }
        t.textContent = msg;
        t.classList.add('show');
        clearTimeout(t._hide);
        t._hide = setTimeout(() => t.classList.remove('show'), 1600);
    }
};

document.addEventListener('DOMContentLoaded', () => window.cart.updateBadge());