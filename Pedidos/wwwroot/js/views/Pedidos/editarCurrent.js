
$(function () {
      

});

function editarCurrentProducto(id) {
    ShowDetallesProducto(id);
}

function deleteCurrentProducto(id) {
    Swal.fire({
        title: 'Tem certeza que deseja deletar o produto?',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim!',
        cancelButtonText: 'Não'
    }).then((result) => {
        if (result.isConfirmed) {

            _CurrentPedido.productos.splice(_CurrentPedido.productos.findIndex(x => x.id === id), 1);
            TABLE_PedidoProductos();
        }
    })
}