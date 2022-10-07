using Data.Models;
using EStudentsManager.DTOs;

namespace eCatalogueManager.Extensions
{
    public static class ExtensionsFromEntity
    {
        public static StudentToGet ToDto(this Student student)
        {
            if (student.Address != null)
            {
                return new StudentToGet
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Age = student.Age,
                    City = student.Address.City,
                    Street = student.Address.Street,
                    StreetNumber = student.Address.StreetNumber
                };
            }

            return new StudentToGet
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                City = null,
                Street = null,
                StreetNumber = null
            };
        }
    }
}
