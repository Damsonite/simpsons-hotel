using SimpsonsHotel.Core.Classes;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Hotel Simpsons - Demo de Biblioteca Core ===\n");

        // Crear habitaciones de ejemplo
        var habitaciones = new List<Habitacion>
        {
            new Sencilla(201),
            new Sencilla(202),
            new Ejecutiva(501),
            new Suite(601)
        };

        // Inicializar oficina
        var oficina = new Oficina(habitaciones);

        // Suscribirse a eventos
        oficina.ClienteCargado += (sender, message) => Console.WriteLine($"✓ {message}");
        oficina.ErrorCargaCliente += (sender, message) => Console.WriteLine($"✗ {message}");
        oficina.ArchivoProcesado += (sender, message) => Console.WriteLine($"\n{message}");
        oficina.ValidacionCompletada += (sender, message) => Console.WriteLine($"{message}\n");
        oficina.NotificacionReservaCancelada += (sender, e) => Console.WriteLine($"\n! Reserva cancelada: {e.Motivo}");

        try
        {
            // Demo 1: Cargar clientes desde archivo
            Console.WriteLine("Demo 1: Cargar clientes desde archivo");
            Console.WriteLine("=====================================");
            oficina.CargarArchivo("clientes.csv");

            // Demo 2: Crear una reserva
            Console.WriteLine("\nDemo 2: Crear una reserva");
            Console.WriteLine("=========================");
            var cliente = oficina.BuscarCliente("1234567890");
            if (cliente != null)
            {
                var reserva = oficina.CrearReserva(cliente, 1, DateTime.Now, DateTime.Now.AddDays(2));
                Console.WriteLine($"Reserva creada: ID {reserva.Id}");
                Console.WriteLine($"Cliente: {reserva.Persona.Nombre}");
                Console.WriteLine($"Habitación: {reserva.Habitacion.Numero}");
            }

            // Demo 3: Realizar check-in
            Console.WriteLine("\nDemo 3: Realizar check-in");
            Console.WriteLine("=========================");
            var reservaActiva = oficina.ObtenerTodasLasReservas().First();
            var huesped = oficina.CheckIn(reservaActiva.Id);
            Console.WriteLine($"Check-in realizado para: {huesped.Nombre}");
            Console.WriteLine($"Habitación asignada: {huesped.HabitacionAsignada?.Numero}");

            // Demo 4: Realizar check-out
            Console.WriteLine("\nDemo 4: Realizar check-out");
            Console.WriteLine("==========================");
            var factura = oficina.CheckOut(reservaActiva.Id);
            Console.WriteLine($"Check-out realizado");
            Console.WriteLine($"Factura generada: #{factura.Id}");
            Console.WriteLine($"Total: ${factura.CalcularTotal():N2}");

            // Demo 5: Cancelar una reserva
            Console.WriteLine("\nDemo 5: Cancelar una reserva");
            Console.WriteLine("===========================");
            var nuevaReserva = oficina.CrearReserva(cliente, 2, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));
            oficina.CancelarReserva(nuevaReserva.Id, "Cliente solicitó cancelación");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError en la demo: {ex.Message}");
        }

        Console.WriteLine("\nPresione cualquier tecla para salir...");
        Console.ReadKey();
    }
} 