$(function () {

    $(".hide-loading").hide(); // Hide loading

    //CAMBIAR ESTADO DE LOS ADICIONAIS
    $("input[name='switchbuttonSelected']").change(function (e) {
        e.preventDefault();

        var checked = $(this).prop('checked');
        var inputId = $(this).prop('id');
        var loadingId = "loading" + inputId;
        var ids = inputId.split("_");
        var idAdicional = ids[0];
        var idCategoria = ids[1];

        var data = {
            idAdicional: parseInt(idAdicional),
            idCategoria: parseInt(idCategoria),
            selected: checked
        }

        //Show Loading
        $("#" + loadingId).show(); 

        $.ajax({
            type: "POST",
            url: "/Adicionais/UpdateCategoriaInAdicionalCategorias",
            traditional: true,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
               
                console.log(result);

                //Hide Loading              
                setTimeout(function () {
                    $("#" + loadingId).hide();
                }, 500)
            },
            failure: function (response) {
                console.log('failure', response);
                // input[0].switchButton('on');
              

                //Hide Loading              
                setTimeout(function () {
                    $("#" + loadingId).hide(); 
                }, 500)

                location.reload();
            },
            error: function (response) {
                console.log('error', response);
                // input[0].switchButton('on');
              
                //Hide Loading              
                setTimeout(function () {
                    $("#" + loadingId).hide();
                }, 500)

                location.reload();
            }
        });

    })

});

function AddCategoriaInAdicionalCategorias(idAdicional, idCategoria) {
    console.log(idAdicional, idCategoria);


}


function DeleteCategoriaInAdicionalCategorias(idAdicional, idCategoria) {

    var data = {
        idAdicional: parseInt(idAdicional),
        idCategoria: parseInt(idCategoria)
    }

    $.ajax({
        type: "POST",
        url: "/Adicionais/DeleteCategoriaInAdicionalCategorias",
        traditional: true,
        data: JSON.stringify(data),
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
}