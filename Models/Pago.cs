using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Pago
    {
        [Key]
        public int PagoId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public double MontoPago { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int ContratacionId { get; set; }
        public Contratacion? Contratacion { get; set; }
    }
}
