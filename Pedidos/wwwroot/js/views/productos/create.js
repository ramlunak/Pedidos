
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


});