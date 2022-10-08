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
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}
