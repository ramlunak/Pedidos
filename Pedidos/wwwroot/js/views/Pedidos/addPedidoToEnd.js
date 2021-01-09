
function addPedidoToEnd(pedido) {

    //Eliminar pedido de la lista
    $('#CARD_PEDIDO_' + pedido.id + '').remove();

    //Agregar al final de la tabla

    var TABLE = $('#ListaPedidosPendientes');
    var CARD = $('<div id="CARD_PEDIDO_' + pedido.id + '" class="card mb-2">');
    var CARD_BODY = $('<div class="card-body  p-1">');

    //INFO DEL PEDIDO 
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
        '                   <div style="font-size:13px">' + pedido.cliente + '</div>  ' +
        '                   <div style="font-size:13px">' + pedido.direccion + '</div>  ' + mesa + aplicativo +
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

        var btnInfo = ' <button type="button" class="btn btn-sm btn-outline-primary" style="font-size:11px">+ Info</button></div><div style="text-align: start;color: cadetblue;">';

        if (producto.adicionales.length == 0 && producto.ingredientes.length == 0) {
            Desplegar = "";
            btnInfo = "";
        }

        // CONTADOR
        var TR0_PRD = $('<tr>');

        var sec = pedido.tiempo_pedido;
        function pad(val) { return val > 9 ? val : "0" + val; }

        //setInterval(function () {
        //    $('#seconds_' + pedido.id + '_' + index + '_' + producto.id + '').html(pad(++sec % 60));
        //    $('#minutes_' + pedido.id + '_' + index + '_' + producto.id + '').html(pad(parseInt(sec / 60, 10)));
        //}, 1000);

        var div_conter_style = 'style="text-align: start;font-size: 11px !important;color: gray;color: mediumorchid;"';
        // TR0_PRD.append('<td colspan="2"><div ' + div_conter_style + ' > <span id="minutes_' + pedido.id + '_' + index + '_' + producto.id + '"></span>: <span id="seconds_' + pedido.id + '_' + index + '_' + producto.id + '"></span></div></td>');
        //FIN

        var TR1_PRD = $('<tr>');
        var TD1_PRD = $('<td style="width:100%">');
        var TD2_PRD = $('<td>');


        TD1_PRD.append('<div class="d-block"><div style="text-align: start;" ' + Desplegar + '>  (<b>' + producto.cantidad + '</b>) ' + producto.nombre.toUpperCase() + btnInfo + producto.observacion + '</div></div>');
        TD2_PRD.append('<div style="font-size:12px;width:70px;text-align:end;" class="cursor-pointer"> R$ ' + producto.valor.toFixed(2) + '</div>');
        TR1_PRD.append(TD1_PRD, TD2_PRD);


        //ADICIONALES E INGREDIENTES DEL PRODUCTO
        var TR2_PRD = $('<tr>');
        var TD1_PRD = $('<td style="width:100%" colspan="2">');

        var TABLA_ADIC = $('<table class="w-100 unselectable">');
        $.each(producto.adicionales, function (index, item) {

            var TD1 = $('<td style="width:100%">');
            var TD2 = $('<td>');
            var TR = $('<tr>');

            TD1.append('<div class="unselectable" style="text-align:start;"><a style="color:blue;">+' + item.cantidad + '</a> ' + item.nombre + '</div>');
            TD2.append('<div class="unselectable" style="width:70px;text-align:end;font-size: 13px;">R$ ' + (item.Valor * item.cantidad).toFixed(2) + '</div>');

            TR.append(TD1, TD2);
            TABLA_ADIC.append(TR);
        });

        var TABLA_INGD = $('<table class="w-100 unselectable">');
        $.each(producto.ingredientes, function (index, item) {
            console.log(item);
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
        TR2_PRD.append(TD1_PRD);

        //ADD TRS A LA TABLA
        TABLA_PRD.append(TR0_PRD, TR1_PRD, TR2_PRD);

    });

    //AGREGAR PRODUCTOS
    CARD_BODY.append(TABLA_PRD);

    //FUTTER
    var CARD_FUTTER = $(' <div class="card-footer text-muted p-2 d-flex justify-content-between ">');

    var futter_botones = $('<div class="d-flex">');
    futter_botones.append('<a onclick="finalizado(' + pedido.id + ')" class="btn btn-sm btn-info cursor-pointer" style="color:white">finalizar</a>');
    CARD_FUTTER.append(futter_botones);

    CARD_FUTTER.append('<a class="btn btn-outline-secondary" href="/Pedidos/Print/' + pedido.id + '" target="_blank"><i class="fa fa-print cursor-pointer float-right" aria-hidden="true" ></i></a>');

    CARD.append(CARD_BODY);
    CARD.append(CARD_FUTTER);

    var TD1 = $('<td style="width:100%">');
    var TR = $('<tr>');
    TD1.append(CARD);

    TR.append(TD1);
    TABLE.append(TR);

}
