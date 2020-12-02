var _Productos;
var _ModalProducto;
var _ModalAdicionales = [];
var _ModalIngredientes = [];

$(function () {

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
    return _Productos.filter(el => el.nombre.toLowerCase().indexOf(query.toLowerCase()) > -1);
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
            _Productos = data;
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
            _ModalProducto = data.producto;
            _ModalIngredientes = data.ingredientes;
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

    $('#spanNomeProducto').html(data.producto.nombre.toUpperCase());
    $('#spanValorProducto').html(data.producto.valor.toFixed(2));
    $('#spanDescripcionProducto').html(data.producto.descripcion);

    TABLE_Adicional(data.adicionales,data.producto.id);

    $('#ModalDetalleProducto').modal('show');
}

function TABLE_Adicional(adicionales, idProducto) {

    var TABLE = $('#modalTableAdicionales');
    TABLE.empty();

    $.each(adicionales, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td>');
        var TD3 = $('<td>');
        var TR = $('<tr>');

        var codigo = "ADC_" + item.id + "_" + item.nombre;

        TD1.append('<div><a id='+codigo+' style="color:blue">+0</a> ' + item.nombre + '</div>');
        TD2.append('<div style="width:50px;text-align:end">R$ ' + item.valor.toFixed(2) + '</div>');
        TD3.append('<div style="width:50px;text-align: end"> <samp class="mr-2"><i class="fa fa-minus cursor-pointer"></i></samp><i onclick="adicionalPlus(' + item.id + ',' + idProducto + ')" class="fa fa-plus cursor-pointer"></i></div>');

        TR.append(TD1, TD2, TD3);
        TABLE.append(TR);
    });

}

function AddProducto() {

    var data = _ModalProducto;
    console.log(_ModalProducto);
    console.log(_ModalIngredientes);
    return;
    $.ajax({
        type: "POST",
        url: "/Pedidos/AddProducto",
        traditional: true,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log('ok', result);

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

function adicionalPlus(item,ds) {
    console.log(item,ds);
}