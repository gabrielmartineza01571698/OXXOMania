using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OXXOMania.Data;
using OXXOMania.Models;

namespace OXXOMania.Pages
{
    public class TiendaModel : PageModel
    {
        // contexto para acceder a la base de datos mediante Entity Framework
        private readonly OxxoContext _context;

        // constructor para inyectar la dependencia del contexto de base de datos
        public TiendaModel(OxxoContext context)
        {
            _context = context;
        }

        // categoria seleccionada por el usuario (Cosmeticos o Premios)
        [BindProperty]
        public string CategoriaSeleccionada { get; set; } = "Cosmeticos";

        // lista de productos a mostrar en la tienda
        public List<Objeto> Productos { get; set; } = new();

        // nombre del producto a canjear 
        [BindProperty]
        public string NombreProducto { get; set; }

        // monedas actuales del usuario
        public long Monedas { get; set; }

        // mensaje de exito 
        public string Mensaje { get; set; }

        // ID simulado del usuario logueado actualmente (temporal para pruebas)
        private int UsuarioId => 1;

        // se ejecuta cuando la página se carga inicialmente mediante GET
        public async Task OnGetAsync()
        {
            CargarProductos(); // carga la lista inicial de productos segun categoria seleccionada

            // obtiene el usuario actual desde la base de datos usando su ID
            var usuario = await _context.Usuarios.FindAsync(UsuarioId);
            Monedas = usuario?.Monedas ?? 0; // asigna la cantidad actual de monedas del usuario
        }

        // se ejecuta cuando la pagina recibe un POST general (cambio de categoría)
        public async Task<IActionResult> OnPostAsync()//devolver respuesta al navegador
        {
            CargarProductos(); // recarga productos después de cambiar la categoría

            // obtiene nuevamente el usuario actual
            var usuario = await _context.Usuarios.FindAsync(UsuarioId);
            Monedas = usuario?.Monedas ?? 0; // acctualiza las monedas del usuario

            return Page(); // devuelve la pagina actualizada
        }

        // maneja especificamente la accion POST de canjear un producto
        public async Task<IActionResult> OnPostCanjearAsync()
        {
            CargarProductos(); // carga productos nuevamente

            // obtiene el producto seleccionado desde la base de datos
            var producto = await _context.Objetos.FirstOrDefaultAsync(p => p.Nombre == NombreProducto);

            // obtiene al usuario actual para actualizar sus datos
            var usuario = await _context.Usuarios.FindAsync(UsuarioId);

            // valida existencia del producto, usuario y saldo suficiente
            if (producto != null && usuario != null && usuario.Monedas >= producto.Precio)
            {
                usuario.Monedas -= producto.Precio; // descuenta monedas del usuario

                // crea nuevo registro en "tenencia" (relación usuario-objeto adquirido)
                var tenencia = new Tenencia
                {
                    Id_Usuario = usuario.Id_Usuario,
                    Id_Objeto = producto.Id_Objeto,
                    Comprado = true,
                    Equipado = false
                };

                _context.Tenencias.Add(tenencia); // añade la nueva tenencia a la base de datos
                await _context.SaveChangesAsync(); // guarda cambios

                Monedas = usuario.Monedas; // actualiza saldo de monedas en la vista

                // mensaje de éxito al usuario
                Mensaje = $"Has canjeado '{producto.Nombre}' por {producto.Precio} monedas.";
            }
            else
            {
                // mensaje si no cumple condiciones para canjear
                Mensaje = "No tienes suficientes monedas.";
            }

            return RedirectToPage(); // recarga la página para reflejar cambios
        }

        // metodo privado para cargar productos desde la base segun categoria seleccionada
        private void CargarProductos()
        {
            if (CategoriaSeleccionada == "Premio")
            {
                Productos = _context.Objetos
                    .Where(o => o.Tipo == "Premio") //querys para acceder y manipular datos desde la base de datos
                    .ToList();
            }
            else // Por defecto carga productos cosméticos
            {
                Productos = _context.Objetos
                    .Where(o => o.Tipo == "Cosmetico")
                    .ToList();
            }
        }
    }
}
