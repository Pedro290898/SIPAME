using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Paquete
    {
        [Key]
        public int PaqueteId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string DescripcionPaquete { get; set; }
        public int EstatusId { get; set; }

        public Estatus? Estatus { get; set; }
    }
}
