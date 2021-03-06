﻿var _PedidosFinalizados = [];

//cargar lista de pedidos pendientes
function CargarPedidosFinalizados() {

    $('#divLoadinFinalizados').show();
    $('#divPedidosFinalizados').hide();

    $.ajax({
        type: "GET",
        url: "/Pedidos/CargarPedidosFinalizados/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            MostarPedidosFinalizados(data.pedidosFinalizados);
            _PedidosFinalizados = data.pedidosFinalizados;

            $('#divLoadinFinalizados').hide();
            $('#divPedidosFinalizados').show();

            $('#dinalizadosValorTotal').html(data.valorTotalPedidos);
            $('#dinalizadosTotalDescuento').html(data.totalDescuentos);
            $('#dinalizadosTotalTasa').html(data.totalTasaEntraga);
            $('#dinalizadosTotalDiferencia').html(data.diferencia);

        },
        failure: function (response) {
            console.log('failure', response);

            $('#divLoadinFinalizados').hide();
            $('#divPedidosFinalizados').show();
        },
        error: function (response) {
            console.log('error', response);

            $('#divLoadinFinalizados').hide();
            $('#divPedidosFinalizados').show();

        }
    });
}

function MostarPedidosFinalizados(pedidosFinalizados) {

    var TABLE = $('#ListaPedidosFinalizados');
    TABLE.empty();

    $.each(pedidosFinalizados, function (indexPedido, pedido) {

        var CARD = $('<div id="CARD_PEDIDO_' + pedido.id + '" class="card mb-2  border border-info">');
        var CARD_BODY = $('<div class="card-body  p-1">');

        var mesa = '';
        var aplicativo = '';
        if (pedido.idMesa !== null && pedido.idMesa !== undefined && pedido.idMesa !== "" && pedido.idMesa > 0) {
            mesa = '<div style="font-size:11px">MESA ' + pedido.idMesa + '</div>';
        } if (pedido.aplicativo !== null && pedido.aplicativo !== undefined && pedido.aplicativo !== "") {
            aplicativo = '<div style="font-size:11px">' + pedido.aplicativo + '</div>';
        }

        var tasaEntrega = ' <div style="font-size:13px">Taxa de entrega: <b>R$ ' + pedido.tasaEntrega.toFixed(2) + '</b></div>  ';
        if (pedido.tasaEntrega === 0) {
            tasaEntrega = "";
        }

        var descuento = ' <div style="font-size:13px">Desconto: <b>R$ ' + pedido.descuento.toFixed(2) + '</b></div>  ';
        if (pedido.descuento === 0) {
            descuento = "";
        }

        var valorPedido = pedido.valorProductos;
        var totalPagar = valorPedido + pedido.tasaEntrega - pedido.descuento;

        //INFO DEL PEDIDO 
        var div_infopedido = '<div class="d-flex justify-content-between">  ' +
            '               <div class="d-block" style="text-align:left">  ' +
            '                   <div style="font-size:13px"><b>Codigo: ' + pedido.codigo + '<b/></div>  ' +
            '                   <div style="font-size:13px">' + pedido.cliente + '</div>  ' +
            '                   <div style="font-size:13px">' + pedido.direccion + ' ' + pedido.barrio + '</div>  ' + mesa + aplicativo +
            '               </div>  ' +
            '               <div class="d-block" style="text-align:right">  ' +
            '                   <div style="font-size:13px">Valor do pedido: <b>R$ ' + valorPedido.toFixed(2) + '</b></div>  ' +
            tasaEntrega +
            descuento +
            '                   <div style="font-size:13px"><b>Total a pagar: R$ ' + totalPagar.toFixed(2) + '</b></div>  ' +
            '               </div>  ' +
            '           </div>  ' +
            '          <hr class="m-2" />  ';

        CARD_BODY.append(div_infopedido);

        //LISTA DE PRODUCTOS
        var TABLA_PRD = $('<table cellspacing="0" class="table-tr-border-radius unselectable" style="font-size: 13px;">');

        $.each(pedido.productos, function (index, producto) {

            //PRODUCTO
            var Desplegar = 'class="cursor-pointer" data-toggle="collapse"   ' +
                '   data-target="#collapseExample_' + pedido.id + '_' + index + '_' + producto.id + '"   ' +
                '   aria-expanded="false" aria-controls="collapseExample_' + pedido.id + '_' + index + '_' + producto.id + '" ';

            var btnInfo = ' <button type="button" class="btn btn-sm btn-outline-primary mr-1 ml-1" style="font-size:11px">+ Info</button></div><div style="text-align: start;color: cadetblue;">';

            if (producto.adicionales.length == 0 && producto.ingredientes.length == 0) {
                Desplegar = "";
                btnInfo = "";
            }

            // CONTADOR
            var TR0_PRD = $('<tr>');

            //TIEMPO DE PEDIDO
            var tiempopedido = producto.tiempo_pedido;

            var timerPedido = null;
            if (producto.fecha_preparado === null) {
                timerPedido = setInterval(function () {

                    let minutesPedido_ = (tiempopedido / 60).toFixed(0);
                    let secondsPedido_ = tiempopedido % 60;

                    $('#minutesPedido_' + pedido.id + '_' + index + '_' + producto.id + '').html(minutesPedido_);
                    $('#secondsPedido_' + pedido.id + '_' + index + '_' + producto.id + '').html(secondsPedido_);
                    tiempopedido++;
                }, 1000);

                intervals.push(timerPedido);
            }
            //   console.log($('#divTiempoPedido_' + pedido.id + '_' + index + '_' + producto.id));
            //  $('#divTiempoPedido_' + pedido.id + '_' + index + '_' + producto.id + '').remove();
            var productoTiempoPedido = '<div id="divTiempoPedido_' + pedido.id + '_' + index + '_' + producto.id + '" class="border border-warning rounded p-1 d-flex align-items-center mr-1 ml-1" style="font-size: 10px;color: darkcyan;font-weight: 700;"> <i class="far fa-clock mr-1"></i> <span id="minutesPedido_' + pedido.id + '_' + index + '_' + producto.id + '"></span>: <span id="secondsPedido_' + pedido.id + '_' + index + '_' + producto.id + '"></span> <a  onclick="marcarProductoPreparado(' + pedido.id + ',' + producto.id + ',' + producto.posicion + ',' + timerPedido + ')"><i class="fas fa-check mr-2 ml-2 cursor-pointer"></i></a> </div>';

            if (producto.fecha_preparado !== null) {
                let minutesPedido_ = (tiempopedido / 60).toFixed(0);
                let secondsPedido_ = tiempopedido % 60;
                productoTiempoPedido = '<div id="divTiempoPedido_' + pedido.id + '_' + index + '_' + producto.id + '" class="border border-success rounded p-1 d-flex align-items-center mr-1 ml-1" style="font-size: 10px;color: black;font-weight: 700;"> <i class="far fa-clock mr-1"></i> <span id="minutesPedido_' + pedido.id + '_' + index + '_' + producto.id + '">' + minutesPedido_ + '</span>: <span id="secondsPedido_' + pedido.id + '_' + index + '_' + producto.id + '">' + secondsPedido_ + '</span>  </div>';
            }

            ////TIEMPO DE ENTRAGA
            //var tiempoentrega = producto.tiempo_entrega;
            //var timerEntregado = null;
            //if (producto.fecha_preparado !== null && producto.fecha_entrega === null) {
            //    timerEntregado = setInterval(function () {

            //        let minutesEntraga_ = (tiempoentrega / 60).toFixed(0);
            //        let secondsEntraga_ = tiempoentrega % 60;

            //        $('#minutesEntraga_' + pedido.id + '_' + index + '_' + producto.id + '').html(minutesEntraga_);
            //        $('#secondsEntraga_' + pedido.id + '_' + index + '_' + producto.id + '').html(secondsEntraga_);
            //        tiempoentrega++;
            //    }, 1000);
            //}

            //$('#divTiempoEntrega_' + pedido.id + '_' + index + '_' + producto.id + '').remove();
            //var productoTiempoEntrega = '<div id="divTiempoEntrega_' + pedido.id + '_' + index + '_' + producto.id + '" class="border border-warning rounded p-1 d-flex align-items-center mr-1 ml-1" style="font-size: 10px;color: darkcyan;font-weight: 700;"> <i class="fa fa-motorcycle  mr-1"></i> <span id="minutesEntraga_' + pedido.id + '_' + index + '_' + producto.id + '"></span>: <span id="secondsEntraga_' + pedido.id + '_' + index + '_' + producto.id + '"></span> <a  onclick="marcarProductoEntregado(' + pedido.id + ',' + producto.id + ',' + producto.posicion +  ',' + timerEntregado + ')"><i class="fas fa-check mr-2 ml-2 cursor-pointer"></i></a> </div>';

            //if (producto.fecha_preparado === null) {
            //    productoTiempoEntrega = "";
            //} else if (producto.fecha_preparado !== null && producto.fecha_entrega !== null) {

            //    let minutesEntraga_ = (tiempoentrega / 60).toFixed(0);
            //    let secondsEntraga_ = tiempoentrega % 60;
            //    productoTiempoEntrega = '<div id="divTiempoEntrega_' + pedido.id + '_' + index + '_' + producto.id + '" class="border border-warning rounded p-1 d-flex align-items-center mr-1 ml-1" style="font-size: 10px;color: darkcyan;font-weight: 700;"> <i class="fa fa-motorcycle  mr-1"></i> <span id="minutesEntraga_' + pedido.id + '_' + index + '_' + producto.id + '">' + minutesEntraga_ + '</span>: <span id="secondsEntraga_' + pedido.id + '_' + index + '_' + producto.id + '">' + secondsEntraga_ + '</span>  </div>';

            //}
            ////FIN 

            var TR1_PRD = $('<tr>');
            var TD1_PRD = $('<td style="width:100%">');
            var TD2_PRD = $('<td>');

            TD1_PRD.append('<div class="d-flex"><div style="text-align: start;" ' + Desplegar + '>  (<b>' + producto.cantidad + '</b>) ' + producto.nombre.toUpperCase() + btnInfo + '</div> ' + productoTiempoPedido + ' </div><div style="color: gray;text-align: start;">' + producto.observacion + '</div>');
            TD2_PRD.append('<div style="font-size:12px;width:70px;text-align:end;" class="cursor-pointer"> R$ ' + producto.valorMasAdicionales.toFixed(2) + '</div>');
            TR1_PRD.append(TD1_PRD, TD2_PRD);

            //ADICIONALES E INGREDIENTES DEL PRODUCTO
            var TR2_PRD = $('<tr>');
            var TD1_PRD = $('<td style="width:100%" colspan="2">');

            var TABLA_ADIC = $('<table class="w-100 unselectable" style="color: blue;font-weight: 500;">');
            $.each(producto.adicionales, function (index, item) {

                var TD1 = $('<td style="width:100%">');
                var TD2 = $('<td>');
                var TR = $('<tr>');

                TD1.append('<div class="unselectable" style="text-align:start;"><a>+' + item.cantidad + '</a> ' + item.nombre + '</div>');
                TD2.append('<div class="unselectable" style="width:70px;text-align:end;font-size: 13px;">R$ ' + (item.valor * item.cantidad).toFixed(2) + '</div>');

                TR.append(TD1, TD2);
                TABLA_ADIC.append(TR);
            });

            var TABLA_INGD = $('<table class="w-100 unselectable" style="color: orangered;font-weight: 500;">');
            $.each(producto.ingredientes, function (index, item) {

                var TD1 = $('<td style="width:100%">');
                var TD2 = $('<td>');
                var TR = $('<tr>');

                TD1.append('<div class="unselectable" style="text-align:start;"><a style="color:blue;">- </a> ' + item.nombre + '</div>');

                TR.append(TD1);
                TABLA_INGD.append(TR);
            });

            var panelBody = $('<div class="card card-body" style="padding: 8px;">');
            panelBody.append(TABLA_ADIC);
            panelBody.append($('<hr style="margin: 5px;">'));
            panelBody.append(TABLA_INGD);

            var panelInredientesAdicionales = $('<div class="collapse" id="collapseExample_' + pedido.id + '_' + index + '_' + producto.id + '">');
            panelInredientesAdicionales.append(panelBody);

            TD1_PRD.append(panelInredientesAdicionales);
            TD1_PRD.append('<hr class="p-0 m-1">');

            TR2_PRD.append(TD1_PRD);

            //ADD TRS A LA TABLA
            TABLA_PRD.append(TR0_PRD, TR1_PRD, TR2_PRD);


        });

        //AGREGAR PRODUCTOS
        CARD_BODY.append(TABLA_PRD);

        //FUTTER
        var CARD_FUTTER = $(' <div class="card-footer text-muted p-2 d-flex justify-content-between ">');

        //ocultar botones

        //var futter_botones = $('<div class="d-flex">');

        //if (pedido.status == "Pendiente") {
        //    futter_botones.append('<a onclick="cancelar(' + pedido.id + ')" class="btn btn-sm btn-danger cursor-pointer"  style="color:white" ><i class="fas fa-ban"></i></a>');
        //    futter_botones.append('<a onclick="editar(' + pedido.id + ')" class="btn btn-sm btn-success cursor-pointer ml-1" style="color:white"><i class="fas fa-edit"></i></a> <span class="m-1"> | <span>  ');
        //    //futter_botones.append('<a onclick="showModalInfoDelivery(' + pedido.id + ')" class="btn btn-sm btn-primary  cursor-pointer mr-2" style="color:white">preparado</a>');
        //}

        //futter_botones.append('<a onclick="actualizarFormaPagamento(' + pedido.id + ',' + false + ')" class="btn btn-sm btn-info  cursor-pointer mr-1" style="color:white">Forma Pagamento</a>');
        //futter_botones.append('<a onclick="finalizar(' + pedido.id + ')" class="btn btn-sm btn-primary  cursor-pointer" style="color:white">Finalizar</a>');

        //CARD_FUTTER.append(futter_botones);

        //var pagpDinheiro = '   <div class="input-group mb-3">  ' +
        //    '     <div class="input-group-prepend">  ' +
        //    '       <span class="input-group-text" id="basic-addon1">' +
        //    '<i style="color:orange" class="fas fa-2x fa-money-bill-wave"></i> | R$</span > ' +
        //    '     </div>  ' +
        //    '     <input type="text" class="form-control" placeholder="Username" aria-label="Username" aria-describedby="basic-addon1">  ' +
        //    '  </div>  ';

        var trocoPara = '';
        if (pedido.deliveryDinheiroTotal !== undefined && pedido.deliveryDinheiroTotal !== null) {
            trocoPara = 'R$ ' + pedido.deliveryDinheiroTotal.toFixed(2);
        }

        var deliveryCartao = '<i style="color:blue;font-size:20px" class="far fa-2x fa-credit-card"></i> ';

        var deliveryDinheiro = '   <div class="card">  ' +
            '     <div class="card-body bg-outline-info p-1 d-flex align-items-center unselectable" style="font-size:11px">  ' +
            '       <i style="color:orange" class="fas fa-2x fa-money-bill-wave mr-1"></i> <b>' + trocoPara + '</b> ' +
            '     </div>  ' +
            '  </div>  ';

        var deliveryPago = '   <div class="card">  ' +
            '     <div class="card-body bg-outline-info p-1 d-flex align-items-center unselectable" style="font-size:14px">  ' +
            '       <i class="fas fa-check-double mr-1" style="color:green"></i> <b>Pago</b> ' +
            '     </div>  ' +
            '  </div>  ';

        if (!pedido.deliveryEmCartao) {
            deliveryCartao = "";
        }

        if (!pedido.deliveryEmdinheiro) {
            deliveryDinheiro = "";
        }

        if (!pedido.deliveryPago) {
            deliveryPago = "";
        }

        if (pedido.idMesa !== null && pedido.idMesa !== undefined && pedido.idMesa) {
            deliveryCartao = "";
            deliveryDinheiro = "";
            deliveryPago = "";
        }

        CARD_FUTTER.append('<div >  </div>');
        CARD_FUTTER.append('<a class="btn btn-outline-secondary cursor-pointer" onclick="imprimirPedido(' + pedido.id + ',' + false + ')" target="_blank"><i class="fa fa-print cursor-pointer float-right" aria-hidden="true" ></i></a>');

        CARD.append(CARD_BODY);
        CARD.append(CARD_FUTTER);

        var TD1 = $('<td style="width:100%">');
        var TR = $('<tr>');
        TD1.append(CARD);

        TR.append(TD1);
        TABLE.append(TR);
    });

    $('[data-toggle="tooltip"]').tooltip();

}

