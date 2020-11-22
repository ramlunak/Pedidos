$(function () {

    $(document).keyup(function (e) {

        //inputNome ENTER
        if ($("#inputNome").is(":focus") && (e.keyCode == 13)) {
            $("#inputTelefone").focus();
        }
        else if ($("#inputTelefone").is(":focus") && (e.keyCode == 13)) {
                $("#inputEndereco").focus();
        }
        else if ($("#inputEndereco").is(":focus") && (e.keyCode == 13)) {
            $("#inputProducto").focus();
        }
        
    });

});