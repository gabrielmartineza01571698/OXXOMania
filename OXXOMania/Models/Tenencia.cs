using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OXXOMania.Models
{
    [Table("tenencia")]
public class Tenencia
{
    [Key]
    public int Id_Tenencia { get; set; }
    public bool Comprado { get; set; }
    public bool Equipado { get; set; }

    public int Id_Objeto { get; set; }
    public int Id_Usuario { get; set; }
}

}

