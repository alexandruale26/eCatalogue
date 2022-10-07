using Data.Data;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

//using var conte = new StudentsManagerContextDB();
//Console.WriteLine(conte.Addresses.Include(a => a.Students).First(a => a.AddressId == 8).Students.Count);


ResedDB();


DataLayer.CreateStudent("Mara", "Danciu", 33, "Bucharest", "Traian", 23);
DataLayer.CreateStudent("Mariana", "Marica", 29, "Brasov", "Cizmarului", 100);
DataLayer.CreateStudent("Catalin", "Varan", 51, "Brasov", "Cazanului", 10);
DataLayer.CreateStudent("Daniel", "Fastoc", 28, "Iasi", "Catedralei", 21);


using var context = new eCatalogueContextDB();


context.SaveChanges();

static void ResedDB()
{
    using var context = new eCatalogueContextDB();

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
        StreetNumber = 134
    };

    Address address4 = new Address
    {
        City = "Iasi",
        Street = "Catedralei",
        StreetNumber = 21
    };

    #endregion


    #region Students

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
}

