var hubConnectionId;


$(function () {

    var conecction = new signalR.HubConnectionBuilder().withUrl('/cardapiohub',).build();

    conecction.start().then(function () {

        //Cargar hubConnectionId
        conecction.invoke('serverGetConnectionId').then(
            (data) => {               
                this.hubConnectionId = data;
            }
        );

        //INVOCAR METODOS AL SERVIDOR
        $('#btnSalvarPedido').on('click', function () {
            conecction.invoke('serverAbrirMesa', "1","sad");
        });

        //EJECUTAR FUNCION DEL CLIENTE
        conecction.on("clientAbrirMesa", function (idCuenta, valor) {
            console.log(idCuenta, valor);
        });
              

    });
       
});