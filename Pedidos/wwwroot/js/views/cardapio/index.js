﻿var cardapioIdCuenta;
var cardapioMesa;

var _CurrentPedido;
var _ModalProducto = {
    cliente: '',
    idCliente: null,
    aplicativo: '',
    idAplicativo: null,
    idMesa: null,
    direccion: '',
    telefono: '',
    observacion: ''
};
var cardapioProductos = [];

$(function () {

    cardapioIdCuenta = $('#inputIdCuenta').val();
    cardapioMesa = $('#inputMesa').val();
    console.log(cardapioIdCuenta, cardapioMesa);

    VerificarCliente(cardapioIdCuenta);

    GetCategorias(cardapioIdCuenta);

});

function VerificarCliente(cardapioIdCuenta) {

    var x = getCookie('ClienteCardapioPlusCookies');
    if (x === null || x === undefined || x === "") {
        PedirNombre(cardapioIdCuenta);
    } 
    //$.ajax({
    //    type: "GET",
    //    url: "/Cardapio/VerificarCliente",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {

    //        if (data === null || data === undefined) {
    //            PedirNombre(cardapioIdCuenta);
    //        }

    //    },
    //    failure: function (response) {
    //        console.log('failure', response);
    //    },
    //    error: function (response) {
    //        console.log('error', response);

    //    }
    //});
}

function PedirNombre(idCuenta) {

    Swal.fire({
        title: 'Por favor forneça seu nome',
        icon: 'info',
        html: '<input id="cardapioModalInputNombre" class="form-control" />',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: false,
        allowOutsideClick: false,
        confirmButtonText: 'Salvar',
        showLoaderOnConfirm: true,
        onOpen: function () {
            $('.swal2-confirm').prop('id', 'cardapioModalNombreBtnOk');
            $('.swal2-confirm').prop('disabled', true);
        },
        preConfirm: (login) => {

            var nombre = $('#cardapioModalInputNombre').val();

            return fetch(`/Cardapio/CadastrarCliente/?idCuenta=${parseInt(idCuenta)}&nombre=${nombre}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Não pudemos cadastar vc")
                    }
                    return response.json()
                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Oops ${error}`
                    )
                })
        }
    }).then((result) => {
        if (result.isConfirmed) {

            setCookie('ClienteCardapioPlusCookies', result.value, 2000);

            Swal.fire({
                icon: 'success',
                title: `Seja bem vindo,`,
            })
        }
    })

    $('#cardapioModalInputNombre').on('input propertychange', function (e) {

        var nombre = $('#cardapioModalInputNombre').val();
        if (nombre != null && nombre != undefined && nombre != "") {

            $('#cardapioModalNombreBtnOk').prop('disabled', false);

        } else {

            $('#cardapioModalNombreBtnOk').prop('disabled', true);

        }

    });
}

function GetCategorias(idCuenta) {

    $("#cardapioLoadingCategorias").show();

    $.ajax({
        type: "GET",
        url: "/Cardapio/CargarCategorias/" + parseInt(idCuenta),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);

            $("#cardapioLoadingCategorias").hide();
            MostarCategorias(data)
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

function MostarCategorias(categorias) {

    var divCategorias = $('#cardapioAcordionCategorias');
    divCategorias.html("");

    $.each(categorias, function (index, item) {

        var acordion = $(' <div class="accordion" id="accordionCategorias">');

        acordion.append('   <div class="card">  ' +
            '                           <div class="card-header cursor-pointer d-flex justify-content-start" id="heading_' + item.id + '_' + item.idCuenta + '" data-toggle="collapse" data-target="#collapse_' + item.id + '_' + item.idCuenta + '" aria-expanded="true" aria-controls="collapse_' + item.id + '_' + item.idCuenta + '">  ' +
            '                               <h5 class="mb-0 text-uppercase d-flex">  ' +
            '                                   ' + item.nombre +
            '                               </h5>  ' +
            '                           </div>  ' +
            '     ' +
            '                           <div id="collapse_' + item.id + '_' + item.idCuenta + '" class="collapse hide collapseCategoria" aria-labelledby="heading_' + item.id + '_' + item.idCuenta + '" data-parent="#accordionCategorias">  ' +
            '                               <div class="card-body">  ' +
            '     ' +
            '                                   <table class="table" id="tableProductosCategoria_' + item.id + '_' + item.idCuenta + '" width="100%" cellspacing="0" cellpadding="0">  ' +
            '                                   </table>  ' +
            '                               </div>  ' +
            '                           </div>  ' +
            '                      </div>  ');

        divCategorias.append(acordion);
    });

    $('.collapseCategoria').on('show.bs.collapse', function () {

        var collapseId = $(this).attr("id");
        var idCategoria = collapseId.split('_')[1];
        var idCuenta = collapseId.split('_')[2];

        let categoria = {
            id: parseInt(idCategoria),
            idCuenta: parseInt(idCuenta),
        };

        $.ajax({
            type: "POST",
            url: "/Cardapio/GetProductos",
            traditional: true,
            data: JSON.stringify(categoria),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result);
                cargarProductosCategoria(parseInt(idCategoria), parseInt(idCuenta), result);
            },
            failure: function (response) {
                console.log('failure', response);
            },
            error: function (response) {
                console.log('error', response);

            }
        });

    })


}

