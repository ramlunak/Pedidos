var app = new Vue({
    el: '#content',
    created: function () {
        GetGruposPedidoPorBarrio();
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

function GetRutas() {

    $.ajax({
        type: "GET",
        url: "/IntegracionPedidos/GetRutas/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            app.rutas = data;
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

function CrearNuevaRuta() {

}