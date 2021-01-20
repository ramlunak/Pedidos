var cardapioIdCuenta;
var cardapioMesa;

$(function () {

    cardapioIdCuenta = $('#inputIdCuenta').val();
    cardapioMesa = $('#inputMesa').val();
    console.log(cardapioIdCuenta, cardapioMesa);

    $('.collapseCategoria').on('show.bs.collapse', function () {

        var collapseId = $(this).attr("id");
        var idCategoria = collapseId.split('_')[1];
        var idCuenta = collapseId.split('_')[2];

        let categoria = {
            id: parseInt(idCategoria),
            idCuenta: parseInt(idCuenta),
        };

        $.ajax({
            type: "POST",
            url: "/Cardapio/GetProductos",
            traditional: true,
            data: JSON.stringify(categoria),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                cargarProductosCategoria(parseInt(idCategoria), parseInt(idCuenta), result);
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

function cargarProductosCategoria(idCategoria, idCuenta, productos) {

    var TABLE = $('#tableProductosCategoria_' + idCategoria + '_' + idCuenta);
    TABLE.empty();

    $.each(productos, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td style="width:auto">');
        var TR = $('<tr class="hover" onclick="ShowDetallesProducto(' + item.id + ')">');

        TD1.append('<div><b>' + item.nombre + '</b></div>');
        var divTamanhos = $('<div style="display: flex">');

        if (item.valor !== 0)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex"> <div class="ml-1" style="color:chartreuse">R$ ' + item.valor + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho1 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex">' + item.tamanho1 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho1.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho2 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex">' + item.tamanho2 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho2.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho3 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex">' + item.tamanho3 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho3.toFixed(2) + '</div> </div>');
        TD1.append(divTamanhos);

        TD2.append('<div class="d-flex align-items-center unselectable" style="font-size:22px;color:green;font-weight: 600;cursor:pointer">+</div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);

    });

    //var tabla = $('#tableProductosCategoria_' + idCategoria + '_' + idCuenta);
    //tabla.empty();

    //console.log(productos.length);
    //if (productos.length > 0) {
    //    $.each(productos, function (index, item) {

    //        //INFO PRODUCTO
    //        var TD1_Content = $('<div class="d-block">');
    //        TD1_Content.append('<div style="font-size: 15px;"><b>' + item.nombre + '</b></div>');
    //        if (item.descripcion !== null) {

    //            TD1_Content.append('<div style="font-size:14px;font-style: italic;">' + item.descripcion + '.</div>');
    //        }
    //        TD1_Content.append('<div style="color: green;font-weight: 600;font-size:14px;">R$ ' + item.valor.toFixed(2) + '</div>');
    //        //--

    //        //IMAGEN 
    //        var TD2_Content = $('<div>');
    //        if (item.imagen !== null) {
    //            TD2_Content.append('<img style="max-height:70px;max-width:70px;object-fit:cover;border-radius:5px" src="data:image/png;base64,' + item.imagen + '" data-toggle="modal" />');
    //        }
    //        //--

    //        var TD1 = $('<td style="width:100%">').append(TD1_Content);
    //        var TD2 = $('<td style="width:auto">').append(TD2_Content);
    //        var tr = $('<tr>').append(TD2, TD1);

    //        tabla.append(tr);

    //    });
    //}
    //else {
    //    var TD1_Content = $('<div class="d-block">');
    //    var TD1 = $('<td style="width:100%;text-align: center;">').append('<div style="color:red">Não há produtos para mostrar</div>');
    //    var tr = $('<tr>').append(TD1);
    //    tabla.append(tr);
    //}
}