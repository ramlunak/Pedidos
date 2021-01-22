
$(function () {

    var ctx = document.getElementById('myChart').getContext('2d');
    var chart = new Chart(ctx, {
        // The type of chart we want to create
        type: 'line',

        // The data for our dataset
        data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
            datasets: [{
                label: 'My First dataset',
                backgroundColor: 'rgb(255, 99, 132)',
                borderColor: 'rgb(255, 99, 132)',
                data: [0, 10, 5, 2, 20, 30, 45]
            }]
        },

        // Configuration options go here
        options: {}
    });


    //CAMBIAR ESTADO DE LOS ADICIONAIS
    $("#btnPrint").on('click', function (e) {
        e.preventDefault();

        $("#demo").printThis({
            debug: false,                   // show the iframe for debugging
            importCSS: true,                // import parent page css
            importStyle: true,             // import style tags
            printContainer: true,           // grab outer container as well as the contents of the selector
            loadCSS: "",      // path to additional css file - use an array [] for multiple
            pageTitle: "Tiked",                  // add title to print page
            removeInline: false,            // remove all inline styles from print elements
            //removeInlineSelector: "body *", // custom selectors to filter inline styles. removeInline must be true
            printDelay: 333,                // variable print delay
            header: null,                   // prefix to html
            footer: null,                   // postfix to html
            base: false,                    // preserve the BASE tag, or accept a string for the URL
            formValues: true,               // preserve input/form values
            canvas: true,                  // copy canvas elements
           // doctypeString: '<!DOCTYPE html>',           // enter a different doctype for older markup
            removeScripts: false,           // remove script tags from print content
            copyTagClasses: true,           // copy classes from the html & body tag
            beforePrintEvent: null,         // callback function for printEvent in iframe
            afterPrint: null                // function called before iframe is removed
        });
    })

    //CAMBIAR ESTADO DE LOS ADICIONAIS
    $("#btnPrint2").on('click', function (e) {
        e.preventDefault();

        $("#demo2").printThis({
            debug: false,                   // show the iframe for debugging
            importCSS: true,                // import parent page css
            importStyle: true,             // import style tags
            printContainer: true,           // grab outer container as well as the contents of the selector
            loadCSS: "",      // path to additional css file - use an array [] for multiple
            pageTitle: "Tiked",                  // add title to print page
            removeInline: false,            // remove all inline styles from print elements
            //removeInlineSelector: "body *", // custom selectors to filter inline styles. removeInline must be true
            printDelay: 333,                // variable print delay
            header: null,                   // prefix to html
            footer: null,                   // postfix to html
            base: false,                    // preserve the BASE tag, or accept a string for the URL
            formValues: true,               // preserve input/form values
            canvas: false,                  // copy canvas elements
            doctypeString: '<!DOCTYPE html>',           // enter a different doctype for older markup
            removeScripts: false,           // remove script tags from print content
            copyTagClasses: true,           // copy classes from the html & body tag
            beforePrintEvent: null,         // callback function for printEvent in iframe
            afterPrint: null                // function called before iframe is removed
        });
    })

    //CAMBIAR ESTADO DE LOS ADICIONAIS
    $("#btnPrint3").on('click', function (e) {
        e.preventDefault();

        $("#demo3").printThis({
            debug: false,                   // show the iframe for debugging
            importCSS: true,                // import parent page css
            importStyle: true,             // import style tags
            printContainer: true,           // grab outer container as well as the contents of the selector
            loadCSS: "",      // path to additional css file - use an array [] for multiple
            pageTitle: "Tiked",                  // add title to print page
            removeInline: false,            // remove all inline styles from print elements
            //removeInlineSelector: "body *", // custom selectors to filter inline styles. removeInline must be true
            printDelay: 333,                // variable print delay
            header: null,                   // prefix to html
            footer: null,                   // postfix to html
            base: false,                    // preserve the BASE tag, or accept a string for the URL
            formValues: true,               // preserve input/form values
            canvas: false,                  // copy canvas elements
            doctypeString: '<!DOCTYPE html>',           // enter a different doctype for older markup
            removeScripts: false,           // remove script tags from print content
            copyTagClasses: true,           // copy classes from the html & body tag
            beforePrintEvent: null,         // callback function for printEvent in iframe
            afterPrint: null                // function called before iframe is removed
        });
    })

    //CAMBIAR ESTADO DE LOS ADICIONAIS
    $("#btnPrint4").on('click', function (e) {
        e.preventDefault();

        $("#demo4").printThis({
            debug: false,                   // show the iframe for debugging
            importCSS: true,                // import parent page css
            importStyle: true,             // import style tags
            printContainer: true,           // grab outer container as well as the contents of the selector
            loadCSS: "",      // path to additional css file - use an array [] for multiple
            pageTitle: "Tiked",                  // add title to print page
            removeInline: false,            // remove all inline styles from print elements
            //removeInlineSelector: "body *", // custom selectors to filter inline styles. removeInline must be true
            printDelay: 333,                // variable print delay
            header: null,                   // prefix to html
            footer: null,                   // postfix to html
            base: false,                    // preserve the BASE tag, or accept a string for the URL
            formValues: true,               // preserve input/form values
            canvas: false,                  // copy canvas elements
            doctypeString: '<!DOCTYPE html>',           // enter a different doctype for older markup
            removeScripts: false,           // remove script tags from print content
            copyTagClasses: true,           // copy classes from the html & body tag
            beforePrintEvent: null,         // callback function for printEvent in iframe
            afterPrint: null                // function called before iframe is removed
        });
    })


});