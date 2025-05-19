using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OXXOMania.Pages;

public class Index_CrearCuentaModel : PageModel
{
    [BindProperty]
    public string? correo {get; set;}
    [BindProperty]
    public int? user {get; set;}
    [BindProperty]
    public int? sucursal {get; set;}
    [BindProperty]
    public int? password {get; set;}

    public void OnGet()
    {

    }
}
