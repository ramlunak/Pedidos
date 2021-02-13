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

        // alert(message);
        ion.sound({
            sounds: [
                { name: "beyond_doubt" }
            ],

            // main config
            path: location.origin + "/ionsound/sounds/",
            preload: true,
            multiplay: true,
            volume: 1
        });

        // play sound
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

function establecimientoSendMessage() {

    var newMessage = {
        idCuenta: 1,
        mesa: 1,
        titulo: "Royber | Mesa 1",
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

