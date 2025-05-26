using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;

namespace OXXOMania.Pages
{
    public class Podium : PageModel
    {
        private readonly IHttpContextAccessor lectorSesion;

        public Podium(IHttpContextAccessor httpContextAccessor)
        {
            lectorSesion = httpContextAccessor;
        }

        public string? NombreUsuario { get; set; }

        public void OnGet()
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");

            if (!string.IsNullOrEmpty(jsonUsr))
            {
                Usuario usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                NombreUsuario = usr.nombre; 
            }

        }
    }
}
