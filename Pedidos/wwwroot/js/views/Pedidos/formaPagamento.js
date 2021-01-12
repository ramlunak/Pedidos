
var totalCalculadoPreparado = 0;

$(function () {

    $("#inputDeliveryDinheiroTotal").mask("###0.00", { reverse: true });

    $('button[name="buttonDeliveryCheck"]').on('click', function (e) {
        e.preventDefault();

        var id = $(e.target)[0].id;

        if (id === "_deliveryEmCartaoCheck") {
            $('#deliveryEmCartaoCheck').show();
            $('#deliveryEmdinheiroCheck').hide();
            $('#deliveryPagoCheck').hide();

            _ModalProducto.deliveryEmCartao = true;
            _ModalProducto.deliveryPago = false;
            _ModalProducto.deliveryEmdinheiro = false;

            $('#divDeliveryDinheiroTotal').hide();
            $('#inputDeliveryDinheiroTotal').val(null);
        }

        if (id === "_deliveryPagoCheck") {
            $('#deliveryPagoCheck').show();
            $('#deliveryEmdinheiroCheck').hide();
            $('#deliveryEmCartaoCheck').hide();

            _ModalProducto.deliveryEmCartao = false;
            _ModalProducto.deliveryPago = true;
            _ModalProducto.deliveryEmdinheiro = false;

            $('#divDeliveryDinheiroTotal').hide();
            $('#inputDeliveryDinheiroTotal').val(null);

        }

        if (id === "_deliveryEmdinheiroCheck") {
            $('#deliveryEmdinheiroCheck').show();
            $('#deliveryPagoCheck').hide();
            $('#deliveryEmCartaoCheck').hide();

            _ModalProducto.deliveryEmCartao = false;
            _ModalProducto.deliveryPago = false;
            _ModalProducto.deliveryEmdinheiro = true;

            $('#divDeliveryDinheiroTotal').show();
        }

    });

});

function metodoPagoDelivery(id) {

    if (id === "_deliveryEmCartaoCheck") {
        $('#deliveryEmCartaoCheck').show();
        $('#deliveryEmdinheiroCheck').hide();
        $('#deliveryPagoCheck').hide();

        _ModalProducto.deliveryEmCartao = true;
        _ModalProducto.deliveryPago = false;
        _ModalProducto.deliveryEmdinheiro = false;

        $('#divDeliveryDinheiroTotal').hide();
        $('#inputDeliveryDinheiroTotal').val(null);
    }

    if (id === "_deliveryPagoCheck") {
        $('#deliveryPagoCheck').show();
        $('#deliveryEmdinheiroCheck').hide();
        $('#deliveryEmCartaoCheck').hide();

        _ModalProducto.deliveryEmCartao = false;
        _ModalProducto.deliveryPago = true;
        _ModalProducto.deliveryEmdinheiro = false;

        $('#divDeliveryDinheiroTotal').hide();
        $('#inputDeliveryDinheiroTotal').val(null);

    }

    if (id === "_deliveryEmdinheiroCheck") {
        $('#deliveryEmdinheiroCheck').show();
        $('#deliveryPagoCheck').hide();
        $('#deliveryEmCartaoCheck').hide();

        _ModalProducto.deliveryEmCartao = false;
        _ModalProducto.deliveryPago = false;
        _ModalProducto.deliveryEmdinheiro = true;

        $('#divDeliveryDinheiroTotal').show();
    }
}