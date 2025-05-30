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

    public int intentosFallidos { get; set; } = 0;
    public DateTime? bloqueadoHasta { get; set; }
    
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

        int intentosFallidos = HttpContext.Session.GetInt32($"Intentos_{user}") ?? 0;
        string bloqueadoStr = HttpContext.Session.GetString($"Bloqueado_{user}");
        DateTime? bloqueadoHasta = string.IsNullOrEmpty(bloqueadoStr) ? null : DateTime.Parse(bloqueadoStr);

        if (bloqueadoHasta != null)
        {
            if (DateTime.Now < bloqueadoHasta)
            {
                var minutosRest = (bloqueadoHasta.Value - DateTime.Now).Minutes;
                ModelState.AddModelError("password", $"Cuenta bloqueada por muchos intentos fallidos, intenta en {minutosRest} minutos.");
                return Page();
            }

            else {
                intentosFallidos = 0;
                HttpContext.Session.Remove($"Intentos_{user}");
                bloqueadoHasta = null;
                HttpContext.Session.Remove($"Bloqueado_{user}");
            }
        }

        usr = _context.GetUsuarioLogin(user);


        if (usr.usuario == "")
        {
            ModelState.AddModelError("user", "El usuario no existe");
            return Page();
        }
        else
        {
            if (usr.contraseña == password)
            {
                
                HttpContext.Session.Remove($"Intentos_{user}");
                HttpContext.Session.Remove($"Bloqueado_{user}");

                HttpContext.Session.SetString("Usr", JsonSerializer.Serialize(usr));
                return RedirectToPage("/Podium");
            }
            else
            {
                
                intentosFallidos++;
                HttpContext.Session.SetInt32($"Intentos_{user}", intentosFallidos);


                if (intentosFallidos >= 5)
                {
                    bloqueadoHasta = DateTime.Now.AddMinutes(3);
                    HttpContext.Session.SetString($"Bloqueado_{user}", bloqueadoHasta.Value.ToString());

                    ModelState.AddModelError("password", "La cuenta se ha bloqueado por 3 minutos debido a muchos intentos fallidos.");
                }
                else
                {
                    ModelState.AddModelError("password", "La contraseña es incorrecta.");
                }

                return Page();
            }
        }

    }
}
