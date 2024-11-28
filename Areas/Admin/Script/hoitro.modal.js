let modal_activities;
modal_activities = modal_activities || {};
modal_activities = {
    modal: '.js-modal',

    modalOnClose: function () {
        $(this.modal).on('hide.bs.modal', function () {
            $(this).find('.modal-body').html('');
        });
    },

    init: function () {
        this.modalOnClose();
    }
}

$(document).ready(function () {
    modal_activities.init();
});