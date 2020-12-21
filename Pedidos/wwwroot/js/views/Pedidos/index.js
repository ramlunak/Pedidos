var _Productos;
var _CurrentPedido;
var _PedidosPendientes;
var _ModalProducto = {
    cliente: '',
    direccion: '',
    telefono: '',
    observacion: ''
};

var _ModalAdicionales = [];
var _ModalIngredientes = [];

$(function () {

    CargarCurrentPedido();
    CargarPedidosPendientes();
    CargarProductos();

    $('#inputNome').on('input propertychange', function (e) {
        $('#spanNombre').html($('#inputNome').val());

        var opt = $('option[value="' + $(this).val() + '"]');
        var id = opt.length ? opt.attr('id') : '';

        if (id !== '' && id !== undefined) {
            $('#inputNome').css({ "border-color": "#82E0AA", "border-weight": "1px", "border-style": "solid" });
            $('#idCliente').val(id);
            _CurrentPedido.idCliente = id;
        } else {
            $('#inputNome').css({ "border": "none" });
            _CurrentPedido.idCliente = null;
        }

        //CargarDirecciones();

        _ModalProducto.cliente = $('#inputNome').val();
        _ModalProducto.idCliente = id;

        console.log(_CurrentPedido);

    });

    $('#inputEndereco').on('input propertychange', function (e) {
        $('#spanEndereco').html($('#inputEndereco').val());
        _ModalProducto.direccion = $('#inputEndereco').val();
    });

    $('#inputTelefone').on('input propertychange', function (e) {
        _ModalProducto.telefono = $('#inputTelefone').val();
    });

    $('#inputProducto').on('input propertychange', function (e) {
        var productosFiltrados = filterItems($('#inputProducto').val());
        FiltrarProductos(productosFiltrados);
    });

});
//filtar productos para escojer
function FiltrarProductos(productos) {

    var TABLE = $('#tableProductos');
    TABLE.empty();

    $.each(productos, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td style="width:auto">');
        var TR = $('<tr class="hover" onclick="ShowDetallesProducto(' + item.id + ')">');

        TD1.append('<div><b>' + item.nombre + '</b></div>');
        TD1.append('<div style="font-size:13px;color:green;font-weight: 700;">R$ ' + item.valor.toFixed(2) + '</div>');

        TD2.append('<div class="d-flex align-items-center unselectable" style="font-size:22px;color:green;font-weight: 600;cursor:pointer">+</div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);

    });

}

const filterItems = (query) => {

    return _Productos.filter(el => el.nombre.toLocaleLowerCase('pt-BR').indexOf(query.toLocaleLowerCase('pt-BR')) > -1);
};

