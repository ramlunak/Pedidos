
$(function () {

    $("#inputTasaEntregaPreparado").mask("###0.00", { reverse: true });
    $("#inputDescuentoPreparado").mask("###0.00", { reverse: true });
    $("#inputTotalDinheiroPreparado").mask("###0.00", { reverse: true });
    $("#inputTrocoPreparado").mask("###0.00", { reverse: true });
    $("#inputTotalPagarPreparado").mask("###0.00", { reverse: true });

    $('input[name="radioFormaPagamentoPreparado"]').on('change', function (e) {

        var selected = e.currentTarget.id;

        if (selected === "SelectedDinheiroPreparado") {

            $('#inputTotalDinheiroPreparado').prop('disabled', false);

        } else {

            $('#inputTotalDinheiroPreparado').val(null);
            $('#inputTotalDinheiroPreparado').prop('disabled', true);

        }

        calcularTotalPagarPreparado();

    });

    $('#inputTasaEntregaPreparado').on('input propertychange', function (e) {
        calcularTotalPagarPreparado();
    });

    $('#inputDescuentoPreparado').on('input propertychange', function (e) {
        calcularTotalPagarPreparado();
    });

    $('#inputTotalDinheiroPreparado').on('input propertychange', function (e) {
        calcularTroco();
    });


});

function calcularTotalPagarPreparado() {
    var total = pedidoValorProdutosPreparado;
    var tasaEntraga = parseFloat($('#inputTasaEntregaPreparado').val());
    var descontoPreparado = parseFloat($('#inputDescuentoPreparado').val());

    if (!isNaN(tasaEntraga)) {
        total = total + tasaEntraga;
    }

    if (!isNaN(descontoPreparado)) {
        total = total - descontoPreparado;
    }

    $('#inputTotalPagarPreparado').val(total.toFixed(2));

    calcularTroco();
}

function calcularTroco() {
    var totalPagar = parseFloat($('#inputTotalPagarPreparado').val());
    var totalDinheiro = parseFloat($('#inputTotalDinheiroPreparado').val());

    if (isNaN(totalPagar) || isNaN(totalDinheiro)) {
        $('#inputTrocoPreparado').val(null);
        return;
    }

    $('#inputTrocoPreparado').val(totalDinheiro - totalPagar);
}

var idPedidoPreparado = 0;
var pedidoValorProdutosPreparado = 0;
function showModalPreparado(idPedido) {

    var findResult = _PedidosPendientes.filter(function (item) {
        return (item.id === idPedido);
    });
    var pedido = findResult[0];
    pedidoValorProdutosPreparado = pedido.valorProductos;

    idPedidoPreparado = idPedido;
    $('#ModalPreparado').modal('show');

    calcularTotalPagarPreparado();

}

function preparado() {

    infoAuxDelivery = {
        idPedido: idPedidoPreparado,
        descuento: parseFloat($('#inputDescontoFinalizado').val()),
        tasaEntrega: 0,
        DeliveryEmdinheiro: null,
        DeliveryDinheiroTotal: null,
        DeliveryTroco: null,
        DeliveryEmCartao: null,
        DeliveryPago: null,
    };

    $.ajax({
        type: "POST",
        url: "/Pedidos/Preparado",
        traditional: true,
        data: JSON.stringify(infoAuxDelivery),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result);

            //const Toast = Swal.mixin({
            //    toast: true,
            //    position: 'top-end',
            //    showConfirmButton: false,
            //    timer: 3000,
            //    timerProgressBar: true,
            //    didOpen: (toast) => {
            //        toast.addEventListener('mouseenter', Swal.stopTimer)
            //        toast.addEventListener('mouseleave', Swal.resumeTimer)
            //    }
            //})

            //Toast.fire({
            //    icon: 'success',
            //    title: 'Ação realizada com sucesso'
            //})

            //$('#CARD_PEDIDO_' + idPedido + '').remove();
            //GetNumeroPedidosFinalizados();
        },
        failure: function (response) {

            Swal.fire(
                'Error!',
                'Erro de servidor.',
                'error'
            )
        },
        error: function (response) {

            Swal.fire(
                'Error',
                'Erro de servidor.',
                'error'
            )
        }
    });


}
