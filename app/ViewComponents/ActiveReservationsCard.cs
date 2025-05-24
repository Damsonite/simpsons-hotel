using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace SimpsonsHotel.app.ViewComponents
{
    public class ActiveReservationsCard : ViewComponent
    {
        public bool ShowLinks { get; set; } = true;

        public IViewComponentResult Invoke(List<Reserva> reservations, bool showLinks = true)
        {
            ViewData["ShowLinks"] = showLinks;
            return View(reservations);
        }
    }
}
