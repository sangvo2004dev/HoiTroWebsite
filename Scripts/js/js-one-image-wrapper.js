document.addEventListener('DOMContentLoaded', function () {
    const wrappers = document.querySelectorAll('.js-one-image-wrapper');
    wrappers.forEach(wrapper => {
        const previewDiv = wrapper.querySelector('.js-one-image-preview');
        const imageInput = wrapper.querySelector('.js-change-image-file');
        const hiddenInput = wrapper.querySelector('.js-input-value');
        const removeButton = wrapper.querySelector('.js-remove-one-image');

        if (imageInput && previewDiv) {
            // Tạo sự kiện khi người dùng thay đổi file
            imageInput.addEventListener('change', function () {
                const file = imageInput.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        previewDiv.style.backgroundImage = `url(${e.target.result})`;
                        hiddenInput.value = e.target.result;
                        removeButton.style.display = 'inline-block';
                    };
                    reader.readAsDataURL(file);
                }
            });
        }
    });
});