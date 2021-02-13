
$(function () {

    $("#chatModal").on('shown.bs.modal', function () {
        $('#tableChatCardapioMensajesCliente').animate({ scrollTop: 1000000 }, 500);
    });

    $('#inputEstablecimientoMessage').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            establecimientoSendMessage();
            return false;
        }
    });

});

function CargarGruposMensajes() {

    $.ajax({
        type: "GET",
        url: "/Pedidos/CargarGruposMensajes/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (grupos) {
            MostarGruposMensajes(grupos);
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

function MostarGruposMensajes(grupos) {

    var TABLE = $('#tableChatCardapioGruposClientes');
    TABLE.empty();

    $.each(grupos, function (index, item) {

        console.log(item);

        var TR = $('<tr style="display:grid">');
        var TD1 = $('<td>');

        TD1.append('<div class= "card hover cursor-pointer" > ' +
            '                                                       <div class="card-body bg-white w-100 p-1">  ' +
            '                                                           <div class="d-flex">  ' +
            '                                                               <div>  ' +
            '                                                                   <img src="../../img/client.png" style="width:40px" />  ' +
            '                                                               </div>  ' +
            '                                                               <div class="d-block ml-2">  ' +
            '                                                                   <div><strong>' + item.nombreCliente + '</strong></div>  ' +
            '                                                                   <div><small>Mesa ' + item.mesa + '</small></div>  ' +
            '                                                               </div>  ' +
            '                                                           </div>  ' +
            '                                                       </div>  ' +
            '</div> ');

        TR.append(TD1);
        TABLE.append(TR);

    });

}