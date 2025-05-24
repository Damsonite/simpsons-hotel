using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.ViewComponents;

public class LavanderiaServiceCard : ViewComponent
{
    public IViewComponentResult Invoke(Habitacion habitacion)
    {
        var model = new LavanderiaServiceViewModel
        {
            Habitacion = habitacion,
            PrecioLavado = Lavanderia.PrecioLavadoPorPrenda,
            PrecioPlanchado = Lavanderia.PrecioPlanchado,
            TiposServicio = Enum.GetValues<Lavanderia.TipoServicio>().ToList()
        };

        return View(model);
    }
}

public class LavanderiaServiceViewModel
{
    public Habitacion Habitacion { get; set; } = null!;
    public decimal PrecioLavado { get; set; }
    public decimal PrecioPlanchado { get; set; }
    public List<Lavanderia.TipoServicio> TiposServicio { get; set; } = new();
}
