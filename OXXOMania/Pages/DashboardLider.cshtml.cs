using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using OXXOMania.Model;
using System;
using System.Collections.Generic;

namespace OXXOMania.Pages
{
    public class DashboardLiderModel : PageModel
    {
        private readonly IHttpContextAccessor lectorSesion;
        private readonly DataBaseContext db;

        public DashboardLiderModel(IHttpContextAccessor httpContextAccessor, DataBaseContext context)
        {
            lectorSesion = httpContextAccessor;
            db = context;
        }

        public List<Empleado> listaEmpleados { get; set; }

        public void OnGet()
        {
            var jsonUsr = lectorSesion.HttpContext?.Session.GetString("Usr");
            if (!string.IsNullOrEmpty(jsonUsr))
            {
                var usr = JsonSerializer.Deserialize<Usuario>(jsonUsr);
                if (usr != null)
                {
                    try
                    {
                        listaEmpleados = db.AgarrarHorarios(usr.id_usuario) ?? new List<Empleado>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al acceder a la base de datos: " + ex.Message);
                        listaEmpleados = new List<Empleado>();
                    }
                }
                else
                {
                    listaEmpleados = new List<Empleado>();
                }
            }
            else
            {
                listaEmpleados = new List<Empleado>();
            }
        }
    }
}
