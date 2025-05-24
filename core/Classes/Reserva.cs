using System.ComponentModel.DataAnnotations;
using SimpsonsHotel.Core.Events;

namespace SimpsonsHotel.Core.Classes
{
    public class Reserva
    {
        private Guid id;
        private Persona? persona;
        private Habitacion? habitacion;
        private DateTime fechaLlegada;
        private DateTime fechaSalida;
        
        public enum EstadoReserva
        {
            SinEmpezar,
            EnCurso,
            Cancelada,
            Finalizada
        }

        private EstadoReserva estado;        // Definimos un evento para notificar cancelaciones
        public event EventHandler<EventoReservaCancelada>? ReservaCancelada;
        
        // Parameterless constructor for model binding
        public Reserva()
        {
            id = Guid.NewGuid();
            estado = EstadoReserva.SinEmpezar;
        }
        
        public Reserva(Persona persona, Habitacion habitacion, DateTime fechaLlegada, DateTime fechaSalida)
        {
            id = Guid.NewGuid();
            this.persona = persona;
            this.habitacion = habitacion;
            this.fechaLlegada = fechaLlegada;
            this.fechaSalida = fechaSalida;
            estado = EstadoReserva.SinEmpezar;
        }

        public Guid Id => id;
        
        [Display(Name = "Fecha de Llegada")]
        public DateTime FechaLlegada { get => fechaLlegada; set => fechaLlegada = value; }
        
        [Display(Name = "Fecha de Salida")]
        public DateTime FechaSalida { get => fechaSalida; set => fechaSalida = value; }
        
        public Persona? Persona { get => persona; set => persona = value; }
        public Habitacion? Habitacion { get => habitacion; set => habitacion = value; }
        public EstadoReserva Estado
        {
            get => estado;
            set => estado = value;
        }
        
        public int DiasReserva => (fechaSalida - fechaLlegada).Days;
        
        public void Cancelar(string motivo)
        {
            if (estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException("La reserva ya ha sido cancelada");

            estado = EstadoReserva.Cancelada;
            habitacion?.Liberar();

            // Disparar evento de cancelación
            OnReservaCancelada(new EventoReservaCancelada(
                habitacion?.Numero ?? 0,
                persona?.Nombre ?? "Desconocido",
                motivo
            ));
        }
        
        public void CheckIn()
        {
            if (estado != EstadoReserva.SinEmpezar)
                throw new InvalidOperationException("La reserva no está en estado 'SinEmpezar'.");

            estado = EstadoReserva.EnCurso;
        }

        public void CheckOut()
        {
            if (estado != EstadoReserva.EnCurso)
                throw new InvalidOperationException("La reserva no está en estado 'EnCurso'.");

            estado = EstadoReserva.Finalizada;
        }

        public void VerificarEstadoAutomatico()
        {
            if (estado == EstadoReserva.SinEmpezar && DateTime.Now.Date > fechaLlegada.Date && DateTime.Now.Date <= fechaSalida.Date)
            {
                estado = EstadoReserva.Cancelada;
            }
        }

        protected virtual void OnReservaCancelada(EventoReservaCancelada e)
        {
            ReservaCancelada?.Invoke(this, e);
        }
    }
}
