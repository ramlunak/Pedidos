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
var _ModalSabores = [];
var SaboresSelecionados = 0;
var CantidadSabores = 0;

$(function () {

    $('html').css('overflowY', 'scroll');
    $('html').css('overflowX', 'hidden');

    $("#inputDescuento").mask("###0.00", { reverse: true });

    GetNumeroPedidosFinalizados();
    CargarCurrentPedido();
    CargarPedidosPendientes();
    CargarProductos();
    CargarListaTelefonos();
    CargarBarrios();

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
        CargarTelefono(id);

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

            //  $('#divInfoPagamentoDelivery').hide();

            $('#inputAplicativo').val(null);
            $('#idAplicativo').val('');
            $('#inputAplicativo').trigger("input");
            $('#inputAplicativo').hide()


            $('#inputEndereco').val(null);
            $('#idDireccion').val('');
            $('#inputEndereco').trigger("input");
            $('#inputEndereco').hide()

            $('#inputBarrio').val(null);
            $('#idBarrio').val('');
            $('#inputBarrio').trigger("input");
            $('#inputBarrio').hide()

        } else {
            $('#spanMesa').hide();
            //  $('#divInfoPagamentoDelivery').show();
            $('#inputEndereco').show();
            $('#inputBarrio').show();
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


    $('#inputBarrio').on('input propertychange', function (e) {
        $('#spanBarrio').html($('#inputBarrio').val());

        var opt = $('option[value="' + $(this).val() + '"]');
        var id = opt.length ? opt.attr('id') : '';

        if (id !== '' && id !== undefined) {
            $('#inputBarrio').css({ "border-color": "#04CD5A", "border-weight": "2px", "border-style": "solid" });
            $('#idBarrio').val(id);
        } else {
            $('#inputBarrio').css({ "border": "1px solid #ced4da" });
            $('#idBarrio').val('');
        }

        _CurrentPedido.barrio = $('#inputBarrio').val();
        _ModalProducto.barrio = $('#inputBarrio').val();

    });

    $('#inputTelefone').on('input propertychange', function (e) {
        _ModalProducto.telefono = $('#inputTelefone').val();
    });

    $("#inputTelefone").change(function () {
        CargarClientePorTelefono($("#inputTelefone").val());
    });

    $('#inputProducto').on('input propertychange', function (e) {

        var filtro = $('#inputProducto').val();
        if (filtro !== null && filtro !== "") {
            var productosFiltrados = filterItems(filtro);
            FiltrarProductos(productosFiltrados);
        } else {
            productosFiltrados = [];
            FiltrarProductos(productosFiltrados);
        }
    });

    $("#ModalDetalleProducto").keypress(function (e) {

        if (SaboresSelecionados !== CantidadSabores) return;

        if (e.which == 13) {
            //ENTER KEY
            AddProducto();
        } else if (e.which == 45) {
            //MINUS KEY
            $('#MINUS_Producto').click();

        } else if (e.which == 43) {
            //PLUS KEY
            $('#ADD_Producto').click();

        }
    });

});

function showInputValorProducto(idAdicional) {
    let labelValor = $('#spanValorProducto');

    let inputValor = $('#inputValorProducto');
    $("#inputValorProducto").mask("###0.00", { reverse: true });

    inputValor.val(_ModalProducto.valor);
    labelValor.hide();
    inputValor.show();
    inputValor.focus();
    inputValor.select();
}

function inputValorProductoChanged() {
    let labelValor = $('#spanValorProducto');
    let inputValor = $('#inputValorProducto');
    labelValor.html(parseFloat(inputValor.val()).toFixed(2));
    _ModalProducto.valor = parseFloat(inputValor.val());
    inputValor.hide();
    labelValor.show();

}

