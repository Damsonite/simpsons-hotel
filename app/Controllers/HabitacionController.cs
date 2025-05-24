using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;
using app.ViewModels;

namespace app.Controllers;

public class HabitacionController : Controller
{
    private readonly ILogger<HabitacionController> _logger;
    private readonly Oficina _oficina;

    public HabitacionController(ILogger<HabitacionController> logger, Oficina oficina)
    {
        _logger = logger;
        _oficina = oficina;
    }

    public IActionResult Index(int? tipo, string? disponibilidad)
    {
        var todasLasHabitaciones = _oficina.ObtenerTodasLasHabitaciones();
        var filteredHabitaciones = todasLasHabitaciones;

        if (tipo.HasValue)
        {
            filteredHabitaciones = filteredHabitaciones.Where(h =>
            {
                return tipo.Value switch
                {
                    1 => h is Sencilla,
                    2 => h is Ejecutiva,
                    3 => h is Suite,
                    _ => true
                };
            }).ToList();
        }

        if (!string.IsNullOrEmpty(disponibilidad))
        {
            filteredHabitaciones = filteredHabitaciones.Where(h =>
            {
                return disponibilidad switch
                {
                    "disponible" => !h.Ocupada,
                    "ocupada" => h.Ocupada,
                    _ => true
                };
            }).ToList();
        }

        return View(filteredHabitaciones);
    }

    public IActionResult Detalle(int id)
    {
        var habitacion = _oficina.ObtenerHabitacionPorNumero(id);

        if (habitacion == null)
        {
            return NotFound();
        }

        // If room is occupied, get the current reservation
        Reserva? reservaActual = null;
        if (habitacion.Ocupada)
        {
            reservaActual = _oficina.BuscarReservaPorHabitacion(habitacion.Numero);
        }

        ViewBag.ReservaActual = reservaActual;
        return View(habitacion);
    }

    [HttpGet]
    public IActionResult Reservar(int id)
    {
        var habitacion = _oficina.ObtenerHabitacionPorNumero(id);

        if (habitacion == null)
        {
            return NotFound();
        }

        var reservaViewModel = new ReservaViewModel
        {
            NumeroHabitacion = habitacion.Numero,
            FechaLlegada = DateTime.Now,
            FechaSalida = DateTime.Now.AddDays(1)
        };

        return View(reservaViewModel);
    }

    [HttpPost]
    public IActionResult Reservar(ReservaViewModel reservaViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(reservaViewModel);
        }

        try
        {
            // Verificar si es cliente existente
            var cliente = _oficina.BuscarCliente(reservaViewModel.NumeroDocumento);
            Persona persona;

            if (cliente != null)
            {
                // Si es un cliente existente, usarlo
                persona = cliente;
            }
            else
            {
                // Si no es cliente, crear un Huesped
                persona = new Huesped(
                    reservaViewModel.Nombre,
                    reservaViewModel.TipoDocumento,
                    reservaViewModel.NumeroDocumento,
                    reservaViewModel.Telefono
                );
            }

            // Obtener habitación
            var habitacion = _oficina.ObtenerHabitacionPorNumero(reservaViewModel.NumeroHabitacion);

            if (habitacion == null || habitacion.Ocupada)
            {
                ModelState.AddModelError("", "La habitación no está disponible");
                return View(reservaViewModel);
            }

            // Crear reserva - map room type correctly
            int tipoHabitacion = habitacion switch
            {
                Sencilla => 1,
                Ejecutiva => 2, 
                Suite => 3,
                _ => throw new ArgumentException("Tipo de habitación desconocido")
            };
            
            var nuevaReserva = _oficina.CrearReserva(
                persona,
                tipoHabitacion,
                reservaViewModel.FechaLlegada,
                reservaViewModel.FechaSalida
            );

            return RedirectToAction("Detalles", "Reserva", new { id = nuevaReserva.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la reserva");
            ModelState.AddModelError("", "Error al crear la reserva: " + ex.Message);
            return View(reservaViewModel);
        }
    }

    public string ObtenerTipoHabitacion(Habitacion habitacion)
    {
        return habitacion switch
        {
            Sencilla => "Sencilla",
            Ejecutiva => "Ejecutiva",
            Suite => "Suite",
            _ => "Desconocido"
        };
    }
}
