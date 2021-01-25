
$(function () {

    //CAMBIAR ESTADO DE LOS ADICIONAIS
    $("input[name='switchbuttonActivo']").change(function (e) {
        e.preventDefault();
        //  console.log($(this).prop('id'));
        console.log('activo');
        var oldValue = $(this).prop('checked');
        var input = $(this);
        console.log(oldValue);

        $.ajax({
            type: "GET",
            url: "/Adicionais/ChangeStatus/" + $(this).prop('id') + "",
            traditional: true,
            // data: JSON.stringify({id:1}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                // window.location.href = '/AlumnoPrueba/Resultado/';
                console.log(result);
            },
            failure: function (response) {
                console.log('failure', response);
                // input[0].switchButton('on');
                location.reload();
            },
            error: function (response) {
                console.log('error', response);
                // input[0].switchButton('on');
                location.reload();

            }
        });

    })

    //CAMBIAR VISIBILIDAD DE LOS ADICIONAIS
    $("input[name='switchbuttonParaTodos']").change(function (e) {
        e.preventDefault();
        //  console.log($(this).prop('id'));
        console.log('para todos');
        var oldValue = $(this).prop('checked');
        var input = $(this);
        console.log(oldValue);

        $.ajax({
            type: "GET",
            url: "/Adicionais/ChangeVsibilidad/" + $(this).prop('id') + "",
            traditional: true,
            // data: JSON.stringify({id:1}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                // window.location.href = '/AlumnoPrueba/Resultado/';
                console.log(result);
            },
            failure: function (response) {
                console.log('failure', response);
                // input[0].switchButton('on');
                location.reload();
            },
            error: function (response) {
                console.log('error', response);
                // input[0].switchButton('on');
                location.reload();

            }
        });

    })

});

function showInputOrden(idAdicional) {
    var labelOrden = $('#labelOrden_' + idAdicional);
    var inputOrden = $('#inputOrden_' + idAdicional);
    labelOrden.hide();
    inputOrden.show();
    inputOrden.focus();
    inputOrden.select();
}

function editarOrden(idAdicional) {
    var labelOrden = $('#labelOrden_' + idAdicional);
    var inputOrden = $('#inputOrden_' + idAdicional);

    $.ajax({
        type: "GET",
        url: "/Adicionais/EditarOrden?id=" + parseInt(idAdicional) + "&orden=" + inputOrden.val(),
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            inputOrden.hide();
            labelOrden.show();
            location.reload();
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });

}