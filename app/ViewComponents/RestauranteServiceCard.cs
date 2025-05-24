using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.ViewComponents;

public class RestauranteServiceCard : ViewComponent
{
    public IViewComponentResult Invoke(Habitacion habitacion)
    {
        var model = new RestauranteServiceViewModel
        {
            Habitacion = habitacion,
            MenuItems = new List<ProductoRestaurante>
            {
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Desayuno, "Desayuno"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Almuerzo, "Almuerzo"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Cena, "Cena")
            },
            CargoServicioHabitacion = ProductoRestaurante.CargoServicioHabitacion
        };

        return View(model);
    }
}

public class RestauranteServiceViewModel
{
    public Habitacion Habitacion { get; set; } = null!;
    public List<ProductoRestaurante> MenuItems { get; set; } = new();
    public decimal CargoServicioHabitacion { get; set; }
}
