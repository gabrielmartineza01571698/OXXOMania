
namespace OXXOMania.Model {
    public class Reconocimientos
    {
        public int id_reconocimiento { get; set; }
        public int id_destinatario { get; set; }
        public int id_remitiente { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }

        public string nombre_remitiente { get; set; }

        public string apellido_remitiente { get; set; }

    }
}
