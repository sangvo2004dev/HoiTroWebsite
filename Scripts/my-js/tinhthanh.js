var select2TinhThanh;
select2TinhThanh = select2TinhThanh || {};
select2TinhThanh = {
    province_select: '.js-select-province',
    district_select: '.js-select-district',
    ward_select: '.js-select-ward',
    myoffcanvas: '#myoffcanvas',
    dataQuanHuyen: {},
    dataPhuongXa: {},

    createSelect2: function () {
        const self = this;

        const $parent = $(this.province_select).closest('.row');
        $(`${this.province_select}, ${this.district_select}, ${this.ward_select}`).select2({ dropdownParent: $parent });

        // sự kiện chọn tỉnh thành phố
        $(this.province_select).on('change', function () {
            const quanhuyen_id = $(this).val();
            console.log(quanhuyen_id);
            let dataD;

            $(self.district_select).find('option:not(:first-child)').remove();
            $(self.district_select).find('option:first').prop('selected', true).trigger('change');

            if (quanhuyen_id === '' || quanhuyen_id === undefined) {
                console.log('da chon tinhthanhpho')
                return;
            }
            console.dir(self.dataQuanHuyen[`${quanhuyen_id}`]);
            if (self.dataQuanHuyen[`${quanhuyen_id}`] === undefined) {
                $.ajax({
                    type: 'GET',
                    url: `/Hanhchinh_VietNam/quan-huyen/${quanhuyen_id}.json`
                }).then(function (data) {
                    dataD = Object.entries(data).sort((item1, item2) => item1[1].name_with_type.localeCompare(item2[1].name_with_type))
                        .map(item => { let newOption = new Option(item[1].name_with_type, item[1].code, false, false); newOption.dataset.slug = item[1].type + '-' + item[1].slug; return newOption; });
                    $(self.district_select).append(dataD).trigger('change');

                    self.dataQuanHuyen[`${quanhuyen_id}`] = dataD;
                    console.log('done quan huyen');
                });
            }
            else {
                $(self.district_select).append(self.dataQuanHuyen[`${quanhuyen_id}`]).trigger('change');
            }
            console.log('da chon tinhthanhpho')
        });

        // sự kiện chọn quanhuyen
        $(this.district_select).on('change', function () {
            const phuongxa_id = $(this).val();
            console.log(phuongxa_id);
            let dataPX;

            $(self.ward_select).find('option:not(:first-child)').remove().trigger('change');
            if (phuongxa_id === '' || phuongxa_id === undefined) {
                console.log('da chon quanhuyen');
                return;
            }
            console.dir(self.dataPhuongXa[`${phuongxa_id}`]);
            if (self.dataPhuongXa[`${phuongxa_id}`] === undefined) {
                $.ajax({
                    type: 'GET',
                    url: `/Hanhchinh_VietNam/xa-phuong/${phuongxa_id}.json`,
                }).then(function (data) {
                    dataPX = Object.entries(data).sort((item1, item2) => item1[1].name_with_type.localeCompare(item2[1].name_with_type))
                        .map(item => { let newOption = new Option(item[1].name_with_type, item[1].code, false, false); newOption.dataset.slug = item[1].type + '-' + item[1].slug; return newOption; });
                    // console.dir(dataPX);
                    $(self.ward_select).append(dataPX).trigger('change');

                    self.dataPhuongXa[`${phuongxa_id}`] = dataPX;
                    console.log('done phuong xa');
                });
            }
            else {
                $(self.ward_select).append(self.dataPhuongXa[`${phuongxa_id}`]).trigger('change');
            }
            console.log('da chon quanhuyen');
        });
    },

    loadProvince: function () {
        const self = this;
        fetch('/Hanhchinh_VietNam/tinh_tp.json')
            .then(response => response.json())
            .then((data) => {
                const dataP = Object.entries(data).sort((item1, item2) => item1[0].localeCompare(item2[0]))
                    .map(item => { let newOption = new Option(item[1].name, item[1].code, false, false); newOption.dataset.slug = item[1].slug; return newOption; });
                // id: item.code, text: item[1].name
                console.dir(dataP)
                $(self.province_select).append(dataP);
                console.log('done province');
            });
    },

    init: function () {
        this.createSelect2();
        this.loadProvince();
    }
}

$(document).ready(function () {
    select2TinhThanh.init();
});