using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpsonsHotel.Core.Classes;

namespace app.Controllers;

public class ServiciosController : Controller
{
    private readonly ILogger<ServiciosController> _logger;
    private readonly Oficina _oficina;

    public ServiciosController(ILogger<ServiciosController> logger, Oficina oficina)
    {
        this._logger = logger;
        this._oficina = oficina;
    }

    [HttpPost]
    public IActionResult UsarLavanderia(int numeroHabitacion, int cantidadLavado = 0, int cantidadPlanchado = 0)
    {
        var habitacion = _oficina.ObtenerHabitacionPorNumero(numeroHabitacion);
        if (habitacion == null)
        {
            return NotFound();
        }

        // Validar que al menos haya una prenda
        if (cantidadLavado <= 0 && cantidadPlanchado <= 0)
        {
            TempData["Error"] = "Debe seleccionar al menos una prenda para el servicio de lavandería";
            return RedirectToAction("Detalle", "Habitacion", new { id = numeroHabitacion });
        }

        var servicioLavanderia = new Lavanderia(cantidadLavado, cantidadPlanchado);
        habitacion.Consumo?.AgregarServicioLavanderia(servicioLavanderia);
        
        TempData["Success"] = "Servicio de lavandería solicitado exitosamente";
        return RedirectToAction("Detalle", "Habitacion", new { id = numeroHabitacion });
    }

    [HttpGet]
    public IActionResult UsarLavanderia(int habitacionId, int cantidadPrendas = 1, Lavanderia.TipoServicio tipoServicio = Lavanderia.TipoServicio.Lavado)
    {
        var habitacion = _oficina.ObtenerHabitacionPorNumero(habitacionId);
        if (habitacion == null)
        {
            return NotFound();
        }

        var servicioLavanderia = new Lavanderia(tipoServicio, cantidadPrendas);
        habitacion.Consumo?.AgregarServicioLavanderia(servicioLavanderia);
        return RedirectToAction("Detalle", "Habitacion", new { id = habitacionId });
    }

    [HttpPost]
    public IActionResult UsarRestaurante(int numeroHabitacion, List<string> productos, bool servicioHabitacion = false)
    {
        var habitacion = _oficina.ObtenerHabitacionPorNumero(numeroHabitacion);
        if (habitacion == null)
        {
            return NotFound();
        }

        if (productos == null || !productos.Any())
        {
            TempData["Error"] = "Debe seleccionar al menos un producto del menú";
            return RedirectToAction("Detalle", "Habitacion", new { id = numeroHabitacion });
        }

        // Validar que servicioHabitacion sea true solo si viene del checkbox
        bool aplicarServicioHabitacion = servicioHabitacion;

        // Agregar productos seleccionados
        foreach (var productoTipo in productos)
        {
            if (Enum.TryParse<ProductoRestaurante.TipoComida>(productoTipo, out var tipo))
            {
                var nombre = tipo switch
                {
                    ProductoRestaurante.TipoComida.Desayuno => "Desayuno",
                    ProductoRestaurante.TipoComida.Almuerzo => "Almuerzo", 
                    ProductoRestaurante.TipoComida.Cena => "Cena",
                    _ => productoTipo
                };
                
                var producto = new ProductoRestaurante(tipo, nombre, aplicarServicioHabitacion);
                habitacion.Consumo?.AgregarServicioRestaurante(producto, aplicarServicioHabitacion); // aplicar cargo de servicio a habitación si está seleccionado
            }
        }

        TempData["Success"] = "Pedido de restaurante realizado exitosamente";
        return RedirectToAction("Detalle", "Habitacion", new { id = numeroHabitacion });
    }

    [HttpPost]
    public IActionResult AgregarProductoRestaurante(int id, ProductoRestaurante.TipoComida tipoComida, string nombre)
    {
        var habitacion = _oficina.ObtenerHabitacionPorNumero(id);
        if (habitacion == null)
        {
            return NotFound();
        }

        var productoRestaurante = new ProductoRestaurante(tipoComida, nombre);
        habitacion.Consumo?.AgregarProductoRestaurante(productoRestaurante);
        return RedirectToAction("Detalle", "Habitacion", new { id });
    }
}
