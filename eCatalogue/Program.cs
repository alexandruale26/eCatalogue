using Data.Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

string connexionString = "Data Source=DESKTOP-42S4FFT\\SQLEXPRESS;Initial Catalog=eCatalogueDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

using var context = new ECatalogueContextDB(connexionString);


ResedDB(connexionString);
//context.Subjects.Remove(context.Subjects.First(s => s.SubjectId == 1));
//context.SaveChanges();







static void ResedDB(string connexionString)
{
    using var context = new ECatalogueContextDB(connexionString);

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    #region Addresses

    Address address1 = new Address
    {
        City = "Bucharest",
        Street = "Traian",
        StreetNumber = 23
    };

    Address address2 = new Address
    {
        City = "Bacau",
        Street = "Mioritei",
        StreetNumber = 2
    };

    Address address3 = new Address
    {
        City = "Bucharest",
        Street = "Decebal",
        StreetNumber = 174
    };

    Address address4 = new Address
    {
        City = "Iasi",
        Street = "Catedralei",
        StreetNumber = 21
    };

    Address address5 = new Address
    {
        City = "Buzau",
        Street = "Fierarului",
        StreetNumber = 87
    };

    #endregion


    #region Teachers

    context.Teachers.Add(new Teacher
    {
        FullName = "Laurentiu Catana",
        Address = address1,
        Rank = Rank.Instructor,
        Subject = new Subject
        {
            Name = "Mathematics",
        }
    });

    context.Teachers.Add(new Teacher
    {
        FullName = "Laura Mihail",
        Address = address5,
        Rank = Rank.Professor,
        Subject = new Subject
        {
            Name = "Civil Engineering",
        }
    });

    context.Teachers.Add(new Teacher
    {
        FullName = "Dan Pop",
        Address = address2,
        Rank = Rank.AssistantProfessor,
        Subject = new Subject
        {
            Name = "Computer Science",
        }
    });

    context.Teachers.Add(new Teacher
    {
        FullName = "Carmen Dinica",
        Address = address1,
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
        LastName = "Varan",
        Age = 51,
        Address = new Address
        {
            City = "Brasov",
            Street = "Cazanului",
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
        Address = address2,
    });

    context.Students.Add(new Student
    {
        FirstName = "Alexandra",
        LastName = "Stancu",
        Age = 34,
        Address = address1,
    });

    context.Students.Add(new Student
    {
        FirstName = "Gica",
        LastName = "Dobre",
        Age = 24
    });

    context.Students.Add(new Student
    {
        FirstName = "Mihaiela",
        LastName = "Morar",
        Age = 18
    });

    context.Students.Add(new Student
    {
        FirstName = "Daniel",
        LastName = "Ciobanu",
        Age = 27,
        Address = address3,
    });

    context.Students.Add(new Student
    {
        FirstName = "Culita",
        LastName = "Victor",
        Age = 44,
        Address = address2,
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
        Address = address4,
    });

    context.Students.Add(new Student
    {
        FirstName = "Bogdan",
        LastName = "Moraru",
        Age = 24
    });

    context.Students.Add(new Student
    {
        FirstName = "Crina",
        LastName = "Vlaicu",
        Age = 26,
        Address = address1,
    });

    context.Students.Add(new Student
    {
        FirstName = "Flavius",
        LastName = "Stancu",
        Age = 21,
        Address = address3,
    });

    #endregion

    context.SaveChanges();

    #region Marks

    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 1).Marks.Add( new Mark { Value = 7, SubjectId = 1, CreateDate = DateTime.Now, TeacherId  = 1 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 1).Marks.Add(new Mark { Value = 9, SubjectId = 1, CreateDate = DateTime.Now, TeacherId = 1 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 1).Marks.Add(new Mark { Value = 6, SubjectId = 3, CreateDate = DateTime.Now, TeacherId  = 3 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 5).Marks.Add(new Mark { Value = 7, SubjectId = 1, CreateDate = DateTime.Now, TeacherId  = 1 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 5).Marks.Add(new Mark { Value = 10, SubjectId = 4, CreateDate = DateTime.Now, TeacherId  = 4 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 10).Marks.Add(new Mark { Value = 5, SubjectId = 1, CreateDate = DateTime.Now, TeacherId  = 1 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 3).Marks.Add(new Mark { Value = 8, SubjectId = 2, CreateDate = DateTime.Now, TeacherId  = 2 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 3).Marks.Add(new Mark { Value = 7, SubjectId = 2, CreateDate = DateTime.Now, TeacherId  = 2 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 3).Marks.Add(new Mark { Value = 4, SubjectId = 1, CreateDate = DateTime.Now, TeacherId  = 1 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 12).Marks.Add(new Mark { Value = 10, SubjectId = 1, CreateDate = DateTime.Now, TeacherId  = 1 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 14).Marks.Add(new Mark { Value = 6, SubjectId = 3, CreateDate = DateTime.Now, TeacherId  = 3 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 11).Marks.Add(new Mark { Value = 9, SubjectId = 4, CreateDate = DateTime.Now, TeacherId  = 4 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 11).Marks.Add(new Mark { Value = 8, SubjectId = 4, CreateDate = DateTime.Now, TeacherId  = 4 });
    context.Students.Include(s => s.Marks).FirstOrDefault(s => s.StudentId == 7).Marks.Add(new Mark { Value = 10, SubjectId = 3, CreateDate = DateTime.Now, TeacherId = 3 });

    #endregion

    context.SaveChanges();
}