//Cargar info del pedido que está en edicion
function CargarProductos() {

    $.ajax({
        type: "GET",
        url: "/Productos/GetProductos/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            // window.location.href = '/AlumnoPrueba/Resultado/';
            _Productos = data;

            // cargarProductosCategoria(parseInt(idCategoria), data);
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

//cargar lista de producto para el combo
function CargarCurrentPedido() {

    $.ajax({
        type: "GET",
        url: "/Pedidos/GetCurrentPedido/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            _CurrentPedido = data.currentPedido;
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

//cargar lista de pedidos pendientes
function CargarPedidosPendientes() {

    $.ajax({
        type: "GET",
        url: "/Pedidos/CargarPedidosPendientes/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            _PedidosPendientes = data.pedidosPendientes;
            MostarPedidosPendientes();
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

//cargar info para mostrar en el modal 
function ShowDetallesProducto(id) {

    $.ajax({
        type: "GET",
        url: "/Productos/GetDetalleProducto/" + id,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var datosClienteFormulario = _ModalProducto;
            _ModalProducto = data.producto;
            _ModalProducto.cliente = datosClienteFormulario.cliente;
            _ModalProducto.direccion = datosClienteFormulario.direccion;
            _ModalProducto.telefono = datosClienteFormulario.telefono;

            _ModalAdicionales = data.adicionales;
            _ModalIngredientes = data.ingredientes;

            CargarDatosModalDetalles(data);
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);
        }
    });

}

//cargar info del producto en el modal 
function CargarDatosModalDetalles(data) {

    $('#spanNomeProducto').html(data.producto.nombre.toUpperCase());
    $('#spanValorProducto').html(data.producto.valor.toFixed(2));
    $('#spanDescripcionProducto').html(data.producto.descripcion);
    $('#modalCantidadProducto').html('(' + data.producto.cantidad + ')');
    $("#MINUS_Producto").attr('disabled', 'disabled');

    TABLE_Adicional(data.adicionales, data.producto.id);
    TABLE_Ingredientes(data.ingredientes, data.producto.id)
    $('#modalObservacionContent').html('');
    $('#modalObservacionContent').append('<textarea id="inputObservacion" rows="2" oninput="observacionOnInput()" class="form-control" placeholder="Observação"></textarea>');

    //TAMAHOS
    mostrarTamanhos(data.producto);
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

        TD1.append('<div class="unselectable"><a id=' + codigo + ' style="color:blue">+0</a> ' + item.nombre + '</div>');
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

//actualizar los datos del cliente en el pedido en edicion
function UpdataDatosCliente() {

    $.ajax({
        type: "POST",
        url: "/Pedidos/UpdataDatosCliente",
        traditional: true,
        data: JSON.stringify(_CurrentPedido),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log('ok', result);

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

}

//cargar datos del pedido en la pantalla
function MostarCurrentPedido() {

    $('#spanCodigo').html(_CurrentPedido.codigo);
    $('#spanTotal').html(_CurrentPedido.valorProductos.toFixed(2));
    TABLE_PedidoProductos();
}

//crear tabla de productos del pedido en edicion
function TABLE_PedidoProductos() {

    var TABLE = $('#CurrentPedidoProductos');
    TABLE.empty();

    $.each(_CurrentPedido.productos, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td>');
        var TR = $('<tr>');

        TD1.append('<div>(<b>' + item.cantidad + '</b>) ' + item.nombre.toUpperCase() + '</div>');
        TD2.append('<div style="font-size:12px;width:70px;text-align:end;" class="cursor-pointer"> R$ ' + item.valorMasAdicionales.toFixed(2) + '</div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);
    });
}

function GuardarCurrentPedido() {

    var pedido = {
        cliente: $('#inputNome').val(),
        idCliente: _CurrentPedido.idCliente,
        direccion: $('#inputEndereco').val(),
        telefono: $('#inputTelefone').val(),
        idFormaPagamento: $('#idFormaPagamento').val()
    }

    $.ajax({
        type: "POST",
        url: "/Pedidos/GuardarCurrentPedido",
        traditional: true,
        data: JSON.stringify(pedido),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.ok) {
                _CurrentPedido = result.currentPedido;
                MostarCurrentPedido();

                _PedidosPendientes = result.pedidosPendientes;
                MostarPedidosPendientes();
            } else {

                Swal.fire({
                    icon: 'info',
                    title: 'Oops...',
                    text: result.erro
                })
            }

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

}

function MostarPedidosPendientes() {
    TABLE_PedidosPendientes();
}

//crear tabla de productos del pedido en edicion
function TABLE_PedidosPendientes() {

    var TABLE = $('#ListaPedidosPendientes');
    TABLE.empty();

    $.each(_PedidosPendientes, function (index, pedido) {


        var CARD = $('<div id="CARD_PEDIDO_' + pedido.id + '" class="card mb-2">');
        var CARD_BODY = $('<div class="card-body  p-1">');

        //INFO DEL PEDIDO 
        var div_infopedido = '<div class="d-flex justify-content-between">  ' +
            '               <div class="d-block" style="text-align:left">  ' +
            '                   <div style="font-size:11px">' + pedido.cliente + '</div>  ' +
            '                   <div style="font-size:11px">' + pedido.direccion + '</div>  ' +
            '               </div>  ' +
            '               <div class="d-block" style="text-align:right">  ' +
            '                   <div style="font-size:11px"><b>TOTAL</b></div>  ' +
            '                   <div style="font-size:11px"><b>R$ ' + pedido.total.toFixed(2) + '</b></div>  ' +
            '               </div>  ' +
            '           </div>  ' +
            '          <hr class="m-2" />  ';

        CARD_BODY.append(div_infopedido);

        //LISTA DE PRODUCTOS
        var TABLA_PRD = $('<table cellspacing="0" class="table-tr-border-radius unselectable" style="font-size: 13px;">');

        $.each(JSON.parse(pedido.jsonListProductos), function (index, producto) {

            //PRODUCTO
            var Desplegar = 'class="cursor-pointer" data-toggle="collapse"   ' +
                '   data-target="#collapseExample_' + pedido.id + '_' + index + '_' + producto.id + '"   ' +
                '   aria-expanded="false" aria-controls="collapseExample_' + pedido.id + '_' + index + '_' + producto.id + '" ';
            var tr_background_color = "background-color: powderblue";

            if (producto.Adicionales.length == 0 && producto.Ingredientes.length == 0) {
                Desplegar = "";
                tr_background_color = "";
            }

            // CONTADOR
            var TR0_PRD = $('<tr>');

            var sec = pedido.tiempo_pedido;
            function pad(val) { return val > 9 ? val : "0" + val; }
            setInterval(function () {
                $('#seconds_' + pedido.id + '_' + index + '_' + producto.id + '').html(pad(++sec % 60));
                $('#minutes_' + pedido.id + '_' + index + '_' + producto.id + '').html(pad(parseInt(sec / 60, 10)));
            }, 1000);

            var div_conter_style = 'style="text-align: start;font-size: 11px !important;color: gray;color: mediumorchid;"';
            TR0_PRD.append('<td colspan="2"><div ' + div_conter_style + ' > <span id="minutes_' + pedido.id + '_' + index + '_' + producto.id + '"></span>: <span id="seconds_' + pedido.id + '_' + index + '_' + producto.id + '"></span></div></td>');
            //FIN

            var TR1_PRD = $('<tr style="' + tr_background_color + '">');
            var TD1_PRD = $('<td style="width:100%">');
            var TD2_PRD = $('<td>');


            TD1_PRD.append('<div style="text-align: start;" ' + Desplegar + '>  (<b>' + producto.cantidad + '</b>) ' + producto.nombre.toUpperCase() + '</div>');
            TD2_PRD.append('<div style="font-size:12px;width:70px;text-align:end;" class="cursor-pointer"> R$ ' + producto.valor.toFixed(2) + '</div>');
            TR1_PRD.append(TD1_PRD, TD2_PRD);

            //ADICIONALES E INGREDIENTES DEL PRODUCTO
            var TR2_PRD = $('<tr>');
            var TD1_PRD = $('<td style="width:100%" colspan="2">');

            var TABLA_ADIC = $('<table class="w-100 unselectable">');
            $.each(producto.Adicionales, function (index, item) {

                var TD1 = $('<td style="width:100%">');
                var TD2 = $('<td>');
                var TR = $('<tr>');

                TD1.append('<div class="unselectable" style="text-align:start;"><a style="color:blue;">+' + item.cantidad + '</a> ' + item.nombre + '</div>');
                TD2.append('<div class="unselectable" style="width:70px;text-align:end;font-size: 13px;">R$ ' + (item.Valor * item.cantidad).toFixed(2) + '</div>');

                TR.append(TD1, TD2);
                TABLA_ADIC.append(TR);
            });

            var TABLA_INGD = $('<table class="w-100 unselectable">');
            $.each(producto.Ingredientes, function (index, item) {
                console.log(item);
                var TD1 = $('<td style="width:100%">');
                var TD2 = $('<td>');
                var TR = $('<tr>');

                TD1.append('<div class="unselectable" style="text-align:start;"><a style="color:blue;">- </a> ' + item.nombre + '</div>');

                TR.append(TD1);
                TABLA_INGD.append(TR);
            });

            var panelBody = $('<div class="card card-body" style="padding: 8px;">');
            panelBody.append(TABLA_ADIC);
            panelBody.append($('<hr style="margin: 5px;">'));
            panelBody.append(TABLA_INGD);

            var panelInredientesAdicionales = $('<div class="collapse" id="collapseExample_' + pedido.id + '_' + index + '_' + producto.id + '">');
            panelInredientesAdicionales.append(panelBody);

            TD1_PRD.append(panelInredientesAdicionales);
            TR2_PRD.append(TD1_PRD);

            //ADD TRS A LA TABLA
            TABLA_PRD.append(TR0_PRD, TR1_PRD, TR2_PRD);

        });

        //AGREGAR PRODUCTOS
        CARD_BODY.append(TABLA_PRD);

        //FUTTER
        var CARD_FUTTER = $(' <div class="card-footer text-muted p-2 d-flex justify-content-between ">');

        var futter_botones = $('<div class="d-flex">');
        futter_botones.append('<a onclick="cancelar(' + pedido.id + ')" class="btn btn-sm btn-danger cursor-pointer mr-2"  style="color:white">cancelar</a>');
        futter_botones.append('<a onclick="finalizado(' + pedido.id + ')" class="btn btn-sm btn-success cursor-pointer" style="color:white">finalizado</a>');
        CARD_FUTTER.append(futter_botones);

        CARD_FUTTER.append('<a href="/Pedidos/Print/' + pedido.id + '" target="_blank"><i class="fa fa-print cursor-pointer float-right" aria-hidden="true" style="color: green"></i></a>');

        CARD.append(CARD_BODY);
        CARD.append(CARD_FUTTER);

        var TD1 = $('<td style="width:100%">');
        var TR = $('<tr>');
        TD1.append(CARD);

        TR.append(TD1);
        TABLE.append(TR);
    });
}

function cancelar(idPedido) {

    Swal.fire({
        title: 'Tem certeza que deseja cancelar o pedido?',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim!',
        cancelButtonText: 'Não'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                type: "GET",
                url: "/Pedidos/Cancelar/" + idPedido,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: '',
                        showConfirmButton: false,
                        timer: 800
                    })

                    $('#CARD_PEDIDO_' + idPedido + '').remove();

                },
                failure: function (response) {

                    Swal.fire(
                        'Error!',
                        'Erro de servidor.',
                        'error'
                    )
                },
                error: function (response) {

                    Swal.fire(
                        'Error',
                        'Erro de servidor.',
                        'error'
                    )
                }
            });

        }
    })


}

function finalizado(idPedido) {

    Swal.fire({
        title: 'O pedido será marcado como preparado',
        text: "",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim!',
        cancelButtonText: 'Não'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                type: "GET",
                url: "/Pedidos/Finalizar/" + idPedido,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: '',
                        showConfirmButton: false,
                        timer: 800
                    })

                    $('#CARD_PEDIDO_' + idPedido + '').remove();

                },
                failure: function (response) {

                    Swal.fire(
                        'Error!',
                        'Erro de servidor.',
                        'error'
                    )
                },
                error: function (response) {

                    Swal.fire(
                        'Error',
                        'Erro de servidor.',
                        'error'
                    )
                }
            });

        }
    })

}
