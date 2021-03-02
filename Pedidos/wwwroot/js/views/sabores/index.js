
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
            url: "/Sabores/ChangeStatus/" + $(this).prop('id') + "",
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
