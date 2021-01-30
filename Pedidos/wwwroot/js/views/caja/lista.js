
$(function () {
    $(".inputDecimal").mask("###0.00", { reverse: true });
    $('#divLoadingAbrirCaija').hide();
});

function AbrirCaja() {

    var valor = $('#txtValorInicial').val();

    if (valor === null || valor === "") {
        Swal.fire({
            icon: 'info',
            title: 'Oops...',
            text: 'Informe o valor inicial por favor!'
        });
        return;
    }

    $('#txtValorInicial').val("");
    $('#divBotonesCaija').hide();
    $('#divLoadingAbrirCaija').show();

    $.ajax({
        type: "GET",
        url: "/Caja/AbrirCaja?valor=" + parseFloat(valor),
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $('#divLoadingBotonesCaija').show();
            $('#divLoadingAbrirCaija').hide();

            if (data.open) {

                Swal.fire({
                    icon: 'success',
                    title: 'A caixa foi aberta corretamente',
                    showConfirmButton: false,
                    timer: 1500
                }).then((result) => {
                    $('#modalAbrirCaja').modal('hide');
                    location.reload();
                })

            } else {

                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: data.erro
                }).then((result) => {
                    $('#modalAbrirCaja').modal('hide');
                    location.reload();
                });

            }
                       

        },
        failure: function (response) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: JSON.stringify(response)
            });

            $('#divLoadingBotonesCaija').show();
            $('#divLoadingAbrirCaija').hide();
        },
        error: function (response) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: JSON.stringify(response)
            });

            $('#divLoadingBotonesCaija').show();
            $('#divLoadingAbrirCaija').hide();
        }
    });

}
