using Microsoft.AspNetCore.Mvc;

namespace SimpsonsHotel.app.ViewComponents
{
    public class CheckInCard : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
