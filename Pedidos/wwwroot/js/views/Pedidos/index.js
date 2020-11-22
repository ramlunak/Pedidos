$(function () {

    var Productos;
    CargarProductos();

    $('#inputProducto').on('input propertychange', function (e) {

        var productosFiltrados = filterItems($('#inputProducto').val());
        //console.log(productosFiltrados);
        FiltrarProductos(productosFiltrados);

    });

});

function FiltrarProductos(productos) {

    var TABLE = $('#tableProductos');
    TABLE.empty();

    $.each(productos, function (index, item) {

        var TD1 = $('<td class="hover" style="width:100%">');
        var TD2 = $('<td style="width:auto">');
        var TR = $('<tr>');

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
