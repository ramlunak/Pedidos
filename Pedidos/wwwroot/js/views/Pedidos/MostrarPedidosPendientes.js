var intervals = [];

//crear tabla de productos del pedido en edicion
function MostarPedidosPendientes() {

    $.each(intervals, function (index, item) {
        clearInterval(item);
    });
    intervals = [];

    var TABLE = $('#ListaPedidosPendientes');
    TABLE.empty();

    $.each(_PedidosPendientes, function (indexPedido, pedido) {

        var CARD = $('<div id="CARD_PEDIDO_' + pedido.id + '" class="card mb-2  border border-info">');
        var CARD_BODY = $('<div class="card-body p-1">');

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

        //TEMPORIZADOR DEL PEDIDO
        var pedidotiempo = pedido.tiempo_pedido;

        var Pedidotimer = null;

        if (pedido.status === "Pendiente") {
            Pedidotimer = setInterval(function () {

                let Pedidominutes_ = (pedidotiempo / 60).toFixed(0);
                let Pedidoseconds_ = pedidotiempo % 60;

                $('#Pedidominutes_' + pedido.id).html(Pedidominutes_);
                $('#Pedidoseconds_' + pedido.id).html(Pedidoseconds_);
                pedidotiempo++;
            }, 1000);

            intervals.push(Pedidotimer);
        }
        var PedidoTiempo = '<div id="divPedidoTiempo_' + pedido.id + '" class=" d-flex text-nowrap align-items-start " style="font-size: 10px;color: darkcyan;font-weight: 700;"><div style="color: slategray"> <i class="far fa-clock mr-1"></i> <span id="Pedidominutes_' + pedido.id + '"></span>: <span id="Pedidoseconds_' + pedido.id + '"></span> </div></div>';


        //INFO DEL PEDIDO 
        var div_infopedido = '<div class="d-flex justify-content-between">  ' +
            '               <div class="d-block" style="text-align:left">  ' +
            '                   <div style="font-size:13px" class="d-flex"><b>Codigo: ' + pedido.codigo + '</b>  </div>  ' +
            '                   <div style="font-size:13px">' + pedido.cliente + '</div>  ' +
            '                   <div style="font-size:13px">' + pedido.direccion + '</div>  ' + mesa + aplicativo +
            '               </div>  ' + PedidoTiempo +
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

        var futter_botones = $('<div class="d-flex">');

        if (pedido.status == "Pendiente") {
            futter_botones.append('<a onclick="cancelar(' + pedido.id + ')" class="btn btn-sm btn-danger cursor-pointer"  style="color:white" ><i class="fas fa-ban"></i></a>');
            futter_botones.append('<a onclick="editar(' + pedido.id + ')" class="btn btn-sm btn-success cursor-pointer ml-1" style="color:white"><i class="fas fa-edit"></i></a> <span class="m-1"> | <span>  ');
            //futter_botones.append('<a onclick="showModalInfoDelivery(' + pedido.id + ')" class="btn btn-sm btn-primary  cursor-pointer mr-2" style="color:white">preparado</a>');
        }

        futter_botones.append('<a onclick="actualizarFormaPagamento(' + pedido.id + ',' + false + ')" class="btn btn-sm btn-info  cursor-pointer mr-1" style="color:white">Forma Pagamento</a>');
        futter_botones.append('<a onclick="fnFinalizarPedido(' + pedido.id + ')" class="btn btn-sm btn-primary  cursor-pointer" style="color:white">Finalizar</a>');

        CARD_FUTTER.append(futter_botones);

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

        CARD_FUTTER.append('<div >' + deliveryCartao + deliveryDinheiro + deliveryPago + '  </div>');
        CARD_FUTTER.append('<a class="btn btn-outline-secondary cursor-pointer" onclick="imprimirPedido(' + pedido.id + ',' + true + ')" target="_blank"><i class="fa fa-print cursor-pointer float-right" aria-hidden="true" ></i></a>');

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

function marcarProductoPreparado(idPedido, idProducto, posicion, timer) {

    var marcarProducto = {
        idPedido: idPedido,
        idProducto: idProducto,
        posicion: posicion
    };

    $.each(intervals, function (index, item) {
        clearInterval(item);
    });

    $.ajax({
        type: "POST",
        url: "/Pedidos/MarcarProductoPreparado",
        traditional: true,
        data: JSON.stringify(marcarProducto),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (pedido) {

            var objIndex = _PedidosPendientes.findIndex((p => p.id == pedido.id));
            _PedidosPendientes[objIndex] = pedido;
            MostarPedidosPendientes();

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });
}


function imprimirPedido(idPedido, pendientes) {

    var NombreEstablecimiento = $('#NombreEstablecimiento').val();
    var TelefonoEstablecimiento = $('#TelefonoEstablecimiento').val();
    var DireccionEstablecimiento = $('#DireccionEstablecimiento').val();
    var CnpjEstablecimiento = $('#CnpjEstablecimiento').val();

    async function cargarAsync() {

        var findResult = _PedidosPendientes.filter(function (item) {
            return (item.id === idPedido);
        });

        if (!pendientes) {
            findResult = _PedidosFinalizados.filter(function (item) {
                return (item.id === idPedido);
            });
        }

        var pedido = findResult[0];



        var comprobantePedido = $("#divComprobantePedido");
        $("#divComprobantePedido").html("");

        comprobantePedido.append('<div class="centrado"> <b>' + pedido.codigo + '</b> </div >');

        if (pedido.idDireccion !== null && pedido.idDireccion !== "") {
            comprobantePedido.append('<div class="centrado mb-1"> <b>VIAGEM</b> </div >');
        }

        if (pedido.idCliente !== null && pedido.idCliente !== "") {
            comprobantePedido.append('<div class="centrado"> ' + pedido.cliente + ' </div >');
        }

        if (pedido.idDireccion !== null && pedido.idDireccion !== "") {
            comprobantePedido.append('<div class="centrado"> ' + pedido.direccion + ' </div >');
        }

        if (pedido.idMesa !== null && pedido.idMesa !== "") {
            comprobantePedido.append('<div class="centrado"> Mesa: ' + pedido.idMesa + ' </div >');
        }

        if (pedido.idAplicativo !== null && pedido.idAplicativo !== "") {
            comprobantePedido.append('<div class="centrado"> Mesa: ' + pedido.idMesa + ' </div >');
        }

        //LISTA PRODUSTOS

        $.each(pedido.productos, function (index, producto) {

            comprobantePedido.append('<hr style="margin:2px"/>');

            var TABLE = $('<table>');
            TABLE.empty();

            var TD1 = $('<td style="vertical-align:top;">');
            var TD2 = $('<td style="width:100%">');
            var TD3 = $('<td style="vertical-align:top;">');
            var TR = $('<tr>');

            TD1.append('<div style="white-space:nowrap;vertical-align:top;"><b>*( ' + producto.cantidad + ' )</b ></div > ');

            var tamanho = "";

            if (producto.tamanhoSeleccionado != null && producto.tamanhoSeleccionado != "") {
                tamanho = '<div> Tamanho ' + producto.tamanhoSeleccionado + ' : R$ ' + producto.valorTamanhoSeleccionado.toFixed(2) + '</div>';

            } else {
                tamanho = '<div> Valor : R$ ' + producto.valor.toFixed(2) + '</div>';
            }

            TD2.append('<div style="display:block"><div>' + producto.nombre + '</div>' + tamanho + '</div>');
            TD3.append('<div style="white-space:nowrap;margin-left:10px"><b>R$ ' + producto.valorMasAdicionales.toFixed(2) + '</b></div>');

            TR.append(TD1, TD2, TD3);
            TABLE.append(TR);

            if (producto.adicionales.length > 0) {

                TABLE.append('<tr><td colspan="3"><b style="font-family: cursive;">Adicionais</b></td></tr>');

                //ADICIONALES
                var TABLE_ADICIONALES = $('<table style="width: 100%;">');
                TABLE_ADICIONALES.empty();
                $.each(producto.adicionales, function (index, adicional) {

                    let TD1 = $('<td style="vertical-align:top;">');
                    let TD2 = $('<td style="width:100%">');
                    let TD3 = $('<td>');
                    let TR = $('<tr>');

                    TD1.append('<div style="white-space:nowrap;vertical-align:top;">  <b>' + adicional.cantidad + '</b></div>');
                    TD2.append('<div>' + adicional.nombre + '</div>');
                    TD3.append('<div style="white-space:nowrap;margin-left:10px;">R$ ' + (adicional.valor * adicional.cantidad).toFixed(2) + '</div>');

                    TR.append(TD1, TD2, TD3);
                    TABLE_ADICIONALES.append(TR);
                });

                //ADD ADICIONLES TABLA PRODUCTO
                var TD_ADICIONALES = $('<td colspan="3">');
                TD_ADICIONALES.append(TABLE_ADICIONALES);

                var TR2 = $('<tr>');
                TR2.append(TD_ADICIONALES);
                TABLE.append(TR2);
            }

            if (producto.ingredientes.length > 0) {

                TABLE.append('<tr><td colspan="3"><b style="font-family: cursive;">Preparar sem:</b></td></tr>');

                //QUITAR INGREDIENTES
                var TABLE_INGREDIENTES = $('<table style="width: 100%;">');
                TABLE_INGREDIENTES.empty();
                $.each(producto.ingredientes, function (index, ingrediente) {

                    let TD1 = $('<td style="vertical-align:top;">');
                    let TR = $('<tr>');

                    TD1.append('<div> - ' + ingrediente.nombre + '</div>');

                    TR.append(TD1);
                    TABLE_INGREDIENTES.append(TR);
                });

                //ADD INGREDIENTES
                var TD_INGREDIENTES = $('<td colspan="3">');
                TD_INGREDIENTES.append(TABLE_INGREDIENTES);

                var TR3 = $('<tr>');
                TR3.append(TD_INGREDIENTES);
                TABLE.append(TR3);

            }

            if (producto.observacion.length > 0) {
                //ADD OBSERVACION
                var TD_OBSERBACION = $('<td colspan="3">');
                TD_OBSERBACION.append('<p style="margin:1px"> <b>Observação: </b>' + producto.observacion + ' </p>');

                var TR4 = $('<tr>');
                TR4.append(TD_OBSERBACION);
                TABLE.append(TR4);
            }

            comprobantePedido.append(TABLE);



        });

        //RESUMEN PAGAMENTO
        comprobantePedido.append('<hr style="margin:4px"/>');

        comprobantePedido.append('<div class="centrado">  ' +
            '               <table style="width:100%">  ' +
            '                   <tr>  ' +
            '                       <td style="width:50%;text-align:right"><b>Total a Pagar:</b></td>  ' +
            '                       <td style="width: 50%;text-align: left"> <b>R$ ' + (pedido.valorProductos - pedido.descuento + pedido.tasaEntrega).toFixed(2) + '</b></td>  ' +
            '                   </tr>  ' +
            '                   <tr>  ' +
            '                       <td style="width:50%;text-align:right">Descuento:</td>  ' +
            '                       <td style="width: 50%;text-align: left">R$ ' + (pedido.descuento).toFixed(2) + '</td>  ' +
            '                   </tr>  ' +
            '                   <tr>  ' +
            '                       <td style="width:50%;text-align:right">Tasa de Entrega:</td>  ' +
            '                       <td style="width: 50%;text-align: left">R$ ' + (pedido.tasaEntrega).toFixed(2) + '</td>  ' +
            '                   </tr>  ' +
            '               </table>  ' +
            '          </div> ');

        comprobantePedido.append('<div style="margin:4px"></div>');

        if (pedido.deliveryEmCartao) {
            comprobantePedido.append('<div class="centrado">  <strong>PAGAMENTO EM CARTÂO</strong> </div >');
        }

        if (pedido.deliveryPago) {
            comprobantePedido.append('<div class="centrado">  <strong>PAGO</strong> </div >');
        }

        if (pedido.deliveryEmdinheiro) {

            if (pedido.deliveryDinheiroTotal !== null && pedido.deliveryDinheiroTotal !== undefined) {
                comprobantePedido.append('<div class="centrado">   <strong>TROCO PARA R$ ' + pedido.deliveryDinheiroTotal.toFixed(2) + '</strong > </div > ');
            } else {
                comprobantePedido.append('<div class="centrado">   <strong>PAGAMENTO EM DINHEIRO (S/T)</strong> </div >');

            }
        }


        if (NombreEstablecimiento) {

            comprobantePedido.append('<hr style="margin:4px"/>');

            comprobantePedido.append('<div class="centrado"><b>' + NombreEstablecimiento + '</b></div>');

            if (TelefonoEstablecimiento) {
                comprobantePedido.append('<div><b>Telefone:</b>' + TelefonoEstablecimiento + '</div>');
            }

            if (DireccionEstablecimiento) {
                comprobantePedido.append('<p style="margin:1px;text-align: justify;"><b>Endereço:</b>' + DireccionEstablecimiento + '</p>');
            }

            if (CnpjEstablecimiento) {
                comprobantePedido.append('<div><b>CNPJ:</b>' + CnpjEstablecimiento + '</div>');
            }

        }

        comprobantePedido.append('<hr style="margin:4px"/>');
        comprobantePedido.append(' <p class="centrado"><b>¡obrigado por sua preferência!</b></p>');


    }

    cargarAsync().then(function () {

        $("#divComprobantePedido").printThis({
            debug: false,                   // show the iframe for debugging
            importCSS: true,                // import parent page css
            importStyle: true,             // import style tags
            printContainer: true,           // grab outer container as well as the contents of the selector
            loadCSS: "",      // path to additional css file - use an array [] for multiple
            pageTitle: "",                  // add title to print page
            removeInline: false,            // remove all inline styles from print elements
            //removeInlineSelector: "body *", // custom selectors to filter inline styles. removeInline must be true
            printDelay: 1,                // variable print delay
            header: null,                   // prefix to html
            footer: null,                   // postfix to html
            base: false,                    // preserve the BASE tag, or accept a string for the URL
            formValues: true,               // preserve input/form values
            canvas: true,                  // copy canvas elements
            // doctypeString: '<!DOCTYPE html>',           // enter a different doctype for older markup
            removeScripts: false,           // remove script tags from print content
            copyTagClasses: true,           // copy classes from the html & body tag
            beforePrintEvent: null,         // callback function for printEvent in iframe
            afterPrint: null                // function called before iframe is removed
        });

    });
    cargarAsync();

}