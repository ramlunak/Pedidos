
$(function () {
    $(".inputDecimal").mask("###0.00", { reverse: true });

    $('#browserInputMotivo').on('input', function () {
        var opt = $('option[value="' + $(this).val() + '"]');
        $('#idMotivo').val(opt.length ? opt.attr('id') : '');
    });

});

window.onbeforeunload = function () {
    $('form')[0].reset();
};
