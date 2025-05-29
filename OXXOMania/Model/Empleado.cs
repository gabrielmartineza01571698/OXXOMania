
namespace OXXOMania.Model {
    public class Empleado
    {
        public string nombre_empleado { get; set; }
        public string apellido_empleado { get; set; }
        public TimeSpan horario_entrada { get; set; }
        public TimeSpan horario_salida { get; set; }
        public string dias_trabajo { get; set; }
    }
}
