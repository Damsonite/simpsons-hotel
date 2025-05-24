namespace SimpsonsHotel.Core.Classes
{    public abstract class Habitacion
    {
        private int numero;
        private int piso;
        private decimal costoPorNoche;
        private bool ocupada = false;
        private Consumo? consumo;
        private decimal seguroHotelero = 0.025m; // 2.5% del precio por noche
        private decimal iva = 0.19m; // 19%
        
        protected Habitacion(int numero, int piso, decimal costoPorNoche)
        {
            this.numero = numero;
            this.piso = piso;
            this.costoPorNoche = costoPorNoche;
            this.ocupada = false;
            this.consumo = new Consumo();
        }

        public int Numero { get => numero; set => numero = value; }
        public int Piso { get => piso; set => piso = value; }
        public decimal CostoPorNoche { get => costoPorNoche; set => costoPorNoche = value; }
        public bool Ocupada { get => ocupada; set => ocupada = value; }
        public Consumo? Consumo { get => consumo; set => consumo = value; }        public virtual void ComprarBatas(int cantidad)
        {
            // Base implementation - to be overridden by rooms that have robes
            throw new NotImplementedException("Esta habitación no dispone de batas para comprar.");
        }

        public virtual decimal CalcularCostoTotal(int noches, bool esExtranjero)
        {
            decimal subTotal = costoPorNoche * noches;
            decimal montoSeguro = subTotal * seguroHotelero;
            decimal montoIva = esExtranjero ? 0 : (subTotal * iva);
            
            return subTotal + montoSeguro + montoIva + (consumo?.CalcularTotal() ?? 0);
        }

        public void Ocupar()
        {
            if (ocupada)
                throw new InvalidOperationException("La habitación ya está ocupada");
                
            ocupada = true;
        }

        public void Liberar()
        {
            ocupada = false;
            consumo = new Consumo();
        }

        public virtual string TipoHabitacion => "Habitación genérica";
    }
}
