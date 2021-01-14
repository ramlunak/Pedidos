var _Productos;
var _CurrentPedido;
var _PedidosPendientes;
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

var _ModalDeliveryFormaPagamento = {
    cliente: '',
};

var _ModalAdicionales = [];
var _ModalIngredientes = [];

$(function () {

    $("#inputDescuento").mask("###0.00", { reverse: true });

    GetNumeroPedidosFinalizados();
    CargarCurrentPedido();
    CargarPedidosPendientes();
    CargarProductos();

    $('#searchCliente').on('input propertychange', function (e) {
        var filtroCliente = $('#searchCliente').val();
        if (filtroCliente === null) return;

        $('#searchMesa').val(null);

        _PedidosPendientes.filter(function (item) {
            if (!item.cliente.toLowerCase().includes(filtroCliente.toLowerCase()) && !item.aplicativo.toLowerCase().includes(filtroCliente.toLowerCase())) {
                $('#CARD_PEDIDO_' + item.id).hide();
            }
            else {
                $('#CARD_PEDIDO_' + item.id).show();
            }

            if (filtroCliente === null || filtroCliente === "") {
                $('#CARD_PEDIDO_' + item.id).show();
            }

        });


    });


    $('#searchMesa').on('input propertychange', function (e) {
        var filtroMesa = $('#searchMesa').val();
        if (filtroMesa === null) return;

        $('#searchCliente').val(null);

        _PedidosPendientes.filter(function (item) {

            if (JSON.stringify(item.idMesa) !== filtroMesa) {
                $('#CARD_PEDIDO_' + item.id).hide();
            }
            else {
                $('#CARD_PEDIDO_' + item.id).show();
            }

            if (filtroMesa === null || filtroMesa === "") {
                $('#CARD_PEDIDO_' + item.id).show();
            }

        });
    });


    $('#inputNome').on('input propertychange', function (e) {
        $('#spanNombre').html($('#inputNome').val());

        var opt = $('option[value="' + $(this).val() + '"]');
        var id = opt.length ? opt.attr('id') : '';

        if (id !== '' && id !== undefined) {
            $('#inputNome').css({ "border-color": "#04CD5A", "border-weight": "2px", "border-style": "solid" });
            $('#idCliente').val(id);
        } else {
            $('#inputNome').css({ "border": "1px solid #ced4da" });
            $('#idCliente').val('');
        }

        CargarDirecciones(id);

        _CurrentPedido.cliente = $('#inputNome').val();
        _ModalProducto.cliente = $('#inputNome').val();

    });

    $('#inputAplicativo').on('input propertychange', function (e) {
        $('#spanAplicativo').html($('#inputAplicativo').val());

        var opt = $('option[value="' + $(this).val() + '"]');
        var id = opt.length ? opt.attr('id') : '';

        if (id !== '' && id !== undefined) {
            $('#inputAplicativo').css({ "border-color": "#04CD5A", "border-weight": "2px", "border-style": "solid" });
            $('#idAplicativo').val(id);
        } else {
            $('#inputAplicativo').css({ "border": "1px solid #ced4da" });
            $('#idAplicativo').val('');
        }

        _CurrentPedido.aplicativo = $('#inputAplicativo').val();
        _ModalProducto.aplicativo = $('#inputAplicativo').val();

    });

    $('#idMesa').on('input propertychange', function (e) {
        if ($('#idMesa').val() !== null && $('#idMesa').val() !== undefined && $('#idMesa').val() !== "") {
            $('#spanMesa').html('MESA ' + $('#idMesa').val());
            $('#spanMesa').show();

            $('#divInfoPagamentoDelivery').hide();

            $('#inputAplicativo').val(null);
            $('#idAplicativo').val('');
            $('#inputAplicativo').trigger("input");
            $('#inputAplicativo').hide()


            $('#inputEndereco').val(null);
            $('#idDireccion').val('');
            $('#inputEndereco').trigger("input");
            $('#inputEndereco').hide()

        } else {
            $('#spanMesa').hide();
            $('#divInfoPagamentoDelivery').show();
            $('#inputEndereco').show();
            $('#inputAplicativo').show();
        }

    });

    $('#inputEndereco').on('input propertychange', function (e) {
        $('#spanEndereco').html($('#inputEndereco').val());

        var opt = $('option[value="' + $(this).val() + '"]');
        var id = opt.length ? opt.attr('id') : '';

        if (id !== '' && id !== undefined) {
            $('#inputEndereco').css({ "border-color": "#04CD5A", "border-weight": "2px", "border-style": "solid" });
            $('#idDireccion').val(id);
        } else {
            $('#inputEndereco').css({ "border": "1px solid #ced4da" });
            $('#idDireccion').val('');
        }

        _CurrentPedido.direccion = $('#inputEndereco').val();
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

//Cargar Direcciones del cliente
function CargarDirecciones(id) {
    $.ajax({
        type: "GET",
        url: "/Pedidos/GetDireccion/" + parseInt(id),
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Llanar lista direcciones
            $("#EnderecoList").empty();
            $.each(data, (index, item) => {

                if (index === 0) {
                    $("#EnderecoList").append($('<option selected id="' + item.id + '">').attr('value', item.text));
                } else {
                    $("#EnderecoList").append($('<option id="' + item.id + '">').attr('value', item.text));
                }


            });

        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}

//filtar productos para escojer
function FiltrarProductos(productos) {

    var TABLE = $('#tableProductos');
    TABLE.empty();

    $.each(productos, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td style="width:auto">');
        var TR = $('<tr class="hover" onclick="ShowDetallesProducto(' + item.id + ')">');

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
            console.log('productos', data);

            _Productos = data;
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
            _ModalProducto.idCliente = parseInt(datosClienteFormulario.idCliente);
            _ModalProducto.aplicativo = datosClienteFormulario.aplicativo;
            _ModalProducto.idAplicativo = parseInt(datosClienteFormulario.idAplicativo);
            _ModalProducto.idMesa = parseInt(datosClienteFormulario.idMesa);
            _ModalProducto.direccion = datosClienteFormulario.direccion;
            _ModalProducto.idDireccion = datosClienteFormulario.idDireccion;
            _ModalProducto.telefono = datosClienteFormulario.telefono;

            _ModalProducto.deliveryEmCartao = datosClienteFormulario.deliveryEmCartao;
            _ModalProducto.deliveryPago = datosClienteFormulario.deliveryPago;
            _ModalProducto.deliveryEmdinheiro = datosClienteFormulario.deliveryEmdinheiro;

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
    $('#modalObservacionContent').append('<textarea id="inputObservacion" rows="2" class="form-control" placeholder="Observação"></textarea>');

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

    _ModalProducto.idCliente = parseInt($('#idCliente').val());
    _ModalProducto.idAplicativo = parseInt($('#idAplicativo').val());
    _ModalProducto.idMesa = parseInt($('#idMesa').val());
    _ModalProducto.idDireccion = parseInt($('#idDireccion').val());

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
    $('#spanNombre').html(_CurrentPedido.cliente);
    $('#spanAplicativo').html(_CurrentPedido.aplicativo);
    if (_CurrentPedido.idMesa !== null && _CurrentPedido.idMesa !== undefined && _CurrentPedido.idMesa) {
        $('#spanMesa').html('MESA ' + $('#idMesa').val());
        $('#spanMesa').show();      
    } else {
        $('#spanMesa').hide();
    }
    $('#idMesa').val(_CurrentPedido.idMesa);
    $('#idMesa').trigger('input');
    $('#spanEndereco').html(_CurrentPedido.direccion);
    $('#idCliente').val(_CurrentPedido.idCliente);
    $('#inputNome').val(_CurrentPedido.cliente);
    $('#inputTelefono').val(_CurrentPedido.telefono);
    $('#inputAplicativo').val(_CurrentPedido.aplicativo);
    $('#idAplicativo').val(_CurrentPedido.idAplicativo);
    $('#inputEndereco').val(_CurrentPedido.direccion);

    CargarDirecciones(_CurrentPedido.idCliente);
    $('#idDireccion').val(_CurrentPedido.idDireccion);

    $('#inputEndereco').val(_CurrentPedido.direccion);
    $('#inputDescuento').val(_CurrentPedido.descuento);
    $('#inputPago').prop("checked", _CurrentPedido.DeliveryPago);

    $('#spanTotal').html(_CurrentPedido.valorProductos + _CurrentPedido.tasaEntrega - _CurrentPedido.descuento);

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

        // var btnEdit = '<div class="btn btn-sm btn-outline-success ml-2" onclick="editarCurrentProducto(' + item.id + ')"><i class="fas fa-edit"></i></div>';
        var btnDelete = '<div class="btn btn-sm btn-outline-danger ml-1"  onclick="deleteCurrentProducto(' + item.id + ')"><i class="fas fa-ban"></i></div>';

        TD1.append('<div>(<b>' + item.cantidad + '</b>) ' + item.nombre.toUpperCase() + '</div>');
        TD2.append('<div style="font-size:12px;text-align:center;font-weight:700" class="cursor-pointer d-flex text-nowrap"> <span> R$ ' + item.valorMasAdicionales.toFixed(2) + '</span><div> ' + btnDelete + '</div></div>');

        TR.append(TD1, TD2);
        TABLE.append(TR);
    });
}

var loading;

function showLoading() {
    loading = Swal.fire({
        showConfirmButton: false,
        allowOutsideClick: false,
        imageAlt: 'A tall image',
        html: "<div class='d-block justify-content-center'> <div class='spinner-border text-primary mr-3' role='status'></div > <div>Espere un momento por favor. </div> </div >"
    })
}

function hideLoading() {
    loading.close();
}

function GuardarCurrentPedido() {

    var pedido = {
        cliente: $('#inputNome').val(),
        idCliente: parseInt($('#idCliente').val()),
        idAplicativo: parseInt($('#idAplicativo').val()),
        aplicativo: $('#inputAplicativo').val(),
        idMesa: parseInt($('#idMesa').val()),
        direccion: $('#inputEndereco').val(),
        idDireccion: parseInt($('#idDireccion').val()),
        telefono: $('#inputTelefone').val(),
        deliveryDinheiroTotal: parseFloat($('#inputDeliveryDinheiroTotal').val()),
        deliveryEmCartao: _ModalProducto.deliveryEmCartao,
        deliveryPago: _ModalProducto.deliveryPago,
        deliveryEmdinheiro: _ModalProducto.deliveryEmdinheiro
    }

    showLoading();

    $.ajax({
        type: "POST",
        url: "/Pedidos/GuardarCurrentPedido",
        traditional: true,
        data: JSON.stringify(pedido),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            hideLoading();

            if (result.ok) {
                _CurrentPedido = result.currentPedido;
                MostarCurrentPedido();
                _PedidosPendientes = result.pedidosPendientes;
                _ModalProducto = {
                    cliente: '',
                    idCliente: null,
                    aplicativo: '',
                    idAplicativo: null,
                    idMesa: null,
                    direccion: '',
                    telefono: '',
                    observacion: ''
                };

                metodoPagoDelivery("_deliveryEmCartaoCheck");

                MostarPedidosPendientes();

                if (result.reload) {
                    location.href = '/Pedidos/Index';
                }
            } else {

                Swal.fire({
                    icon: 'info',
                    title: 'Oops...',
                    text: result.erro
                })
            }

            $('#divInfoPagamentoDelivery').show();
            $('#inputEndereco').show();
            $('#inputAplicativo').show();

        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
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

                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'success',
                        title: 'Ação realizada com sucesso'
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

var valorInputFormaPagamento = 0;
var totalPedido = 0;
var firstInputChecked = null;

function actualizarFormaPagamento(idPedido, finalizar) {

    valorInputFormaPagamento = 0;
    totalPedido = 0;
    firstInputChecked = null;

    var findResult = _PedidosPendientes.filter(function (item) {
        return (item.id === idPedido);
    });
    var pedido = findResult[0];

    console.log(pedido);

    var formaPagamentoContainer = '<div style="display: block;text-align:start;font-size: 14px;">';

    $.each(JSON.parse(pedido.jsonFormaPagamentoAux), function (index, formaPagamento) {
        var formaPagamento = '          <div class="d-flex justify-content-between mt-2"><div class="form-check">  ' +
            '                           <input class="form-check-input" type="checkbox" onchange="radioFormaPagamentoChange(this,' + idPedido + ')" name="radioFormaPagamento" id="radioFormaPagamento_' + formaPagamento.id + '" >  ' +
            '                           <label class="form-check-label unselectable">  ' +
            '                                ' + formaPagamento.nombre + '  ' +
            '                           </label>  ' +
            '                           </div> ' +
            '                           <div class="d-flex justify-content-between"> ' +
            '                           <div><input name="valorFormaPagamento" id="valorFormaPagamento_' + formaPagamento.id + '" onchange="valorFormaPagamentoInput(this,' + idPedido + ')" class="form-control form-control-sm  float-right " /></div>  ' +
            '                           <div><input name="sumarvalorFormaPagamento" id="sumarvalorFormaPagamento_' + formaPagamento.id + '" onchange="sumarvalorFormaPagamentoInput(this,' + idPedido + ')" class="form-control form-control-sm  float-right ml-1" style="width:70px" placeholder="+" /></div>  ' +
            '                           <div><input hidden id="tasaFormaPagamento_' + formaPagamento.id + '" class="form-control form-control-sm  float-right " style="width:70px" value="' + formaPagamento.tasa + '" /></div>  ' +
            '                       </div>  ' +
            '                       </div>  ';
        formaPagamentoContainer += formaPagamento;
    });

    deliveryCartaoHidden = "display:inline-block";
    deliveryDinheiroHidden = "display:inline-block";
    deliveryPagoHidden = "display:inline-block";
    deliveryInputDinheiro = "display:flex";

    if (!pedido.deliveryEmCartao) {
        deliveryCartaoHidden = 'display:none';
    }

    if (!pedido.deliveryEmdinheiro) {
        deliveryDinheiroHidden = 'display:none';
    }

    if (!pedido.deliveryPago) {
        deliveryPagoHidden = 'display:none';
        deliveryInputDinheiro = 'display:none';
    }

    var deliveryPagamento = '  <hr/>  <div class="btn-toolbar mb-3" role="toolbar" aria-label="Toolbar with button groups">  ' +
        '                       <div class="btn-group mr-2" role="group" aria-label="First group">  ' +
        '                           <button id="_deliveryEmCartaoCheck2" type="button" class="btn btn-secondary" name="buttonDeliveryCheck2">  ' +
        '                               Cartão  ' +
        '                               <i id="deliveryEmCartaoCheck2"  style="' + deliveryCartaoHidden + ';color:white;font-size: 12px;" class="fa fa-check"></i>  ' +
        '                           </button>  ' +
        '                           <button id="_deliveryPagoCheck2" type="button" class="btn btn-secondary" name="buttonDeliveryCheck2">  ' +
        '                               Pago  ' +
        '                               <i id="deliveryPagoCheck2"  class="fa fa-check" style=" ' + deliveryDinheiroHidden + ';color: white;font-size: 12px;"></i>  ' +
        '                           </button>  ' +
        '                           <button id="_deliveryEmdinheiroCheck2" type="button" class="btn btn-secondary" name="buttonDeliveryCheck2">  ' +
        '                               Dineiro  ' +
        '                               <i id="deliveryEmdinheiroCheck2"  class="fa fa-check" style="' + deliveryPagoHidden + ';color: white;font-size: 12px;"></i>  ' +
        '                           </button>  ' +
        '                       </div>  ' +
        '     ' +
        '                       <div  class="input-group" id="divDeliveryDinheiroTotal2" style="' + deliveryInputDinheiro + '">  ' +
        '                           <div class="input-group-prepend">  ' +
        '                               <div class="input-group-text" id="btnGroupAddon2">R$</div>  ' +
        '                           </div>  ' +
        '                           <input type="text" id="inputDeliveryDinheiroTotal2" style="width:100px" class="form-control" placeholder="Dinheiro">  ' +
        '                       </div>  ' +
        '                  </div>  ';

    // formaPagamentoContainer += deliveryPagamento;


    formaPagamentoContainer += '<hr/><div style="font-size: 13px;font-weight:700"><div class="d-flex mb-2 justify-content-between"><div><b>Total</b></div><div id="divTotalPagar"></div></div>';
    formaPagamentoContainer += '<div class="d-flex mb-2 justify-content-between"><div><b>Pago</b></div><div id="divResultadoCalculoFormaPagamento"></div></div>';
    formaPagamentoContainer += '<div class="d-flex mb-2 justify-content-between"><div><b>Diferença</b></div><div id="divDiferençaCalculoFormaPagamento"></div></div></div>';

    formaPagamentoContainer += '</div>';


    Swal.fire({
        title: 'Finalizar Pedido',
        html: '   <div class="card card-body" style="font-size: 14px;">  ' +
            '                       <div class="row col-12 d-block justify-content-center">  ' +
            '                           <div class="d-flex mb-2">  ' +
            '                               <label class="mr-2 col-5">Desconto:</label>  ' +
            '                               <input id="inputDescontoFinalizado" name="inputDescontoFinalizado" placeholder="Desconto" class="form-control form-control-sm " />  ' +
            '                           </div>' +
            '                           <div class="d-flex mb-2">  ' +
            '                               <label class="mr-2 col-5">Taxa entrega:</label>  ' +
            '                               <input id="inputTasaFormaPagamentoDelivery" placeholder="Taxa de entrega" class="form-control form-control-sm " />  ' +
            '                           </div><hr/>' +
            '     ' + formaPagamentoContainer +
            //'   <hr/> <div class="form-check d-flex mb-2">  ' +
            //'                                   <input class="form-check-input" type="checkbox" id="inputPagoFinalizado" name="inputPagoFinalizado">  ' +
            //'                                   <label class="form-check-label">Marcar como pago </label>  ' +
            //'                              </div>  ' +
            //'                       </div>  ' +
            '                  </div>  ',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        width: '600px',
        confirmButtonText: 'Salvar!',
        cancelButtonText: 'Não',
        onOpen: function () {
            $('.swal2-confirm').prop('id', 'modalBtnOk');
            $('.swal2-confirm').prop('disabled', true);
        },
    }).then((result) => {
        if (result.isConfirmed) {

            var formasPagamento = [];
            var formasPagSelected = $('input[name="radioFormaPagamento"]:checked');
            $.each(formasPagSelected, function (index, item) {
                var idFormaPagamento = $(item).prop('id').split('_')[1];
                var valor = $('#valorFormaPagamento_' + idFormaPagamento + '').val();
                var tasa = $('#tasaFormaPagamento_' + idFormaPagamento + '').val();
                var valorTasa = 0;
                if (tasa !== null && tasa !== undefined && tasa !== "null") {
                    valorTasa = (tasa / 100) * valor;
                } else {
                    tasa = null;
                }

                formasPagamento.push({
                    id: idFormaPagamento,
                    valor: parseFloat(valor),
                    tasa: tasa,
                    valorTasa: valorTasa
                });
            });

            finalizarPedido = {
                idPedido: idPedido,
                descuento: parseFloat($('#inputDescontoFinalizado').val()),
                pago: $("#inputPagoFinalizado").prop('checked'),
                listaFormaPagamento: JSON.stringify(formasPagamento),
                tasaEntrega: $('#inputTasaFormaPagamentoDelivery').val() === undefined ? 0 : parseFloat($('#inputTasaFormaPagamentoDelivery').val()),
                troco: $('#inputTrocoFormaPagamento').val() === "" ? 0 : $('#inputTrocoFormaPagamento').val(),
                finalizar: finalizar,
                deliveryDinheiroTotal: parseFloat($('#inputDeliveryDinheiroTotal2').val()),
                deliveryEmCartao: _ModalDeliveryFormaPagamento.deliveryEmCartao,
                deliveryPago: _ModalDeliveryFormaPagamento.deliveryPago,
                deliveryEmdinheiro: _ModalDeliveryFormaPagamento.deliveryEmdinheiro
            };

            console.log(finalizarPedido);

            $.ajax({
                type: "POST",
                url: "/Pedidos/ActualizarFormaPagamento",
                traditional: true,
                data: JSON.stringify(finalizarPedido),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (pedido) {

                    console.log(pedido);

                    var objIndex = _PedidosPendientes.findIndex((p => p.id == pedido.id));
                    _PedidosPendientes[objIndex] = pedido;

                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'success',
                        title: 'Ação realizada com sucesso'
                    })
                    if (finalizar) {
                        $('#CARD_PEDIDO_' + idPedido + '').remove();
                        GetNumeroPedidosFinalizados();
                    } else {
                        MostarPedidosPendientes();
                    }
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

    $("#inputDescontoFinalizado").mask("###0.00", { reverse: true });
    $("#inputTasaFormaPagamento").mask("###0.00", { reverse: true });
    $("#inputTrocoFormaPagamento").mask("###0.00", { reverse: true });

    $.each(JSON.parse(pedido.jsonFormaPagamentoAux), function (index, formaPagamento) {
        $('#valorFormaPagamento_' + formaPagamento.id + '').mask("###0.00", { reverse: true });
        $('#sumarvalorFormaPagamento_' + formaPagamento.id + '').mask("###0.00", { reverse: true });
        $('#valorFormaPagamento_' + formaPagamento.id + '').prop('disabled', true);
        $('#sumarvalorFormaPagamento_' + formaPagamento.id + '').prop('disabled', true);
    });

    var findResult = _PedidosPendientes.filter(function (item) {
        return (item.id === idPedido);
    });
    var pedido = findResult[0];

    $("#inputDescontoFinalizado").val(pedido.descuento);
    $("#inputPagoFinalizado").prop('checked', pedido.DeliveryPago);


    $('#inputTasaFormaPagamento').on('input propertychange', function (e) {
        calcularTotalPagado();
    });

    $('#inputTrocoFormaPagamento').on('input propertychange', function (e) {
        calcularTotalPagado();
    });

    $("#inputDeliveryDinheiroTotal2").mask("###0.00", { reverse: true });

    $('button[name="buttonDeliveryCheck2"]').on('click', function (e) {
        e.preventDefault();

        var id = $(e.target)[0].id;

        if (id === "_deliveryEmCartaoCheck2") {
            $('#deliveryEmCartaoCheck2').show();
            $('#deliveryEmdinheiroCheck2').hide();
            $('#deliveryPagoCheck2').hide();

            _ModalDeliveryFormaPagamento.deliveryEmCartao = true;
            _ModalDeliveryFormaPagamento.deliveryPago = false;
            _ModalDeliveryFormaPagamento.deliveryEmdinheiro = false;

            $('#divDeliveryDinheiroTotal2').hide();
            $('#inputDeliveryDinheiroTotal2').val(null);
        }

        if (id === "_deliveryPagoCheck2") {
            $('#deliveryPagoCheck2').show();
            $('#deliveryEmdinheiroCheck2').hide();
            $('#deliveryEmCartaoCheck2').hide();

            _ModalDeliveryFormaPagamento.deliveryEmCartao = false;
            _ModalDeliveryFormaPagamento.deliveryPago = true;
            _ModalDeliveryFormaPagamento.deliveryEmdinheiro = false;

            $('#divDeliveryDinheiroTotal2').hide();
            $('#inputDeliveryDinheiroTotal2').val(null);

        }

        if (id === "_deliveryEmdinheiroCheck2") {
            $('#deliveryEmdinheiroCheck2').show();
            $('#deliveryPagoCheck2').hide();
            $('#deliveryEmCartaoCheck2').hide();

            _ModalDeliveryFormaPagamento.deliveryEmCartao = false;
            _ModalDeliveryFormaPagamento.deliveryPago = false;
            _ModalDeliveryFormaPagamento.deliveryEmdinheiro = true;

            $('#divDeliveryDinheiroTotal2').show();
        }

    });

}

function radioFormaPagamentoChange(input, idPedido) {
    var idFormaPagamento = $(input).prop('id').split('_')[1];
    var selected = $(input).prop('checked');
    var numberOfChecked = $('input[name="radioFormaPagamento"]:checked').length;

    if (firstInputChecked === null) {
        firstInputChecked = input;
    }

    //TOTAL DEL PEDIDO
    var findResult = _PedidosPendientes.filter(function (item) {
        return (item.id === idPedido);
    });
    var pedido = findResult[0];

    totalPedido = pedido.valorProductos;

    var desconto = $('#inputDescontoFinalizado').val();
    if (!isNaN(parseFloat(desconto))) {
        totalPedido = totalPedido - desconto;
    }


    //HABILITAR DESCUENTO

    if (numberOfChecked > 0) {
        $('#inputDescontoFinalizado').prop('disabled', true);
    } else {
        $('#inputDescontoFinalizado').prop('disabled', false);
        firstInputChecked = null;
        valorInputFormaPagamento = 0;
    }

    if (numberOfChecked > 3) {
        $(input).prop("checked", false);
        return;
    }

    //HABILITAR CAMPOS DE TEXTO

    if (selected) {
        $('#valorFormaPagamento_' + idFormaPagamento + '').prop('disabled', false);
        $('#sumarvalorFormaPagamento_' + idFormaPagamento + '').prop('disabled', false);

        if (numberOfChecked === 1) {
            $('#valorFormaPagamento_' + idFormaPagamento + '').val(totalPedido.toFixed(2));
        }

    } else {
        $('#valorFormaPagamento_' + idFormaPagamento + '').prop('disabled', true);
        $('#sumarvalorFormaPagamento_' + idFormaPagamento + '').prop('disabled', true);
        $('#valorFormaPagamento_' + idFormaPagamento + '').val(null);
        $('#sumarvalorFormaPagamento_' + idFormaPagamento + '').val(null);
    }

    $('#divTotalPagar').html('R$ ' + totalPedido.toFixed(2));
    calcularTotalPagado();
}

function valorFormaPagamentoInput(input, idPedido) {
    var idFormaPagamento = $(input).prop('id').split('_')[1];
    var numberOfChecked = $('input[name="radioFormaPagamento"]:checked').length;
    var valor = $(input).val();

    //PRIMER INPUT ABLITADO
    if (numberOfChecked === 2) {
        var firstInputCheckedID = "valorFormaPagamento_" + $(firstInputChecked).prop('id').split('_')[1];

        if ($(input).prop('id') !== firstInputCheckedID) {

            if (isNaN(valor)) {
                valor = 0;
            }
            var diferencia = totalPedido - valor;
            if (diferencia < 0) {
                $(input).val(0);
            } else {
                $('#' + firstInputCheckedID + '').val(diferencia.toFixed(2));
            }
        }
    }

    calcularTotalPagado();
}

function sumarvalorFormaPagamentoInput(input, idPedido) {
    var idFormaPagamento = $(input).prop('id').split('_')[1];
    var numberOfChecked = $('input[name="radioFormaPagamento"]:checked').length;
    var valor = $(input).val();
    var oldValue = $('#valorFormaPagamento_' + idFormaPagamento + '').val();

    if (valor !== null && valor !== undefined && valor !== "") {
        if (oldValue === null || oldValue === undefined || oldValue === "") {
            oldValue = 0;
        }
        $('#valorFormaPagamento_' + idFormaPagamento + '').val((parseFloat(valor) + parseFloat(oldValue)).toFixed(2));
    }

    //PRIMER INPUT ABLITADO
    var newValue = $('#valorFormaPagamento_' + idFormaPagamento + '').val();
    if (numberOfChecked === 2) {
        var firstInputCheckedID = "valorFormaPagamento_" + $(firstInputChecked).prop('id').split('_')[1];

        if ($(input).prop('id') !== firstInputCheckedID) {

            if (isNaN(newValue)) {
                newValue = 0;
            }
            var diferencia = totalPedido - newValue;
            if (diferencia < 0) {
                $(input).val(0);
            } else {
                $('#' + firstInputCheckedID + '').val(diferencia.toFixed(2));
            }
        }
    }

    $(input).val(null);
    console.log(valor, oldValue);

    calcularTotalPagado();

}

function calcularTotalPagado() {

    valorInputFormaPagamento = 0;
    $.each($('input[name="valorFormaPagamento"]'), function (index, input) {
        if (!isNaN(parseFloat($(input).val()))) {
            valorInputFormaPagamento += parseFloat($(input).val());
        }
    });

    var tasa = isNaN(parseFloat($('#inputTasaFormaPagamento').val())) ? 0 : parseFloat($('#inputTasaFormaPagamento').val());
    var troco = isNaN(parseFloat($('#inputTrocoFormaPagamento').val())) ? 0 : parseFloat($('#inputTrocoFormaPagamento').val());

    valorInputFormaPagamento = valorInputFormaPagamento + tasa;
    valorInputFormaPagamento = valorInputFormaPagamento - troco;

    $('#divResultadoCalculoFormaPagamento').html('R$ ' + valorInputFormaPagamento.toFixed(2));
    $('#divDiferençaCalculoFormaPagamento').html('R$ ' + (totalPedido - valorInputFormaPagamento).toFixed(2));

    if (parseFloat(totalPedido - valorInputFormaPagamento) === 0) {
        $('#modalBtnOk').prop('disabled', false);
    } else {
        $('#modalBtnOk').prop('disabled', true);
    }
}

function editar(idPedido) {
    $.ajax({
        type: "GET",
        url: "/Pedidos/GetCurrentPedido/" + idPedido,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            _CurrentPedido = data.currentPedido;
            _CurrentPedido.isNew = false;

            _ModalProducto.cliente = _CurrentPedido.cliente;
            _ModalProducto.idCliente = _CurrentPedido.idCliente;
            _ModalProducto.aplicativo = _CurrentPedido.aplicativo;
            _ModalProducto.idAplicativo = _CurrentPedido.idAplicativo;
            _ModalProducto.idMesa = _CurrentPedido.idMesa;
            _ModalProducto.direccion = _CurrentPedido.direccion;
            _ModalProducto.idDireccion = _CurrentPedido.idDireccion;
            _ModalProducto.telefono = _CurrentPedido.telefono;

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

function CancelarCurrentPedido() {
    $.ajax({
        type: "GET",
        url: "/Pedidos/CancelarCurrentPedido/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            _CurrentPedido = data.currentPedido;
            _ModalProducto = {
                cliente: '',
                idCliente: null,
                aplicativo: '',
                idAplicativo: null,
                idMesa: null,
                direccion: '',
                telefono: '',
                observacion: ''
            };
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

function GetNumeroPedidosFinalizados() {
    $.ajax({
        type: "GET",
        url: "/Pedidos/GetNumeroPedidosFinalizados/",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#spanCountPedidosFinalizados').html(data);
        },
        failure: function (response) {
            console.log('failure', response);
        },
        error: function (response) {
            console.log('error', response);

        }
    });
}