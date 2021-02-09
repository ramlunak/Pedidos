var datasets_ventas_anual = [];
var chatConnectionId;

$(function () {

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
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}

function CargarGrafico() {


    //RELATORIO VENTAS MENSUAL

    var ctxVendtasMensual = document.getElementById('chartVendtasMensual').getContext('2d');
    var chartVendtasMensual = new Chart(ctxVendtasMensual, {
        // The type of chart we want to create
        type: 'line',

        // The data for our dataset
        data: {
            labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
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

    //RELATORIO VENTAS ANUAL

    var ctxVendtasAnual = document.getElementById('chartVendtasAnual').getContext('2d');
    var chartVendtasAnual = new Chart(ctxVendtasAnual, {
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