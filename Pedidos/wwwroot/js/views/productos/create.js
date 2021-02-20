
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

    //$('#inputCantidadSabores').on('input', function () {

    //    $('#spanCantidadSabores').html($('#inputCantidadSabores').val());

    //});

    function CargarSubCategorias() {

        $.getJSON('/Categorias/GetSubCategarias', { idCategoria: $('#idCategoria').val() }, function (data) {

            $.each(data, function () {
                $('#idSubCategoria').append('<option value=' + this.value + '>' + this.text + '</option>');
            });
            $('#idSubCategoria').append('<option selected ></option>');

        });
    }

    //TAMANHOS
    $('#panelTamanhos').on('show.bs.collapse', function () {
        $('#form_group_valor').hide('slow');
    })

    $('#panelTamanhos').on('hide.bs.collapse', function () {
        $('#form_group_valor').show('slow');
    })

});

window.onbeforeunload = function () {
    $('form')[0].reset();
};
