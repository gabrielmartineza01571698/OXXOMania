using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;
using Org.BouncyCastle.Tls;
using System.ComponentModel.DataAnnotations;


namespace OXXOMania.Pages
{
    public class ReconocimientosModel : PageModel
    {
        [BindProperty]
        //[Required(ErrorMessage = "El nombre es obligatorio")]
        public int id_destinatario { get; set; }

        [BindProperty]
        //[Required(ErrorMessage = "El usuario es obligatorio")]
        public string? tipo { get; set; }
        [BindProperty]
        //[Required(ErrorMessage = "La sucursal es obligatoria")]
        public string? descripcion { get; set; }
        private readonly IHttpContextAccessor lectorSesion;
        private readonly DataBaseContext db;

        public Usuario UsuarioActual { get; set; }
        public List<Reconocimientos> ListaReconocimientos { get; set; } = new();
        public List<Usuario> ListaDestinatarios { get; set; } = new();

        public ReconocimientosModel(IHttpContextAccessor httpContextAccessor)
        {
            lectorSesion = httpContextAccessor;
            db = new DataBaseContext();
        }


        public void OnGet()
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");

            if (!string.IsNullOrEmpty(jsonUsr))
            {
                UsuarioActual = JsonSerializer.Deserialize<Usuario>(jsonUsr);

                ListaReconocimientos = db.GetReconocimientos(UsuarioActual.id_usuario);
                ListaDestinatarios = db.GetDestinatarios(UsuarioActual.id_usuario);
            }

        }

        public void OnPost()
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                UsuarioActual = JsonSerializer.Deserialize<Usuario>(jsonUsr);

                db.agregarReconocimiento(id_destinatario, UsuarioActual.id_usuario, tipo, descripcion);
                ListaReconocimientos = db.GetReconocimientos(UsuarioActual.id_usuario);
                ListaDestinatarios = db.GetDestinatarios(UsuarioActual.id_usuario);
            }
        }

    }
}
