var QL_bai_dang = QL_bai_dang || {};
QL_bai_dang = {
    base_url: '',
    list_post: '.js-list-post',
    paging: '.js-paging',
    tab_all_an_duyet: '#my_tab_post',
    btn_xoa_post: '.js-delete-post',
    btn_an_tin: '.js-hide-tin',
    btn_hien_tin: '.js-show-tin',
    truong_data: {
        chonDaAn: false,
        chonDaDuyet: false,
    },

    loadPost: function (page, chonDaAn, chonDaDuyet) {
        console.log('load post');
        page = page || 1;
        //chonDaAn = chonDaDuyet && true;
        //chonDaDuyet = chonDaDuyet && true;

        let self = this;
        $.ajax({
            url: self.base_url + '/api/post/get-all',
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

    xoaBaiDang: function () {
        const self = this;
        $(document).on('click', this.btn_xoa_post, function () {
            const idPost = $(this).data('id');
            const $btn_xoa = $(this);
            Swal.fire({
                title: 'Bạn có chắc!',
                text: `Xóa bài đăng có Mã tin: ${idPost}`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                cancelButtonText: 'Thoát',
                confirmButtonText: "Có, xóa!"
            }).then(function (result) {
                if (result.isConfirmed) {
                    $.ajax({
                        url: self.base_url + '/api/post/xoa-tin',
                        type: 'GET',
                        data: { id: idPost },
                        success: function (data) {
                            console.log('đã xóa');
                            //Swal.fire('Đã xóa', '', 'success').then(function () {
                            //    $btn_xoa.closest('li').remove();
                            //});
                            $btn_xoa.closest('content').remove();

                        },
                        error: function (xhr, status, error) {
                            const response = xhr.responseJSON;
                            Swal.fire('Lỗi!', response.message, 'error');
                        }
                    });
                }
            });
        });
    },

    anTin: function () {
        const self = this;
        $(document).on('click', this.btn_an_tin, function () {
            const idPost = $(this).data('id');
            const $btn_an = $(this);
            Swal.fire({
                title: 'Bạn có chắc!',
                text: `Ẩn bài đăng có Mã tin: ${idPost}`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                cancelButtonText: 'Thoát',
                confirmButtonText: "Có, ẩn!"
            }).then(function (result) {
                if (result.isConfirmed) {
                    $.ajax({
                        url: self.base_url + '/api/post/an-hien',
                        type: 'GET',
                        data: { id: idPost, hanhDong: 'an-tin' },
                        success: function (data) {
                            console.log(data);
                            const $newButton = $(data.button);
                            const $newSpan = $(data.span);
                            $btn_an.closest('content').find('span[user-hide]').before($newSpan).remove();
                            $btn_an.before($newButton).remove();
                            console.log('đã ẩn');

                        },
                        error: function (xhr, status, error) {
                            const response = xhr.responseJSON;
                            //Swal.fire('Lỗi!', response.message, 'error');
                            console.log(response);
                        }
                    });
                }
            });
        });
    },

    hienTin: function () {
        const self = this;
        $(document).on('click', this.btn_hien_tin, function () {
            const idPost = $(this).data('id');
            const $btn_hien = $(this);
            $.ajax({
                url: self.base_url + '/api/post/an-hien',
                type: 'GET',
                data: { id: idPost, hanhDong: 'hien-tin' },
                success: function (data) {
                    console.log(data);
                    const $newButton = $(data.button);
                    const $newSpan = $(data.span);
                    $btn_hien.closest('content').find('span[user-hide]').before($newSpan).remove();
                    $btn_hien.before($newButton).remove();
                    console.log('đã hien');
                },
                error: function (xhr, status, error) {
                    const response = xhr.responseJSON;
                    //Swal.fire('Lỗi!', response.message, 'error');
                    console.log(response);
                }
            });
        });
    },


    init: function () {
        this.togglePage();
        this.clickTab();
        this.actionLoad();
        this.xoaBaiDang();
        this.anTin();
        this.hienTin();
    }
}

$(document).ready(function () {
    QL_bai_dang.init();
    QL_bai_dang.base_url = window.location.origin;
});