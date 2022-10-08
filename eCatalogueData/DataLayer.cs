using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Exceptions;

namespace Data
{
    public class DataLayer
    {
        private readonly string connectionString;

        public DataLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public List<Student> GetAllStudents()
        {
            using var context = new ECatalogueContextDB(connectionString);
            return context.Students.Include(s => s.Address).ToList();
        }

        public Student GetStudent(int studentId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }
            return context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
        }

        public Student CreateStudent(Student student)
        {
            using var context = new ECatalogueContextDB(connectionString);
            Student newStudent;

            newStudent = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
            };

            context.Students.Add(newStudent);
            context.SaveChanges();
            return student;
        }

        public void RemoveStudent(int studentId, bool wantToRemoveAddress)
        {
            using var context = new ECatalogueContextDB(connectionString);

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
                RemoveStudentAddressIfIsEmpty(studentAddressId);
            }
        }

        public Student ModifyStudentData(int studentId, Student student)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student studentToModify = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);

            studentToModify.FirstName = student.FirstName;
            studentToModify.LastName = student.LastName;
            studentToModify.Age = student.Age;

            context.SaveChanges();
            return studentToModify;
        }

        public Address ModifyStudentAddress(int studentId, bool removeAddressIfEmpty, Address address)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException(studentId);
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
            Address newAddress = new Address();

            int oldAddressId = 0;
            int addressId = GetAddressID(address);

            if (existingStudent.Address != null) oldAddressId = existingStudent.Address.AddressId;

            if (addressId != 0)
            {
                newAddress = context.Addresses.First(a => a.AddressId == addressId);
            }
            else
            {
                newAddress = CreateAddress(address);
            }
            
            existingStudent.Address = newAddress;
            context.SaveChanges();

            if (removeAddressIfEmpty && oldAddressId != 0) RemoveStudentAddressIfIsEmpty(oldAddressId);
            return newAddress;
        }


        private Address CreateAddress(Address address)
        {
            using var context = new ECatalogueContextDB(connectionString);

            Address newAddress = new Address
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber,
            };

            context.Add(newAddress);
            context.SaveChanges();
            return newAddress;
        }

        private int GetAddressID(Address address)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (address == null || address.StreetNumber <= 0) return 0;

            var existingAddress = context.Addresses
                .Where(a => a.City.ToLower() == address.City.ToLower())
                .Where(a => a.Street.ToLower() == address.Street.ToLower())
                .First(a => a.StreetNumber == address.StreetNumber);

            if (existingAddress != null)
            {
                return existingAddress.AddressId;
            }
            return 0;
        }

        private void RemoveStudentAddressIfIsEmpty(int addressId)
        {
            using var context = new ECatalogueContextDB(connectionString);

            if (context.Addresses.Any(a => a.AddressId == addressId) && context.Addresses.Include(s => s.Students).First(a => a.AddressId == addressId).Students.Count == 0)
            {
                context.Addresses.Remove(context.Addresses.First(a => a.AddressId == addressId));
            }
            context.SaveChanges();
        }
    }
}
