using Microsoft.AspNetCore.Mvc.RazorPages;
using OXXOMania.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OXXOMania.Pages
{
    public class PersonalizacionModel : PageModel
    {
        private readonly DataBaseContext _context;

        public PersonalizacionModel(DataBaseContext context)
        {
            _context = context;
        }

        public class ObjetoConEstado : Objeto
        {
            public bool Equipado { get; set; }
        }

        public List<ObjetoConEstado> ObjetosEquipables { get; set; } = new();
        public int UsuarioId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool MostrarTodos { get; set; }

        [BindProperty]
        public int idObjeto { get; set; }

        [BindProperty]
        public string tipo { get; set; }

        public void OnGet()
        {
            var usuarioJson = HttpContext.Session.GetString("Usr");
            if (usuarioJson != null)
            {
                var usuario = JsonSerializer.Deserialize<Usuario>(usuarioJson);
                UsuarioId = usuario.id_usuario;
            }

            CargarObjetos();
        }

        

        public IActionResult OnPost()
        {
            var usuarioJson = HttpContext.Session.GetString("Usr");
            if (usuarioJson != null)
            {
                var usuario = JsonSerializer.Deserialize<Usuario>(usuarioJson);
                UsuarioId = usuario.id_usuario;
            }

            using var connection = _context.GetConnection();
            connection.Open();

            string desactivarQuery = @"
                UPDATE tenencia t
                JOIN objeto o ON t.id_objeto = o.id_objeto
                SET t.equipado = 0
                WHERE t.id_usuario = @usuarioId AND o.tipo = @tipo;
            ";

            using (var cmd1 = new MySqlCommand(desactivarQuery, connection))
            {
                cmd1.Parameters.AddWithValue("@usuarioId", UsuarioId);
                cmd1.Parameters.AddWithValue("@tipo", tipo);
                cmd1.ExecuteNonQuery();
            }

            string activarQuery = "UPDATE tenencia SET equipado = 1 WHERE id_usuario = @usuarioId AND id_objeto = @idObjeto;";
            using (var cmd2 = new MySqlCommand(activarQuery, connection))
            {
                cmd2.Parameters.AddWithValue("@usuarioId", UsuarioId);
                cmd2.Parameters.AddWithValue("@idObjeto", idObjeto);
                cmd2.ExecuteNonQuery();
            }

            return RedirectToPage(new { mostrarTodos = MostrarTodos });
        }

        private void CargarObjetos()
        {
            ObjetosEquipables.Clear();

            using var connection = _context.GetConnection();
            connection.Open();

            string query = @"
                SELECT o.id_objeto, o.nombre, o.tipo, o.precio, o.descripcion, o.imagen, t.equipado
                FROM tenencia t
                JOIN objeto o ON t.id_objeto = o.id_objeto
                WHERE t.id_usuario = @usuarioId AND t.comprado = 1
            ";

            if (!MostrarTodos)
            {
                query += " AND t.equipado = 1";
            }

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@usuarioId", UsuarioId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ObjetosEquipables.Add(new ObjetoConEstado
                {
                    IdObjeto = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Tipo = reader.GetString(2),
                    Precio = reader.GetInt64(3),
                    Descripcion = reader.GetString(4),
                    Imagen = reader.GetString(5),
                    Equipado = reader.GetBoolean(6)
                });
            }
        }
    }
}
