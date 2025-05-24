using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.Controllers;

public class HomeController : Controller
{
    private readonly Oficina _oficina;

    public HomeController(Oficina oficina)
    {
        _oficina = oficina;
    }

    public IActionResult Index()
    {
        var hotel = Hotel.Instance;
        ViewData["Mensaje"] = "Bienvenido al Hotel Simpsons";

        return View(hotel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View("Error", requestId);
    }
}
