document.addEventListener('DOMContentLoaded', function () {
    const hiddenInputs = document.querySelectorAll('.js-input-value');
    hiddenInputs.forEach(input => {
        // Khi cần, có thể lấy giá trị của input này để lưu vào cơ sở dữ liệu
        console.log('Input value:', input.value);
    });
});
