document.addEventListener('DOMContentLoaded', function () {
    const inners = document.querySelectorAll('.js-one-image-inner');
    inners.forEach(inner => {
        // Thêm các xử lý hoặc hiệu ứng bên trong hình ảnh
        inner.style.transition = 'all 0.3s ease';
    });
});