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

        var TD = $('<td>');
        var TR = $('<tr>');

        TD.append('<div>' + item.nombre + '</div>');
        TR.append(TD);
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
