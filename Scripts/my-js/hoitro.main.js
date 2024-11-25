let user_function;
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

$(document).ready(function () {
    user_function.init();
});