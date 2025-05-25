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
        int logged;

        public void OnGet(Usuario User)
        {
        usr = User;

        UserName = usr.nombre;


        }

    }
}
