
var totalCalculadoPreparado = 0;

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
            $('#inputTotalDinheiroPreparado').val(totalCalculadoPreparado);

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


    //$("#inputTasaEntregaPreparado").mask("###0.00", { reverse: true });
    //$("#inputDescuentoPreparado").mask("###0.00", { reverse: true });
    //$("#inputTotalDinheiroPreparado").mask("###0.00", { reverse: true });
    //$("#inputTrocoPreparado").mask("###0.00", { reverse: true });
    //$("#inputTotalPagarPreparado").mask("###0.00", { reverse: true });

    //$('input[name="radioFormaPagamentoPreparado"]').on('change', function (e) {

    //    var selected = e.currentTarget.id;

    //    if (selected === "SelectedDinheiroPreparado") {

    //        $('#inputTotalDinheiroPreparado').prop('disabled', false);
    //        $('#inputTotalDinheiroPreparado').val(totalCalculadoPreparado);

    //    } else {

    //        $('#inputTotalDinheiroPreparado').val(null);
    //        $('#inputTotalDinheiroPreparado').prop('disabled', true);

    //    }

    //    calcularTotalPagarPreparado();

    //});

    //$('#inputTasaEntregaPreparado').on('input propertychange', function (e) {
    //    calcularTotalPagarPreparado();
    //});

    //$('#inputDescuentoPreparado').on('input propertychange', function (e) {
    //    calcularTotalPagarPreparado();
    //});

    //$('#inputTotalDinheiroPreparado').on('input propertychange', function (e) {
    //    calcularTroco();
    //});


});

//function calcularTotalPagarPreparado() {

//    var total = pedidoValorProdutosPreparado;
//    var tasaEntraga = parseFloat($('#inputTasaEntregaPreparado').val());
//    var descontoPreparado = parseFloat($('#inputDescuentoPreparado').val());

//    if (!isNaN(tasaEntraga)) {
//        total = total + tasaEntraga;
//    }

//    if (!isNaN(descontoPreparado)) {
//        total = total - descontoPreparado;
//    }

//    totalCalculadoPreparado = total;
//    $('#inputTotalPagarPreparado').val(total.toFixed(2));

//    calcularTroco();
//}

//function calcularTroco() {
//    var totalPagar = parseFloat($('#inputTotalPagarPreparado').val());
//    var totalDinheiro = parseFloat($('#inputTotalDinheiroPreparado').val());

//    if (isNaN(totalPagar) || isNaN(totalDinheiro)) {
//        $('#inputTrocoPreparado').val(null);
//        return;
//    }

//    $('#inputTrocoPreparado').val(totalDinheiro - totalPagar);
//}

//var idPedidoPreparado = 0;
//var pedidoValorProdutosPreparado = 0;
//var pedidoIsPreparado = false;

//function showModalInfoDelivery(idPedido) {

//    var pedido = null;

//    if (idPedido !== null && idPedido !== undefined && idPedido !== "") {

//        var findResult = _PedidosPendientes.filter(function (item) {
//            return (item.id === idPedido);
//        });
//        pedido = findResult[0];
//        pedidoValorProdutosPreparado = pedido.valorProductos;
//        idPedidoPreparado = idPedido;
//        pedidoIsPreparado = true;

//    } else {

//        pedido = _CurrentPedido;
//        pedidoValorProdutosPreparado = _CurrentPedido.valorProductos;
//        pedidoIsPreparado = false;
//    }
//    if (pedido.descuento !== null && pedido.descuento !== undefined && pedido.descuento > 0) {
//        $('#inputDescuentoPreparado').val(pedido.descuento);
//    }
//    if (pedido.tasaEntrega !== null && pedido.tasaEntrega !== undefined && pedido.tasaEntrega > 0) {
//        $('#inputTasaEntregaPreparado').val(pedido.tasaEntrega);
//    }
//    if (pedido.deliveryDinheiroTotal !== null && pedido.deliveryDinheiroTotal !== undefined && pedido.deliveryDinheiroTotal > 0) {
//        $('#inputTotalDinheiroPreparado').val(pedido.deliveryDinheiroTotal);
//        $('#inputTotalDinheiroPreparado').prop('disabled', false);
//        $('#inputTrocoPreparado').val(pedido.deliveryTroco);
//        $('#SelectedDinheiroPreparado').attr('checked', 'checked');
//    }

