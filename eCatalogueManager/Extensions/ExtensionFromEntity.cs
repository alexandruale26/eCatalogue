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

        public static SubjectToGet ToDto(this Subject subject)
        {
            if (subject.TeacherId == null)
            {
                return new SubjectToGet
                {
                    Name = subject.Name,
                    TeacherId = 0,
                };
            }

            return new SubjectToGet
            {
                Name = subject.Name,
                TeacherId = (int)subject.TeacherId,
            };
        }

        public static MarkToGet ToDto(this Mark mark)
        {
            return new MarkToGet
            {
                Value = mark.Value,
                StudentId = mark.StudentId,
                SubjectId = mark.SubjectId,
                CreateDaSte = mark.CreateDate
            };
        }
    }
}
