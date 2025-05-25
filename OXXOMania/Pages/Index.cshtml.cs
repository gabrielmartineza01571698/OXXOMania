using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using OXXOMania.Model;
using System.Text.Json;


namespace OXXOMania.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string user { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string password { get; set; }
    
    private readonly DataBaseContext _context;

    public Usuario usr {get; set;}

    public IndexModel(DataBaseContext context) {
        _context = context;
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        usr = _context.GetUsuarioLogin(user);
        if (usr.usuario == "")
        {
            //si no existe regresa "" entonces manda mensaje aqui de "usuario no existe"
            ModelState.AddModelError("user", "El usuario no existe");
            return Page();
        }
        else
        {
            if (usr.contraseña == password)
            {
                //usuario y contraseña correctos entrar a homepage
                return RedirectToPage("/Podium", new {Usr = usr});
            }
            else
            {
                //password no coincide con usuario mandar mensaje "password incorrecto"
                ModelState.AddModelError("password", "La contraseña es incorrecta.");
                return Page();
            }
        }
        //si si existe entonces regirige a la pantalla de inicio
        //return Page();
    }
}
