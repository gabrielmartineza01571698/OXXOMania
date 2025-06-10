using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OXXOMania.Model;
using System.Collections.Generic;
using System.Text.Json;

namespace OXXOMania.Pages
{
    public class TiendaModel : PageModel
    {
        private readonly DataBaseContext _context;

        public TiendaModel()
        {
            _context = new DataBaseContext();
        }

        public List<Objeto> ObjetosTienda { get; set; } = new();
        public List<PremioSpin> PremiosTienda { get; set; } = new();
        public int MonedasUsuario { get; set; }
        public int ObjetoCompradoId { get; set; } = -1;
        public List<int> IdsObjetosComprados { get; set; } = new();
        public List<int> IdsPremiosCanjeados { get; set; } = new();

        public IActionResult OnGet()
        {
            // ✅ Leer objeto de usuario completo desde sesión
            var usrJson = HttpContext.Session.GetString("Usr");

            if (string.IsNullOrEmpty(usrJson))
            {
                return RedirectToPage("/Index");
            }

            var usr = JsonSerializer.Deserialize<Usuario>(usrJson);

            MonedasUsuario = usr.monedas ?? 0;

            ObjetosTienda = _context.ObtenerObjetosTienda();
            PremiosTienda = _context.ObtenerPremiosSpin();

            IdsObjetosComprados = _context.ObtenerIdsObjetosComprados(usr.id_usuario);
            IdsPremiosCanjeados = _context.ObtenerIdsPremiosCanjeados(usr.id_usuario);

            return Page();
        }

        public IActionResult OnPostCanjear(int idObjeto, int precio)
        {
            var usrJson = HttpContext.Session.GetString("Usr");
            if (string.IsNullOrEmpty(usrJson)) return RedirectToPage("/Index");

            var usr = JsonSerializer.Deserialize<Usuario>(usrJson);
            var idsComprados = _context.ObtenerIdsObjetosComprados(usr.id_usuario);

            if (!idsComprados.Contains(idObjeto) && usr.monedas >= precio)
            {
                _context.RegistrarCompraYRestarMonedas(usr.id_usuario, idObjeto, precio);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostCanjearPremio(int idPremio, int precio)
        {
            var usrJson = HttpContext.Session.GetString("Usr");
            if (string.IsNullOrEmpty(usrJson)) return RedirectToPage("/Index");

            var usr = JsonSerializer.Deserialize<Usuario>(usrJson);
            var idsCanjeados = _context.ObtenerIdsPremiosCanjeados(usr.id_usuario);

            if (!idsCanjeados.Contains(idPremio) && usr.monedas >= precio)
            {
                _context.RegistrarCanjePremio(usr.id_usuario, idPremio, precio); 
            }

            return RedirectToPage();
        }
    }
}
