using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OXXOMania.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string? user { get; set; }
    [BindProperty]
    public int? password { get; set; }

    public void OnGet()
    {

    }
    
    public IActionResult OnPost()
    {
        // Aquí podrías guardar los datos o hacer alguna validación

        // Redirige a la página Personalizacion
        return RedirectToPage("/Podium");
    }
}
