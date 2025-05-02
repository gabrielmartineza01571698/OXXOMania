using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OXXOMania.Models{

    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public long Monedas { get; set; }
        public int? Id_Asesor { get; set; }
        public string Oxxo { get; set; }
    }
}
