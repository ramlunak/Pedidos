

$(function () {

    $('#txtValor').maskMoney({ prefix: '', allowNegative: false, thousands: '', decimal: '.', affixesStay: false });

});


window.onbeforeunload = function () {
    $('form')[0].reset();
};