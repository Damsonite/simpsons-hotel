using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Aspects;
using SimpsonsHotel.Core.Events;

namespace SimpsonsHotel.Core.Classes
{
    public class Recepcion
    {
        private readonly Oficina oficina;
        private readonly List<Factura> facturas;
        private readonly CargaArchivoAspect cargaArchivoAspect;
        private readonly ValidacionAspect validacionAspect;
        
        public Recepcion(Oficina oficina, CargaArchivoAspect cargaArchivoAspect, ValidacionAspect validacionAspect)
        {
            this.oficina = oficina;
            this.facturas = new List<Factura>();
            this.cargaArchivoAspect = cargaArchivoAspect;
            this.validacionAspect = validacionAspect;
        }

        [RequiereAutenticacion(RolRequerido = "Recepcionista")]
        [Validacion(MensajeError = "La reserva no es válida")]
        public async Task<Huesped> CheckIn(Reserva reserva)
        {
            if (reserva == null || reserva.Estado == Reserva.EstadoReserva.Cancelada)
                throw new InvalidOperationException("La reserva no es válida o ha sido cancelada");
                
            if (reserva.Habitacion == null)
                throw new InvalidOperationException("La reserva no tiene una habitación asignada");
            
            if (reserva.Persona == null)
                throw new InvalidOperationException("La reserva no tiene una persona asociada");
            
            if (reserva.Habitacion.Ocupada)
                throw new InvalidOperationException("La habitación ya está ocupada");
                
            var persona = reserva.Persona;
            var huesped = new Huesped(
                persona.Nombre,
                persona.TipoDocumento,
                persona.NumeroDocumento,
                persona.Telefono
            );
            
            huesped.FechaLlegada = reserva.FechaLlegada;
            huesped.FechaSalida = reserva.FechaSalida;
            huesped.HabitacionAsignada = reserva.Habitacion;
            
            reserva.Habitacion.Ocupar();
            
            // Notificar el check-in
            EventosHotel.NotificarCheckIn(huesped, reserva.Habitacion);
            
            return huesped;
        }

        [RequiereAutenticacion(RolRequerido = "Recepcionista")]
        [Validacion(MensajeError = "El huésped no es válido")]
        public async Task<Factura> CheckOut(Huesped huesped)
        {
            if (huesped.HabitacionAsignada == null)
                throw new InvalidOperationException("El huésped no tiene habitación asignada");
                
            var habitacion = huesped.HabitacionAsignada;
            var dias = huesped.DiasEstadia;
            
            var factura = new Factura(
                huesped, 
                habitacion,
                dias
            );

            // Agregar consumos del minibar si la habitación lo tiene
            if (habitacion is Ejecutiva ejecutiva)
            {
                var consumosMinibar = ejecutiva.Minibar.ObtenerConsumos();
                foreach (var consumo in consumosMinibar)
                {
                    factura.AgregarConsumo(consumo);
                    EventosHotel.NotificarConsumoMinibar(habitacion, consumo);
                }
            }
            else if (habitacion is Suite suite)
            {
                var consumosMinibar = suite.Minibar.ObtenerConsumos();
                foreach (var consumo in consumosMinibar)
                {
                    factura.AgregarConsumo(consumo);
                    EventosHotel.NotificarConsumoMinibar(habitacion, consumo);
                }
            }

            // Aplicar descuento de fidelidad si el huésped es cliente
            var cliente = oficina.BuscarCliente(huesped.NumeroDocumento);
            if (cliente != null)
            {
                factura.AplicarDescuentoFidelidad(cliente.PorcentajeDescuento);
            }
            
            // Liberar la habitación
            habitacion.Liberar();
            huesped.HabitacionAsignada = null;
            
            facturas.Add(factura);
            
            // Notificar el check-out
            EventosHotel.NotificarCheckOut(huesped, factura);
            
            return factura;
        }
        
        [RequiereAutenticacion(RolRequerido = "Recepcionista")]
        [Validacion(MensajeError = "El cliente no es válido")]
        public async Task<Cliente> AsignarHabitacionSinReserva(Cliente cliente, int tipoHabitacion, int dias = 1)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));
                
            var habitacionesDisponibles = oficina.ObtenerHabitacionesDisponibles(tipoHabitacion);
            if (habitacionesDisponibles.Count == 0)
                throw new InvalidOperationException("No hay habitaciones disponibles del tipo solicitado");
                
            var habitacion = habitacionesDisponibles.First();
            
            var fechaLlegada = DateTime.Now;
            var fechaSalida = fechaLlegada.AddDays(dias);
            oficina.CrearReserva(cliente, tipoHabitacion, fechaLlegada, fechaSalida);
            
            return cliente;
        }

        [RequiereAutenticacion(RolRequerido = "Recepcionista")]
        public async Task<List<Factura>> ObtenerFacturas()
        {
            return facturas;
        }

        [RequiereAutenticacion(RolRequerido = "Recepcionista")]
        public async Task<Factura?> ObtenerFactura(Guid id)
        {
            return facturas.FirstOrDefault(f => f.Id == id);
        }

        [CargaArchivo(RutaArchivo = "Data/facturas.txt")]
        public async Task CargarFacturas()
        {
            var lineas = await cargaArchivoAspect.CargarArchivo(ConfiguracionHotel.RutaArchivoHabitaciones);
            // Implementar lógica de carga de facturas
        }

        [CargaArchivo(RutaArchivo = "Data/facturas.txt")]
        public async Task GuardarFacturas()
        {
            var lineas = facturas.Select(f => f.ToString()).ToArray();
            await cargaArchivoAspect.GuardarArchivo(ConfiguracionHotel.RutaArchivoHabitaciones, lineas);
        }
    }
}
