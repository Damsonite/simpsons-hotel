using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class ProductoRestaurante
    {
        /// <summary>
        /// Cargo por servicio a habitación
        /// </summary>
        public const decimal CargoServicioHabitacion = 5000M;
        
        public enum TipoComida
        {
            Desayuno,
            Almuerzo,
            Cena
        }

        private TipoComida tipo;
        private string nombre;
        private decimal precio;
        private bool servicioHabitacion;        public ProductoRestaurante(TipoComida tipo, string nombre)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.precio = ObtenerPrecio(tipo);
            this.servicioHabitacion = false;
        }
        
        public ProductoRestaurante(TipoComida tipo, string nombre, bool servicioHabitacion)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.precio = ObtenerPrecio(tipo);
            this.servicioHabitacion = servicioHabitacion;
        }

        private static decimal ObtenerPrecio(TipoComida tipo)
        {
            return tipo switch
            {
                TipoComida.Desayuno => 15000m,
                TipoComida.Almuerzo => 25000m,
                TipoComida.Cena => 20000m,
                _ => throw new ArgumentOutOfRangeException(nameof(tipo))
            };
        }        public string Nombre => nombre;
        public decimal Precio => precio;
        public TipoComida Tipo => tipo;
        public bool ServicioHabitacion => servicioHabitacion;
        
        public void SetServicioHabitacion(bool valor)
        {
            this.servicioHabitacion = valor;
        }
    }
}
