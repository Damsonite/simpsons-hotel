using System;

namespace SimpsonsHotel.Core.Classes
{
    public static class ConfiguracionHotel
    {
        // Precios de habitaciones
        public static readonly decimal PrecioSencilla = 200000m;
        public static readonly decimal PrecioEjecutiva = 350000m;
        public static readonly decimal PrecioSuite = 500000m;

        // Precios de servicios
        public static readonly decimal PrecioDesayuno = 15000m;
        public static readonly decimal PrecioAlmuerzo = 25000m;
        public static readonly decimal PrecioCena = 20000m;
        public static readonly decimal PrecioServicioHabitacion = 5000m;
        public static readonly decimal PrecioLavanderia = 12000m;
        public static readonly decimal PrecioPlanchada = 9000m;

        // Precios de productos minibar
        public static readonly decimal PrecioLicor = 25000m;
        public static readonly decimal PrecioVino = 50000m;
        public static readonly decimal PrecioKitAseo = 9000m;
        public static readonly decimal PrecioAgua = 3500m;
        public static readonly decimal PrecioGaseosa = 3000m;
        public static readonly decimal PrecioBata = 70000m;

        // Porcentajes
        public static readonly decimal PorcentajeSeguroHotelero = 0.025m;
        public static readonly decimal PorcentajeIVA = 0.19m;

        // Capacidad de habitaciones
        public static readonly int CapacidadSencilla = 30;
        public static readonly int CapacidadEjecutiva = 10;
        public static readonly int CapacidadSuite = 5;

        // Pisos
        public static readonly int PisoSencillaInicio = 2;
        public static readonly int PisoSencillaFin = 4;
        public static readonly int PisoEjecutiva = 5;
        public static readonly int PisoSuite = 6;

        // Configuración de archivos
        public static readonly string RutaArchivoHabitaciones = "Data/habitaciones.txt";
        public static readonly string RutaArchivoReservas = "Data/reservas.txt";
        public static readonly string RutaArchivoClientes = "Data/clientes.txt";
        public static readonly string RutaArchivoConsumos = "Data/consumos.txt";
    }
} 