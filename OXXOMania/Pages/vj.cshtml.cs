using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;

namespace OXXOMania.Pages
{
     public class vjModel : PageModel
    {
        public List<Instruccion> Instrucciones { get; set; }
        public void OnGet()
        {
            DataBaseContext db = new DataBaseContext();
            //Instrucciones = db.ObtenerInstrucciones();
        }
    }

}
