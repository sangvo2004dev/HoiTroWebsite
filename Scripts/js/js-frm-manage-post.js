$(document).ready(function() {
    // Quản lý việc submit form đăng bài (post)
    $('.js-frm-manage-post').on('submit', function(e) {
        e.preventDefault();  // Ngăn reload trang

        var $form = $(this);  // Form hiện tại
        var actionUrl = $form.data('action-url');  // URL API để gửi dữ liệu
        var formData = new FormData(this);  // Tạo đối tượng FormData để gửi dữ liệu

        $.ajax({
            url: actionUrl,  // API URL
            type: 'POST',
            data: formData,
            contentType: false,  // Không thay đổi Content-Type
            processData: false,  // Không xử lý thành query string
            success: function(response) {
                // Hiển thị thông báo hoặc chuyển hướng sau khi đăng thành công
                window.location.href = "/trang-chu";  // Chuyển hướng sau khi thành công
            },
            error: function(xhr, status, error) {
                // Thông báo khi gặp lỗi
                alert('Có lỗi xảy ra, vui lòng thử lại!');
            }
        });
    });
});
