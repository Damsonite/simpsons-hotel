using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.Controllers;

public class ClienteController : Controller
{
    private readonly ILogger<ClienteController> _logger;
    private readonly Oficina _oficina;

    public ClienteController(ILogger<ClienteController> logger, Oficina oficina)
    {
        _logger = logger;
        _oficina = oficina;
    }

    public IActionResult Index()
    {
        try
        {
            var clientes = _oficina.ObtenerClientes();
            return View(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de clientes");
            TempData["Error"] = "Error al cargar la lista de clientes";
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Detalles(string id)
    {
        try
        {
            var cliente = _oficina.BuscarCliente(id);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener detalles del cliente {ClienteId}", id);
            TempData["Error"] = "Error al cargar los detalles del cliente";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Cliente("", Persona.TipoId.CC, "", "", ""));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Cliente cliente)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            // Verificar si el cliente ya existe
            var clienteExistente = _oficina.BuscarCliente(cliente.NumeroDocumento);
            if (clienteExistente != null)
            {
                ModelState.AddModelError("NumeroDocumento", "Ya existe un cliente con ese número de documento");
                return View(cliente);
            }

            _oficina.RegistrarCliente(cliente);
            TempData["Mensaje"] = "Cliente registrado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            TempData["Error"] = "Error al registrar el cliente";
            return View(cliente);
        }
    }

    [HttpGet]
    public IActionResult Editar(string id)
    {
        try
        {
            var cliente = _oficina.BuscarCliente(id);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente para edición {ClienteId}", id);
            TempData["Error"] = "Error al cargar los datos del cliente";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Cliente cliente)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var existingCliente = _oficina.BuscarCliente(cliente.NumeroDocumento);
            if (existingCliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            // Update the existing client details
            existingCliente.Nombre = cliente.Nombre;
            existingCliente.TipoDocumento = cliente.TipoDocumento;
            existingCliente.Telefono = cliente.Telefono;
            existingCliente.CodigoFidelidad = cliente.CodigoFidelidad;

            TempData["Mensaje"] = "Cliente actualizado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar cliente {ClienteId}", cliente.NumeroDocumento);
            TempData["Error"] = "Error al actualizar el cliente";
            return View(cliente);
        }
    }

    [HttpGet]
    public IActionResult Eliminar(string id)
    {
        try
        {
            var cliente = _oficina.BuscarCliente(id);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente para eliminación {ClienteId}", id);
            TempData["Error"] = "Error al cargar los datos del cliente";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmarEliminar(string id)
    {
        try
        {
            var cliente = _oficina.BuscarCliente(id);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            _oficina.EliminarCliente(id);
            TempData["Mensaje"] = "Cliente eliminado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {ClienteId}", id);
            TempData["Error"] = "Error al eliminar el cliente";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Historial(string id)
    {
        try
        {
            var cliente = _oficina.BuscarCliente(id);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener historial del cliente {ClienteId}", id);
            TempData["Error"] = "Error al cargar el historial del cliente";
            return RedirectToAction(nameof(Index));
        }
    }
}
