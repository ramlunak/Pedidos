
$(function () {

    $("#inputTasa").mask("###0.00", { reverse: true });

    //CAMBIAR ESTADO 
    $("input[name='switchbuttonActivo']").change(function (e) {
        e.preventDefault();

        $.ajax({
            type: "GET",
            url: "/FormaPagamento/ChangeStatus/" + $(this).prop('id') + "",
            traditional: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
            },
            failure: function (response) {
                console.log('failure', response);
                location.reload();
            },
            error: function (response) {
                console.log('error', response);
                location.reload();

            }
        });
    })

});

function addFormaPagamento(idAplicativo) {

    var formaPagamento = {
        idAplicativo: idAplicativo,
        nombre: $('#inputNome').val(),
        tasa: parseFloat($('#inputTasa').val())
    };

    if (formaPagamento.nombre === null || formaPagamento.nombre === undefined || formaPagamento.nombre === "") {
        Swal.fire('Oops...', 'Complete os dados do formulário!', 'error'); return;
    }

    if (isNaN(formaPagamento.tasa)) {
        Swal.fire('Oops...', 'Complete os dados do formulário!', 'error'); return;
    }

    if (formaPagamento.tasa <= 0) {
        Swal.fire('Oops...', 'O valor da Taxa tem que ser maior que 0.00', 'error'); return;
    }

    console.log(formaPagamento);

    $.ajax({
        type: "POST",
        url: "/Aplicativo/AddFormaPagamento",
        traditional: true,
        data: JSON.stringify(formaPagamento),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            console.log(result);
            //$('#ModalDetalleProducto').modal('hide');
            //_CurrentPedido = result.currentPedido;
            //MostarCurrentPedido();
            location.reload();
        },
        failure: function (response) {
            console.log('failure', response);
            Swal.fire('Oops...', 'Erro de servidor', 'error'); return;
        },
        error: function (response) {
            console.log('error', response);
            Swal.fire('Oops...', 'Erro de servidor', 'error'); return;
        }
    });
}

function edit(item) {
    console.log(item);
}

//function deleteFormaPagamento(id) {
//    Swal.fire({
//        title: 'Do you want to save the changes?',        
//        showCancelButton: true,
//        confirmButtonText: `Apagar`,
//        denyButtonText: `Cancelar`,
//    }).then((result) => {
//        /* Read more about isConfirmed, isDenied below */
//        if (result.isConfirmed) {

//            $.ajax({
//                type: "GET",
//                url: "/Aplicativo/DeleteFormaPagamento/" + id,
//                traditional: true,
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    console.log(data);
//                    location.reload();
//                },
//                failure: function (response) {
//                    console.log('failure', response);
//                    Swal.fire('Erro do servidor', '', 'erro')
//                },
//                error: function (response) {
//                    console.log('error', response);
//                    Swal.fire('Erro do servidor', '', 'erro')
//                }
//            });
//        }
//    })
//}
