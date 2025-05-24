using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.Controllers;

public class ReservaController : Controller
{
    private readonly ILogger<ReservaController> _logger;
    private readonly Oficina _oficina;

    public ReservaController(ILogger<ReservaController> logger, Oficina oficina)
    {
        _logger = logger;
        _oficina = oficina;
    }

    public IActionResult Index()
    {
        var reservas = _oficina.ObtenerTodasLasReservas();
        return View(reservas);
    }

    public IActionResult Detalles(Guid id)
    {
        var reserva = _oficina.ObtenerReservaPorId(id);
        
        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    [HttpGet]
    public IActionResult Cancelar(Guid id)
    {
        var reserva = _oficina.ObtenerReservaPorId(id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    [HttpPost]
    public IActionResult Cancelar(Guid id, string motivo)
    {
        var reserva = _oficina.ObtenerReservaPorId(id);

        if (reserva == null)
        {
            return NotFound();
        }

        try
        {
            _oficina.CancelarReserva(id, motivo);
            TempData["Mensaje"] = "Reserva cancelada correctamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cancelar la reserva");
            ModelState.AddModelError("", "Error al cancelar la reserva: " + ex.Message);
            return View(reserva);
        }
    }

    [HttpGet]
    public IActionResult CheckIn(Guid id)
    {
        var reserva = _oficina.ObtenerReservaPorId(id);
        
        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    [HttpPost]
    public IActionResult ConfirmarCheckIn(Guid id)
    {
        try
        {
            _oficina.CheckIn(id);
            TempData["Mensaje"] = "Check-in realizado correctamente";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al realizar check-in");
            TempData["Error"] = "Error al realizar check-in: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult CheckOut(string numeroDocumento)
    {
        var huesped = _oficina.BuscarHuesped(numeroDocumento);
        
        if (huesped == null || huesped.HabitacionAsignada == null)
        {
            TempData["Error"] = "No se encontró un huésped con habitación asignada usando ese número de documento";
            return RedirectToAction("Index");
        }

        // Buscar la reserva asociada al huésped
        var reserva = _oficina.BuscarReservaPorPersona(numeroDocumento);
        ViewBag.Reserva = reserva;

        return View(huesped);
    }

    [HttpPost]
    public IActionResult ConfirmarCheckOut(Guid id)
    {
        var reserva = _oficina.ObtenerReservaPorId(id);

        if (reserva == null)
        {
            TempData["Error"] = "Reserva no encontrada";
            return RedirectToAction("Index");
        }

        try
        {
            var factura = _oficina.CheckOut(id);
            TempData["Mensaje"] = $"Check-out realizado correctamente. Se ha generado la factura #{factura.Id.ToString().Substring(0, 8)}";
            return RedirectToAction("CheckOut", new { numeroDocumento = reserva.Persona?.NumeroDocumento });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al realizar check-out");
            TempData["Error"] = "Error al realizar check-out: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult FacturaDetalles(Guid id)
    {
        var factura = _oficina.ObtenerFacturaPorId(id);

        if (factura == null)
        {
            TempData["Error"] = "Factura no encontrada";
            return RedirectToAction("Index");
        }

        return View(factura);
    }
}
