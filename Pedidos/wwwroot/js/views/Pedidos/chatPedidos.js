
$(function () {

    $("#chatModal").on('shown.bs.modal', function () {
        $('#tableChatCardapioMensajesCliente').animate({ scrollTop: 1000000 }, 500);
    });

});
