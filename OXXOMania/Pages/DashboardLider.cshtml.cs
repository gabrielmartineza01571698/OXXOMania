using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;

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
        public List<Empleado> listaEmpleados { get; set; } = new List<Empleado>();

        public void OnGet(int id_usuario)
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                var usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                if (usr != null)
                {

                    listaEmpleados = db.AgarrarHorarios(id_usuario);

                }
            }
        }
    }

}