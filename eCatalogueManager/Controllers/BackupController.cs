using Data.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly ECatalogueContextDB context;
        private readonly string studentsPath = @"C:\Users\Alexandru\OneDrive\Desktop\studentsBackup.txt";
        private readonly string teachersPath = @"C:\Users\Alexandru\OneDrive\Desktop\teachersBackup.txt";
        private readonly string addressesPath = @"C:\Users\Alexandru\OneDrive\Desktop\addressesBackup.txt";
        private readonly string subjectsPath = @"C:\Users\Alexandru\OneDrive\Desktop\subjectsBackup.txt";
        private readonly string marksPath = @"C:\Users\Alexandru\OneDrive\Desktop\marksBackup.txt";

        public BackupController(ECatalogueContextDB context)
        {
            this.context = context;
        }


        /// <summary>
        /// Creates a full batabase backup
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult BackupData()
        {
            CheckOrCreateFile(studentsPath);
            var studentsToJson = JsonSerializer.Serialize(context.Students);
            System.IO.File.WriteAllText(studentsPath, studentsToJson);

            CheckOrCreateFile(teachersPath);
            var teachersToJson = JsonSerializer.Serialize(context.Teachers);
            System.IO.File.WriteAllText(teachersPath, teachersToJson);

            CheckOrCreateFile(addressesPath);
            var addressesToJson = JsonSerializer.Serialize(context.Addresses);
            System.IO.File.WriteAllText(addressesPath, addressesToJson);

            CheckOrCreateFile(subjectsPath);
            var subjectsToJson = JsonSerializer.Serialize(context.Subjects);
            System.IO.File.WriteAllText(subjectsPath, subjectsToJson);

            CheckOrCreateFile(marksPath);
            var marksToJson = JsonSerializer.Serialize(context.Marks);
            System.IO.File.WriteAllText(marksPath, marksToJson);

            return Ok("Backup created");
        }

        /// <summary>
        /// Restores latest backup
        /// </summary>
        /// <returns></returns>
        [HttpGet("restore")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RestoreData()
        {
            bool backupExists = CheckIfFileExists(studentsPath) && CheckIfFileExists(teachersPath)
                                    && CheckIfFileExists(addressesPath) && CheckIfFileExists(marksPath)
                                    && CheckIfFileExists(subjectsPath);

            if (backupExists)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // just one SET IDENTITY_INSERT per Table (more will give Exceptions)
                using (var transaction = context.Database.BeginTransaction())
                {
                    var studentsAsString = System.IO.File.ReadAllText(studentsPath);
                    var studentsFromJson = JsonSerializer.Deserialize<List<Student>>(studentsAsString);

                    studentsFromJson.ForEach(s =>
                    {
                        context.Students.Add(s);
                    });

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Students ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Students OFF;");
                    transaction.Commit();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    var teachersAsString = System.IO.File.ReadAllText(teachersPath);
                    var teachersFromJson = JsonSerializer.Deserialize<List<Teacher>>(teachersAsString);

                    teachersFromJson.ForEach(t =>
                    {
                        context.Teachers.Add(t);
                    });

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Teachers ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Teachers OFF;");
                    transaction.Commit();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    var addressesAsString = System.IO.File.ReadAllText(addressesPath);
                    var addressesFromJson = JsonSerializer.Deserialize<List<Address>>(addressesAsString);

                    addressesFromJson.ForEach(a =>
                    {
                        context.Addresses.Add(a);
                    });

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Addresses ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Addresses OFF;");
                    transaction.Commit();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    var subjectsAsString = System.IO.File.ReadAllText(subjectsPath);
                    var subjectsFromJson = JsonSerializer.Deserialize<List<Subject>>(subjectsAsString);

                    subjectsFromJson.ForEach(s =>
                    {
                        context.Subjects.Add(s);
                    });

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Subjects ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Subjects OFF;");
                    transaction.Commit();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    var marksAsString = System.IO.File.ReadAllText(marksPath);
                    var marksFromJson = JsonSerializer.Deserialize<List<Mark>>(marksAsString);

                    marksFromJson.ForEach(m =>
                    {
                        context.Marks.Add(m);
                    });

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Marks ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Marks OFF;");
                    transaction.Commit();
                }

                return Ok("Restore successful");
            }
            return NotFound("Restore failed");
        }


        static void CheckOrCreateFile(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                using (System.IO.File.Create(path)) { }
            }
        }

        static bool CheckIfFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
