using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Interfaces;

namespace SimpsonsHotel.Core.Classes
{
    public class MinibarSuite : Minibar
    {
        protected override void ConfigurarMinibar()
        {
            // Configuración inicial: 1 vino, 4 licores, 3 kits de aseo, 4 gaseosas
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Vino, "Vino tinto", 50000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Whisky", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Ron", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Vodka", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Licor, "Tequila", 25000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.KitAseo, "Kit de aseo personal", 9000m, 3));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Cola", 3000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Limonada", 3000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Agua con gas", 3000m, 1));
            productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, "Naranja", 3000m, 1));
        }

        public override void RellenarMinibar()
        {
            // Verificar qué productos faltan y agregarlos
            int vinos = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.Vino)
                                          .Sum(p => p.Cantidad);
            int licores = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.Licor)
                                            .Sum(p => p.Cantidad);
            int kitsAseo = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.KitAseo)
                                             .Sum(p => p.Cantidad);
            int gaseosas = productosDisponibles.Where(p => p.Tipo == ProductoMinibar.TipoProducto.Gaseosa)
                                             .Sum(p => p.Cantidad);

            // Rellenar los faltantes
            if (vinos < 1)
                productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Vino, "Vino tinto", 50000m, 1));
            
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
            
            if (kitsAseo < 3)
                productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.KitAseo, "Kit de aseo personal", 9000m, 3 - kitsAseo));
                
            if (gaseosas < 4)
            {
                string[] gaseosasDisponibles = { "Cola", "Limonada", "Agua con gas", "Naranja" };
                var gaseosasActuales = productosDisponibles
                    .Where(p => p.Tipo == ProductoMinibar.TipoProducto.Gaseosa)
                    .Select(p => p.Nombre)
                    .ToList();
                
                foreach (var gaseosa in gaseosasDisponibles)
                {
                    if (!gaseosasActuales.Contains(gaseosa))
                    {
                        productosDisponibles.Add(new ProductoMinibar(ProductoMinibar.TipoProducto.Gaseosa, gaseosa, 3000m, 1));
                    }
                }
            }
        }

        public decimal CalcularCostoTotal()
        {
            return productosDisponibles.Sum(p => p.Precio * p.Cantidad);
        }
    }
}
