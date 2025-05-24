using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class Consumo
    {        private List<ProductoMinibar> productosMinibar;
        private List<ProductoRestaurante> productosRestaurante;
        private List<Lavanderia> serviciosLavanderia;
        private bool servicioHabitacion;
        private int cantidadBatas;
        private decimal costoUnitarioBata = 70000m;
        
        public static readonly decimal CargoServicioHabitacion = 5000m;

        public Consumo()
        {
            productosMinibar = new List<ProductoMinibar>();
            productosRestaurante = new List<ProductoRestaurante>();
            serviciosLavanderia = new List<Lavanderia>();
            servicioHabitacion = false;
            cantidadBatas = 0;
        }

        public decimal CalcularTotal()
        {
            decimal totalMinibar = productosMinibar.Sum(p => p.Precio);
            decimal totalRestaurante = productosRestaurante.Sum(p => p.Precio);
            decimal totalLavanderia = serviciosLavanderia.Sum(s => s.CalcularCosto());
            decimal recargo = servicioHabitacion ? 5000m : 0;
            decimal totalBatas = cantidadBatas * costoUnitarioBata;
            
            return totalMinibar + totalRestaurante + totalLavanderia + recargo + totalBatas;
        }

        public void AgregarProductoMinibar(ProductoMinibar producto)
        {
            productosMinibar.Add(producto);
        }

        public void AgregarServicioRestaurante(ProductoRestaurante producto, bool aLaHabitacion = false)
        {
            productosRestaurante.Add(producto);
            if (aLaHabitacion)
                servicioHabitacion = true;
        }

        public void AgregarServicioLavanderia(Lavanderia servicio)
        {
            serviciosLavanderia.Add(servicio);
        }

        public void AgregarProductoRestaurante(ProductoRestaurante producto)
        {
            productosRestaurante.Add(producto);
        }

        public void ComprarBatas(int cantidad)
        {
            cantidadBatas += cantidad;
        }

        public List<ProductoMinibar> ProductosMinibar => productosMinibar;
        public List<ProductoRestaurante> ProductosRestaurante => productosRestaurante;
        public List<Lavanderia> ServiciosLavanderia => serviciosLavanderia;
        public bool ServicioHabitacion => servicioHabitacion;
        public int CantidadBatas => cantidadBatas;
    }
}