//Cargar Direcciones del cliente
function CargarDirecciones(id) {
    $.ajax({
        type: "GET",
        url: "/Pedidos/GetDireccion/" + parseInt(id),
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $("#EnderecoList").empty();

            if (data !== null && data.length > 0) {
                //Llanar lista direcciones

                $("#inputEndereco").val(null);
                $("#spanEndereco").html(null);
            }

            $.each(data, (index, item) => {

                if (index === 0) {
                    $("#EnderecoList").append($('<option selected id="' + item.id + '">').attr('value', item.text));
                    $("#inputEndereco").val(item.text);
                    $("#idDireccion").val(item.id);
                    $("#spanEndereco").html(item.text);

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

//Cargar Lista de Telefonos todos los clientes
function CargarListaTelefonos() {
    $.ajax({
        type: "GET",
        url: "/Pedidos/GetListaTelefonos",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Llanar lista direcciones
            $("#TelefonoList").empty();

            $.each(data, (index, item) => {

                if (item !== null && item !== "") {
                    $("#TelefonoList").append($('<option >').attr('value', item));
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

//Cargar Barrios
function CargarBarrios() {
    $.ajax({
        type: "GET",
        url: "/Pedidos/GetBarrios",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            //Llanar lista barrios
            $("#BarrioList").empty();

            $.each(data, (index, item) => {

                $("#BarrioList").append($('<option id="' + item.id + '">').attr('value', item.text));

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


//Cargar Cliente Por Telefono
function CargarClientePorTelefono(telefono) {
    $.ajax({
        type: "GET",
        url: "/Pedidos/CargarClientePorTelefono?telefono=" + telefono,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data !== null && data !== "") {
                $('#inputNome').val(data);
                $('#inputNome').trigger('input');
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


function CargarTelefono(id) {
    $.ajax({
        type: "GET",
        url: "/Pedidos/CargarTelefono/" + parseInt(id),
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data !== null && data.length > 0) {
                $("#inputTelefone").val(data);
            }
            ////Llanar lista direcciones
            //
            //$.each(data, (index, item) => {

            //    if (index === 0) {
            //        $("#EnderecoList").append($('<option selected id="' + item.id + '">').attr('value', item.text));
            //    } else {
            //        $("#EnderecoList").append($('<option id="' + item.id + '">').attr('value', item.text));
            //    }

            //});

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
        var divTamanhos = $('<div style="display: inline-table">');

        if (item.valor !== 0)
            divTamanhos.append('<div class="btn btn-sm btn-secondary text-nowrap  m-1 d-flex" style="width:max-content;float: left;"> <div class="ml-1" style="color:chartreuse">R$ ' + item.valor + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho1 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary text-nowrap  m-1 d-flex" style="width:max-content;float: left;">' + item.tamanho1 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho1.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho2 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary text-nowrap  m-1 d-flex" style="width:max-content;float: left;">' + item.tamanho2 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho2.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho3 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary text-nowrap  m-1 d-flex" style="width:max-content;float: left;">' + item.tamanho3 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho3.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho4 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary text-nowrap  m-1 d-flex" style="width:max-content;float: left;">' + item.tamanho4 + ' <div class="ml-1 " style="color:chartreuse">R$ ' + item.valorTamanho4.toFixed(2) + '</div> </div>');
        if (item.valor === 0 && item.valorTamanho5 !== null)
            divTamanhos.append('<div class="btn btn-sm btn-secondary text-nowrap  m-1 d-flex" style="width:max-content;float: left;">' + item.tamanho5 + ' <div class="ml-1" style="color:chartreuse">R$ ' + item.valorTamanho5.toFixed(2) + '</div> </div>');

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
        url: "/Productos/GetProductos",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            console.log('data', data);
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
            _ModalProducto.idBarrio = datosClienteFormulario.idBarrio;
            _ModalProducto.barrio = datosClienteFormulario.barrio;
            _ModalProducto.telefono = datosClienteFormulario.telefono;

            _ModalProducto.deliveryEmCartao = datosClienteFormulario.deliveryEmCartao;
            _ModalProducto.deliveryPago = datosClienteFormulario.deliveryPago;
            _ModalProducto.deliveryEmdinheiro = datosClienteFormulario.deliveryEmdinheiro;

            _ModalAdicionales = data.adicionales;
            _ModalIngredientes = data.ingredientes;
            _ModalSabores = data.sabores;

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

function ModalMostarValorProducto() {

    let valorProducto = _ModalProducto.valor;

    if (_ModalProducto.valorTamanhoSeleccionado != null && _ModalProducto.valorTamanhoSeleccionado > 0) {
        valorProducto = _ModalProducto.valorTamanhoSeleccionado;
    }

    $.each(_ModalAdicionales, function (index, item) {
        if (item.valor != null && item.cantidad > 0) {
            valorProducto = valorProducto + (item.cantidad * item.valor);
        }
    });

    $.each(_ModalSabores, function (index, item) {
        if (item.valor != null && item.selected) {
            valorProducto = valorProducto + item.valor;
        }
    });

    $('#spanValorProducto').html((_ModalProducto.cantidad * parseFloat(valorProducto)).toFixed(2));
    $('#spanValorProducto').animate({ fontSize: '18px' }, 80);
    $('#spanValorProducto').animate({ fontSize: '15px' }, 80);

    if (SaboresSelecionados !== CantidadSabores) {
        $('#btnModalAdicionar').prop('disabled', true);
    } else {
        $('#btnModalAdicionar').prop('disabled', false);
    }

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
    TABLE_Sabores(data.sabores, data.producto.id, data.producto.cantidadSabores)

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

    if (producto.tamanho4 !== null && producto.tamanho4 !== "" && producto.tamanho4 !== undefined) {
        $('#nomeTamanho4').html(producto.tamanho4);
        $('#valorTamanho4').html(producto.valorTamanho4.toFixed(2));
        $('#btnTamanho4').addClass('btn-outline-primary');
        $('#btnTamanho4').show();
        $('#checkedTamanho4').hide();
    } else {
        $('#btnTamanho4').hide();
    }

    if (producto.tamanho5 !== null && producto.tamanho5 !== "" && producto.tamanho5 !== undefined) {
        $('#nomeTamanho5').html(producto.tamanho5);
        $('#valorTamanho5').html(producto.valorTamanho5.toFixed(2));
        $('#btnTamanho5').addClass('btn-outline-primary');
        $('#btnTamanho5').show();
        $('#checkedTamanho5').hide();
    } else {
        $('#btnTamanho5').hide();
    }

}

function checkTamanho(tamanho) {


    if (tamanho === 1) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho4').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho5').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#checkedTamanho1').show('slow');
        $('#checkedTamanho2').hide('slow');
        $('#checkedTamanho3').hide('slow');
        $('#checkedTamanho4').hide('slow');
        $('#checkedTamanho5').hide('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho1;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho1;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho1.toFixed(2));

    } else if (tamanho === 2) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho4').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho5').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#checkedTamanho1').hide('slow');
        $('#checkedTamanho2').show('slow');
        $('#checkedTamanho3').hide('slow');
        $('#checkedTamanho4').hide('slow');
        $('#checkedTamanho5').hide('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho2;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho2;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho2.toFixed(2));

    } else if (tamanho === 3) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#btnTamanho4').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho5').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#checkedTamanho1').hide('slow');
        $('#checkedTamanho2').hide('slow');
        $('#checkedTamanho3').show('slow');
        $('#checkedTamanho4').hide('slow');
        $('#checkedTamanho5').hide('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho3;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho3;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho3.toFixed(2));
    } else if (tamanho === 4) {
        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho4').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#btnTamanho5').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#checkedTamanho1').hide('slow');
        $('#checkedTamanho2').hide('slow');
        $('#checkedTamanho3').hide('slow');
        $('#checkedTamanho4').show('slow');
        $('#checkedTamanho5').hide('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho4;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho4;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho4.toFixed(2));
    } else if (tamanho === 5) {

        $('#btnTamanho1').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho2').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho3').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho4').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-outline-primary');
        $('#btnTamanho5').removeClass('btn-outline-primary').removeClass('btn-primary').addClass('btn-primary');
        $('#checkedTamanho1').hide('slow');
        $('#checkedTamanho2').hide('slow');
        $('#checkedTamanho3').hide('slow');
        $('#checkedTamanho4').hide('slow');
        $('#checkedTamanho5').show('slow');
        _ModalProducto.tamanhoSeleccionado = _ModalProducto.tamanho5;
        _ModalProducto.valorTamanhoSeleccionado = _ModalProducto.valorTamanho5;
        $('#spanValorProducto').html(_ModalProducto.valorTamanho5.toFixed(2));
    }

    ModalMostarValorProducto();
}


//crear tabla de los adicionales en el modal
function TABLE_Adicional(adicionales, idProducto) {

    var TABLE = $('#modalTableAdicionales');
    TABLE.empty();

    if (adicionales.length === 0) {
        $('#divSeparadorAdicionales').hide();
    } else {
        $('#divSeparadorAdicionales').show();
    }

    $.each(adicionales, function (index, item) {

        var TD1 = $('<td style="width:100%">');
        var TD2 = $('<td>');
        var TD3 = $('<td>');
        var TR = $('<tr>');

        var codigo = "ADC_" + item.id + "_" + idProducto;
        var minusId = "Minus_" + item.id + "_" + idProducto;

        TD1.append('<div class="unselectable"> <a id=' + codigo + ' style="color:blue"></a> ' + item.nombre + '</div>');
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

    if (ingredientes.length === 0) {
        $('#divSeparadorIngredientes').hide();
    } else {
        $('#divSeparadorIngredientes').show();
    }

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


//crear tabla de los ingredientes en el modal
function TABLE_Sabores(sabores, idProducto, cantidadSabores) {

    var TABLE = $('#modalTableSabores');
    TABLE.empty();

    if (sabores.length === 0 || cantidadSabores === 0) {

        $('#divSeparadorSabores').hide();
    } else {
        $('#divSeparadorSabores').show();
        if (cantidadSabores === 1) {
            TABLE.append('<tr><td colspan="3" style="text-align: center;"><span style="font-family: cursive;">Seleccione 1 Sabor</span></td></tr>');
        } else if (cantidadSabores > 1) {
            TABLE.append('<tr><td colspan="3" style="text-align: center;"><span style="font-family: cursive;">Seleccione ' + cantidadSabores + ' Sabores</span></td></tr>');
        }

    }

    CantidadSabores = cantidadSabores;

    if (SaboresSelecionados !== CantidadSabores) {
        $('#btnModalAdicionar').prop('disabled', true);
    } else {
        $('#btnModalAdicionar').prop('disabled', false);
    }

    if (cantidadSabores > 0) {
        $.each(sabores, function (index, item) {

            var TD1 = $('<td style="width:100%">');
            var TD2 = $('<td>');
            var TR = $('<tr>');

            let valor = '<div class="mr-2"></div>';

            if (item.valor !== null && item.valor !== undefined && item.valor > 0) {
                valor = '<div class="mr-2">R$ ' + item.valor.toFixed(2) + '</div>';
            }

            TD1.append('<div class="d-flex justify-content-between"> <div>' + item.nombre + '</div> ' + valor + '</div>');
            TD2.append('<div class="cursor-pointer"> <input name="inputChekedSabores" onchange="saboresOnChange(this,' + item.id + ',' + idProducto + ')" type="checkbox" /></div>');

            TR.append(TD1, TD2);
            TABLE.append(TR);
        });
    }
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
            $(codigo).animate({ fontSize: '19px' }, 80);
            $(codigo).animate({ fontSize: '15px' }, 80);

            if (item.cantidad === 0) {
                $(codigo).html('');
            }
        }
        return item.id === id;
    });

    ModalMostarValorProducto();

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
            $(codigo).animate({ fontSize: '19px' }, "fast",);
            $(codigo).animate({ fontSize: '15px' }, "fast");

            $(codigo).html('+' + item.cantidad);
        }
        return item.id === id;
    });

    ModalMostarValorProducto();
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

//evento de marcar y desmarcar ingredeinte
function saboresOnChange(input, id, idProducto) {

    $.grep(_ModalSabores, (item, index) => {
        if (item.id === id) {
            item.selected = $(input).is(":checked");
        }
        return item.id === id;
    });

    if (SaboresSelecionados == CantidadSabores) {

        $(input).prop("checked", false);

        $.grep(_ModalSabores, (item, index) => {
            if (item.id === id) {
                item.selected = $(input).is(":checked");
            }
            return item.id === id;
        });

    }

    SaboresSelecionados = _ModalSabores.filter(x => x.selected).length;
    if (SaboresSelecionados == CantidadSabores) {
        $('input[name="inputChekedSabores"]:unchecked').prop('disabled', true);
    } else {
        $('input[name="inputChekedSabores"]').prop('disabled', false);
    }

    ModalMostarValorProducto();
}

//evento de restar contidad producto
function productoMinus(btn) {

    if (parseInt(_ModalProducto.cantidad) === 1) {
        return;
    }

    _ModalProducto.cantidad--;
    $('#modalCantidadProducto').html('(' + _ModalProducto.cantidad + ')');

    $('#modalCantidadProducto').animate({ fontSize: '19px' }, 80);
    $('#modalCantidadProducto').animate({ fontSize: '17px' }, 80);
    ModalMostarValorProducto();

    if (parseInt(_ModalProducto.cantidad) === 1) {
        $("#MINUS_Producto").attr('disabled', 'disabled');
    }

}

//evento de adicionar contidad de productos
function productoPlus() {
    _ModalProducto.cantidad++;
    $('#modalCantidadProducto').html('(' + _ModalProducto.cantidad + ')');

    $('#modalCantidadProducto').animate({ fontSize: '19px' }, 80);
    $('#modalCantidadProducto').animate({ fontSize: '17px' }, 80);
    ModalMostarValorProducto();

    $("#MINUS_Producto").removeAttr('disabled');

}

//agregar el producto al pedido en adicion
function AddProducto() {

    _ModalProducto.adicionales = _ModalAdicionales;
    _ModalProducto.ingredientes = _ModalIngredientes;
    _ModalProducto.sabores = _ModalSabores;

    _ModalProducto.observacion = $('#inputObservacion').val();

    _ModalProducto.idCliente = parseInt($('#idCliente').val());
    _ModalProducto.idAplicativo = parseInt($('#idAplicativo').val());
    _ModalProducto.idMesa = parseInt($('#idMesa').val());
    _ModalProducto.idDireccion = parseInt($('#idDireccion').val());
    _ModalProducto.direccion = $('#inputEndereco').val();
    _ModalProducto.idBarrio = parseInt($('#idBarrio').val());
    _ModalProducto.barrio = $('#inputBarrio').val();
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


        },
        failure: function (response) {
            console.log('failure', response);

        },
        error: function (response) {
            console.log('error', response);

        }
    });

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
            _ModalProducto.idBarrio = _CurrentPedido.idBarrio;
            _ModalProducto.barrio = _CurrentPedido.barrio;
            _ModalProducto.telefono = _CurrentPedido.telefono;

            if (_CurrentPedido.deliveryEmCartao) {

                metodoPagoDelivery("_deliveryEmCartaoCheck");

            } else if (_CurrentPedido.deliveryPago) {

                metodoPagoDelivery("_deliveryPagoCheck");

            } else if (_CurrentPedido.deliveryEmdinheiro) {

                metodoPagoDelivery("_deliveryEmdinheiroCheck");
                $('#inputDeliveryDinheiroTotal').val(_CurrentPedido.deliveryDinheiroTotal);
            }

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

    //if (_CurrentPedido.idCliente != null && _CurrentPedido.idCliente != undefined && _CurrentPedido.idCliente != "") {

    //}

    $('#inputNome').val(_CurrentPedido.cliente);

    $('#inputTelefone').val(_CurrentPedido.telefono);

    $('#inputAplicativo').val(_CurrentPedido.aplicativo);
    $('#idAplicativo').val(_CurrentPedido.idAplicativo);

    $('#inputEndereco').val(_CurrentPedido.direccion);
    $('#idDireccion').val(_CurrentPedido.idDireccion);

    $('#inputBarrio').val(_CurrentPedido.barrio);
    $('#idBarrio').val(_CurrentPedido.idBarrio);
    $('#spanBarrio').html(_CurrentPedido.barrio);

    $('#inputDescuento').val(_CurrentPedido.descuento);
    $('#inputPago').prop("checked", _CurrentPedido.DeliveryPago);

    $('#spanTotal').html((_CurrentPedido.valorProductos + _CurrentPedido.tasaEntrega - _CurrentPedido.descuento).toFixed(2));

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
        var btnDelete = '<div class="btn btn-sm btn-outline-danger ml-1" data-toggle="tooltip" data-placement="top" title="Cancelar Produto"  onclick="deleteCurrentProducto(' + item.id + ')"><i class="fas fa-ban"></i></div>';

        TD1.append('<div>(<b>' + item.cantidad + '</b>) ' + item.nombre.toUpperCase() + '</div>');
        TD2.append('<div style="font-size:12px;text-align:center;font-weight:700;justify-content: flex-end;" class="cursor-pointer d-flex text-nowrap"> <span> R$ ' + item.valorMasAdicionales.toFixed(2) + '</span><div> ' + btnDelete + '</div></div>');

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

function GuardarCurrentPedido(cadastrar) {

    var pedido = {
        cliente: $('#inputNome').val(),
        idCliente: parseInt($('#idCliente').val()),
        idAplicativo: parseInt($('#idAplicativo').val()),
        aplicativo: $('#inputAplicativo').val(),
        idMesa: parseInt($('#idMesa').val()),
        direccion: $('#inputEndereco').val(),
        barrio: $('#inputBarrio').val(),
        idBarrio: parseInt($('#idBarrio').val()),
        idDireccion: parseInt($('#idDireccion').val()),
        telefono: $('#inputTelefone').val(),
        deliveryDinheiroTotal: parseFloat($('#inputDeliveryDinheiroTotal').val()),
        deliveryEmCartao: _ModalProducto.deliveryEmCartao,
        deliveryPago: _ModalProducto.deliveryPago,
        deliveryEmdinheiro: _ModalProducto.deliveryEmdinheiro,
        cadastrarCliente: cadastrar
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
                    idBarrio: null,
                    direccion: '',
                    barrio: '',
                    telefono: '',
                    observacion: ''
                };

                metodoPagoDelivery("_deliveryEmCartaoCheck");

                $("#EnderecoList").empty();
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

function fnFinalizarPedido(idPedido, finalizarConfirmado) {

    var findResult = _PedidosPendientes.filter(function (item) {
        return (item.id === idPedido);
    });
    var pedido = findResult[0];

    if (pedido.jsonFormaPagamento === undefined || pedido.jsonFormaPagamento === null || pedido.jsonFormaPagamento === "") {
        Swal.fire(
            'Informação',
            'Complete os dados de forma de pagamento do pedido.',
            'info'
        )
        return;
    }

    if (!finalizarConfirmado) {
        Swal.fire({
            title: 'Finalizar Pedido',
            text: "Tem certeza que deseja finalizar o pedido?",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            width: '600px',
            confirmButtonText: 'Finalizar!',
            cancelButtonText: 'Cancelar',
        }).then((result) => {

            if (result.isConfirmed) {

                $.ajax({
                    type: "GET",
                    url: "/Pedidos/Finalizar/" + idPedido,
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
                        _PedidosPendientes = $.grep(_PedidosPendientes, function (pedido) {
                            return pedido.id != idPedido;
                        });
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

        });
    } else {
        $.ajax({
            type: "GET",
            url: "/Pedidos/Finalizar/" + idPedido,
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
                _PedidosPendientes = $.grep(_PedidosPendientes, function (pedido) {
                    return pedido.id != idPedido;
                });
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
                    _PedidosPendientes = $.grep(_PedidosPendientes, function (pedido) {
                        return pedido.id != idPedido;
                    });

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

function actualizarFormaPagamento(idPedido) {

    valorInputFormaPagamento = 0;
    totalPedido = 0;
    firstInputChecked = null;

    var findResult = _PedidosPendientes.filter(function (item) {
        return (item.id === idPedido);
    });
    var pedido = findResult[0];


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
        title: 'Forma Pagamento',
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
        showDenyButton: true,
        confirmButtonText: `Save`,
        denyButtonText: `Finalizar`,
        confirmButtonColor: '#3085d6',
        //cancelButtonColor: '#d33',
        width: '600px',
        confirmButtonText: 'Apenas salvar!',
        cancelButtonText: 'Cancelar',
        onOpen: function () {
            $('.swal2-confirm').prop('id', 'modalBtnOk');
            $('.swal2-deny').prop('id', 'modalBtnOkEnd');
            $('.swal2-confirm').prop('disabled', true);
            $('.swal2-deny').prop('disabled', true);
            $('.swal2-popup, .swal2-modal, .swal2-show').css('padding', '0px');
            $('.card, .card-body').css('padding', '5px');
            $('.swal2-content').css('padding', '5px');

            $('input[name="valorFormaPagamento"]').css('width', '110px');
        },
    }).then((result) => {

        if (result.isConfirmed || result.isDenied) {
            if (result.isConfirmed) {
                SetFormaPagamento(idPedido, false);
            } else {
                SetFormaPagamento(idPedido, true);
            }
        }
    })

    $("#inputTasaFormaPagamentoDelivery").mask("###0.00", { reverse: true });
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

    //$("#inputDeliveryDinheiroTotal2").mask("###0.00", { reverse: true });

    //$('button[name="buttonDeliveryCheck2"]').on('click', function (e) {
    //    e.preventDefault();

    //    var id = $(e.target)[0].id;

    //    if (id === "_deliveryEmCartaoCheck2") {
    //        $('#deliveryEmCartaoCheck2').show();
    //        $('#deliveryEmdinheiroCheck2').hide();
    //        $('#deliveryPagoCheck2').hide();

    //        _ModalDeliveryFormaPagamento.deliveryEmCartao = true;
    //        _ModalDeliveryFormaPagamento.deliveryPago = false;
    //        _ModalDeliveryFormaPagamento.deliveryEmdinheiro = false;

    //        $('#divDeliveryDinheiroTotal2').hide();
    //        $('#inputDeliveryDinheiroTotal2').val(null);
    //    }

    //    if (id === "_deliveryPagoCheck2") {
    //        $('#deliveryPagoCheck2').show();
    //        $('#deliveryEmdinheiroCheck2').hide();
    //        $('#deliveryEmCartaoCheck2').hide();

    //        _ModalDeliveryFormaPagamento.deliveryEmCartao = false;
    //        _ModalDeliveryFormaPagamento.deliveryPago = true;
    //        _ModalDeliveryFormaPagamento.deliveryEmdinheiro = false;

    //        $('#divDeliveryDinheiroTotal2').hide();
    //        $('#inputDeliveryDinheiroTotal2').val(null);

    //    }

    //    if (id === "_deliveryEmdinheiroCheck2") {
    //        $('#deliveryEmdinheiroCheck2').show();
    //        $('#deliveryPagoCheck2').hide();
    //        $('#deliveryEmCartaoCheck2').hide();

    //        _ModalDeliveryFormaPagamento.deliveryEmCartao = false;
    //        _ModalDeliveryFormaPagamento.deliveryPago = false;
    //        _ModalDeliveryFormaPagamento.deliveryEmdinheiro = true;

    //        $('#divDeliveryDinheiroTotal2').show();
    //    }

    //});

    //CARGAR FORMA PAGAMENTO

    $('#inputDescontoFinalizado').val(pedido.descuento);
    $('#inputTasaFormaPagamentoDelivery').val(pedido.tasaEntrega);

    $.each(JSON.parse(pedido.jsonFormaPagamento), function (index, formaPagamento) {

        $("#radioFormaPagamento_" + formaPagamento.id).prop('checked', true);
        $("#radioFormaPagamento_" + formaPagamento.id).trigger('onchange');

        $("#valorFormaPagamento_" + formaPagamento.id).val(formaPagamento.valor);
        $("#valorFormaPagamento_" + formaPagamento.id).trigger('input');

        calcularTotalPagado();

    });
}

function SetFormaPagamento(idPedido, finalizar) {

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

    const invalid = formasPagamento.filter(x => isNaN(parseFloat(x.valor)));
    if (invalid.length > 0) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Preencha todos os campos corretamente!'
        })
        return;
    }

    finalizarPedido = {
        idPedido: idPedido,
        descuento: parseFloat($('#inputDescontoFinalizado').val()),
        pago: $("#inputPagoFinalizado").prop('checked'),
        listaFormaPagamento: JSON.stringify(formasPagamento),
        tasaEntrega: $('#inputTasaFormaPagamentoDelivery').val() === undefined ? 0 : parseFloat($('#inputTasaFormaPagamentoDelivery').val()),
        troco: $('#inputTrocoFormaPagamento').val() === "" ? 0 : $('#inputTrocoFormaPagamento').val(),
        finalizar: false,
        deliveryDinheiroTotal: parseFloat($('#inputDeliveryDinheiroTotal2').val()),
        deliveryEmCartao: _ModalDeliveryFormaPagamento.deliveryEmCartao,
        deliveryPago: _ModalDeliveryFormaPagamento.deliveryPago,
        deliveryEmdinheiro: _ModalDeliveryFormaPagamento.deliveryEmdinheiro
    };

    $.ajax({
        type: "POST",
        url: "/Pedidos/ActualizarFormaPagamento",
        traditional: true,
        data: JSON.stringify(finalizarPedido),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (pedido) {

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

            MostarPedidosPendientes();

            if (finalizar) {
                //$('#CARD_PEDIDO_' + idPedido + '').remove();
                //_PedidosPendientes = $.grep(_PedidosPendientes, function (pedido) {
                //    return pedido.id != idPedido;
                //});
                //GetNumeroPedidosFinalizados();

                fnFinalizarPedido(idPedido);
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
        totalPedido = totalPedido - parseFloat(desconto);
    }

    var tasaEntraga = $('#inputTasaFormaPagamentoDelivery').val();
    if (!isNaN(parseFloat(tasaEntraga))) {
        totalPedido = totalPedido + parseFloat(tasaEntraga);
    }

    //HABILITAR DESCUENTO

    if (numberOfChecked > 0) {
        $('#inputDescontoFinalizado').prop('disabled', true);
        $('#inputTasaFormaPagamentoDelivery').prop('disabled', true);
    } else {
        $('#inputDescontoFinalizado').prop('disabled', false);
        $('#inputTasaFormaPagamentoDelivery').prop('disabled', false);
        firstInputChecked = null;
        valorInputFormaPagamento = 0;
    }

    //if (numberOfChecked > 3) {
    //    $(input).prop("checked", false);
    //    return;
    //}

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
        $('#modalBtnOkEnd').prop('disabled', false);
    } else {
        $('#modalBtnOk').prop('disabled', true);
        $('#modalBtnOkEnd').prop('disabled', true);
    }
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
                idBarrio: null,
                direccion: '',
                barrio: '',
                telefono: '',
                observacion: ''
            };
            metodoPagoDelivery("_deliveryEmCartaoCheck");
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
