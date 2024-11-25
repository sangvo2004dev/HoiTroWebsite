const provinceSelect = $('#province_id');
const districtSelect = $('#district_id');
const wardSelect = $(document.getElementById('phuongxa'));
const addressFeild = $('#sonha_duong');
const dia_chi = $('#dia_chi');

// ################ xử lý load địa chỉ
const loadDataFromApi = (url, seclectElement) => {
    fetch(url)
        .then(response => response.json())
        .then((data) => {
            let arrData = data.data.data;
            const dataP = []
            arrData.map(p => dataP.push({ id: `${p.code}`, text: `${p.name}` }));

            $(seclectElement).select2({
                data: dataP,
            });
        })
        .catch(function (error) {
            console.error("Xãy ra lỗi: ", error);
        });
}

// api lấy tỉnh, thành phố
fetch('https://vn-public-apis.fpo.vn/provinces/getAll?limit=-1')
    .then(function (response) {
        return response.json();
    })
    .then(function (data) {
        let provinces = data.data.data;
        const dataP = []
        // console.dir(provinces)
        provinces.map(p => dataP.push({ id: `${p.code}`, text: `${p.name}` }));

        $(provinceSelect).select2({
            data: dataP,
        });
    })
    .catch(function (error) {
        console.error("Xãy ra lỗi: ", error);
    });

// api lấy quận huyện
const loadDistric = (province_code) => {
    fetch(`https://vn-public-apis.fpo.vn/districts/getByProvince?provinceCode=${province_code}&limit=-1`)
        .then(function (response) {
            return response.json()
        })
        .then(function (data) {
            let districts = data.data.data;
            const dataP = []
            districts.map(p => dataP.push({ id: `${p.code}`, text: `${p.name_with_type}` }));
            $(districtSelect).select2({
                data: dataP
            });
        })
        .catch(function (error) {
            console.error("Xãy ra lỗi: ", error);
        });
}

// api lấy phường xã
const loadWard = (district_code) => {
    fetch(`https://vn-public-apis.fpo.vn/wards/getByDistrict?districtCode=${district_code}&limit=-1`)
        .then(function (response) {
            return response.json()
        })
        .then(function (data) {
            let wards = data.data.data;
            const dataP = []
            wards.map(p => dataP.push({ id: `${p.code}`, text: `${p.name_with_type}` }));
            $(wardSelect).select2({
                data: dataP
            });
        })
        .catch(function (error) {
            console.error("Xãy ra lỗi: ", error);
        });
}

$(provinceSelect).on('change', (e) => {
    // console.log(e.currentTarget);
    const value = $(e.currentTarget).val();
    $(districtSelect).find('option:not(:first-child)').remove();
    $(wardSelect).find('option:not(:first-child)').remove();

    if (value !== '') {
        loadDistric(value);
    }

    dia_chi.val(addressString());
})

$('#district_id').on('change', (e) => {
    // console.log(e.currentTarget);
    const value = $(e.currentTarget).val();
    $(wardSelect).find('option:not(:first-child)').remove();

    if (value !== '') {
        loadWard(value);
    }
    dia_chi.val(addressString());
})

$(wardSelect).on('change', (e) => {
    $(dia_chi).val(addressString());
});

$(addressFeild).on('keyup', (e) => {
    $(dia_chi).val(addressString());
});

// tạo thành địa chỉ hoàn chình
const addressString = () => {
    const getTextOfValue = (e) => e.val() === '' ? '' : e.find(`option[value="${e.val()}"]`).text();
    const provinceName = getTextOfValue($('#province_id'));
    const districtName = getTextOfValue($('#district_id'));
    const wardName = getTextOfValue($('#phuongxa'));
    const address = $(addressFeild).val();

    if (provinceName === '') return;
    if (districtName === '') return [provinceName].join(', ');
    if (wardName === '') return [districtName, provinceName].join(', ');
    if (address === '') return [wardName, districtName, provinceName].join(', ');
    return [address, wardName, districtName, provinceName].join(', ');
};


 //import {default as VNnum2words} from 'https://cdn.jsdelivr.net/gh/linhpn96/VNnum2words/src/index.mjs';
 //console.log(VNnum2words('123674352345'));
