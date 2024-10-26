document.addEventListener('DOMContentLoaded', function () {
    const removeButtons = document.querySelectorAll('.js-remove-one-image');
    removeButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const wrapper = button.closest('.js-one-image-wrapper');
            const previewDiv = wrapper.querySelector('.js-one-image-preview');
            const hiddenInput = wrapper.querySelector('.js-input-value');
            const imageInput = wrapper.querySelector('.js-change-image-file');

            // Reset về hình ảnh mặc định
            previewDiv.style.backgroundImage = 'url("/Content/images/default-user.jpg")';
            hiddenInput.value = '';
            imageInput.value = '';
            button.style.display = 'none';
        });
    });
});