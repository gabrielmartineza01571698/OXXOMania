using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OXXOMania.Models
{
    [Table("objeto")] 
    public class Objeto
    {
        [Key]
        public int Id_Objeto { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public long Precio { get; set; }
        public string Descripcion { get; set; }
        public string? Imagen { get; set; }
    }
}

