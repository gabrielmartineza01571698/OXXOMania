using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using OXXOMania.Model;
using System.Text.Json;

namespace OXXOMania.Pages;

public class Index_CrearCuentaModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? nombre { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string? apellido { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string? user { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "La sucursal es obligatoria")]
    public string? sucursal { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string? password { get; set; }

    public Usuario usr { get; set; }

    private readonly DataBaseContext _context;
    public Index_CrearCuentaModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        usr = _context.GetUsuarioLogin(user);

        if (usr.usuario != "")
        {
            ModelState.AddModelError("user", "El usuario ya tiene una cuenta existente.");
            return Page();
        }
        else
        {
            _context.AgregarUsuario(nombre, apellido, user, sucursal, password);
            return RedirectToPage("/Index");
        }


    }
}
