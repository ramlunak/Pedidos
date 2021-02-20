var datasets_ventas_anual = [];
var chatConnectionId;

$(function () {
    CargarVentasAnual();
    CargarVentasMensual();
    CargarVentasSemana();
    CargarVentasDia();
    CargarGraficoVentasAnual();

});


function CargarVentasAnual() {

    $.ajax({
        type: "GET",
        url: "/Home/GetVentasAnual",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#spanVentasAnual').html(parseFloat(data).toFixed(2));
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}

function CargarVentasMensual() {

    $.ajax({
        type: "GET",
        url: "/Home/GetVentasMensual",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#spanVentasMensual').html(parseFloat(data).toFixed(2));
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

}

function CargarVentasSemana() {

    $.ajax({
        type: "GET",
        url: "/Home/GetVentasSemana",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#spanVentasSemana').html(parseFloat(data).toFixed(2));
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response);
        }
    });

}

function CargarVentasDia() {

    $.ajax({
        type: "GET",
        url: "/Home/GetVentasDia",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#spanVentasDia').html(parseFloat(data).toFixed(2));
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response);
        }
    });

}

function CargarGraficoVentasAnual() {

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

    //  var ctxVendtasMensual = document.getElementById('chartVendtasMensual').getContext('2d');
    //var chartVendtasMensual = new Chart(ctxVendtasMensual, {      
    //    type: 'line',
    //    data: {
    //        labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
    //        datasets: [
    //            {
    //                label: 'Vendas',                  
    //                borderColor: 'blue',
    //                data: datasets_ventas_anual
    //            }
    //        ],

    //    },       
    //    options: {
    //        title: {
    //            display: true,
    //            text: 'RELATORIO DE VENDAS E SAIDAS ANUAL'
    //        }
    //    }
    //});

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
                    label: '',
                    // backgroundColor: 'rgb(255, 99, 132)',
                    borderColor: 'blue',
                    data: datasets_ventas_anual
                }
            ],

        },
        // Configuration options go here
        options: {
            legend: {
                display: false,
                labels: {
                    fontColor: 'rgb(255, 99, 132)'
                }
            },
            title: {
                display: false,
                text: 'RELATORIO DE VENDAS E SAIDAS ANUAL'
            }
        }
    });

}