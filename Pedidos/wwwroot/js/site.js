// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    HubConnect();

})


// CHAT HUB

var chat;
var chatConnectionId;
var chatDisconnected = true;
var chatIntervelReconnect;

function HubConnect() {

    chat = new signalR.HubConnectionBuilder().withUrl('/cardapiohub').build();

    chat.start().then(function () {

        chatDisconnected = false;

        //GET CONNECTION ID
        chat.invoke('getConnectionId').then((data) => {
            chatConnectionId = data;
        });

        setInterval(function () {
            if (chatDisconnected) {
                chatReconnect();
            }
        }, 3000);

    });

    chat.onclose(() => {
        chatDisconnected = true;
    });

    //----------- FUNTIONS ------------
    //SEND
    $('#btnEstablecimientoSendMessage').on('click', function () {
        establecimientoSendMessage();
    });

    //RECIVED
    chat.on("serverReceivedMessage", function (message) {

        guardaMensaje(message);

        ion.sound({
            sounds: [
                { name: "beyond_doubt" }
            ],

            path: location.origin + "/ionsound/sounds/",
            preload: true,
            multiplay: true,
            volume: 0.5
        });

        ion.sound.play("beyond_doubt");
    });

    chat.on("serverReceivedProducto", function (producto) {

        guardaProductoPendiente(producto);
        ion.sound({
            sounds: [
                { name: "beyond_doubt" }
            ],

            path: location.origin + "/ionsound/sounds/",
            preload: true,
            multiplay: true,
            volume: 0.2
        });

        ion.sound.play("beyond_doubt");
    });

}

async function chatReconnect() {
    await chat.start().then(function () {

        chatDisconnected = false;
        //GET CONNECTION ID
        chat.invoke('getConnectionId').then((data) => {
            chatConnectionId = data;
        });

        clearInterval(chatIntervelReconnect);

        chatIntervelReconnect = setInterval(function () {
            if (chatDisconnected) {
                chatReconnect();
            }
        }, 3000);

        console.log('reconected success');
    });
}

var MensajesSinLeer = 0;

