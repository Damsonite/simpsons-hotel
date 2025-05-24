using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public class Factura
    {
        private Guid id;
        private Persona persona;
        private Habitacion habitacion;
        private int dias;
        private DateTime fechaEmision;
        private decimal montoTotal;
        private List<Consumo> consumos;
        private decimal descuentoFidelidad;
        private decimal montoHabitacion;
        private decimal montoSeguro;
        private decimal montoIva;
        private decimal montoConsumos;
        private decimal montoServicios;

        public Factura(Persona persona, Habitacion habitacion, int dias)
        {
            this.id = Guid.NewGuid();
            this.persona = persona;
            this.habitacion = habitacion;
            this.dias = dias;
            this.fechaEmision = DateTime.Now;
            this.consumos = new List<Consumo>();
            this.descuentoFidelidad = 0;
            this.montoServicios = 0;
            
            // Calcular montos iniciales
            CalcularMontos();
        }

        public Guid Id => id;
        public Persona Persona => persona;
        public Habitacion Habitacion => habitacion;
        public int Dias => dias;
        public DateTime FechaEmision => fechaEmision;
        public decimal MontoTotal => montoTotal;
        public List<Consumo> Consumos => consumos;
        public decimal DescuentoFidelidad => descuentoFidelidad;
        public decimal MontoHabitacion => montoHabitacion;
        public decimal MontoSeguro => montoSeguro;
        public decimal MontoIva => montoIva;
        public decimal MontoConsumos => montoConsumos;
        public decimal MontoServicios => montoServicios;

        public void AgregarConsumo(Consumo consumo)
        {
            consumos.Add(consumo);
            CalcularMontos();
        }

        public void AgregarServicio(decimal monto)
        {
            montoServicios += monto;
            CalcularMontos();
        }

        public void AplicarDescuentoFidelidad(decimal porcentaje)
        {
            if (porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100");
            
            this.descuentoFidelidad = porcentaje;
            CalcularMontos();
        }
        
        private void CalcularMontos()
        {
            // Costo base de la habitación
            montoHabitacion = habitacion.CostoPorNoche * dias;

            // Seguro hotelero (2.5% del costo de la habitación)
            montoSeguro = montoHabitacion * 0.025m;

            // Consumos del minibar y otros
            montoConsumos = consumos.Sum(c => c.CalcularTotal());

            // Subtotal antes de descuentos e IVA
            decimal subtotal = montoHabitacion + montoSeguro + montoConsumos + montoServicios;

            // Aplicar descuento de fidelidad si existe
            if (descuentoFidelidad > 0)
            {
                subtotal = subtotal * (1 - (descuentoFidelidad / 100));
            }

            // IVA (19% solo para huéspedes colombianos)
            montoIva = persona.EsExtranjero ? 0 : subtotal * 0.19m;

            // Monto total final
            montoTotal = subtotal + montoIva;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Factura #{Id}");
            sb.AppendLine($"Fecha: {FechaEmision:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Cliente: {Persona.Nombre}");
            sb.AppendLine($"Documento: {Persona.TipoDocumento} {Persona.NumeroDocumento}");
            sb.AppendLine($"Habitación: {Habitacion.Numero}");
            sb.AppendLine($"Días de estadía: {Dias}");
            
            sb.AppendLine("\nDesglose:");
            sb.AppendLine($"- Habitación ({Dias} noches): ${MontoHabitacion:N2}");
            sb.AppendLine($"- Seguro hotelero (2.5%): ${MontoSeguro:N2}");
            
            if (montoServicios > 0)
            {
                sb.AppendLine($"- Servicios adicionales: ${MontoServicios:N2}");
            }

            if (consumos.Any())
            {
                sb.AppendLine("\nConsumos:");
                foreach (var consumo in consumos)
                {
                    sb.AppendLine($"- {consumo}");
                }
                sb.AppendLine($"Total consumos: ${MontoConsumos:N2}");
            }

            if (descuentoFidelidad > 0)
            {
                sb.AppendLine($"\nDescuento por fidelidad: {descuentoFidelidad}%");
            }

            if (!Persona.EsExtranjero)
            {
                sb.AppendLine($"\nIVA (19%): ${MontoIva:N2}");
            }

            sb.AppendLine($"\nMonto Total: ${MontoTotal:N2}");
            return sb.ToString();
        }
    }
}
