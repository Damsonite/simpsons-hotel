using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class Huesped : Persona
    {
        private DateTime fechaLlegada;
        private DateTime fechaSalida;
        private Habitacion? habitacionAsignada;

        public Huesped(string nombre, TipoId tipoDocumento, string numeroDocumento, string telefono)
            : base(nombre, tipoDocumento, numeroDocumento, telefono)
        {
            this.fechaLlegada = DateTime.Now;
            this.fechaSalida = DateTime.Now.AddDays(1); // Por defecto, una noche
        }

        public DateTime FechaLlegada { get => fechaLlegada; set => fechaLlegada = value; }
        public DateTime FechaSalida { get => fechaSalida; set => fechaSalida = value; }
        public Habitacion? HabitacionAsignada { get => habitacionAsignada; set => habitacionAsignada = value; }
        
        public int DiasEstadia => (FechaSalida - FechaLlegada).Days;
    }
}
