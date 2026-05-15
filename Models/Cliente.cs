using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        []
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos")]
        public string TelContacto { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string DireccionDomicilio { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        
        public int EstatusId { get; set; }

        public Estatus? Estatus { get; set; }
        public int ZonaId { get; set; }

        public Zona? Zona { get; set; }
    }
}
