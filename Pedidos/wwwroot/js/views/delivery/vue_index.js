var app = new Vue({
    el: '#content',
    created: function () {
        CargarDatos();
    },
    data: {
        isLoading: false,
        categorias: [],
        productos: []
    },
    methods: {
        newfuntion: function () {

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
            app.categorias = data.categorias;
            app.productos = data.productos;
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
