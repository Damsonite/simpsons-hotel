using System.Collections.Generic;
using SimpsonsHotel.Core.Classes;

namespace SimpsonsHotel.Core.Interfaces
{
    public interface IMinibar
    {
        List<Consumo> ObtenerConsumos();
        void LlenarMinibar();
        void AgregarConsumo(Consumo consumo);
        decimal CalcularCostoTotal();
    }
}
