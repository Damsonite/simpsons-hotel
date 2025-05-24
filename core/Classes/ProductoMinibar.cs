using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class ProductoMinibar
    {
        public enum TipoProducto
        {
            Licor,
            Vino,
            Agua,
            Gaseosa,
            KitAseo
        }

        private TipoProducto tipo;
        private string nombre;
        private decimal precio;
        private int cantidad;

        public ProductoMinibar(TipoProducto tipo, string nombre, decimal precio, int cantidad)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.precio = precio;
            this.cantidad = cantidad;
        }

        public static decimal ObtenerPrecioUnitario(TipoProducto tipo)
        {
            return tipo switch
            {
                TipoProducto.Licor => 25000m,
                TipoProducto.Vino => 50000m,
                TipoProducto.Agua => 3500m,
                TipoProducto.Gaseosa => 3000m,
                TipoProducto.KitAseo => 9000m,
                _ => throw new ArgumentOutOfRangeException(nameof(tipo))
            };
        }

        public string Nombre => nombre;
        public decimal Precio => precio;
        public int Cantidad => cantidad;
        public TipoProducto Tipo => tipo;
    }
}
