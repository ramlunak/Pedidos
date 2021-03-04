using Microsoft.AspNetCore.Http;
using Pedidos.Extensions;
using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Productos
    {
        public int id { get; set; }
                
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }

        [DisplayName("Descrição")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatoria")]
        [DisplayName("Categoria")]
        public int idCategoria { get; set; }
        public int? idSubCategoria { get; set; }


        [Column(TypeName = "image")]
        public byte[] imagen { get; set; }

        public int idCuenta { get; set; }

        [Required(ErrorMessage = "O valor é obrigatorio")]
        [DisplayName("Valor")]
        public decimal valor { get; set; }

        [NotMapped]
        [DisplayName("Valor")]
        public string strValor { get; set; } = string.Empty;

        [DisplayName("U/M")]
        public UnidadMedida? unidadeMedida { get; set; }

        public int? horasPreparacion { get; set; }
        public int? minutosPreparacion { get; set; }

        public int? cantidadSabores { get; set; } = 0;
        public bool actualizarValorSaborMayor { get; set; } = false;
        public bool actualizarValorSaborMenor { get; set; } = false;
        public bool actualizarValorMediaSabores { get; set; } = false;

        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;

        public string tamanho1 { get; set; }
        public decimal? valorTamanho1 { get; set; }
        public string tamanho2 { get; set; }
        public decimal? valorTamanho2 { get; set; }
        public string tamanho3 { get; set; }
        public decimal? valorTamanho3 { get; set; }
        public string tamanho4 { get; set; }
        public decimal? valorTamanho4 { get; set; }
        public string tamanho5 { get; set; }
        public decimal? valorTamanho5 { get; set; }
        public string tamanho6 { get; set; }
        public decimal? valorTamanho6 { get; set; }
        public string tamanho7 { get; set; }
        public decimal? valorTamanho7 { get; set; }
        public string tamanho8 { get; set; }
        public decimal? valorTamanho8 { get; set; }
        public string tamanho9 { get; set; }
        public decimal? valorTamanho9 { get; set; }

        //PARA CARGAR DETALLES DEL PRODUCTO      
        public string JsonAdicionales { get; set; }
        public string JsonIngredientes { get; set; }
        public string JsonSabores { get; set; }

        //PROPIEDADES AUXILIARES

        [NotMapped]
        public int tiempo_pedido
        {
            get
            {
                if (fecha_pedido == null || fecha_pedido.Value < new DateTime(2020, 1, 1)) return 0;
                var tiempo = DateTime.Now.ToSouthAmericaStandard() - fecha_pedido;
                if (fecha_preparado != null && fecha_preparado.Value > new DateTime(2020, 1, 1))
                {
                    tiempo = fecha_preparado.Value - fecha_pedido.Value;
                }
                var segusdos = tiempo.Value.TotalSeconds;
                return Convert.ToInt32(segusdos);
            }
        }

        [NotMapped]
        public int tiempo_entrega
        {
            get
            {
                if (fecha_preparado == null || fecha_preparado.Value < new DateTime(2020, 1, 1)) return 0;
                var tiempo = DateTime.Now.ToSouthAmericaStandard() - fecha_preparado;
                if (fecha_entrega != null && fecha_entrega.Value > new DateTime(2020, 1, 1))
                {
                    tiempo = fecha_entrega.Value - fecha_preparado.Value;
                }
                var segusdos = tiempo.Value.TotalSeconds;
                return Convert.ToInt32(segusdos);
            }
        }

        [NotMapped]
        public int posicion { get; set; }
        [NotMapped]
        public DateTime? fecha_pedido { get; set; }
        [NotMapped]
        public DateTime? fecha_preparado { get; set; }
        [NotMapped]
        public DateTime? fecha_entrega { get; set; }

        [NotMapped]
        public bool isNew { get; set; } = true;
        [NotMapped]
        public string tempo
        {
            get
            {
                if (this.horasPreparacion is null)
                {
                    if (this.minutosPreparacion != null)
                        return $"00:{this.minutosPreparacion}:00";
                }
                else
                {
                    if (this.minutosPreparacion is null)
                        return $"{this.horasPreparacion}:00:00";
                    else
                        return $"{this.horasPreparacion}:{this.minutosPreparacion}:00";
                }

                return null;
            }
        }
        [NotMapped]
        [Required(ErrorMessage = "A Categoria é obrigatoria")]
        [DisplayName("Categoria")]
        public string Categoria { get; set; }

        [NotMapped]
        public IFormFile ImageName { get; set; }
        [NotMapped]
        public string ImageBase64 { get; set; }
        [NotMapped]
        public float desconto { get; set; }

        //PARA LOS PEDIDOS
        [NotMapped]
        public int cantidad { get; set; } = 1;
        [NotMapped]
        public string observacion { get; set; }
        [NotMapped]
        public List<P_Adicionais> Adicionales { get; set; } = new List<P_Adicionais>();
        [NotMapped]
        public List<P_Ingredientes> Ingredientes { get; set; } = new List<P_Ingredientes>();
        [NotMapped]
        public List<P_Sabor> Sabores { get; set; } = new List<P_Sabor>();
        [NotMapped]
        public decimal ValorMasAdicionales
        {
            get
            {
                try
                {

                    var valor_producto = this.valorTamanhoSeleccionado;

                    if (this.valorTamanhoSeleccionado == 0)
                    {
                        valor_producto = this.valor;
                    }

                    decimal valor_adicionales = 0;

                    foreach (var item in this.Adicionales)
                    {
                        if (item.cantidad > 0)
                        {
                            valor_adicionales += (item.Valor * item.cantidad);
                        }
                    }

                    //Sumar valor sabores

                    //CALCULAR VALOR SEGÙN OPCIONES DE LOS SABORES
                    decimal mayorValorSabores = 0;
                    decimal menorValorSabores = 0;
                    decimal sumaValoresSabores = 0;
                    var cantidadSaboresConValor = 0;

                    foreach (var item in this.Sabores)
                    {
                        if (item.valor.HasValue && item.selected)
                        {
                            cantidadSaboresConValor++;
                            sumaValoresSabores = sumaValoresSabores + item.valor.Value;

                            //Selecionar el valor mayor
                            if (item.valor > mayorValorSabores)
                            {
                                mayorValorSabores = item.valor.Value;
                            }

                            //Selecionar el valor menor
                            if (menorValorSabores == 0)
                            {
                                menorValorSabores = item.valor.Value;
                            }

                            if (item.valor < menorValorSabores)
                            {
                                menorValorSabores = item.valor.Value;
                            }
                        }
                    }

                    if (this.actualizarValorSaborMayor)
                    {
                        valor_adicionales = valor_adicionales + mayorValorSabores;
                    }
                    else if (this.actualizarValorSaborMenor)
                    {
                        valor_adicionales = valor_adicionales + menorValorSabores;
                    }
                    else if (this.actualizarValorMediaSabores)
                    {
                        valor_adicionales = valor_adicionales + (sumaValoresSabores / cantidadSaboresConValor);
                    }
                    else
                    {
                        valor_adicionales = valor_adicionales + sumaValoresSabores;
                    }

                    //foreach (var item in this.Sabores)
                    //{
                    //    if (item.selected && item.valor.HasValue)
                    //    {
                    //        valor_adicionales += (item.valor.Value);
                    //    }
                    //}

                    //-----------------------------------------

                    return this.cantidad * (valor_producto + valor_adicionales);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        [NotMapped]
        public string tamanhoSeleccionado { get; set; }
        [NotMapped]
        public decimal valorTamanhoSeleccionado { get; set; }

        //Solo para enviar esos dados al controlador y ponercelos al pedido
        [NotMapped]
        public string cliente { get; set; }
        [NotMapped]
        public int? idCliente { get; set; }
        [NotMapped]
        public string aplicativo { get; set; }
        [NotMapped]
        public int? idAplicativo { get; set; }
        [NotMapped]
        public int? idMesa { get; set; }
        [NotMapped]
        public string direccion { get; set; }
        [NotMapped]
        public int? idDireccion { get; set; }
        [NotMapped]
        public string barrio { get; set; }
        [NotMapped]
        public int? idBarrio { get; set; }
        [NotMapped]
        public string telefono { get; set; }

        [NotMapped]
        public decimal? deliveryDinheiroTotal { get; set; }
        [NotMapped]
        public decimal? deliveryTroco { get; set; }
        [NotMapped]
        public bool? deliveryEmCartao { get; set; }
        [NotMapped]
        public bool? deliveryPago { get; set; }
        [NotMapped]
        public bool? deliveryEmdinheiro { get; set; }

        [NotMapped]
        public string codigoConeccionCliente { get; set; }

    }

    public class ProductoDetalle
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int idCategoria { get; set; }
        public decimal valor { get; set; }
        public string JsonAdicionales { get; set; }
        public string JsonIngredientes { get; set; }
    }

    public class ProductosDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal valor { get; set; }
        public float descuento { get; set; }
        public string jsonAdicionales { get; set; }
        //Ingredientes que serén retirados
        public string jsonIngredientes { get; set; }
    }

    public class MarcarProducto
    {
        public int idPedido { get; set; }
        public int idProducto { get; set; }
        public int posicion { get; set; }
    }
}
