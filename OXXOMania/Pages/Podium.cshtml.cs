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

        public PodiumModel(IHttpContextAccessor httpContextAccessor, DataBaseContext context)
        {
            lectorSesion = httpContextAccessor;
            db = context;
        }

        public string? NombreUsuario { get; set; }
        public List<PodiumUsuario> ListaUsuarios { get; set; } = new();

        public string AvatarPath1 { get; set; }
        public string AvatarPath2 { get; set; }
        public string AvatarPath3 { get; set; }

        private readonly Dictionary<int, string> headOptions = new()
        {
            {6, "~/images/objetos/f_1.png"},
            {7, "~/images/objetos/f_2.png"},
            {8, "~/images/objetos/f_3.png"},
            {9, "~/images/objetos/f_4.png"}
        };

        public void OnGet()
        {
            db.actualizarPuntajes();
            ListaUsuarios = db.AgarrarLugares();

            AvatarPath1 = GetAvatarPath(ListaUsuarios[0].cabeza);
            AvatarPath2 = GetAvatarPath(ListaUsuarios[1].cabeza);
            AvatarPath3 = GetAvatarPath(ListaUsuarios[2].cabeza);
            
        }

        private string GetAvatarPath(int cabeza)
        {
            return Url.Content(headOptions.ContainsKey(cabeza) ? headOptions[cabeza] : "~/images/objetos/f_1.png");
        }
    }
}
