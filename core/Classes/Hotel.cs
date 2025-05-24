using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SimpsonsHotel.Core.Aspects;

namespace SimpsonsHotel.Core.Classes
{
    public class Hotel
    {
        private List<Habitacion> habitaciones;
        private Recepcion recepcion;
        private Restaurante restaurante;
        private Oficina oficina;
        private List<Cliente> clientes;
        private List<Huesped> huespedes;
        private static Hotel? instance;
        private readonly ValidacionAspect validacionAspect;
        private readonly CargaArchivoAspect cargaArchivoAspect;
        private readonly ILoggerFactory loggerFactory;

        private Hotel()
        {
            // Inicializar el logger factory
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            // Inicializar aspectos con sus dependencias
            validacionAspect = new ValidacionAspect(loggerFactory.CreateLogger<ValidacionAspect>());
            cargaArchivoAspect = new CargaArchivoAspect();

            // Inicializar las habitaciones
            habitaciones = new List<Habitacion>();
            var random = new Random();
            
            // Crear habitaciones Sencillas (pisos 2-4, 10 por piso)
            for (int piso = 2; piso <= 4; piso++)
            {
                for (int num = 1; num <= 10; num++)
                {
                    var numero = piso * 100 + num;
                    var tipoCamaSencilla = random.NextDouble() < 0.5 ? Sencilla.TipoCama.CAMA_DOBLE : Sencilla.TipoCama.DOS_SENCILLAS;
                    habitaciones.Add(new Sencilla(numero, piso, tipoCamaSencilla));
                }
            }
            
            // Crear habitaciones Ejecutivas (piso 5, 10 habitaciones)
            for (int num = 1; num <= 10; num++)
            {
                var numero = 500 + num;
                var tipoCamaEjecutiva = random.NextDouble() < 0.5 ? Ejecutiva.TipoCama.CAMA_QUEEN : Ejecutiva.TipoCama.DOS_SEMIDOBLES;
                habitaciones.Add(new Ejecutiva(numero, tipoCamaEjecutiva));
            }
            
            // Crear habitaciones Suite (piso 6, 5 habitaciones)
            for (int num = 1; num <= 5; num++)
            {
                var numero = 600 + num;
                var tipoCamaSuite = random.NextDouble() < 0.5 ? Suite.TipoCama.CAMA_KING : Suite.TipoCama.QUEEN_SEMIDOBLE;
                habitaciones.Add(new Suite(numero, tipoCamaSuite));
            }
            
            // Inicializar los demás componentes del hotel
            restaurante = new Restaurante();
            oficina = new Oficina(habitaciones);
            recepcion = new Recepcion(oficina, cargaArchivoAspect, validacionAspect);
            clientes = new List<Cliente>();
            huespedes = new List<Huesped>();
        }
        
        public static Hotel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Hotel();
                }
                return instance;
            }
        }

        public List<Habitacion> Habitaciones => habitaciones;
        public Recepcion Recepcion => recepcion;
        public Restaurante Restaurante => restaurante;
        public Oficina Oficina => oficina;
        public List<Cliente> Clientes => clientes;
        public List<Huesped> Huespedes => huespedes;
        public List<Reserva> Reservas => oficina.ObtenerTodasLasReservas();
        
        public void RegistrarCliente(Cliente cliente)
        {
            clientes.Add(cliente);
        }
        
        public void RegistrarHuesped(Huesped huesped)
        {
            huespedes.Add(huesped);
        }
        
        public List<Habitacion> ObtenerHabitacionesDisponibles(int tipoHabitacion)
        {
            return oficina.ObtenerHabitacionesDisponibles(tipoHabitacion);
        }
        
        public Cliente? BuscarCliente(string numeroDocumento)
        {
            return clientes.FirstOrDefault(c => c.NumeroDocumento == numeroDocumento);
        }
        
        public Huesped? BuscarHuesped(string numeroDocumento)
        {
            return huespedes.FirstOrDefault(h => h.NumeroDocumento == numeroDocumento);
        }

        // Update the properties to filter only available rooms using the 'Ocupada' property
        public List<Habitacion> HabitacionesSencillas => habitaciones.Where(h => h is Sencilla && !h.Ocupada).ToList();
        public List<Habitacion> HabitacionesEjecutivas => habitaciones.Where(h => h is Ejecutiva && !h.Ocupada).ToList();
        public List<Habitacion> HabitacionesSuites => habitaciones.Where(h => h is Suite && !h.Ocupada).ToList();
    }
}
