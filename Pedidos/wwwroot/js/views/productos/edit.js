
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
    });

    //TAMANHOS
    $('#panelTamanhos').on('show.bs.collapse', function () {
        $('#form_group_valor').hide('slow');
    })

    $('#panelTamanhos').on('hide.bs.collapse', function () {
        $('#form_group_valor').show('slow');
    })

    //$(".inputDecimal").change(function () {

    //    let vT1 = $('#inputValorTamanho1').val();
    //    let vT2 = $('#inputValorTamanho2').val();
    //    let vT3 = $('#inputValorTamanho3').val();

    //    if (vT1 === "" && vT2 === "" && vT3 === "") {
    //        $('#btnToggleTamanhos').click();
    //    }

    //});

});


window.onbeforeunload = function () {
    $('form')[0].reset();
};