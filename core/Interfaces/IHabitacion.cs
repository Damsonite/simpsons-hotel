using System.Collections.Generic;
using SimpsonsHotel.Core.Classes;

namespace SimpsonsHotel.Core.Interfaces
{
    public interface IHabitacion
    {
        int Numero { get; }
        bool Ocupada { get; }
        decimal PrecioPorNoche { get; }
        void Ocupar();
        void Liberar();
        decimal CalcularCostoTotal(int dias, bool esExtranjero);
        List<Consumo> ObtenerConsumosRestaurante();
        List<Consumo> ObtenerConsumosLavanderia();
        void AgregarConsumoRestaurante(Consumo consumo);
        void AgregarConsumoLavanderia(Consumo consumo);
    }
} 