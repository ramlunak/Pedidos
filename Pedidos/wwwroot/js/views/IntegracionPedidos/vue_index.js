var app = new Vue({
    el: '#content',
    created: function () {
        // this.isLoading = true;
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
    },
    data: {
        isLoading: false,
        message: 'Hello Vue!',
        grupoPedidosPorBarrio: []
    },
    methods: {
        CargarGruposPedidos: function () {

          

        },
    }
})
