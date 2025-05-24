using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpsonsHotel.Core.Interfaces;
using SimpsonsHotel.Core.Events;

namespace SimpsonsHotel.Core.Classes
{
    public abstract class Minibar : IMinibar
    {
        protected List<ProductoMinibar> productosDisponibles;
        protected List<Consumo> consumos;
        // Definimos un evento para notificar consumos
        public event EventHandler<EventoMinibar>? ConsumoRegistrado;

        protected Minibar()
        {
            productosDisponibles = new List<ProductoMinibar>();
            consumos = new List<Consumo>();
            ConfigurarMinibar();
        }

        // Método para ser implementado por las subclases
        protected abstract void ConfigurarMinibar();

        public List<ProductoMinibar> ProductosDisponibles => productosDisponibles;

        public abstract void RellenarMinibar();

        public List<Consumo> ObtenerConsumos() => consumos;

        public void LlenarMinibar() => RellenarMinibar();

        public void AgregarConsumo(Consumo consumo)
        {
            consumos.Add(consumo);
        }

        public decimal CalcularCostoTotal()
        {
            return consumos.Sum(c => c.CalcularTotal());
        }

        public void ConsumirProducto(string nombreProducto, int cantidad = 1)
        {
            var producto = productosDisponibles.FirstOrDefault(p => p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));
            
            if (producto == null)
            {
                throw new InvalidOperationException($"El producto {nombreProducto} no existe en el minibar");
            }
            
            // Verificar si hay suficiente cantidad
            if (TieneProducto(nombreProducto, cantidad))
            {
                // Disparar evento de consumo
                OnConsumoRegistrado(new EventoMinibar(producto.Tipo, nombreProducto, cantidad));
                
                // Eliminar el producto consumido
                productosDisponibles.Remove(producto);
                
                // Si queda cantidad, agregar un nuevo producto con la cantidad restante
                if (producto.Cantidad > cantidad)
                {
                    productosDisponibles.Add(
                        new ProductoMinibar(
                            producto.Tipo,
                            producto.Nombre,
                            ProductoMinibar.ObtenerPrecioUnitario(producto.Tipo),
                            producto.Cantidad - cantidad
                        )
                    );
                }
            }
            else
            {
                throw new InvalidOperationException($"No hay suficiente cantidad de {nombreProducto}");
            }
        }

        public bool TieneProducto(string nombreProducto, int cantidadRequerida = 1)
        {
            var producto = productosDisponibles.FirstOrDefault(p => p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));
            return producto != null && producto.Cantidad >= cantidadRequerida;
        }

        protected virtual void OnConsumoRegistrado(EventoMinibar e)
        {
            ConsumoRegistrado?.Invoke(this, e);
        }
    }
}
