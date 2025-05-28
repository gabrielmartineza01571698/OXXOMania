using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using OXXOMania.Model;
using System.Collections.Generic;

namespace OXXOMania.Pages
{
    public class PodiumModel : PageModel
    {
        private readonly IHttpContextAccessor lectorSesion;
        private readonly DataBaseContext db;

        public PodiumModel(IHttpContextAccessor httpContextAccessor)
        {
            lectorSesion = httpContextAccessor;
            db = new DataBaseContext(); // o mejor: inyectar vía constructor si usas DI
        }

        public string? NombreUsuario { get; set; }
        public List<PodiumUsuario> ListaUsuarios { get; set; } = new List<PodiumUsuario>();

        public void OnGet()
        {
            ListaUsuarios = db.AgarrarLugares();
        }
    }
}
