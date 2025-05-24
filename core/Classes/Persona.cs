using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Classes
{
    public abstract class Persona
    {
        public enum TipoId { CC, TI, CE, PA };

        private string nombre;
        private TipoId tipoDocumento;
        private string numeroDocumento;
        private string telefono;

        protected Persona(string nombre, TipoId tipoDocumento, string numeroDocumento, string telefono)
        {
            this.nombre = nombre;
            this.tipoDocumento = tipoDocumento;
            this.numeroDocumento = numeroDocumento;
            this.telefono = telefono;
        }

        public string Nombre { get => nombre; set => nombre = value; }

        [Display(Name = "Tipo de Documento")]
        public TipoId TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }

        [Display(Name = "Número de Documento")]
        public string NumeroDocumento { get => numeroDocumento; set => numeroDocumento = value; }

        [Display(Name = "Teléfono")]
        public string Telefono { get => telefono; set => telefono = value; }

        public bool EsExtranjero { get => tipoDocumento == TipoId.CE || tipoDocumento == TipoId.PA; }
    }
}
