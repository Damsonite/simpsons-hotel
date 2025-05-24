using System;
using SimpsonsHotel.Core.Classes;

namespace SimpsonsHotel.Core.Events
{
    public class EventosHotel
    {
        // Evento para notificar check-in
        public static event EventHandler<CheckInEventArgs> CheckInRealizado;
        public static void NotificarCheckIn(Huesped huesped, Habitacion habitacion)
        {
            CheckInRealizado?.Invoke(null, new CheckInEventArgs(huesped, habitacion));
        }

        // Evento para notificar check-out
        public static event EventHandler<CheckOutEventArgs> CheckOutRealizado;
        public static void NotificarCheckOut(Huesped huesped, Factura factura)
        {
            CheckOutRealizado?.Invoke(null, new CheckOutEventArgs(huesped, factura));
        }

        // Evento para notificar consumo de minibar
        public static event EventHandler<ConsumoMinibarEventArgs> ConsumoMinibarRealizado;
        public static void NotificarConsumoMinibar(Habitacion habitacion, Consumo consumo)
        {
            ConsumoMinibarRealizado?.Invoke(null, new ConsumoMinibarEventArgs(habitacion, consumo));
        }

        // Evento para notificar reserva cancelada
        public static event EventHandler<ReservaCanceladaEventArgs> ReservaCancelada;
        public static void NotificarReservaCancelada(Reserva reserva)
        {
            ReservaCancelada?.Invoke(null, new ReservaCanceladaEventArgs(reserva));
        }
    }

    public class CheckInEventArgs : EventArgs
    {
        public Huesped Huesped { get; }
        public Habitacion Habitacion { get; }

        public CheckInEventArgs(Huesped huesped, Habitacion habitacion)
        {
            Huesped = huesped;
            Habitacion = habitacion;
        }
    }

    public class CheckOutEventArgs : EventArgs
    {
        public Huesped Huesped { get; }
        public Factura Factura { get; }

        public CheckOutEventArgs(Huesped huesped, Factura factura)
        {
            Huesped = huesped;
            Factura = factura;
        }
    }

    public class ConsumoMinibarEventArgs : EventArgs
    {
        public Habitacion Habitacion { get; }
        public Consumo Consumo { get; }

        public ConsumoMinibarEventArgs(Habitacion habitacion, Consumo consumo)
        {
            Habitacion = habitacion;
            Consumo = consumo;
        }
    }

    public class ReservaCanceladaEventArgs : EventArgs
    {
        public Reserva Reserva { get; }

        public ReservaCanceladaEventArgs(Reserva reserva)
        {
            Reserva = reserva;
        }
    }
} 