function cargarProductosCategoria(idCategoria, idCuenta, productos) {

    cardapioProductos = productos;

    var TABLE = $('#tableProductosCategoria_' + idCategoria + '_' + idCuenta);
    TABLE.empty();

    $.each(productos, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td style="width:auto">');
        var TR = $('<tr class="hover" onclick="CargarDatosModalDetalles(' + item.id + ')">');

        TD1.append('<div><b>' + item.nombre + '</b></div>');
        var divTamanhos = $('<div style="display: flex">');

        if (item.valor !== 0)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex"> <div class="ml-1" style="color:chartreuse">R$ ' + item.valor + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho1 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex">' + item.tamanho1 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho1.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho2 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex">' + item.tamanho2 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho2.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho3 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary   m-1 d-flex">' + item.tamanho3 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho3.toFixed(2) + '</div> </div>');
        TD1.append(divTamanhos);

        TD2.append('<div class="d-flex align-items-center unselectable" style="font-size:22px;color:green;font-weight: 600;cursor:pointer">+</div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);

    });

    //var tabla = $('#tableProductosCategoria_' + idCategoria + '_' + idCuenta);
    //tabla.empty();

    //console.log(productos.length);
    //if (productos.length > 0) {
    //    $.each(productos, function (index, item) {

    //        //INFO PRODUCTO
    //        var TD1_Content = $('<div class="d-block">');
    //        TD1_Content.append('<div style="font-size: 15px;"><b>' + item.nombre + '</b></div>');
    //        if (item.descripcion !== null) {

    //            TD1_Content.append('<div style="font-size:14px;font-style: italic;">' + item.descripcion + '.</div>');
    //        }
    //        TD1_Content.append('<div style="color: green;font-weight: 600;font-size:14px;">R$ ' + item.valor.toFixed(2) + '</div>');
    //        //--

    //        //IMAGEN 
    //        var TD2_Content = $('<div>');
    //        if (item.imagen !== null) {
    //            TD2_Content.append('<img style="max-height:70px;max-width:70px;object-fit:cover;border-radius:5px" src="data:image/png;base64,' + item.imagen + '" data-toggle="modal" />');
    //        }
    //        //--

    //        var TD1 = $('<td style="width:100%">').append(TD1_Content);
    //        var TD2 = $('<td style="width:auto">').append(TD2_Content);
    //        var tr = $('<tr>').append(TD2, TD1);

    //        tabla.append(tr);

    //    });
    //}
    //else {
    //    var TD1_Content = $('<div class="d-block">');
    //    var TD1 = $('<td style="width:100%;text-align: center;">').append('<div style="color:red">Não há produtos para mostrar</div>');
    //    var tr = $('<tr>').append(TD1);
    //    tabla.append(tr);
    //}
}

