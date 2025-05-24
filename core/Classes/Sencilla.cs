namespace SimpsonsHotel.Core.Classes
{
    public class Sencilla : Habitacion
    {
        public enum TipoCama 
        { 
            CAMA_DOBLE, 
            DOS_SENCILLAS 
        };
        
        private TipoCama tipoCama;

        public Sencilla(int numero, int piso, TipoCama tipoCama) 
            : base(numero, piso, 200000m) // Costo fijo de $200.000 por noche
        {
            this.tipoCama = tipoCama;
        }

        public TipoCama TipoDeCama => tipoCama;
        public override string TipoHabitacion => "Sencilla";
    }
}
