using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;

namespace OXXOMania.Pages
{
    public class VistaAsesorModel : PageModel
    {
        private readonly IHttpContextAccessor lectorSesion;
        private readonly DataBaseContext db;

        public VistaAsesorModel(IHttpContextAccessor httpContextAccessor)
        {
            lectorSesion = httpContextAccessor;
            db = new DataBaseContext();
        }

        public int? id_asesor { get; set; }
        public List<LideresAsesor> listaLideres { get; set; } = new List<LideresAsesor>();

        public void OnGet()
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                var usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                if (usr != null)
                {
                    id_asesor = usr.id_asesor = usr.id_usuario;
                    if (id_asesor.HasValue)
                    {
                        listaLideres = db.AgarrarLideresdeAsesor(id_asesor.Value);
                    }
                }
            }
        }

        public IActionResult OnGetDetalleLiderParcial(int id)
        {
            var empleados = db.AgarrarHorarios(id);
            return new PartialViewResult
            {
                ViewName = "_EmpleadosDeLiderPartial",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<List<Empleado>>(ViewData, empleados)
            };
        }
    }
}
