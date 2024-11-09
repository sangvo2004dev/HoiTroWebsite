const listPhotoPreview = $('#list-photos-dropzone-previews');
const dropAndChooseField = $('#photos-dropzone');
const from = $('#from_dangtin');

// tạo photo-item
const createPhotoItem = (srcEncoded, fileName, file) => {
    const photoItem = $('<div class="photo_item col-md-2 col-3">');

    // Create the photo element with the encoded source
    const photo = $('<div class="photo">').append($('<img src="' + srcEncoded + '">'));

    // Create the progress bar element (assuming you don't need to modify it)
    const progress = $('<div class="dz-progress"><span class="dz-upload"></span></div>');

    // Create the bottom section with delete button and hidden input
    const bottom = $('<div class="bottom clearfix">');
    const deleteButton = $('<span class="photo_delete" data-dz-remove="">')
        .append($(`<svg xmlns="http://www.w3.org/2000/svg" width="24"
                                                            height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                            stroke-linecap="round" stroke-linejoin="round" class="feather feather-trash-2">
                                                            <polyline points="3 6 5 6 21 6"></polyline>
                                                            <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2">
                                                            </path>
                                                            <line x1="10" y1="11" x2="10" y2="17"></line>
                                                            <line x1="14" y1="11" x2="14" y2="17"></line>
                                                        </svg>`)) // SVG code for trash icon
        .append(' Xóa'); // Text for delete button
    const dataT = new DataTransfer();
    dataT.items.add(file);
    const hiddenInput = $('<input type="file" hidden>');
    hiddenInput.prop('files', dataT.files);
    //console.log(hiddenInput.prop('files'));

    // Append all elements to the photo item
    photoItem.append(photo);
    photoItem.append(progress);
    bottom.append(deleteButton);
    bottom.append(hiddenInput);
    photoItem.append(bottom);

    // You can now use the photoItem element as needed
    // (e.g., append it to a container)

    return photoItem;
}

// sự kiện khi chọn ảnh
dropAndChooseField.on('change', function (e) {
    const files = this.files;
    Array.from(files).forEach((file) => {
        const fileName = file.name;
        const type = file.type;

        if (!file) return;

        const reader = new FileReader();

        reader.readAsDataURL(file);
        reader.onload = function (e) {
            const imgElement = document.createElement("img");

            imgElement.src = e.target.result;
            imgElement.onload = function (e) {
                const canvas = document.createElement('canvas');
                const ctx = canvas.getContext('2d');

                // giảm kích thước ảnh thành 120 x 120
                ctx.drawImage(e.target, 0, 0, 120, 120);

                // chuyển thành base64 và giảm chất lượng con 0.3
                const srcEncoded = ctx.canvas.toDataURL(type, 0.5);

                $(listPhotoPreview).append(createPhotoItem(srcEncoded, fileName, file));
            };
        };
    });
});

// sortable
$(listPhotoPreview).sortable({
    helper: 'original',
    cursor: 'move',
});

$(from).on('submit', function (e) {
    e.preventDefault();

    // lấy các file lưu trong các input của listPhotoPreview
    const imgList = new DataTransfer();

    $(listPhotoPreview).find('input').each((i, input) => {
        imgList.items.add($(input).prop('files')[0]);
    });

    //console.log(imgList.files);
    const input = $('<input type="file" name="imgFiles" multiple>').prop('files', imgList.files);
    $(this).append(input);

    if ($(this).valid()) {
        this.submit();
    }
});

// tạo Đối tượng File từ ảnh đã load từ server 
//const imgOrderTemp = imgOrder();
//$(listPhotoPreview).find('img').forEach((img, i = 0) => {
//    fetch(img.src)
//        .then(response => response.blob())
//        .then(blob => {
//            // Tạo đối tượng File từ blob
//            const file = new File([blob], 'image.jpg', { type: 'image/jpeg' });

//            // Sử dụng file như bình thường
//            dictImg[imgOrderTemp[i]] = file;
//            i++;
//        });
//});