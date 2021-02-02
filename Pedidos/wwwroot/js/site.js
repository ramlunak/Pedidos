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
