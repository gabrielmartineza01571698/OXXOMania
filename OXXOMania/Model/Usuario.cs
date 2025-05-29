
namespace OXXOMania.Model {
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        public int? monedas { get; set; }
        public int? score { get; set; }
        public int id_asesor { get; set; }
        public string sucursal {get; set;}
        public string tipo_usuario {get; set;}
    }
}
