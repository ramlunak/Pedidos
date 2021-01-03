
$(function () {

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

    //OCULTAR EN APLICACIONES DE DELIVERY
    $("input[name='switchbuttonHideApp']").change(function (e) {
        e.preventDefault();

        $.ajax({
            type: "GET",
            url: "/FormaPagamento/HideApp/" + $(this).prop('id') + "",
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