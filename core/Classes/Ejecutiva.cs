using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Interfaces;

namespace SimpsonsHotel.Core.Classes
{
    public class Ejecutiva : Habitacion
    {
        public enum TipoCama { CAMA_QUEEN, DOS_SEMIDOBLES };

        private TipoCama tipoCama;
        private MinibarEjecutiva minibar;

        public Ejecutiva(int numero, TipoCama tipoCama) 
            : base(numero, 5, 350000m) // Costo fijo de $350.000 por noche, siempre en piso 5
        {
            this.tipoCama = tipoCama;
            this.minibar = new MinibarEjecutiva();
            
            // Registrarse para escuchar eventos del minibar
            this.minibar.ConsumoRegistrado += (sender, e) => {
                // Agregar al consumo de la habitación
                if (Consumo != null)
                {
                    decimal precio = ProductoMinibar.ObtenerPrecioUnitario(e.TipoProducto) * e.Cantidad;
                    Consumo.AgregarProductoMinibar(
                        new ProductoMinibar(e.TipoProducto, e.NombreProducto, precio, e.Cantidad)
                    );
                }
            };
        }

        public TipoCama TipoDeCama => tipoCama;
        public IMinibar Minibar => minibar;

        public void RellenarMinibar()
        {
            minibar.RellenarMinibar();
        }

        public override decimal CalcularCostoTotal(int noches, bool esHuespedColombiano)
        {
            decimal costoHabitacion = noches * 350000m;
            decimal costoMinibar = minibar.CalcularCostoTotal();
            decimal seguro = costoHabitacion * 0.025m;
            decimal iva = esHuespedColombiano ? (costoHabitacion + costoMinibar) * 0.19m : 0;
            return costoHabitacion + costoMinibar + seguro + iva;
        }

        public override string TipoHabitacion => "Ejecutiva";
    }
}
