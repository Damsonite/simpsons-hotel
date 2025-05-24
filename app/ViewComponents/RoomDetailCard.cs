using Microsoft.AspNetCore.Mvc;
using SimpsonsHotel.Core.Classes;

namespace app.ViewComponents
{
    public class RoomDetailCardViewComponent : ViewComponent
    {
        public class RoomDetailCardViewModel
        {
            public Habitacion? HabitacionModel { get; set; } = null;
            public string? ButtonClass { get; set; } = null;
            public List<KeyValuePair<string, string>>? CaracteristicasEspecificas { get; set; } = null;
            public string? TipoHabitacion { get; set; } = null;
        }

        public IViewComponentResult Invoke(Habitacion habitacion)
        {
            var caracteristicasEspecificas = new List<KeyValuePair<string, string>>();
            string descripcionTipoCama = string.Empty;
            string tipoHabitacion = string.Empty;

            if (habitacion is Sencilla sencillaHab)
            {
                tipoHabitacion = "Sencilla";
                descripcionTipoCama = sencillaHab.TipoDeCama switch
                {
                    Sencilla.TipoCama.CAMA_DOBLE => "1 cama doble",
                    Sencilla.TipoCama.DOS_SENCILLAS => "2 camas sencillas",
                    _ => string.Empty
                };
                caracteristicasEspecificas.Add(new("Tipo de cama:", descripcionTipoCama));
            }
            else if (habitacion is Ejecutiva ejecutivaHab)
            {
                tipoHabitacion = "Ejecutiva";
                descripcionTipoCama = ejecutivaHab.TipoDeCama switch
                {
                    Ejecutiva.TipoCama.CAMA_QUEEN => "1 cama queen",
                    Ejecutiva.TipoCama.DOS_SEMIDOBLES => "2 camas semidobles",
                    _ => string.Empty
                };
                caracteristicasEspecificas.Add(new("Tipo de cama:", descripcionTipoCama));
                caracteristicasEspecificas.Add(new("Minibar:", "Incluido"));
            }
            else if (habitacion is Suite suiteHab)
            {
                tipoHabitacion = "Suite";
                descripcionTipoCama = suiteHab.TipoDeCama switch
                {
                    Suite.TipoCama.CAMA_KING => "1 cama king",
                    Suite.TipoCama.QUEEN_SEMIDOBLE => "1 cama queen y 1 semidoble",
                    _ => string.Empty
                };
                caracteristicasEspecificas.Add(new("Tipo de cama:", descripcionTipoCama));
                caracteristicasEspecificas.Add(new("Minibar:", "Premium incluido"));
                caracteristicasEspecificas.Add(new("Batas de baño:", $"{suiteHab.CantidadBatas} (Incluidas)"));
            }

            var viewModel = new RoomDetailCardViewModel
            {
                HabitacionModel = habitacion,
                TipoHabitacion = tipoHabitacion,
                CaracteristicasEspecificas = caracteristicasEspecificas
            };

            return View(viewModel);
        }
    }
}
