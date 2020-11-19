﻿
$(function () {

    $('.collapseCategoria').on('show.bs.collapse', function () {
        var collapseId = $(this).attr("id");
        var idCategoria = collapseId.split('_')[1];

        $.ajax({
            type: "GET",
            url: "/Cardapio/GetProductos/" + parseInt(idCategoria) + "",
            traditional: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                // window.location.href = '/AlumnoPrueba/Resultado/';
                console.log(data);
                cargarProductosCategoria(parseInt(idCategoria), data);
            },
            failure: function (response) {
                console.log('failure', response);
            },
            error: function (response) {
                console.log('error', response);

            }
        });

    })

});

function cargarProductosCategoria(idCategoria, productos) {

    var tabla = $('#tableProductosCategoria_' + idCategoria);
    tabla.empty();

    console.log(productos.length);
    if (productos.length > 0) {
        $.each(productos, function (index, item) {

            //INFO PRODUCTO
            var TD1_Content = $('<div class="d-block">');
            TD1_Content.append('<div style="font-size: 15px;"><b>' + item.nombre + '</b></div>');
            if (item.descripcion !== null) {

                TD1_Content.append('<div style="font-size:14px;font-style: italic;">' + item.descripcion + '.</div>');
            }
            TD1_Content.append('<div style="color: green;font-weight: 600;font-size:14px;">R$ ' + item.valor.toFixed(2) + '</div>');
            //--

            //IMAGEN 
            var TD2_Content = $('<div>');
            if (item.imagen !== null) {
                TD2_Content.append('<img style="max-height:70px;max-width:70px;object-fit:cover;border-radius:5px" src="data:image/png;base64,' + item.imagen + '" data-toggle="modal" />');
            }
            //--

            var TD1 = $('<td style="width:100%">').append(TD1_Content);
            var TD2 = $('<td style="width:auto">').append(TD2_Content);
            var tr = $('<tr>').append(TD2, TD1);

            tabla.append(tr);

        });
    }
    else {
        var TD1_Content = $('<div class="d-block">');
        var TD1 = $('<td style="width:100%;text-align: center;">').append('<div style="color:red">Não há produtos para mostrar</div>');
        var tr = $('<tr>').append(TD1);
        tabla.append(tr);
    }
}