using Microsoft.EntityFrameworkCore;
using Data.Data;
using Data.Models;

namespace Data
{
    public class SeedDB
    {
        private readonly ECatalogueContextDB context;
        public SeedDB(ECatalogueContextDB context)
        {
            this.context = context;
        }


        public void PopulateDB()
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            #region Teachers

            context.Teachers.Add(new Teacher
            {
                FullName = "Laurentiu Catarg",
                Address = new Address
                {
                    City = "Bucharest",
                    Street = "Traian",
                    StreetNumber = 23
                },
                Rank = Rank.Instructor,
                Subject = new Subject
                {
                    Name = "Mathematics",
                }
            });

            context.Teachers.Add(new Teacher
            {
                FullName = "Laura Mihail",
                Address = new Address
                {
                    City = "Buzau",
                    Street = "Fierarului",
                    StreetNumber = 87
                },
                Rank = Rank.Professor,
                Subject = new Subject
                {
                    Name = "Civil Engineering",
                }
            });

            context.Teachers.Add(new Teacher
            {
                FullName = "Dan Pop",
                Address = new Address
                {
                    City = "Bacau",
                    Street = "Mioritei",
                    StreetNumber = 2
                },
                Rank = Rank.AssistantProfessor,
                Subject = new Subject
                {
                    Name = "Computer Science",
                }
            });

            context.Teachers.Add(new Teacher
            {
                FullName = "Carmen Dinica",
                Address = new Address
                {
                    City = "Bucharest",
                    Street = "Traian",
                    StreetNumber = 23
                },
                Rank = Rank.AssociateProfessor,
                Subject = new Subject
                {
                    Name = "Psychology",
                }
            });

            #endregion

            #region Students

            context.Students.Add(new Student
            {
                FirstName = "Daniel",
                LastName = "Fastoc",
                Age = 28,
                Address = new Address
                {
                    City = "Iasi",
                    Street = "Catedralei",
                    StreetNumber = 25
                },
            });

            context.Students.Add(new Student
            {
                FirstName = "Catalin",
                LastName = "Varciu",
                Age = 51,
                Address = new Address
                {
                    City = "Brasov",
                    Street = "Garii",
                    StreetNumber = 13
                },
            });

            context.Students.Add(new Student
            {
                FirstName = "Mariana",
                LastName = "Marica",
                Age = 29,
                Address = new Address
                {
                    City = "Brasov",
                    Street = "Cizmarului",
                    StreetNumber = 100
                },
            });

            context.Students.Add(new Student
            {
                FirstName = "Mara",
                LastName = "Danciu",
                Age = 33,
                Address = new Address
                {
                    City = "Bacau",
                    Street = "Mioritei",
                    StreetNumber = 2
                }
            });

            context.Students.Add(new Student
            {
                FirstName = "Alexandra",
                LastName = "Stancu",
                Age = 34,
                Address = new Address
                {
                    City = "Bucharest",
                    Street = "Traian",
                    StreetNumber = 23
                }
            });

            context.Students.Add(new Student
            {
                FirstName = "Gica",
                LastName = "Dobre",
                Age = 24
            });

            context.Students.Add(new Student
            {
                FirstName = "Mihaela",
                LastName = "Morar",
                Age = 18
            });

            context.Students.Add(new Student
            {
                FirstName = "Daniel",
                LastName = "Ciobanu",
                Age = 27,
                Address = new Address
                {
                    City = "Bucharest",
                    Street = "Decebal",
                    StreetNumber = 174
                }
            });

            context.Students.Add(new Student
            {
                FirstName = "Culita",
                LastName = "Victoras",
                Age = 44,
                Address = new Address
                {
                    City = "Bacau",
                    Street = "Mioritei",
                    StreetNumber = 2
                }
            });

            context.Students.Add(new Student
            {
                FirstName = "Ana",
                LastName = "Manole",
                Age = 37
            });

            context.Students.Add(new Student
            {
                FirstName = "Carmen",
                LastName = "Ciuca",
                Age = 22,
                Address = new Address
                {
                    City = "Iasi",
                    Street = "Catedralei",
                    StreetNumber = 21
                }
            });

            context.Students.Add(new Student
            {
                FirstName = "Bogdan",
                LastName = "Moraciu",
                Age = 24
            });

            context.Students.Add(new Student
            {
                FirstName = "Crina",
                LastName = "Vlaicu",
                Age = 26,
                Address = new Address
                {
                    City = "Bucharest",
                    Street = "Traian",
                    StreetNumber = 23
                }
            });

            context.Students.Add(new Student
            {
                FirstName = "Flavius",
                LastName = "Stancuta",
                Age = 21,
                Address = new Address
                {
                    City = "Bucharest",
                    Street = "Decebal",
                    StreetNumber = 174
                }
            });

            #endregion

            context.SaveChanges();

            #region Marks

            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 1).Marks.Add(new Mark { Value = 7, SubjectId = 1, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 1).Marks.Add(new Mark { Value = 9, SubjectId = 1, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 1).Marks.Add(new Mark { Value = 6, SubjectId = 3, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 5).Marks.Add(new Mark { Value = 7, SubjectId = 1, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 5).Marks.Add(new Mark { Value = 10, SubjectId = 4, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 10).Marks.Add(new Mark { Value = 5, SubjectId = 1, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 3).Marks.Add(new Mark { Value = 8, SubjectId = 2, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 3).Marks.Add(new Mark { Value = 7, SubjectId = 2, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 3).Marks.Add(new Mark { Value = 4, SubjectId = 1, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 12).Marks.Add(new Mark { Value = 10, SubjectId = 1, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 14).Marks.Add(new Mark { Value = 6, SubjectId = 3, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 11).Marks.Add(new Mark { Value = 9, SubjectId = 4, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 11).Marks.Add(new Mark { Value = 8, SubjectId = 4, CreationDate = DateTime.Now});
            context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 7).Marks.Add(new Mark { Value = 10, SubjectId = 3, CreationDate = DateTime.Now});

            #endregion

            context.SaveChanges();
        }
    }
}
