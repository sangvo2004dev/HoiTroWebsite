let user_function, login_logout;
const baseurl = '';
user_function = user_function || {};
user_function = {
    btn_dang_hien_thi: '.js-dang-hien-thi',
    btn_tin_an: '.js-tin-an',

    manage_post_function: function () {
        $(this.btn_dang_hien_thi).click(function () {
            $(document).ready(function () {
                alert('Tai xong dom');
            });
        });
    },

    init: function () {

    }
}

login_logout = login_logout || {}
login_logout = {
    form_dang_ky: '#from-dang-ky',
    form_dang_nhap: '#from-dang-nhap',

    logicLoginLogout: function () {
        const self = this;
        $(this.form_dang_ky).on('submit', function (e) {
            //console.log('passed1');
            e.preventDefault();
            if (!$(self.form_dang_ky).valid()) return;
            //console.log('passed2');
            const formData = $(self.form_dang_ky).serializeArray();
            $.ajax({
                url: baseurl + '/dang-ky',
                type: 'POST',
                data: formData,
                success: function (response) {
                    console.log(response)
                    Swal.fire({ title: 'Thành công', text: 'Tạo tài khoản thành công', icon: 'success', timer: 2000, showConfirmButton: false, timerProgressBar: true })
                        .then(function () {
                            window.location.href = response.redirectUrl;
                        });
                },
                error: function (xhr, status, error) {
                    const response = xhr.responseJSON;
                    const message = response.message;
                    //console.log(message, xhr);
                    Swal.fire('Lỗi', message, 'error');
                },
            });
        });

        $(this.form_dang_nhap).on('submit', function (e) {
            console.log('passed1');
            e.preventDefault();
            if (!$(self.form_dang_nhap).valid()) return;
            console.log('passed2');
            const formData = $(self.form_dang_nhap).serializeArray();
            $.ajax({
                url: baseurl + '/dang-nhap',
                type: 'POST',
                data: formData,
                success: function (response) {
                    console.log(response)
                    Swal.fire({ title: 'Thành công', text: 'Đăng nhập thành công', icon: 'success', timer: 2000, showConfirmButton: false, timerProgressBar: true })
                        .then(function () {
                            window.location.href = response.redirectUrl;
                        });
                },
                error: function (xhr, status, error) {
                    const response = xhr.responseJSON;
                    const message = response.message;
                    //console.log(message, xhr);
                    Swal.fire('Lỗi', message, 'error');
                },
            });
        });
    },
    init: function () {
        this.logicLoginLogout();
    }
}

$(document).ready(function () {
    user_function.init();
    login_logout.init();
});