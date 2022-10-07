using Data.Models;
using ECatalogueManager.DTOs;

namespace ECatalogueManager.Extensions
{
    public static class ExtensionToEntity
    {
        public static Student ToEntity(this StudentToCreate student)
        {
            return new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age
            };
        }

        public static Address ToEntity(this AddressToCreate address)
        {
            return new Address
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber
            };
        }
    }
}
