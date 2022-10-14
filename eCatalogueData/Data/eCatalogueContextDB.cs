using Microsoft.EntityFrameworkCore;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Data
{
    public class ECatalogueContextDB: DbContext
    {
        private readonly string connectionString;
        public ECatalogueContextDB(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DBConnection");
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}
