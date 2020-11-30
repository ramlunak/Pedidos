$(function () {

    var Productos;
    CargarProductos();

    $('#inputNome').on('input propertychange', function (e) {
        $('#spanNombre').html($('#inputNome').val());
    });

    $('#inputEndereco').on('input propertychange', function (e) {
        $('#spanEndereco').html($('#inputEndereco').val());
    });

    $('#inputProducto').on('input propertychange', function (e) {
        var productosFiltrados = filterItems($('#inputProducto').val());
        FiltrarProductos(productosFiltrados);
    });

});

function FiltrarProductos(productos) {

    var TABLE = $('#tableProductos');
    TABLE.empty();

    $.each(productos, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td style="width:auto">');
        var TR = $('<tr class="hover" onclick="ShowDetallesProducto(' + item.id + ')">');

        TD1.append('<div><b>' + item.nombre + '</b></div>');
        TD1.append('<div style="font-size:13px;color:green;font-weight: 700;">R$ ' + item.valor.toFixed(2) + '</div>');

        TD2.append('<div class="d-flex align-items-center unselectable" style="font-size:22px;color:green;font-weight: 600;cursor:pointer">+</div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);

    });

}


const filterItems = (query) => {
    return Productos.filter(el => el.nombre.toLowerCase().indexOf(query.toLowerCase()) > -1);
};

function CargarProductos() {

    $.ajax({
        type: "GET",
        url: "/Productos/GetProductos/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            // window.location.href = '/AlumnoPrueba/Resultado/';
            Productos = data;
            console.log(data);
            // cargarProductosCategoria(parseInt(idCategoria), data);
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

function ShowDetallesProducto(id) {

    $.ajax({
        type: "GET",
        url: "/Productos/GetDetalleProducto/" + id,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            CargarDatosModalDetalles(data);
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);
        }
    });

}


function CargarDatosModalDetalles(data) {
    //var producto = data.jsonProducto[0];
    //var dicionales = data.jsonAdicionales;
    //var ingredientes = data.jsonIngredientes;

    $('#spanNomeProducto').html(data.producto.nombre.toUpperCase());
    $('#spanValorProducto').html(data.producto.valor.toFixed(2));
    $('#spanDescripcionProducto').html(data.producto.descripcion);
    TABLE_Adicional(data.adicionales);

    console.log(data.producto);
    console.log(data.adicionales);
    console.log(data.ingredientes);

    $('#ModalDetalleProducto').modal('show');
}

function TABLE_Adicional(adicionales) {

    var TABLE = $('#modalTableAdicionales');
    TABLE.empty();

    $.each(adicionales, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td>');
        var TD3 = $('<td>');
        var TR = $('<tr>');

        TD1.append('<div>' + item.nombre + '</div>');
        TD2.append('<div style="width:50px;text-align:end">R$ ' + item.valor.toFixed(2) + '</div>');
        TD3.append('<div style="width:50px;text-align: end"> <samp class="mr-2"><i class="fa fa-minus cursor-pointer"></i></samp>   <i class="fa fa-plus cursor-pointer"></i></div>');

        TR.append(TD1, TD2, TD3);
        TABLE.append(TR);

    });

}