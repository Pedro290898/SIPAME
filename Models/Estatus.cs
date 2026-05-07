using System.ComponentModel.DataAnnotations;

namespace SIPAME.Models
{
    public class Estatus
    {
        [Key]
        public int EstatusId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string DescripcionEstatus { get; set; }


        //pasar la conelleccion a la tabla que se va ausar
        //ejemplo: public ICollection<Pedido>? Pedidos { get; set; }
    }
}
