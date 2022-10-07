using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Data
{
    public class ECatalogueContextDB: DbContext
    {
        private readonly string connectionString;
        public ECatalogueContextDB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}
