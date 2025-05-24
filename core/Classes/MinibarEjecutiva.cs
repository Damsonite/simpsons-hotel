using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Interfaces;

namespace SimpsonsHotel.Core.Classes
{
    public class MinibarEjecutiva : Minibar
    {
        protected override void ConfigurarMinibar()
        {
            // Configuración inicial: 4 licores, 2 aguas, 1 kit de aseo, 2 gaseosas
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Whisky", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Ron", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Vodka", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Tequila", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Agua, "Agua mineral", 3500m, 2));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.KitAseo, "Kit de aseo personal", 9000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Cola", 3000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Limonada", 3000m, 1));
        }

        public override void RellenarMinibar()
        {
            // Verificar qué productos faltan y agregarlos
            int licores = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.Licor)
                                            .Sum(p => p.Cantidad);
            int aguas = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.Agua)
                                           .Sum(p => p.Cantidad);
            int kitsAseo = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.KitAseo)
                                            .Sum(p => p.Cantidad);
            int gaseosas = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.Gaseosa)
                                             .Sum(p => p.Cantidad);

            // Rellenar los faltantes
            if (licores < 4)
            {
                string[] licoresDisponibles = { "Whisky", "Ron", "Vodka", "Tequila" };
                var licoresActuales = productosDisponibles
                    .Where(p => p.Tipo == ProductoMinibar.TipoProducto.Licor)
                    .Select(p => p.Nombre)
                    .ToList();
                
                foreach (var licor in licoresDisponibles)
                {
                    if (!licoresActuales.Contains(licor))
                    {
                        productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, licor, 25000m, 1));
                    }
                }
            }
            
            if (aguas < 2)
                productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Agua, "Agua mineral", 3500m, 2 - aguas));
                
            if (kitsAseo < 1)
                productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.KitAseo, "Kit de aseo personal", 9000m, 1));
                
            if (gaseosas < 2)
            {
                if (!productosDisponibles.Any(p => p.Nombre == "Cola" && p.Tipo == ProductoMinibar.TipoProducto.Gaseosa))
                    productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Cola", 3000m, 1));
                    
                if (!productosDisponibles.Any(p => p.Nombre == "Limonada" && p.Tipo == ProductoMinibar.TipoProducto.Gaseosa))
                    productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Limonada", 3000m, 1));
            }
        }

        public decimal CalcularCostoTotal()
        {
            return productosDisponibles.Sum(p => p.Precio * p.Cantidad);
        }
    }
}
