function popUpOpen(selector) {
    $(selector).on('click', function (e) {
        var targeted_popup_class = $(this).attr('data-popup-open');
        $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);
        e.preventDefault();
    });

};

function popUpClose(selector) {
    $(selector).on('click', function (e) {
        var targeted_popup_class = $(this).attr('data-popup-close');
        $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);
        e.preventDefault();
    })
};

$(function () {
        popUpOpen('[data-popup-open]');
        popUpClose('[data-popup-close]');
    }
);
