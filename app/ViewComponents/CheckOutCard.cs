using Microsoft.AspNetCore.Mvc;

namespace SimpsonsHotel.app.ViewComponents
{
    public class CheckOutCard : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
