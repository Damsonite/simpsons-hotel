using System.ComponentModel.DataAnnotations;
using SimpsonsHotel.Core.Classes;

namespace app.ViewModels;

public class ReservaViewModel
{
    [Required]
    public int NumeroHabitacion { get; set; }
    
    [Required]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = "";
    
    [Required]
    [Display(Name = "Tipo de Documento")]
    public Persona.TipoId TipoDocumento { get; set; }
    
    [Required]
    [Display(Name = "Número de Documento")]
    public string NumeroDocumento { get; set; } = "";
    
    [Required]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = "";
    
    [Required]
    [Display(Name = "Fecha de Llegada")]
    public DateTime FechaLlegada { get; set; }
    
    [Required]
    [Display(Name = "Fecha de Salida")]
    public DateTime FechaSalida { get; set; }
}
