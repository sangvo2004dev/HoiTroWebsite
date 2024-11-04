// ################ xử lý load địa chỉ
let province_select = document.getElementById('province_id');
let district_select = document.getElementById('district_id');
let ward_select = document.getElementById('phuongxa');

// api lấy tỉnh, thành phố
fetch('https://vn-public-apis.fpo.vn/provinces/getAll?limit=-1')
.then(function(response) {
    return response.json();
})
.then(function(data) {
    let provinces = data.data.data;
    // console.dir(provinces)
    provinces.map(p => province_select.innerHTML += `<option value="${p.name}" code="${p.code}">${p.name_with_type}</option>`)
})
.catch(function(error) {
    console.error("Xãy ra lỗi: ", error);
});

// api lấy quận huyện
function loadDistric(province_code) {
    fetch(`https://vn-public-apis.fpo.vn/districts/getByProvince?provinceCode=${province_code}&limit=-1`)
    .then(function (response) {
        return response.json()
    })
    .then(function (data) {
        let districts = data.data.data;
        districts.map(d => district_select.innerHTML += `<option value="${d.name}" code="${d.code}">${d.name_with_type}</option>`)
    });
}

// api lấy phường xã
function loadWard(district_code) {
    fetch(`https://vn-public-apis.fpo.vn/wards/getByDistrict?districtCode=${district_code}&limit=-1`)
    .then(function (response) {
        return response.json()
    })
    .then(function (data) {
        let wards = data.data.data;
        wards.map(w => ward_select.innerHTML += `<option value="${w.name}" code="${w.code}">${w.name_with_type}</option>`)
    });
}

// sự kiện chọn tỉnh
province_select.addEventListener('change', function(e) {
    if (this.selectedIndex == 0) {
        district_select.options.length = 1 // xóa danh sách value
    }
    else {
        let selected_option = this.options[this.selectedIndex];
        // console.log(selected_option.getAttribute('code'));
        district_select.options.length = 1 // xóa danh sách value
        loadDistric(selected_option.getAttribute('code'));
    }
});

// sự kiện chọn quận
district_select.addEventListener('change', function(e) {
    if (this.selectedIndex == 0) {
        ward_select.options.length = 1 // xóa danh sách value
    }
    else {
        let selected_option = this.options[this.selectedIndex];
        ward_select.options.length = 1 // xóa danh sách value
        loadWard(selected_option.getAttribute('code'));
    }
});