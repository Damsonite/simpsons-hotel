using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;
using System.Globalization;

namespace app.ViewComponents
{
    public class RoomTypeCardViewModel
    {
        public string? Title { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public int AvailableCount { get; set; }
        public int RoomType { get; set; } 
    }
    public class RoomTypeCard : ViewComponent
    {
        public IViewComponentResult Invoke(List<Habitacion> rooms)
        {
            var firstRoom = rooms.FirstOrDefault(); 
            string title = string.Empty;
            List<string> features = new List<string>();
            int roomTypeInt = 0;
            string cost = firstRoom != null ? firstRoom.CostoPorNoche.ToString("C0", new CultureInfo("es-CO")) : string.Empty;    
            
            if (firstRoom != null)
            {
                switch (firstRoom)
                {
                    case Sencilla sencilla:
                        title = "Habitación Sencilla";
                        roomTypeInt = 1;
                        features = [
                            "1 cama doble / 2 sencillas",
                            "Ubicadas en los pisos 2 al 4",
                            $"Precio por noche: {cost}"
                        ];
                        break;
                    case Ejecutiva ejecutiva:
                        title = "Habitación Ejecutiva";
                        roomTypeInt = 2;
                        features = [
                            "1 cama queen / 2 semidobles",
                            "Ubicadas en el piso 5",
                            "Minibar",
                            $"Precio por noche: {cost}"
                        ];
                        break;
                    case Suite suite:
                        title = "Suite de Lujo";
                        roomTypeInt = 3;
                        features = [
                            "1 cama king / 1 cama queen y 1 semidoble",
                            "Ubicadas en el piso 6",
                            "Minibar Prémium",
                            "2 batas de baño",
                            $"Precio por noche: {cost}"
                        ];
                        break;
                }
            }

            var viewModel = new RoomTypeCardViewModel
            {
                Title = title,
                Features = features,
                AvailableCount = rooms.Count,
                RoomType = roomTypeInt
            };
            return View(viewModel);
        }
    }
}
