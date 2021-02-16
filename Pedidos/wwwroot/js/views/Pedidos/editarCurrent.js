
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
                success: function (currentPedido) {
                    _CurrentPedido = currentPedido;
                    MostarCurrentPedido();
                },
                failure: function (response) {
                    Swal.fire(
                        'Error!',
                        'Erro de servidor.' + response,
                        'error'
                    )
                },
                error: function (response) {
                    Swal.fire(
                        'Error!',
                        'Erro de servidor.' + response,
                        'error'
                    )

                }
            });


        }
    })
}