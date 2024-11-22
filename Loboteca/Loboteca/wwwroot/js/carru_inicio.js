let currentIndex = 0;
const images = document.querySelectorAll('.carousel-image');

function showImage(index) {
    images.forEach((img, i) => {
        img.style.display = i === index ? 'block' : 'none';
    });
}

function moveCarousel(direction) {
    currentIndex = (currentIndex + direction + images.length) % images.length;
    showImage(currentIndex);
}

// Inicializa el carrusel mostrando la primera imagen
showImage(currentIndex);
