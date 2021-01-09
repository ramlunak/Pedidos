using Microsoft.AspNetCore.Http;
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

        public string codigo { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }

        [DisplayName("descrição")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "A Categoria é obrigatoria")]
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

        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;

        public string tamanho1 { get; set; }
        public decimal? valorTamanho1 { get; set; }
        public string tamanho2 { get; set; }
        public decimal? valorTamanho2 { get; set; }
        public string tamanho3 { get; set; }
        public decimal? valorTamanho3 { get; set; }

        //PROPIEDADES AUXILIARES

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
        public decimal ValorMasAdicionales
        {
            get
            {
                try
                {
                    var valor_producto = this.valor;

                    if (this.valor == 0)
                    {
                        valor_producto = this.valorTamanhoSeleccionado;
                        if (valorTamanhoSeleccionado == 0)
                        {
                            valor_producto = this.valor;
                        }
                    }

                    decimal valor_adicionales = 0;

                    foreach (var item in this.Adicionales)
                    {
                        if (item.cantidad > 0)
                        {
                            valor_adicionales += (item.Valor * item.cantidad);
                        }
                    }

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
        public string telefono { get; set; }


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

}
