using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OXXOMania.Pages;

public class VistaAsesor : PageModel
{
    [BindProperty]
    public string? user {get; set;}
    [BindProperty]
    public int? password {get; set;}

    public void OnGet()
    {

    }
}
