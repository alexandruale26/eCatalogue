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
                SubjectId = mark.SubjectId,
                TeacherId= mark.TeacherId,
                CreationDate = mark.CreateDate.ToString(),
            };
        }

        public static MarkByTeacherToGet ToDtoByTeacher(this Mark mark)
        {
            return new MarkByTeacherToGet
            {
                Value = mark.Value,
                StudentId = mark.StudentId,
                CreatedDate = mark.CreateDate.ToString(),
            };
        }

        public static List<AveragesPerSubjectToGet> ToDtoByAverage(this Student student)
        {
            return student.Marks
                .GroupBy(m => m.SubjectId)
                .Select(m => new AveragesPerSubjectToGet { SubjectId = m.Key, Value = m
                .Average(m => m.Value) })
                .ToList();
        }

        public static StudentOrderedToGet ToDtoOrdered(this Student student)
        {
            if (student.Marks.Count == 0)
            {
                return new StudentOrderedToGet
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Age = student.Age,
                    Average = 0.0
                };
            }

            return new StudentOrderedToGet
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                Average = Math.Round(student.Marks.Average(s => s.Value),2)
            };
        }

        public static TeacherToGet ToDto(this Teacher teacher)
        {
            if (teacher.Address == null)
            {
                return new TeacherToGet
                {
                    FullName = teacher.FullName,
                    Rank = teacher.Rank.RankToName(),
                    Subject = teacher.Subject.Name,
                    City = null,
                    Street = null,
                    StreetNumber = null
                };
            }

            return new TeacherToGet
            {
                FullName = teacher.FullName,
                Rank = teacher.Rank.RankToName(),
                Subject = teacher.Subject.Name,
                City = teacher.Address.City,
                Street = teacher.Address.Street,
                StreetNumber = teacher.Address.StreetNumber
            };
        }
    }
}
