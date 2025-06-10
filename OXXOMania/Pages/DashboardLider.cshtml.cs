using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;
using System; // Necesario para Exception

namespace OXXOMania.Pages
{
    public class DashboardLiderModel : PageModel
    {
        private readonly IHttpContextAccessor lectorSesion;
        private readonly DataBaseContext db;

        public DashboardLiderModel(IHttpContextAccessor httpContextAccessor)
        {
            lectorSesion = httpContextAccessor;
            db = new DataBaseContext();
        }

        public int? id_lider { get; set; }
        public List<Empleado> listaEmpleados { get; set; } = null;

        public void OnGet(int id_usuario)
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                var usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                if (usr != null)
                {
                    try
                    {
                        // Intenta obtener la lista de empleados desde la base de datos
                        listaEmpleados = db.AgarrarHorarios(id_usuario);
                    }
                    catch (Exception ex)
                    {
                        // Aquí puedes registrar el error si tienes un sistema de logging
                        Console.WriteLine("Error al acceder a la base de datos: " + ex.Message);

                        // Dejar listaEmpleados en null indica que falló la conexión a la BD
                        listaEmpleados = null;
                    }
                }
            }
        }
    }
}
