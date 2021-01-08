
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

            $.ajax({
                type: "GET",
                url: "/Pedidos/DeletePruducto/" + parseInt(id),
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    _CurrentPedido.productos.splice(_CurrentPedido.productos.findIndex(x => x.id === id), 1);
                    TABLE_PedidoProductos();

                },
                failure: function (response) {
                    console.log('failure', response);
                },
                error: function (response) {
                    console.log('error', response);

                }
            });


        }
    })
}