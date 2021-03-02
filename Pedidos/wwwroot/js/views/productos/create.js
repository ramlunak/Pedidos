
$(function () {
    $(".inputDecimal").mask("###0.00", { reverse: true });

    $('.custom-file-input').on("change", function () {
        console.log(this);
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    })

    $('#browserInputCategoria').on('input', function () {
        var opt = $('option[value="' + $(this).val() + '"]');
        $('#idCategoria').val(opt.length ? opt.attr('id') : '');
        CargarSubCategorias();
    });

    function CargarSubCategorias() {

        $.getJSON('/Categorias/GetSubCategarias', { idCategoria: $('#idCategoria').val() }, function (data) {

            $.each(data, function () {
                $('#idSubCategoria').append('<option value=' + this.value + '>' + this.text + '</option>');
            });
            $('#idSubCategoria').append('<option selected ></option>');

        });
    }

    //SABORES
    MostrarOpcionesCalculoSabores();

    $('#inputCantidadSabores').on('change', function (e) {

        if (!isNaN(parseInt($('#inputCantidadSabores').val()))) {

            if (parseInt($('#inputCantidadSabores').val()) >= 2) {
                $('#divOpcinosCalculoSabores').show();
            } else {
                $('#divOpcinosCalculoSabores').hide();
            }

        } else {
            $('#divOpcinosCalculoSabores').hide();
        }

    });

    $('#actualizarValorSaborMayor').on('change', function (e) {

        if ($('#actualizarValorSaborMayor').prop('checked')) {
            document.getElementById('actualizarValorSaborMenor').switchButton('off');
            document.getElementById('actualizarValorMediaSabores').switchButton('off');
        }

    });

    $('#actualizarValorSaborMenor').on('change', function (e) {

        if ($('#actualizarValorSaborMenor').prop('checked')) {
            document.getElementById('actualizarValorSaborMayor').switchButton('off');
            document.getElementById('actualizarValorMediaSabores').switchButton('off');
        }

    });

    $('#actualizarValorMediaSabores').on('change', function (e) {

        if ($('#actualizarValorMediaSabores').prop('checked')) {
            document.getElementById('actualizarValorSaborMenor').switchButton('off');
            document.getElementById('actualizarValorSaborMayor').switchButton('off');
        }

    });

    //TAMANHOS
    $('#panelTamanhos').on('show.bs.collapse', function () {
        $('#form_group_valor').hide('slow');
    })

    $('#panelTamanhos').on('hide.bs.collapse', function () {
        $('#form_group_valor').show('slow');
    })


});

function MostrarOpcionesCalculoSabores() {
    if (!isNaN(parseInt($('#inputCantidadSabores').val()))) {

        if (parseInt($('#inputCantidadSabores').val()) >= 2) {
            $('#divOpcinosCalculoSabores').show();
        } else {
            $('#divOpcinosCalculoSabores').hide();
        }

    } else {
        $('#divOpcinosCalculoSabores').hide();
    }
}

window.onbeforeunload = function () {
    $('form')[0].reset();
};
