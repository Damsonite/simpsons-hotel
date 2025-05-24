using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class Lavanderia
    {
        public enum TipoServicio
        {
            Lavado,
            Planchado,
            LavadoYPlanchado
        }        private TipoServicio tipo;
        private int cantidadPrendas;
        private int cantidadLavado;
        private int cantidadPlanchado;
        private decimal costoLavadoPrenda = 12000m;
        private decimal costoPlanchado = 9000m;

        // Static properties to access pricing without creating instances
        public static decimal PrecioLavadoPorPrenda => 12000m;
        public static decimal PrecioPlanchado => 9000m;

        // Constructor original para compatibilidad
        public Lavanderia(TipoServicio tipo, int cantidadPrendas)
        {
            this.tipo = tipo;
            this.cantidadPrendas = cantidadPrendas;
            
            // Asignar cantidades según el tipo
            switch (tipo)
            {
                case TipoServicio.Lavado:
                    this.cantidadLavado = cantidadPrendas;
                    this.cantidadPlanchado = 0;
                    break;
                case TipoServicio.Planchado:
                    this.cantidadLavado = 0;
                    this.cantidadPlanchado = cantidadPrendas;
                    break;
                case TipoServicio.LavadoYPlanchado:
                    this.cantidadLavado = cantidadPrendas;
                    this.cantidadPlanchado = cantidadPrendas;
                    break;
            }
        }

        // Nuevo constructor para cantidades separadas
        public Lavanderia(int cantidadLavado, int cantidadPlanchado)
        {
            this.cantidadLavado = cantidadLavado;
            this.cantidadPlanchado = cantidadPlanchado;
            this.cantidadPrendas = cantidadLavado + cantidadPlanchado;
            
            // Determinar tipo según las cantidades
            if (cantidadLavado > 0 && cantidadPlanchado > 0)
                this.tipo = TipoServicio.LavadoYPlanchado;
            else if (cantidadLavado > 0)
                this.tipo = TipoServicio.Lavado;
            else
                this.tipo = TipoServicio.Planchado;
        }

        public decimal CalcularCosto()
        {
            return (cantidadLavado * costoLavadoPrenda) + (cantidadPlanchado * costoPlanchado);
        }

        public decimal CalcularTotal()
        {
            return CalcularCosto();
        }        public TipoServicio Tipo => tipo;
        public int CantidadPrendas => cantidadPrendas;
        public int CantidadLavado => cantidadLavado;
        public int CantidadPlanchado => cantidadPlanchado;
        public decimal CostoLavadoPrenda => costoLavadoPrenda;
        public decimal CostoPlanchado => costoPlanchado;
    }
}
