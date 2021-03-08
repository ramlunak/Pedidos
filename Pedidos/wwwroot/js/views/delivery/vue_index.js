var app = new Vue({
    el: '#content',
    created: function () {
        CargarDatos();
    },
    data: {
        isLoading: false,
        grupoProductos: [],

        //DETALLES PRODUCTO
        modalProducto: {},
        modalValorProducto: 0,
        modalAdicionales: [],
        modalIngredientes: [],
    },
    methods: {
        DetalleProducto: function (idCuenta, id) {
            CargarDetalleProducto(idCuenta, id);
        },
        OpenModalDetalle: function () {
            $('#ModalDetalleProducto').modal('show');
        },
        CloseModalDetalle: function () {
            $('#ModalDetalleProducto').modal('hide');
        }
    }
});


function CargarDatos() {

    $.ajax({
        type: "GET",
        url: "/Delivery/CargarDatos/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            app.grupoProductos = data.grupoProductos;
            console.log(data);
            // this.isLoading = false;
        },
        failure: function (response) {
            console.log('failure', response);
            // this.isLoading = false;
        },
        error: function (response) {
            console.log('error', response);
            //this.isLoading = false;
        }
    });
}

function CargarDetalleProducto(idCuenta, id) {

    $.ajax({
        type: "GET",
        url: '/Delivery/GetDetalleProducto?idCuenta=' + idCuenta + '&id=' + id,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            app.modalProducto = data.producto;
            app.modalAdicionales = data.adicionales;
            app.modalIngredientes = data.ingredientes;

            console.log(data);
            app.OpenModalDetalle();
            // this.isLoading = false;

        },
        failure: function (response) {
            console.log('failure', response);
            // this.isLoading = false;
        },
        error: function (response) {
            console.log('error', response);
            //this.isLoading = false;
        }
    });
}