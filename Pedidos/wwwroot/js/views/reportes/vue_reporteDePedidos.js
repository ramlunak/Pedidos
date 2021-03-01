var app = new Vue({
    el: '#content',
    created: function () {
        CargarReporteDePedidos();
    },
    data: {
        isLoading: false,
        ListaPedidos: []
    },
    methods: {
        ShowModalFiltro: function () {
            $('#modalFiltroReporteDePedidos').modal('show');
        }

    }
});


function CargarReporteDePedidos() {

    let fechaInicio = $('#inputFechaInicio').val();
    let fechaFin = $('#inputFechaFin').val();

    let filtroReporteDePedido = {
        fechaInicio: fechaInicio == "" ? null : fechaInicio,
        fechaFin: fechaFin == "" ? null : fechaFin
    };

    $.ajax({
        type: "POST",
        url: "/Reportes/GetDataReporteDePedidos",
        traditional: true,
        data: JSON.stringify(filtroReporteDePedido),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
            app.ListaPedidos = data;
        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

}