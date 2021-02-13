
$(function () {

    $("#chatModal").on('shown.bs.modal', function () {
        $('#tableChatCardapioMensajesCliente').animate({ scrollTop: 1000000 }, 500);
    });

    $('#inputEstablecimientoMessage').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            establecimientoSendMessage();
            return false;
        }
    });


});
