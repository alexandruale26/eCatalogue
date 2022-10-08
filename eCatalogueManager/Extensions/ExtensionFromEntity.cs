using Data.Models;
using ECatalogueManager.DTOs;

namespace ECatalogueManager.Extensions
{
    public static class ExtensionFromEntity
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

        public static AddressToGet ToDto(this Address address)
        {
            return new AddressToGet
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber
            };
        }
    }
}
