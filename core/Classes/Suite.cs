using SimpsonsHotel.Core.Interfaces;

namespace SimpsonsHotel.Core.Classes
{
    public class Suite : Habitacion
    {
        public enum TipoCama { CAMA_KING, QUEEN_SEMIDOBLE };

        private TipoCama tipoCama;
        private MinibarSuite minibar;
        private int cantidadBatas;
        private const decimal PRECIO_BATA = 70000m;
        private const int BATAS_INCLUIDAS = 2;

        public Suite(int numero, TipoCama tipoCama) 
            : base(numero, 6, 500000m) // Costo fijo de $500.000 por noche, siempre en piso 6
        {
            this.tipoCama = tipoCama;
            this.minibar = new MinibarSuite();
            this.cantidadBatas = BATAS_INCLUIDAS; // Incluye 2 batas de baño
            
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
        public int CantidadBatas => cantidadBatas;

        public override void ComprarBatas(int cantidad)
        {
            cantidadBatas += cantidad;
            if (Consumo != null)
            {
                Consumo.ComprarBatas(cantidad);
            }
        }

        public void RefillMinibar()
        {
            minibar.RellenarMinibar();
        }

        public void PurchaseBathrobes(int quantity)
        {
            cantidadBatas += quantity;
            if (Consumo != null)
            {
                Consumo.ComprarBatas(quantity);
            }
        }

        public decimal CalculateTotalCost(int nights, bool isColombianGuest)
        {
            decimal roomCost = nights * 500000m;
            decimal minibarCost = minibar.CalcularCostoTotal();
            decimal bathrobeCost = (cantidadBatas - BATAS_INCLUIDAS) * PRECIO_BATA;
            decimal insurance = roomCost * 0.025m;
            decimal iva = isColombianGuest ? (roomCost + minibarCost + bathrobeCost) * 0.19m : 0;
            return roomCost + minibarCost + bathrobeCost + insurance + iva;
        }

        public override string TipoHabitacion => "Suite de Lujo";
    }
}
