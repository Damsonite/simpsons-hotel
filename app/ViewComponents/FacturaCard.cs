using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.ViewComponents;

public class FacturaCard : ViewComponent
{
    public IViewComponentResult Invoke(Habitacion habitacion, DateTime? fechaLlegada = null, DateTime? fechaSalida = null, string nombreHuesped = "", decimal cargoServicioHabitacion = 5000M)
    {
        var model = new FacturaViewModel
        {
            Habitacion = habitacion,
            Consumo = habitacion.Consumo ?? new Consumo(),
            FechaLlegada = fechaLlegada,
            FechaSalida = fechaSalida,
            NombreHuesped = nombreHuesped,
            CargoServicioHabitacion = cargoServicioHabitacion
        };

        return View(model);
    }
}

public class FacturaViewModel
{
    public Habitacion Habitacion { get; set; } = null!;
    public Consumo Consumo { get; set; } = null!;
    public DateTime? FechaLlegada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public string NombreHuesped { get; set; } = "";
    public decimal CargoServicioHabitacion { get; set; }
}
