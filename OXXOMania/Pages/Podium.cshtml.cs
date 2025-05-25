using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;

namespace OXXOMania.Pages
{
    public class Podium : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Podium(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? NombreUsuario { get; set; }

        public void OnGet()
        {
            var jsonUsr = _httpContextAccessor.HttpContext?.Session.GetString("Usr");

            if (!string.IsNullOrEmpty(jsonUsr))
            {
                Usuario usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                NombreUsuario = usr.nombre;
            }

            // Aquí podrías cargar los datos del podio desde base de datos o lógica de negocio
        }
    }
}
