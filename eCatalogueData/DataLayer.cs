using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Exceptions;

namespace Data
{
    public static class DataLayer
    {
        public static List<Student> GetAllStudents()
        {
            using var context = new StudentsManagerContextDB();
            return context.Students.Include(s => s.Address).ToList();
        }

        public static Student GetStudent(int studentId)
        {
            using var context = new StudentsManagerContextDB();

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException();
            }
            return context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
        }

        public static Student CreateStudent(string firstName, string lastName, int age, string? city, string? street, int? streetNumber)
        {
            using var context = new StudentsManagerContextDB();

            int DBAddressId = GetAddressID(city, street, (int)streetNumber);

            Student newStudent = new Student();
            Address newAddress = null;

            if (DBAddressId == 0 && city != null && street != null && streetNumber != null && streetNumber > 0)
            {
                newAddress = CreateAddress(city, street, (int)streetNumber);
            }
            else if (DBAddressId > 0)
            {
                newAddress = context.Addresses.First(a => a.AddressId == DBAddressId);
            }

            newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
            };

            context.Students.Add(newStudent);
            context.SaveChanges();

            if (newAddress != null) newStudent.Address = newAddress;
            context.SaveChanges();
            return newStudent;
        }

        public static int RemoveStudent(int studentId, bool wantToRemoveAddress)
        {
            using var context = new StudentsManagerContextDB();

            Student existingStudent = GetStudent(studentId);
            int studentAddressId = 0;

            if (existingStudent.Address != null)
            {
                studentAddressId = existingStudent.Address.AddressId;
            }

            context.Students.Remove(existingStudent);
            context.SaveChanges();

            if (wantToRemoveAddress && studentAddressId > 0)
            {
                RemoveAddressIfIsEmpty(studentAddressId);
            }
            return existingStudent.StudentId;
        }

        public static Student ModifyStudentData(int studentId, string? firstName, string? lastName, int? age)
        {
            using var context = new StudentsManagerContextDB();

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException();
            }

            Student studentToModify = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);

            if (firstName != null) studentToModify.FirstName = firstName;
            if (lastName != null) studentToModify.LastName = lastName;
            if (age != null && age > 15) studentToModify.Age = (int)age;

            context.SaveChanges();
            return studentToModify;
        }

        public static Student ModifyStudentAddress(int studentId, bool removeAddressIfEmpty, string city, string street, int streetNumber)
        {
            using var context = new StudentsManagerContextDB();

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException();
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
            Address newAddress = new Address();

            int oldAddressId = 0;
            int addressId = GetAddressID(city, street, streetNumber);

            if (existingStudent.Address != null) oldAddressId = existingStudent.Address.AddressId;

            if (addressId != 0)
            {
                newAddress = context.Addresses.First(a => a.AddressId == addressId);
            }
            else
            {
                newAddress = CreateAddress(city, street, streetNumber);
            }

            
            existingStudent.Address = newAddress;
            context.SaveChanges();

            if (removeAddressIfEmpty && oldAddressId != 0) RemoveAddressIfIsEmpty(oldAddressId);
            return existingStudent;
        }


        private static Address CreateAddress(string city, string street, int number)
        {
            using var context = new StudentsManagerContextDB();

            Address newAddress = new Address
            {
                City = city,
                Street = street,
                StreetNumber = number
            };

            context.Add(newAddress);
            context.SaveChanges();
            return newAddress;
        }

        private static int GetAddressID(string city, string street, int streetNumber)
        {
            using var context = new StudentsManagerContextDB();

            if (street == null || city == null || streetNumber == null || streetNumber <= 0) return 0;

            var existingAddress = context.Addresses
                .Where(a => a.City.ToLower() == city.ToLower())
                .Where(a => a.Street.ToLower() == street.ToLower())
                .FirstOrDefault(a => a.StreetNumber == streetNumber);

            if (existingAddress != null)
            {
                return existingAddress.AddressId;
            }
            return 0;
        }

        private static void RemoveAddressIfIsEmpty(int addressId)
        {
            using var context = new StudentsManagerContextDB();

            if (context.Addresses.Any(a => a.AddressId == addressId) && context.Addresses.Include(s => s.Students).First(a => a.AddressId == addressId).Students.Count == 0)
            {
                context.Addresses.Remove(context.Addresses.First(a => a.AddressId == addressId));
            }
            context.SaveChanges();
        }
    }
}
