using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;
using System;

namespace OXXOMania.Pages
{
    public class VistaAsesorModel : PageModel
    {
        private readonly IHttpContextAccessor lectorSesion;
        private readonly DataBaseContext db;

        public VistaAsesorModel(IHttpContextAccessor httpContextAccessor)
        {
            lectorSesion = httpContextAccessor;
            db = new DataBaseContext();
        }

        public int? id_asesor { get; set; }
        public List<LideresAsesor> listaLideres { get; set; } = null;
        public List<Empleado> listaEmpleados { get; set; } = null;

        public void OnGet()
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                var usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                if (usr != null)
                {
                    id_asesor = usr.id_usuario;
                    if (id_asesor.HasValue)
                    {
                        try
                        {
                            listaLideres = db.AgarrarLideresdeAsesor(id_asesor.Value);
                            listaEmpleados = db.AgarrarTodosHorarios();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al acceder a la base de datos: " + ex.Message);

                            listaLideres = null;
                            listaEmpleados = null;
                        }
                    }
                }
            }
        }

        public IActionResult OnPostVerDetalle(int id_usuario)
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                var usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                if (usr != null)
                {
                    id_asesor = usr.id_usuario;
                    if (id_asesor.HasValue)
                    {
                        try
                        {
                            listaLideres = db.AgarrarLideresdeAsesor(id_asesor.Value);
                            listaEmpleados = db.AgarrarHorarios(id_usuario);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al acceder a la base de datos: " + ex.Message);

                            listaLideres = null;
                            listaEmpleados = null;
                        }
                    }
                }
            }
            return Page();
        }
    }
}
