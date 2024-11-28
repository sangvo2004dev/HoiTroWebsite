let manage_post;
manage_post = manage_post || {};
manage_post = {
    form: (function () { return $('form'); })(),
    url_upload_image: '/api/upload',
    chon_anh: '.chon-anh',
    list_post: '.js-list-post',
    paging: '.js-paging',
    baseurl: '',
    tab_all_an_duyet: '#my_tab_post',
    truong_data: {
        chonDaAn: false,
        chonDaDuyet: false,
    },

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
                data: { file_name: file_name },
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

    loadPost: function (page, chonDaAn, chonDaDuyet) {
        console.log('load post');
        page = page || 1;
        //chonDaAn = chonDaDuyet && true;
        //chonDaDuyet = chonDaDuyet && true;

        let self = this;
        $.ajax({
            url: this.baseurl + '/api/post/get-all',
            type: 'POST',
            data: {
                pg: page,
                'chonDaAn': chonDaAn,
                'chonDaDuyet': chonDaDuyet
            },
            success: function (data, status) {
                //console.log(data);
                $(self.list_post).html(data.list_post);
                $(self.paging).html(data.paging);
                console.log('ok');
                return true;
            },

            error: function (error) {
                console.log(error);
                return false;
            },
        });
    },

    togglePage: function () {
        let self = this;
        $(document).on('click', this.paging + ' ' + '.page-item a.page-link', function () {
            let page = $(this).attr('asp-route-pg');
            self.loadPost(pg = page, chonDaAn = self.truong_data['chonDaAn'], chonDaDuyet = self.truong_data['chonDaDuyet']);
        })
    },

    switchTab: function (action = '') {
        let self = this;
        Object.keys(self.truong_data).forEach(key => { self.truong_data[key] = false; });
        self.truong_data[action] = true;
        //console.log(self.truong_data);
        self.loadPost(pg = 1, chonDaAn = self.truong_data['chonDaAn'], chonDaDuyet = self.truong_data['chonDaDuyet']);
    },

    clickTab: function () {
        let self = this;
        $(this.tab_all_an_duyet + ' a').on('shown.bs.tab', function () {
            let action = $(this).attr('truong_data');
            self.switchTab(action);
        });
    },

    actionLoad: function () {
        let self = this;
        const action = new URLSearchParams(window.location.search).get('action');
        if (!action) {
            //self.loadPost();
            return;
        }
        $(document).ready(function () {
            $(self.tab_all_an_duyet).find(`a[truong_data="${action}"]`).tab('show');
        });
    },

    init: function () {
        this.uploadImage();
        this.deleteTempImages();
        this.dangTin();
        //this.loadPost();
        this.togglePage();
        this.clickTab();
        this.actionLoad();
    }
}

$(document).ready(() => {
    manage_post.init();
})