//cargar info del producto en el modal 
function CargarDatosModalDetalles(idProducto) {

    var findResult = cardapioProductos.filter(function (item) {
        return (item.id === idProducto);
    });
    let data = findResult[0];

    $('#spanNomeProducto').html(data.nombre.toUpperCase());
    $('#spanValorProducto').html(data.valor.toFixed(2));
    $('#spanDescripcionProducto').html(data.descripcion);
    $('#modalCantidadProducto').html('(' + data.cantidad + ')');
    $("#MINUS_Producto").attr('disabled', 'disabled');

    TABLE_Adicional(data.adicionales, data.id);
    TABLE_Ingredientes(data.ingredientes, data.id)
    $('#modalObservacionContent').html('');
    $('#modalObservacionContent').append('<textarea id="inputObservacion" rows="2" class="form-control" placeholder="Observação"></textarea>');

    //TAMAHOS
    mostrarTamanhos(data);
    $('#ModalDetalleProducto').modal('show');
}

function mostrarTamanhos(producto) {

    $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary');
    $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary');
    $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary');
    $('#checkedTamanho1').hide();
    $('#checkedTamanho2').hide();
    $('#checkedTamanho3').hide();

    if (producto.tamanho1 !== null && producto.tamanho1 !== "" && producto.tamanho1 !== undefined) {
        $('#nomeTamanho1').html(producto.tamanho1);
        $('#valorTamanho1').html(producto.valorTamanho1.toFixed(2));
        $('#btnTamanho1').addClass('btn-primary');
        $('#btnTamanho1').show();
        $('#checkedTamanho1').show();
        _ModalProducto.tamanhoSeleccionado = producto.tamanho1;
        _ModalProducto.valorTamanhoSeleccionado = producto.valorTamanho1;
        $('#spanValorProducto').html(producto.valorTamanho1.toFixed(2));
    }
    else {
        $('#btnTamanho1').hide();
    }

    if (producto.tamanho2 !== null && producto.tamanho2 !== "" && producto.tamanho2 !== undefined) {
        $('#nomeTamanho2').html(producto.tamanho2);
        $('#valorTamanho2').html(producto.valorTamanho2.toFixed(2));
        $('#btnTamanho2').addClass('btn-outline-primary');
        $('#btnTamanho2').show();
        $('#checkedTamanho2').hide();
    } else {
        $('#btnTamanho2').hide();
    }

    if (producto.tamanho3 !== null && producto.tamanho3 !== "" && producto.tamanho3 !== undefined) {
        $('#nomeTamanho3').html(producto.tamanho3);
        $('#valorTamanho3').html(producto.valorTamanho3.toFixed(2));
        $('#btnTamanho3').addClass('btn-outline-primary');
        $('#btnTamanho3').show();
        $('#checkedTamanho3').hide();
    } else {
        $('#btnTamanho3').hide();
    }

}

function checkTamanho(tamanho) {


    if (tamanho === 1) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#checkedTamanho1').show('slow');
        $('#checkedTamanho2').hide('slow');
        $('#checkedTamanho3').hide('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho1;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho1;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho1.toFixed(2));

    } else if (tamanho === 2) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#checkedTamanho1').hide('slow');
        $('#checkedTamanho2').show('slow');
        $('#checkedTamanho3').hide('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho2;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho2;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho2.toFixed(2));

    } else if (tamanho === 3) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#checkedTamanho1').hide('slow');
        $('#checkedTamanho2').hide('slow');
        $('#checkedTamanho3').show('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho3;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho3;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho3.toFixed(2));
    }

}

//crear tabla de los adicionales en el modal
function TABLE_Adicional(adicionales, idProducto) {

    var TABLE = $('#modalTableAdicionales');
    TABLE.empty();

    $.each(adicionales, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td>');
        var TD3 = $('<td>');
        var TR = $('<tr>');

        var codigo = "ADC_" + item.id + "_" + idProducto;
        var minusId = "Minus_" + item.id + "_" + idProducto;

        TD1.append('<div class="unselectable"> <a id=' + codigo + ' style="color:blue">+0</a> ' + item.nombre + '</div>');
        TD2.append('<div class="unselectable" style="width:50px;text-align:end">R$ ' + item.valor.toFixed(2) + '</div>');
        TD3.append('<div style="width:60px;text-align: end"> <button id=' + minusId + ' disabled="disabled" onclick="adicionalMinus(' + item.id + ',' + idProducto + ')" class="btn-plano mr-2 unselectable"><i  class="fa fa-minus cursor-pointer"></i></button><button onclick="adicionalPlus(' + item.id + ',' + idProducto + ')" class="btn-plano unselectable"><i  class="fa fa-plus cursor-pointer"></i></button></div>');

        TR.append(TD1, TD2, TD3);
        TABLE.append(TR);
    });
}

