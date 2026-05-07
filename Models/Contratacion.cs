using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Contratacion
    {
        [Key]
        public int ContratacionId { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public DateTime FechaContratacion { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string TpoContratacion { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string DescripcionPaquete { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public int PaqueteId { get; set; }
        public Paquete? Paquete { get; set; }
        public int EstatusId { get; set; }
        public Estatus? Estatus { get; set; }




    }
}
