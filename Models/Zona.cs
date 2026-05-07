using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Zona
    {
        [Key]
        public int ZonaId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string DescripcionZona { get; set; }
    }
}
