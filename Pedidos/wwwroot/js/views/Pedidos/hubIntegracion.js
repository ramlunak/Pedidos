
function EnviarPedidoIntegracion() {
    chat.invoke('enviarPedidoIntegracion',12,"").then((data) => {
        console.log(data);
    });
}

function CancelarPedidoIntegracion() {
    chat.invoke('cancelarPedidoIntegracion',12,"").then((data) => {
        console.log(data);
    });
}