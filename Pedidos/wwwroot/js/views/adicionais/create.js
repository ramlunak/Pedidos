

$(function () {

    $('#txtValor').mask("###0.00", { reverse: true });

});

window.onbeforeunload = function () {
    $('form')[0].reset();
};