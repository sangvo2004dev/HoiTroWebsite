let filter;
filter = filter || {};
filter = {
    gia_tien: '#gia_tien',
    dien_tich: '#dien_tich',
    area_tag: '.js-area-tag',
    form_tim_kiem: '#form-tim-kiem',
    searchValues: {},

    createRangeSelect: function () {
        // tạo slider cho giá tiền
        $('.js-price-slider').slider({
            range: true,
            min: 0,
            max: 15,
            // step: 1,
            // values: [0, 1],
            animate: 500,
            // start: function () {},
            // slide: function() {},
            create: function () {
                $(this).find('span:nth-of-type(1)').append('<span class="js-left-tooltip tooltip text-white fs-9">0</span>');
                $(this).find('span:nth-of-type(2)').append('<span class="js-right-tooltip tooltip text-white fs-9">0</span>');
                $('.ui-slider-handle').addClass('slider-handle-custom');
                $('.ui-slider-range').addClass('slider-range-custom');
                $(this).data('left_tooltip', '.js-left-tooltip');
                $(this).data('right_tooltip', '.js-right-tooltip');
            },
            start: function (event, ui) {
                $(ui.handle).addClass('slider-handle-active');
            },
            slide: function (event, ui) {
                $(this).find($(this).data('left_tooltip')).text(ui.values[0]).addClass('opacity-100');
                $(this).find($(this).data('right_tooltip')).text(ui.values[1]).addClass('opacity-100');
            },
            stop: function (event, ui) {
                $(ui.handle).removeClass('slider-handle-active');
                let values = $(this).slider('values');
                const $hidden_price_input = $('#price_range');
                $hidden_price_input.attr('data-min-value', values[0]);
                $hidden_price_input.attr('data-max-value', values[1]);
                $hidden_price_input.prop('checked', true);

                // console.log($(this));
                // console.log(values, $(this).siblings('.js-min-value'), $(this).siblings('.js-max-value'));
                $(this).siblings('.js-min-value').html(values[0]);
                $(this).siblings('.js-max-value').html(values[1]);
                $(this).find($(this).data('left_tooltip')).text(ui.values[0]).removeClass('opacity-100');
                $(this).find($(this).data('right_tooltip')).text(ui.values[1]).removeClass('opacity-100');
            },
        });

        // tạo slider cho diện tích
        $(this.dien_tich).slider({
            range: true,
            min: 0,
            max: 99,
            animate: 500,
            create: function () {
                $(this).find('span:nth-of-type(1)').append('<span class="js-left-tooltip tooltip text-white fs-9">0</span>');
                $(this).find('span:nth-of-type(2)').append('<span class="js-right-tooltip tooltip text-white fs-9">0</span>');
                $('.ui-slider-handle').addClass('slider-handle-custom');
                $('.ui-slider-range').addClass('slider-range-custom');
                $(this).data('left_tooltip', '.js-left-tooltip');
                $(this).data('right_tooltip', '.js-right-tooltip');
            },
            start: function (event, ui) {
                $(ui.handle).addClass('slider-handle-active');
            },
            slide: function (event, ui) {
                $(this).find($(this).data('left_tooltip')).text(ui.values[0]).addClass('opacity-100');
                $(this).find($(this).data('right_tooltip')).text(ui.values[1]).addClass('opacity-100');
            },
            stop: function (event, ui) {
                $(ui.handle).removeClass('slider-handle-active');
                let values = $(this).slider('values');
                const $hidden_area_input = $('#area_range');
                $hidden_area_input.attr('data-min-value', values[0]).attr('data-max-value', values[1]).prop('checked', true);
                // console.log($(this));
                // console.log(values, $(this).siblings('.js-min-value'), $(this).siblings('.js-max-value'));
                $(this).siblings('.js-min-value').html(values[0]);
                $(this).siblings('.js-max-value').html(values[1]);
                $(this).find($(this).data('left_tooltip')).text(ui.values[0]).removeClass('opacity-100');
                $(this).find($(this).data('right_tooltip')).text(ui.values[1]).removeClass('opacity-100');
            },
        });
    },
    choosePrice: function () {
        let self = this;
        $('.js-price-tag').change(function (e) {
            // console.log(this);
            let $input = $(e.target);
            if ($input.prop('checked') === true) {
                let min = $input.attr('data-min-value');
                let max = $input.attr('data-max-value');
                let temp_max = max;

                $(self.dien_tich).siblings('.js-min-value').text(min);
                $(self.dien_tich).siblings('.js-max-value').text(max);

                if (min !== '' && max === '') {
                    temp_max = $(self.gia_tien).slider('option', 'max');
                }
                min = min === '' ? 0 : min;
                temp_max = temp_max === '' ? 0 : temp_max;

                // console.log(min, max)

                // Thay đổi giá trị bằng $.slider('option', 'values')
                $(self.gia_tien).slider('option', 'values', [min, temp_max]);
                $(self.gia_tien).slider("option", "slide").call($(self.gia_tien)[0], null, { values: [min, temp_max] });
            }
        });
    },
    chooseArea: function () {
        let self = this;
        // $('lable.js-area-tag').on('click', () => {console.log('label')})
        $('.js-area-tag').change(function (e) {
            // console.log(this);
            let input = e.target;
            if ($(input).prop('checked') === true) {
                let min = $(input).attr('data-min-value');
                let max = $(input).attr('data-max-value');
                let temp_max = max;

                $(self.dien_tich).siblings('.js-min-value').text(min);
                $(self.dien_tich).siblings('.js-max-value').text(max);

                if (min !== '' && max === '') {
                    temp_max = $(self.dien_tich).slider('option', 'max');
                }
                min = min === '' ? 0 : min;
                temp_max = temp_max === '' ? 0 : temp_max;

                // console.log(min, max)

                // Thay đổi giá trị bằng $.slider('option', 'values')
                $(self.dien_tich).slider('option', 'values', [min, temp_max]);
                $(self.dien_tich).slider("option", "slide").call($(self.dien_tich)[0], null, { values: [min, temp_max] });
                // console.log($(self.dien_tich).siblings('.js-min-value').text());
            }
        });
    },

    getUrl: function (searchValues) {
        let params = new URLSearchParams();
        this.queryString = {};
        if (searchValues === undefined) return;
        let url = '';
        let danh_muc = $('input[name="danh_muc"]:checked').data('meta') === undefined ? 'phong-tro-cho-thue' : $('input[name="danh_muc"]:checked').data('meta');
        url += '/tim-kiem-' + danh_muc;
        // console.log(select2TinhThanh.province_select);
        if (searchValues.tinhthanhpho !== 'all') {
            url += '/dia-chi' + '/' + $(select2TinhThanh.province_select).find('option:selected').data('slug');
        }
        if (searchValues.quanhuyen !== 'all') {
            url += '/' + $(select2TinhThanh.district_select).find('option:selected').data('slug');
        }
        if (searchValues.phuongxa !== 'all') {
            url += '/' + $(select2TinhThanh.ward_select).find('option:selected').data('slug');
        }
        if (searchValues.min_price != 'all') {
            params.append('gia_tu', searchValues.min_price * 1000000);
        }
        if (searchValues.max_price != 'all') {
            params.append('gia_den', searchValues.max_price * 1000000);
        }
        if (searchValues.min_area != 'all') {
            params.append('dien_tich_tu', searchValues.min_area);
        }
        if (searchValues.max_area != 'all') {
            params.append('dien_tich_den', searchValues.max_area);
        }
        return url + '?' + params.toString();
    },

    clickBtnApDung: function () {
        let self = this;
        $(this.form_tim_kiem).on('submit', function (e) {
            e.preventDefault();
            // const formData = $(this).serialize();
            // console.dir(formData);
            const searchData = {
                'danh_muc': $(this).find('input[name="danh_muc"]:checked').val(),
                'tinhthanhpho': $(this).find('select[name="tinhthanhpho"]').val() == '' ? 'all' : $(this).find('select[name="tinhthanhpho"] option:selected').text(),
                'quanhuyen': $(this).find('select[name="quanhuyen"]').val() == '' ? 'all' : $(this).find('select[name="quanhuyen"] option:selected').text(),
                'phuongxa': $(this).find('select[name="phuongxa"]').val() == '' ? 'all' : $(this).find('select[name="phuongxa"] option:selected').text(),
                'min_price': $(this).find('input[name="price"]:checked').attr('data-min-value') == '' ? 'all' : $(this).find('input[name="price"]:checked').attr('data-min-value'),
                'max_price': $(this).find('input[name="price"]:checked').attr('data-max-value') == '' ? 'all' : $(this).find('input[name="price"]:checked').attr('data-max-value'),
                'min_area': $(this).find('input[name="area"]:checked').attr('data-min-value') == '' ? 'all' : $(this).find('input[name="area"]:checked').attr('data-min-value'),
                'max_area': $(this).find('input[name="area"]:checked').attr('data-max-value') == '' ? 'all' : $(this).find('input[name="area"]:checked').attr('data-max-value'),
            };
            console.log('Dữ liệu tìm kiếm');
            console.dir(searchData);
            const url = self.getUrl(searchData);
            console.log(url);
            window.location.href = url;
        });
        $('.js-btn-submit').click(function () {
            $(self.form_tim_kiem).submit();
        });
    },

    init: function () {
        this.createRangeSelect();
        this.chooseArea();
        this.choosePrice();
        this.clickBtnApDung();
        // this.getUrl();
    }
}

$(document).ready(function () {
    filter.init();
});