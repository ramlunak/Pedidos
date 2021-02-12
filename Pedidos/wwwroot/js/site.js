// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    HubConnect();

    // var FileProvider = Directory.GetCurrentDirectory();   
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
        msg.color = "bg-secondary";
    } else {
        msg.position = "float-right";
        msg.margin = "ml-5";
        msg.color = "bg-success";
    }
   
    tableChatCardapioMensajesCliente.append('   <tr>  ' +
        '                           <td style="font-size: 13px">  ' +
        '                               <div class="alert ' + msg.color + ' p-1 text-white m-0 ml-1 mr-1 ' + msg.position + ' ' + msg.margin + ' " style="display:inline-grid">  ' +
        '                                   ' + msg.message +
        '                               </div>  ' +
        '                           </td>  ' +
        '                      </tr>  ');


    $('#tableChatCardapioMensajesCliente').animate({ scrollTop: 1000000 }, 500);

}

function CuentaSendMessage() {

    var newMessage = {
        idCliente: 1,
        idCuenta: 1,
        mesa: 1,
        titulo: "Royber | Mesa 1",
        message: $('#inputClienteMessage').val(),
        position: "float-right",
        color: "bg-success",
        margin: "ml-5",
        clientSend: false,
        cuentaSend: true
    };

    ChatAddMessage(JSON.stringify(newMessage));
    $('#inputClienteMessage').focus();

    chat.invoke('CuentaSendMessage', chatConnectionId, $('#inputIdCuenta').val(), $('#inputMesa').val(), JSON.stringify(newMessage))
        .then((res) => {
            var asdasd = 'dd';
            console.log(asdasd);
        })
        .catch(err => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Erro ao conectar com o estabelecimento!',
                footer: '<a>Verifique se seu telefone está conectado à internet</a>'
            });
        });
    $('#inputClienteMessage').val("");

}

