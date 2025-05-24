using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Classes;

namespace SimpsonsHotel.Core.Events
{
    public class EventoReservaCancelada : EventArgs
    {
        private int numeroHabitacion;
        private DateTime fechaCancelacion;
        private string motivoCancelacion;
        private string nombreCliente;

        public EventoReservaCancelada(int numeroHabitacion, string nombreCliente, string motivoCancelacion)
        {
            this.numeroHabitacion = numeroHabitacion;
            this.nombreCliente = nombreCliente;
            this.motivoCancelacion = motivoCancelacion;
            this.fechaCancelacion = DateTime.Now;
        }

        public int NumeroHabitacion => numeroHabitacion;
        public DateTime FechaCancelacion => fechaCancelacion;
        public string MotivoCancelacion => motivoCancelacion;
        public string NombreCliente => nombreCliente;

        public override string ToString()
        {
            return $"Reserva cancelada para habitación {numeroHabitacion} de {nombreCliente} el {fechaCancelacion:dd/MM/yyyy HH:mm:ss}. Motivo: {motivoCancelacion}";
        }
    }
}
