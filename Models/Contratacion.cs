using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Contratacion
    {
        [Key]
        public int ContratacionId { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaContratacion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [AllowedValues("Antena", "Fibra", ErrorMessage = "El tipo de contratación debe ser Antena o Fibra.")]
        public string TpoContratacion { get; set; }

        //[Required(ErrorMessage = "El campo es requerido")]
        [AllowedValues("Renta", "Compra", ErrorMessage = "El modo de contratación debe ser Renta o Compra.")]
        public string Modocontratacion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        //public string DescripcionPaquete { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public int PaqueteId { get; set; }
        public Paquete? Paquete { get; set; }
        public int EstatusId { get; set; }
        public Estatus? Estatus { get; set; }




    }
}
