using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Classes;

namespace SimpsonsHotel.Core.Events
{
    public class EventoMinibar : EventArgs
    {
        private ProductoMinibar.TipoProducto tipoProducto;
        private string nombreProducto;
        private int cantidad;
        private DateTime fechaHora;

        public EventoMinibar(ProductoMinibar.TipoProducto tipoProducto, string nombreProducto, int cantidad)
        {
            this.tipoProducto = tipoProducto;
            this.nombreProducto = nombreProducto;
            this.cantidad = cantidad;
            this.fechaHora = DateTime.Now;
        }

        public DateTime FechaHora => fechaHora;
        public ProductoMinibar.TipoProducto TipoProducto => tipoProducto;
        public string NombreProducto => nombreProducto;
        public int Cantidad => cantidad;

        public override string ToString()
        {
            return $"Se consumió {cantidad} de {nombreProducto} ({tipoProducto}) el {fechaHora:dd/MM/yyyy HH:mm:ss}.";
        }
    }
}
