document.addEventListener('DOMContentLoaded', function () {
    const previews = document.querySelectorAll('.js-one-image-preview');
    previews.forEach(preview => {
        preview.style.backgroundSize = 'cover';
        preview.style.backgroundPosition = 'center';
    });
});