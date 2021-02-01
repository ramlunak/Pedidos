var datasets_ventas_anual = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
var ctxVendtasAnual;

$(function () {

    ctxVendtasAnual = document.getElementById('chartVendtasAnual').getContext('2d');

    CargarGrafico();
    CargarVentasAnual();

});

function CargarVentasAnual() {

    $.ajax({
        type: "GET",
        url: "/Home/GetReporteVentasAnual",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            datasets_ventas_anual = data;
            CargarGrafico();
            console.log(data);
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}

function CargarGrafico() {

    var chart = new Chart(ctxVendtasAnual, {
        // The type of chart we want to create
        type: 'line',

        // The data for our dataset
        data: {
            labels: ['janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho', 'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'],
            datasets: [
                {
                    label: 'Vendas',
                    // backgroundColor: 'rgb(255, 99, 132)',
                    borderColor: 'blue',
                    data: datasets_ventas_anual
                }
            ],

        },
        // Configuration options go here
        options: {
            title: {
                display: true,
                text: 'RELATORIO DE VENDAS E SAIDAS ANUAL'
            }
        }
    });

}