function guardaMensaje(mensaje) {

    $.ajax({
        type: "POST",
        url: "/Pedidos/GuardarMensaje",
        traditional: true,
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {
            CargarGruposMensajes();
            ChatAddMessageCliente(message);
        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

}

function guardaProductoPendiente(producto) {

    $.ajax({
        type: "POST",
        url: "/Pedidos/CardapioGuardarProductoPendiente",
        traditional: true,
        data: producto,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            pedidosCardapioAddProductoPendiente(data.producto, data.index);
        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

}

function establecimientoSendMessage() {

    var newMessage = {
        codigoConeccionCliente: "cli_acc5_1",
        idCuenta: 1,
        mesa: 1,
        nombreCliente: "Royber",
        message: $('#inputEstablecimientoMessage').val(),
        position: "float-right",
        color: "bg-success",
        margin: "ml-5",
        clientSend: false,
        cuentaSend: true
    };

    $('#inputEstablecimientoMessage').focus();

    chat.invoke('establecimientoSendMessage', 'cli_acc5_1', JSON.stringify(newMessage))
        .then((res) => {
            console.log('establecimientoSendMessage', res);

            ChatAddMessageCliente(newMessage);
        })
        .catch(err => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Erro ao conectar com o cliente!',
                footer: '<a>Verifique que está conectado à internet</a>'
            });
        });
    $('#inputEstablecimientoMessage').val("");

}

function ChatAddMessageCliente(msg) {

    MensajesSinLeer++;
    $('#spanCountSMS').html(MensajesSinLeer);

    var tableChatCardapioMensajesCliente = $('#tableChatCardapioMensajesCliente');

    if (msg.clientSend) {
        msg.position = "float-left";
        msg.margin = "mr-5";
        msg.color = "border-secondary";
    } else {
        msg.position = "float-right";
        msg.margin = "ml-5";
        msg.color = "border-success";
    }

    tableChatCardapioMensajesCliente.append('<tr style="display: grid;">  ' +
        '                           <td style="font-size: 13px">  ' +
        '                               <div class="alert ' + msg.color + ' p-1 text-black m-0 ml-1 mr-1 ' + msg.position + ' ' + msg.margin + ' " style="display:inline-grid">  ' +
        '                                   ' + msg.message +
        '                               </div>  ' +
        '                           </td>  ' +
        '                      </tr>  ');


    $('#tableChatCardapioMensajesCliente').animate({ scrollTop: 1000000 }, 500);

}

var productosPendientesCount = 0;

function pedidosCardapioAddProductoPendiente(producto, index) {

    productosPendientesCount++;
    $('#spanProductosPendientesCount').html(productosPendientesCount);

    var tableChatCardapioPedidosPendientes = $('#tableChatCardapioPedidosPendientes');

    tableChatCardapioPedidosPendientes.append('<tr style="display:grid" > ' +
        '                               <td>  ' +
        '                                   <div class="card">  ' +
        '                                       <div class="card-header">  ' +
        '                                           <div class="d-flex">  ' +
        '                                               <img src="../../img/client.png" style="width:25px;height:25px" class="mr-2" />  ' +
        '                                               <div><strong>' + producto.cliente + ' | </strong></div>  ' +
        '                                               <div class="ml-1"><small>Mesa ' + producto.idMesa + '</small></div>  ' +
        '                                           </div>  ' +
        '                                       </div>  ' +
        '                                       <div class="card-body bg-white w-100" id="card_index_' + index + '_' + producto.codigoConeccionCliente + '">  ' +

        '                                       </div>  ' +
        '                                   </div>  ' +
        '                               </td>  ' +
        '                          </tr>  ');

    //LISTA DE PRODUCTOS
    var TABLA_PRD = $('<table cellspacing="0" class="table-tr-border-radius unselectable" style="font-size: 13px;">');

    let pedido = {};
    pedido.id = producto.codigoConeccionCliente;

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
    var productoTiempoPedido = '<div id="divTiempoPedido_' + pedido.id + '_' + index + '_' + producto.id + '" class="border border-warning rounded p-1 d-flex align-items-center mr-1 ml-1" style="font-size: 10px;color: darkcyan;font-weight: 700;"> <i class="far fa-clock mr-1"></i> <span id="minutesPedido_' + pedido.id + '_' + index + '_' + producto.id + '"></span>: <span id="secondsPedido_' + pedido.id + '_' + index + '_' + producto.id + '"></span>  </div>';

    if (producto.fecha_preparado !== null) {
        let minutesPedido_ = (tiempopedido / 60).toFixed(0);
        let secondsPedido_ = tiempopedido % 60;
        productoTiempoPedido = '<div id="divTiempoPedido_' + pedido.id + '_' + index + '_' + producto.id + '" class="border border-success rounded p-1 d-flex align-items-center mr-1 ml-1" style="font-size: 10px;color: black;font-weight: 700;"> <i class="far fa-clock mr-1"></i> <span id="minutesPedido_' + pedido.id + '_' + index + '_' + producto.id + '">' + minutesPedido_ + '</span>: <span id="secondsPedido_' + pedido.id + '_' + index + '_' + producto.id + '">' + secondsPedido_ + '</span>  </div>';
    }


    var TR1_PRD = $('<tr>');
    var TD1_PRD = $('<td style="width:100%">');
    var TD2_PRD = $('<td>');

    TD1_PRD.append('<div class="d-flex"><div style="text-align: start;" ' + Desplegar + '>  (<b>' + producto.cantidad + '</b>) ' + producto.nombre.toUpperCase() + btnInfo + '</div> ' + productoTiempoPedido + ' </div><div style="color: gray;text-align: start;">' + producto.observacion + '</div>');
    TD2_PRD.append('<div style="font-size:12px;width:70px;text-align:end;" class="cursor-pointer"><strong> R$ ' + producto.valorMasAdicionales.toFixed(2) + '</strong></div>');
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

    TR2_PRD.append(TD1_PRD);

    //ADD TRS A LA TABLA
    TABLA_PRD.append(TR0_PRD, TR1_PRD, TR2_PRD);



    $('#card_index_' + index + '_' + producto.codigoConeccionCliente).html(TABLA_PRD);

    $('#tableChatCardapioPedidosPendientes').animate({ scrollTop: 1000000 }, 500);



}