//crear tabla de los ingredientes en el modal
function TABLE_Ingredientes(ingredientes, idProducto) {

    var TABLE = $('#modalTableIngredientes');
    TABLE.empty();

    $.each(ingredientes, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td>');
        var TR = $('<tr>');

        // var codigo = "ADC_" + item.id + "_" + idProducto;
        //var minusId = "Minus_" + item.id + "_" + idProducto;

        TD1.append('<div>' + item.nombre + '</div>');
        TD2.append('<div class="cursor-pointer"> <input id="" onchange="ingredienteOnChange(this,' + item.id + ',' + idProducto + ')" type="checkbox" checked /></div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);
    });
}

//evento de adicionar contidad del adicional
function adicionalPlus(id, idProducto) {

    var codigo = "#ADC_" + id + "_" + idProducto;
    var minusId = "#Minus_" + id + "_" + idProducto;

    var item = $.grep(_ModalAdicionales, (item, index) => {
        if (item.id === id) {
            if (item.cantidad === null || item.cantidad === undefined) {
                item.cantidad = 1;
                $(minusId).removeAttr('disabled');
            } else {
                item.cantidad++;
                $(minusId).removeAttr('disabled');
            }

            $(codigo).html('+' + item.cantidad);
        }
        return item.id === id;
    });

}

//evento de restar contidad del adicional
function adicionalMinus(id, idProducto) {

    var codigo = "#ADC_" + id + "_" + idProducto;
    var minusId = "#Minus_" + id + "_" + idProducto;

    var item = $.grep(_ModalAdicionales, (item, index) => {
        if (item.id === id) {
            if (item.cantidad === null || item.cantidad === undefined) {
                return;
            } else {
                item.cantidad--;
                if (parseInt(item.cantidad) === 0) {
                    $(minusId).attr('disabled', 'disabled');
                }
            }
            $(codigo).html('+' + item.cantidad);
        }
        return item.id === id;
    });
}

//evento de marcar y desmarcar ingredeinte
function ingredienteOnChange(input, id, idProducto) {

    $.grep(_ModalIngredientes, (item, index) => {
        if (item.id === id) {
            item.selected = $(input).is(":checked");
        }
        return item.id === id;
    });
}

//evento de restar contidad producto
function productoMinus(btn) {

    if (parseInt(_ModalProducto.cantidad) === 1) {
        return;
    }

    _ModalProducto.cantidad--;
    $('#modalCantidadProducto').html('(' + _ModalProducto.cantidad + ')');

    if (parseInt(_ModalProducto.cantidad) === 1) {
        $("#MINUS_Producto").attr('disabled', 'disabled');
    }

}

//evento de adicionar contidad de productos
function productoPlus() {
    _ModalProducto.cantidad++;
    $('#modalCantidadProducto').html('(' + _ModalProducto.cantidad + ')');

    $("#MINUS_Producto").removeAttr('disabled');

}

//agregar el producto al pedido en adicion
function AddProducto() {

    _ModalProducto.adicionales = _ModalAdicionales;
    _ModalProducto.ingredientes = _ModalIngredientes;
    _ModalProducto.observacion = $('#inputObservacion').val();

    _ModalProducto.idCliente = parseInt($('#idCliente').val());
    _ModalProducto.idAplicativo = parseInt($('#idAplicativo').val());
    _ModalProducto.idMesa = parseInt($('#idMesa').val());
    _ModalProducto.idDireccion = parseInt($('#idDireccion').val());
    _ModalProducto.telefono = $('#inputTelefone').val();

    _ModalProducto.deliveryDinheiroTotal = parseFloat($('#inputDeliveryDinheiroTotal').val());

    $.ajax({
        type: "POST",
        url: "/Pedidos/AddProducto",
        traditional: true,
        data: JSON.stringify(_ModalProducto),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            $('#ModalDetalleProducto').modal('hide');
            _CurrentPedido = result.currentPedido;
            MostarCurrentPedido();

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });
}


function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}