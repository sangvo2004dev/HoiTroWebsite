let manage_post;
manage_post = manage_post || {};
manage_post = {
    form: (function () { return $('form'); })(),
    base_url: '',
    url_upload_image: '/api/upload',
    chon_anh: '.chon-anh',

    uploadImage: function () {
        if (typeof Dropzone === 'undefined') return;
        if ($('.js-dropzone').length == 0) return;
        // INIT DROPZONE
        let self = this;
        let btn_submit = $(".js-form-submit-data").find('button[type="submit"]');
        let btn_chon_anh = $('.js-btn-chon-anh');
        let btn_chon_anh_text = btn_chon_anh.html();
        let dropzoneOptions = {
            // url: "https://static123.com/api/upload",
            url: self.url_upload_image,
            acceptedFiles: ".jpg,.jpeg,.png",
            maxFilesize: 10,
            maxFiles: 20,
            previewsContainer: '#list-photos-dropzone-previews',
            previewTemplate: $('#tpl').length > 0 ? document.querySelector('#tpl').innerHTML : '',
            dictRemoveFileConfirmation: "Bạn chắc chắn muốn xóa hình ảnh này?"
        };

        // console.log(Dropzone.instances.length);

        let myDropzone;
        if (Dropzone.instances.length === 0) {
            myDropzone = new Dropzone(this.chon_anh, dropzoneOptions);
        } else {
            // Nếu đã có Dropzone, hủy và tạo lại (nếu cần)
            Dropzone.instances.forEach(function (dropzone) {
                dropzone.destroy();
            });
            myDropzone = new Dropzone(this.chon_anh, dropzoneOptions);
        }

        var _window_with = $(document).width();
        var _action = 'click';
        if (_window_with <= 480) {
            _action = 'touchstart tap';
        }

        $('#list-photos-dropzone-previews').sortable();
        // Bind manual remove
        $('#list-photos-dropzone-previews').on(_action, '.js-photo-manual .photo_delete', function (event) {
            event.preventDefault();

            let _self = $(this);
            console.log(myDropzone.options.dictRemoveFileConfirmation);
            if (myDropzone.options.dictRemoveFileConfirmation) {
                return Dropzone.confirm(myDropzone.options.dictRemoveFileConfirmation, function () {
                    const preview_element = $(_self).closest('.js-photo-manual');
                    const file_name = preview_element.find('input[name ="file_name_list"]').val();
                    const input_delete = $(`<input type="hidden" name="file_delete_list" value="${file_name}">`);
                    console.log(file_name)

                    if (preview_element.find('input').hasClass('js-not-temp')) {
                        $('#list-photos-dropzone-previews').append(input_delete);
                    };
                    _self.closest('.js-photo-manual').remove();
                });
            } else {
                console.log('here2');
                $(this).closest('.js-photo-manual').remove();
            }
        });

        myDropzone.on('removedfile', function (file) {
            const preview_element = file.previewElement;
            const file_name = $(preview_element).find('input').val();
            console.log(file_name);
            $.ajax({
                url: '/api/delete',
                type: 'POST',
                data: { fileName: file_name },
                //success: function (response, status) {
                //    console.log(status, response);
                //    if (typeof fnAccepted === 'function') {
                //        fnAccepted();
                //    }
                //},
                error: () => {
                    Swal.fire('Lỗi', 'Vui lòng thử lại sau', 'error');
                    return;
                }
            });
        });

        myDropzone.on('error', function (file, message) {
            console.log(file, message);
            if (file.size > myDropzone.options.maxFilesize * 1024 * 1024) {
                Swal.fire('Lỗi', 'Video tối đa được upload ' + myDropzone.options.maxFilesize + 'MB', 'error');
                myDropzone.removeFile(file);
                return;
            }
            Swal.fire('Lỗi', message, 'error');
            myDropzone.removeFile(file);
        });

        myDropzone.on('sending', function (file, xhr, formData) {
            //console.log(file);
            // formData.append('source', 'phongtro123');
            // formData.append('source_url', window.location.href);
            // formData.append('from', 'dangtin');
            btn_submit.addClass('disabled');
            btn_chon_anh.html('Đang đăng hình...');
        });

        myDropzone.on("addedfile", function (file, response) {
            if ($('.js-photo-manual').length > 20) {
                Swal.fire('Lỗi', 'Bạn chỉ được đăng tải 20 hình ảnh. Vui lòng xóa để đăng lại', 'error');
                myDropzone.removeFile(file);
                return;
            }
        });

        myDropzone.on("success", function (file, response) {
            console.log(response.success, response.file_name);
            let el_preview = $(file.previewElement);
            let input_file = $('<input type="hidden" class="js-photo-preview-temp" name="file_name_list" value="' + response.file_name + '"/>');
            el_preview.append(input_file);
        });

        myDropzone.on("complete", function (file) {
            btn_submit.removeClass('disabled');
            btn_chon_anh.html(btn_chon_anh_text);
        });
    },
    deleteTempImages: function () {
        $(window).on('beforeunload', function (e) {

            let list_file_delete = [];
            $('#list-photos-dropzone-previews').find('.js-photo-preview-temp').each(function (i, element) {
                list_file_delete.push($(element).val());
            });
            console.log(list_file_delete);
            if (list_file_delete.length > 0) {
                navigator.sendBeacon('/api/delete-multiple', JSON.stringify({ list_file_delete }));
            }
            //e.returnValue = "Bạn có chắc chắn muốn rời khỏi trang?";
        });
    },

    dangTin: function () {
        let self = this;
        $(this.form).submit(() => {
            // xóa sự kiện beforereunload
            $(window).off('beforeunload');

            //phần editpost
            let file_delete_list = [];
            $('#list-photos-dropzone-previews').find('input[name="file_delete_list"]').each(function (i, element) {
                file_delete_list.push($(element).val());
            });

            console.log(file_delete_list);
            if (file_delete_list.length > 0 && $(self.form).valid()) {
                navigator.sendBeacon('/api/delete-multiple', JSON.stringify({ file_delete_list }));
            }

            // đặt lại sự kiện
            setTimeout(() => { self.deleteTempImages(); }, 500);
        });
    },

    init: function () {
        this.uploadImage();
        this.deleteTempImages();
        this.dangTin();       
    }
}

$(document).ready(() => {
    manage_post.init();
    manage_post.base_url = window.location.origin;
})
