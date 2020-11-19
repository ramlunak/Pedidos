
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

    $.each(productos, function (index, item) {

        //INFO PRODUCTO
        var TD1_Content = $('<div class="d-block">');
        TD1_Content.append('<div>' + item.nombre + '</div>');
        if (item.descripcion !== null) {

            TD1_Content.append('<div style="font-size:11px">' + item.descripcion + '.</div>');
        }
        TD1_Content.append('<div>R$ ' + item.valor.toFixed(2) + '</div>');
        //--

        //IMAGEN 
        var TD2_Content = $('<div>');
        TD2_Content.append('<img style="max-height:70px;max-width:70px;object-fit:cover;border-radius:5px" src="data:image/png;base64,' + item.imagen + '" data-toggle="modal" />');
        //--

        var TD1 = $('<td style="width:100%">').append(TD1_Content);
        var TD2 = $('<td style="width:auto">').append(TD2_Content);
        var tr = $('<tr>').append(TD2,TD1);

        tabla.append(tr);

    });
}