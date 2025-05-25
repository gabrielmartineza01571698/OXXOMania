using Microsoft.AspNetCore.Mvc.RazorPages;
using OXXOMania.Model;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Tls;
using Microsoft.AspNetCore.Mvc;

namespace OXXOMania.Pages
{
    public class _LayoutModel : PageModel
    {
        
        private readonly DataBaseContext _context;

        [BindProperty]
        public Usuario usr { get; set; }
        public String UserName;
        public string avatarHead;
        public Usuario Usr { get; set; }

        private Dictionary<int, string> selectImage = new Dictionary<int, string>()
                {
                    {1, "~/images/objetos/f_1.png"}, {2, "~/images/objetos/f_2.png"}, {3, "~/images/objetos/f_3.png"}
                };

    public void OnGet()
    {
        if (TempData["Usr"] != null)
        {
            Usr = JsonSerializer.Deserialize<Usuario>(TempData["Usr"].ToString());
        }
        UserName = Usr.nombre;

        int cabeza = _context.AgarrarCabeza(Usr.id_usuario);
        if (cabeza == 4)
        {
            //si no existe regresa "" entonces manda mensaje aqui de "usuario no existe"
            ModelState.AddModelError("cabeza", "No existe cabeza");
        }
        else
        {
            avatarHead = selectImage[cabeza];
        }
    }


    }
}
