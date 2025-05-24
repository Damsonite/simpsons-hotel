using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class Restaurante
    {
        private List<ProductoRestaurante> carta;

        public Restaurante()
        {
            // Inicializar la carta con productos predefinidos
            carta = new List<ProductoRestaurante>
            {
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Desayuno, "Desayuno Americano"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Desayuno, "Desayuno Continental"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Almuerzo, "Almuerzo Ejecutivo"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Almuerzo, "Almuerzo Gourmet"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Cena, "Cena Ligera"),
                new ProductoRestaurante(ProductoRestaurante.TipoComida.Cena, "Cena Gourmet")
            };
        }

        public List<ProductoRestaurante> Carta => carta;

        public ProductoRestaurante? ObtenerProducto(string nombre)
        {
            return carta.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        public ProductoRestaurante PedirComida(ProductoRestaurante.TipoComida tipoComida, string nombre, bool servicioHabitacion)
        {
            var producto = ObtenerProducto(nombre) ?? 
                new ProductoRestaurante(tipoComida, $"{tipoComida} del día"); // Si no existe, se crea uno genérico
                
            // Los productos del restaurante ya tienen el precio según su tipo
            return producto;
        }
    }
}
