using SimpsonsHotel.Core.Classes;
using Microsoft.AspNetCore.Mvc;

namespace app.ViewComponents
{
    public class RoomCardViewModel
    {
        public Habitacion? Habitacion { get; set; }
        public string TipoHabitacion { get; set; } = "Desconocido";
        public string TipoCamaTexto { get; set; } = string.Empty; // Added this line
        public string CardBgClass { get; set; } = "bg-light";
        public string CardHeaderClass { get; set; } = "bg-secondary text-white";
        public string StatusText { get; set; } = string.Empty; 
        public string StatusBadgeClass { get; set; } = string.Empty;
    }

    public class RoomCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Habitacion habitacion)
        {
            var viewModel = new RoomCardViewModel
            {
                Habitacion = habitacion
            };

            viewModel.StatusText = habitacion.Ocupada ? "Ocupada" : "Disponible";
            viewModel.StatusBadgeClass = $"badge badge-{(habitacion.Ocupada ? "danger" : "confirm")}";

            if (habitacion is Sencilla sencilla) // Added variable 'sencilla'
            {
                viewModel.TipoHabitacion = "Sencilla";
                viewModel.CardBgClass = "room-card-sencilla";
                viewModel.CardHeaderClass = "room-card-header-sencilla";
                viewModel.TipoCamaTexto = sencilla.TipoDeCama switch // Populate TipoCamaTexto
                {
                    Sencilla.TipoCama.CAMA_DOBLE => "1 Cama Doble",
                    Sencilla.TipoCama.DOS_SENCILLAS => "2 Camas Sencillas",
                    _ => "N/A"
                };
            }
            else if (habitacion is Ejecutiva ejecutiva) // Added variable 'ejecutiva'
            {
                viewModel.TipoHabitacion = "Ejecutiva";
                viewModel.CardBgClass = "room-card-ejecutiva";
                viewModel.CardHeaderClass = "room-card-header-ejecutiva";
                viewModel.TipoCamaTexto = ejecutiva.TipoDeCama switch // Populate TipoCamaTexto
                {
                    Ejecutiva.TipoCama.CAMA_QUEEN => "1 Cama Queen",
                    Ejecutiva.TipoCama.DOS_SEMIDOBLES => "2 Camas Semidobles",
                    _ => "N/A"
                };
            }
            else if (habitacion is Suite suite) // Added variable 'suite'
            {
                viewModel.TipoHabitacion = "Suite";
                viewModel.CardBgClass = "room-card-suite";
                viewModel.CardHeaderClass = "room-card-header-suite";
                viewModel.TipoCamaTexto = suite.TipoDeCama switch // Populate TipoCamaTexto
                {
                    Suite.TipoCama.CAMA_KING => "1 Cama King",
                    Suite.TipoCama.QUEEN_SEMIDOBLE => "1 Cama Queen + 1 Semidoble",
                    // Add case for the new bed type here once defined
                    _ => "N/A"
                };
            }

            return View(viewModel);
        }
    }
}
