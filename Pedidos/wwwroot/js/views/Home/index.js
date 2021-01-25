
$(function () {

    var ctx = document.getElementById('myChart').getContext('2d');
    var chart = new Chart(ctx, {
        // The type of chart we want to create
        type: 'line',

        // The data for our dataset
        data: {
            labels: ['janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho', 'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'],
            datasets: [
                {
                    label: 'Saidas',
                    backgroundColor: ' rgb(255, 255, 255, 0.5)',
                    borderColor: 'rgb(255, 99, 132)',
                    data: [-100, 10, 5, 2, 20, 30, 45, 0, 10, 5, 2, 300]
                },
                {
                    label: 'Vendas',
                    // backgroundColor: 'rgb(255, 99, 132)',
                    borderColor: 'blue',
                    data: [50, 30, 5, 2, 20, 30, 45, 120, 10, 5, 2, 150]
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


});
