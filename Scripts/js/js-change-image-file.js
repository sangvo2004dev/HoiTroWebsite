document.addEventListener('DOMContentLoaded', function () {
    const imageInputs = document.querySelectorAll('.js-change-image-file');
    imageInputs.forEach(input => {
        input.addEventListener('change', function () {
            const file = input.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const previewDiv = input.closest('.js-one-image-wrapper').querySelector('.js-one-image-preview');
                    previewDiv.style.backgroundImage = `url(${e.target.result})`;
                };
                reader.readAsDataURL(file);
            }
        });
    });
});
