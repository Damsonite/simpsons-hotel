using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{    public class Cliente : Persona
    {
        private string codigoFidelidad;
        private decimal porcentajeDescuento;
        private List<Reserva> historialReservas;
        
        public Cliente(string nombre, TipoId tipoDocumento, string numeroDocumento, 
                      string telefono, string codigoFidelidad)
            : base(nombre, tipoDocumento, numeroDocumento, telefono)
        {
            this.codigoFidelidad = codigoFidelidad;
            this.historialReservas = new List<Reserva>();
            // El descuento cambia semanalmente
            ActualizarDescuento();
        }        public string CodigoFidelidad { get => codigoFidelidad; set => codigoFidelidad = value; }
        public decimal PorcentajeDescuento { get => porcentajeDescuento; }
        public IReadOnlyList<Reserva> HistorialReservas => historialReservas.AsReadOnly();
        
        // El descuento cambia semanalmente basado en la fecha actual
        private void ActualizarDescuento()
        {
            // Usamos el número de semana del año para variar el descuento entre 5% y 15%
            int numeroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                DateTime.Now, 
                System.Globalization.CalendarWeekRule.FirstFourDayWeek, 
                DayOfWeek.Monday);
            
            // Valor base de 5% + hasta 10% adicional basado en la semana del año (módulo 10 para ciclar)
            porcentajeDescuento = 0.05m + (numeroSemana % 10) * 0.01m;
        }
          public decimal AplicarDescuento(decimal monto)
        {
            return monto * (1 - porcentajeDescuento);
        }
        
        public void AgregarReservaHistorial(Reserva reserva)
        {
            if (reserva == null)
                throw new ArgumentNullException(nameof(reserva));
                
            historialReservas.Add(reserva);
        }
        
        public int ContarReservasActivas()
        {
            return historialReservas.Count(r => r.Estado == Reserva.EstadoReserva.EnCurso);
        }
        
        public bool EsClienteFrecuente()
        {
            // Un cliente es frecuente si ha realizado al menos 3 reservas
            return historialReservas.Count >= 3;
        }
    }
}
