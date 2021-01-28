var chatConnectionId;

$(function () {
    HubConnect()
});

function HubConnect() {

    chat = new signalR.HubConnectionBuilder().withUrl('/cardapiohub').build();

    chat.start().then(function () {

        chat.invoke('getConnectionId').then((data) => {
            chatConnectionId = data;
        });

        $('#btnSendMessageChat').on('click', function () {
            chat.invoke('chatSendMessage', chatConnectionId, $('#inputMessageChat').val());
        });

        chat.on("receivedMessage", function (message) {
            $('#divChat').append($('<div />').html(message));
            $('#inputMessageChat').val("");
            $('#inputMessageChat').focus();

        });


    });

}