//    if (pedido.deliveryEmCartao !== null && pedido.deliveryEmCartao !== undefined && pedido.deliveryEmCartao) {
//        $('#SelectedCartaoPreparado').attr('checked', 'checked');
//    }

//    if (pedido.deliveryPago !== null && pedido.deliveryPago !== undefined && pedido.deliveryPago) {
//        $('#SelectedPagoPreparado').attr('checked', 'checked');
//    }

//    $('#ModalPreparado').modal('show');

//    calcularTotalPagarPreparado();

//}

//function preparado() {

//    infoAuxDelivery = {
//        idPedido: idPedidoPreparado,
//        descuento: $('#inputDescuentoPreparado').val() == "" ? null : parseFloat($('#inputDescuentoPreparado').val()),
//        tasaEntrega: $('#inputTasaEntregaPreparado').val() == "" ? null : parseFloat($('#inputTasaEntregaPreparado').val()),
//        DeliveryDinheiroTotal: $('#inputTotalDinheiroPreparado').val() == "" ? null : parseFloat($('#inputTotalDinheiroPreparado').val()),
//        DeliveryTroco: $('#inputTrocoPreparado').val() == "" ? null : parseFloat($('#inputTrocoPreparado').val()),
//        DeliveryEmdinheiro: $('#SelectedDinheiroPreparado').is(':checked'),
//        DeliveryEmCartao: $('#SelectedCartaoPreparado').is(':checked'),
//        DeliveryPago: $('#SelectedPagoPreparado').is(':checked'),
//        pedidoIsPreparado: pedidoIsPreparado
//    };

//    if (totalCalculadoPreparado < 0) {
//        Swal.fire('Erro', 'O desconto não pode ser maior que o total a pagar.', 'error');
//        return;
//    };

//    if (infoAuxDelivery.DeliveryEmdinheiro && isNaN(parseFloat($('#inputTotalDinheiroPreparado').val()))) {
//        Swal.fire('Erro', 'Informe a quantidade de dinheiro.', 'error');
//        return;
//    };

//    if (infoAuxDelivery.DeliveryEmdinheiro && parseFloat($('#inputTrocoPreparado').val()) < 0) {
//        Swal.fire('Erro', 'A quantidade de dinheiro não pode ser menor que o total a pagar.', 'error');
//        return;
//    };

//    showLoading();

//    $.ajax({
//        type: "POST",
//        url: "/Pedidos/Preparado",
//        traditional: true,
//        data: JSON.stringify(infoAuxDelivery),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (pedido) {

//            hideLoading();

//            const Toast = Swal.mixin({
//                toast: true,
//                position: 'top-end',
//                showConfirmButton: false,
//                timer: 3000,
//                timerProgressBar: true,
//                didOpen: (toast) => {
//                    toast.addEventListener('mouseenter', Swal.stopTimer)
//                    toast.addEventListener('mouseleave', Swal.resumeTimer)
//                }
//            })

//            Toast.fire({
//                icon: 'success',
//                title: 'Ação realizada com sucesso'
//            })

//            if (pedidoIsPreparado) {
//               // addPedidoToEnd(pedido);
//            } else {
//                _CurrentPedido = pedido;
//                $('#spanTotal').html(_CurrentPedido.valorProductos + _CurrentPedido.tasaEntrega - _CurrentPedido.descuento);
//            }

//            idPedidoPreparado = 0;
//            $('#formInfoDelivery')[0].reset();
//            $('#ModalPreparado').modal('hide');

//        },
//        failure: function (response) {

//            Swal.fire(
//                'Error!',
//                'Erro de servidor.',
//                'error'
//            )
//        },
//        error: function (response) {

//            Swal.fire(
//                'Error',
//                'Erro de servidor.',
//                'error'
//            )
//        }
//    });


//}
