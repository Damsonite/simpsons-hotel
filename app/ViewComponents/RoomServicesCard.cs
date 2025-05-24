using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.ViewComponents
{
    public class RoomServicesCardViewComponent : ViewComponent
    {
        public class RoomServicesCardViewModel
        {
            public Habitacion? HabitacionModel { get; set; } = null;
        }

        public IViewComponentResult Invoke(Habitacion habitacion)
        {
            var viewModel = new RoomServicesCardViewModel
            {
                HabitacionModel = habitacion
            };
            return View(viewModel);
        }
    }
}
