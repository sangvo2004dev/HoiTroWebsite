$(document).ready(function() {
    // Khi người dùng submit form
    $('.js-form-submit-data').on('submit', function(e) {
        e.preventDefault();  // Ngăn chặn việc reload trang

        var $form = $(this);  // Lấy form hiện tại
        var actionUrl = $form.data('action-url');  // Lấy URL từ thuộc tính data-action-url
        var formData = new FormData(this);  // Tạo FormData để gửi dữ liệu

        $.ajax({
            url: actionUrl,  // URL từ data-action-url
            type: 'POST',
            data: formData,
            contentType: false,  // Không đặt Content-Type, mặc định FormData sẽ đặt
            processData: false,  // Không xử lý dữ liệu thành chuỗi query string
            success: function(response) {
                // Xử lý khi gửi form thành công
                alert('Form submitted successfully!');
                console.log(response);  // In ra phản hồi từ server
            },
            error: function(xhr, status, error) {
                // Xử lý lỗi
                alert('Error submitting form');
                console.log(xhr.responseText);  // In ra lỗi
            }
        });
    });
});
