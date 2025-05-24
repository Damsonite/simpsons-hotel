using SimpsonsHotel.Core.Events;

namespace SimpsonsHotel.Core.Classes
{    public class Oficina
    {
        private List<Reserva> reservas;
        private List<Habitacion> habitacionesDisponibles;
        private List<Cliente> clientes;
        private List<Habitacion> todasLasHabitaciones = new List<Habitacion>();
        private List<Factura> facturas = new List<Factura>();
        
        // Evento para notificaciones
        public event EventHandler<EventoReservaCancelada>? NotificacionReservaCancelada;

        public Oficina(List<Habitacion> habitacionesDisponibles)
        {
            this.reservas = new List<Reserva>();
            this.habitacionesDisponibles = habitacionesDisponibles;
            this.clientes = new List<Cliente>();
            
            // Inicializar todasLasHabitaciones con todas las habitaciones disponibles al inicio
            todasLasHabitaciones = habitacionesDisponibles.ToList();
            
            // Escuchar eventos de reservas canceladas
            NotificacionReservaCancelada += (sender, e) => {
                Console.WriteLine(e.ToString());
            };
        }        public Reserva CrearReserva(Persona persona, int tipoHabitacion, DateTime fechaLlegada, DateTime fechaSalida)
        {
            var habitacion = habitacionesDisponibles
                .Where(h => !h.Ocupada && ObtenerPisoHabitacion(h) == tipoHabitacion)
                .FirstOrDefault();
                
            if (habitacion == null)
                throw new InvalidOperationException("No hay habitaciones disponibles del tipo seleccionado");
                
            // Crear la reserva
            var reserva = new Reserva(persona, habitacion, fechaLlegada, fechaSalida);
            
            // Registrar evento de cancelación
            reserva.ReservaCancelada += (sender, e) => OnReservaCancelada(e);
            
            reservas.Add(reserva);

            return reserva;
        }
        
        // Helper para determinar el tipo de habitación por su piso
        private int ObtenerPisoHabitacion(Habitacion h)
        {
            return h switch
            {
                Sencilla _ => 1, // Pisos 2-4
                Ejecutiva _ => 2, // Piso 5
                Suite _ => 3, // Piso 6
                _ => throw new ArgumentException("Tipo de habitación desconocido")
            };
        }
        
        public Reserva? BuscarReserva(Guid id)
        {
            return reservas.FirstOrDefault(r => r.Id == id);
        }
        
        public Reserva? BuscarReservaPorHabitacion(int numeroHabitacion)
        {
            return reservas.FirstOrDefault(r => r.Habitacion?.Numero == numeroHabitacion && r.Estado == Reserva.EstadoReserva.EnCurso);
        }

        public Reserva? BuscarReservaPorPersona(string numeroDocumento)
        {
            return reservas.FirstOrDefault(r => r.Persona?.NumeroDocumento == numeroDocumento && r.Estado == Reserva.EstadoReserva.EnCurso);
        }

        public void CancelarReserva(Guid id, string motivo)
        {
            var reserva = BuscarReserva(id);
            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada");
                
            reserva.Cancelar(motivo);
        }
        
        public List<Habitacion> ObtenerHabitacionesDisponibles(int tipoHabitacion)
        {
            return habitacionesDisponibles
                .Where(h => !h.Ocupada && ObtenerPisoHabitacion(h) == tipoHabitacion)
                .ToList();
        }
        
        public List<Reserva> ReservasActivas => reservas.Where(r => r.Estado == Reserva.EstadoReserva.EnCurso).ToList();

        public List<Reserva> GetAllReservas() => reservas; // Added this public method

        public List<Reserva> ObtenerTodasLasReservas()
        {
            return reservas;
        }
        
        public List<Habitacion> ObtenerTodasLasHabitaciones()
        {
            return todasLasHabitaciones; 
        }

        public Habitacion? ObtenerHabitacionPorNumero(int numero)
        {
            return todasLasHabitaciones.FirstOrDefault(h => h.Numero == numero);
        }

        public Cliente? BuscarCliente(string numeroDocumento)
        {
            return clientes.FirstOrDefault(c => c.NumeroDocumento == numeroDocumento);
        }

        public void RegistrarCliente(Cliente cliente)
        {
            if (!clientes.Any(c => c.NumeroDocumento == cliente.NumeroDocumento))
            {
                clientes.Add(cliente);
            }
        }

        public void EliminarCliente(string numeroDocumento)
        {
            var cliente = clientes.FirstOrDefault(c => c.NumeroDocumento == numeroDocumento);
            if (cliente != null)
            {
                clientes.Remove(cliente);
            }
        }
        
        protected virtual void OnReservaCancelada(EventoReservaCancelada e)
        {
            NotificacionReservaCancelada?.Invoke(this, e);
        }

        public Reserva? ObtenerReservaPorId(Guid id)
        {
            return reservas.FirstOrDefault(r => r.Id == id);
        }

        public Huesped CheckIn(Guid reservaId)
        {
            var reserva = ObtenerReservaPorId(reservaId);
            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada");

            if (reserva.Habitacion == null)
                throw new InvalidOperationException("La reserva no tiene una habitación asignada");

            if (reserva.Habitacion.Ocupada)
                throw new InvalidOperationException("La habitación ya está ocupada");

            reserva.Estado = Reserva.EstadoReserva.EnCurso;
            reserva.Habitacion.Ocupar();
            if (habitacionesDisponibles.Contains(reserva.Habitacion))
            {
                habitacionesDisponibles.Remove(reserva.Habitacion);
            }

            if (reserva.Persona is Huesped huesped)
            {
                huesped.HabitacionAsignada = reserva.Habitacion;
                return huesped;
            }
            else if (reserva.Persona != null)
            {
                // Convert Persona to Huesped
                var nuevoHuesped = new Huesped(
                    reserva.Persona.Nombre,
                    reserva.Persona.TipoDocumento,
                    reserva.Persona.NumeroDocumento,
                    reserva.Persona.Telefono);
                nuevoHuesped.HabitacionAsignada = reserva.Habitacion;
                reserva.Persona = nuevoHuesped; // Update the reservation's Persona property
                return nuevoHuesped;
            }

            throw new InvalidOperationException("La reserva no está asociada a un huésped");
        }

        public Factura CheckOut(Guid reservaId)
        {
            var reserva = ObtenerReservaPorId(reservaId);
            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada");

            if (reserva.Estado != Reserva.EstadoReserva.EnCurso)
                throw new InvalidOperationException("La reserva no está en estado 'EnCurso'");

            if (reserva.Habitacion == null)
                throw new InvalidOperationException("La reserva no tiene una habitación asignada");

            var dias = (reserva.FechaSalida - reserva.FechaLlegada).Days;
            var factura = new Factura(reserva.Persona, reserva.Habitacion, dias);

            // Agregar consumos del minibar
            if (reserva.Habitacion.Consumo != null)
            {
                factura.AgregarConsumo(reserva.Habitacion.Consumo);
            }

            // Liberar la habitación
            reserva.Habitacion.Liberar();
            habitacionesDisponibles.Add(reserva.Habitacion);

            // Si es un huésped, limpiar la referencia a la habitación
            if (reserva.Persona is Huesped huesped)
            {
                huesped.HabitacionAsignada = null;
            }

            // Marcar la reserva como finalizada
            reserva.Estado = Reserva.EstadoReserva.Finalizada;

            // Guardar la factura
            facturas.Add(factura);
            return factura;
        }

        public Huesped? BuscarHuesped(string numeroDocumento)
        {
            // Buscar en todas las reservas, no solo en las activas
            return reservas
                .Where(r => r.Persona is Huesped h && h.NumeroDocumento == numeroDocumento && r.Estado == Reserva.EstadoReserva.EnCurso)
                .Select(r => r.Persona as Huesped)
                .FirstOrDefault();
        }

        public List<Cliente> ObtenerClientes()
        {
            return clientes;
        }

        public Factura? ObtenerFacturaPorId(Guid id)
        {
            return facturas.FirstOrDefault(f => f.Id == id);
        }

        public List<Factura> ObtenerTodasLasFacturas()
        {
            return facturas;
        }

        public event EventHandler<string>? ClienteCargado;
        public event EventHandler<string>? ErrorCargaCliente;
        public event EventHandler<string>? ArchivoProcesado;
        public event EventHandler<string>? ValidacionCompletada;

        public void CargarArchivo(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    throw new FileNotFoundException($"El archivo {rutaArchivo} no existe");
                }

                var lineas = File.ReadAllLines(rutaArchivo);
                var clientesCargados = 0;
                var errores = 0;

                foreach (var linea in lineas.Skip(1)) // Skip header
                {
                    try
                    {
                        var campos = linea.Split(',');
                        if (campos.Length != 5)
                        {
                            throw new FormatException("Formato de línea inválido");
                        }

                        var nombre = campos[0].Trim();
                        var tipoDocumento = Enum.Parse<Persona.TipoId>(campos[1].Trim());
                        var numeroDocumento = campos[2].Trim();
                        var telefono = campos[3].Trim();
                        var codigoFidelidad = campos[4].Trim();

                        // Validar datos
                        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(numeroDocumento) || string.IsNullOrEmpty(telefono))
                        {
                            throw new ArgumentException("Datos incompletos");
                        }

                        // Crear y registrar cliente
                        var cliente = new Cliente(nombre, tipoDocumento, numeroDocumento, telefono, codigoFidelidad);
                        RegistrarCliente(cliente);
                        clientesCargados++;
                        ClienteCargado?.Invoke(this, $"Cliente {nombre} cargado exitosamente");
                    }
                    catch (Exception ex)
                    {
                        errores++;
                        ErrorCargaCliente?.Invoke(this, $"Error al cargar cliente: {ex.Message}");
                    }
                }

                ArchivoProcesado?.Invoke(this, $"Proceso completado. Clientes cargados: {clientesCargados}, Errores: {errores}");
                ValidacionCompletada?.Invoke(this, $"Validación completada. Total de clientes en sistema: {clientes.Count}");
            }
            catch (Exception ex)
            {
                ErrorCargaCliente?.Invoke(this, $"Error al procesar archivo: {ex.Message}");
                throw;
            }
        }
    }
}
