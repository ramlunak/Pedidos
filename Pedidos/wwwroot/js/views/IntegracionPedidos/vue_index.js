var app = new Vue({
    el: '#content',
    created: function () {
        GetGruposPedidoPorBarrio();
        GetRutas();
    },
    data: {
        isLoading: false,
        grupoPedidosPorBarrio: [],
        rutas: []
    },
    methods: {
        CargarGruposPedidos: function () {
            GetGruposPedidoPorBarrio();
        },
        CrearNuevaRuta: function () {
            AddNuevaRuta();
        },
        AddBarrio: function (item) {
            AddBarrioCurrentRuta(item);
        }
    }
});


function GetGruposPedidoPorBarrio() {

    $.ajax({
        type: "GET",
        url: "/IntegracionPedidos/GetGruposPedidoPorBarrio/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            app.grupoPedidosPorBarrio = data;
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

function GetRutas() {

    $.ajax({
        type: "GET",
        url: "/IntegracionPedidos/GetRutas/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            console.log(data);
            app.rutas = [];
            app.rutas = data;

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

function AddNuevaRuta() {

    $.ajax({
        type: "GET",
        url: "/IntegracionPedidos/AddNuevaRuta/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (ruta) {
            app.rutas.push(ruta);
            console.log(ruta);
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

function AddBarrioCurrentRuta(item) {

    $.ajax({
        type: "POST",
        url: "/IntegracionPedidos/AddBarrio",
        traditional: true,
        data: JSON.stringify(item),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            app.grupoPedidosPorBarrio = data.grupoPedidosPorBarrio;
            app.rutas = [];
            data.currentRuta.barrios = data.barrios;
            app.rutas.push(data.currentRuta);
        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

}