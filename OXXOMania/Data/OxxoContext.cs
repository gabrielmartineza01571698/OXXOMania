using Microsoft.EntityFrameworkCore;
using OXXOMania.Models;

namespace OXXOMania.Data
{
    public class OxxoContext : DbContext
    {
        public OxxoContext(DbContextOptions<OxxoContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Objeto> Objetos { get; set; }
        public DbSet<Tenencia> Tenencias { get; set; }
    }
}
