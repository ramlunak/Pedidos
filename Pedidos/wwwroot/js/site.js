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

function HubConnect() {

    chat = new signalR.HubConnectionBuilder().withUrl('/cardapiohub').build();

    chat.start().then(function () {

        //GET CONNECTION ID
        chat.invoke('getConnectionId').then((data) => {
            chatConnectionId = data;
        });

        //RECIVED
        chat.on("serverReceivedMessage", function (message) {
            // alert(message);
            ion.sound({
                sounds: [
                    { name: "beyond_doubt" },
                    { name: "bell_ring" },
                    { name: "branch_break" },
                    { name: "button_click" }
                ],

                // main config
                path: "ion-sound/sounds/",
                preload: true,
                multiplay: true,
                volume: 0.5
            });

            // play sound
            ion.sound.play("beyond_doubt", {
                loop: 1
            });
        });


    });

}