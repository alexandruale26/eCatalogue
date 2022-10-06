using Microsoft.AspNetCore.Mvc;
using EStudentsManager.DTOs;
using Data.Models;
using Data;
using eCatalogueManager.Extensions;

namespace EStudentsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        /// <summary>
        /// Returns all students
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IEnumerable<StudentToGet> GetAllStudentsFromDB()
        {
            var students = DataLayer.GetAllStudents();
            var result = new List<StudentToGet>();

            students.ForEach(s => result.Add(s.ToDto()));
            return result;
        }

        /// <summary>
        /// Return a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="getAddress">If want to get the student's address</param>
        /// <returns></returns>
        [HttpGet("student/{id}")]
        public StudentToGet GetStudent([FromRoute] int id)
        {
            var student = DataLayer.GetStudent(id);

            return FullStudentDetails(student.FirstName, student.LastName, student.Age, student.Address.City, student.Address.Street, student.Address.StreetNumber);
        }

        /// <summary>
        /// Create a student
        /// </summary>
        /// <param name="newStudent">Student data</param>
        /// <param name="hasAddress">If new student has address</param>
        /// <returns></returns>
        [HttpPost("create")]
        public StudentToGet CreateStudent([FromBody] StudentToCreate newStudent, [FromQuery] bool hasAddress)
        {
            Student student;

            if (hasAddress)
            {
                student =  DataLayer.CreateStudentWithAddress(newStudent.FirstName, newStudent.LastName, newStudent.Age, newStudent.City, newStudent.Street, newStudent.StreetNumber);
                return FullStudentDetails(student.FirstName, student.LastName, student.Age, student.Address.City, student.Address.Street, student.Address.StreetNumber);

            }
            student =  DataLayer.CreateStudentWithoutAddress(newStudent.FirstName, newStudent.LastName, newStudent.Age);
            return StudentNoAddressDetails(student.FirstName, student.LastName, student.Age);
        }

        /// <summary>
        /// Remove a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="removeAddress">If want to remove address from database if address has no students</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public int RemoveStudent([FromRoute]int id, [FromQuery] bool removeAddress)
        {
            return DataLayer.RemoveStudent(id, removeAddress);
        }

        /// <summary>
        /// Update student's data
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="studUpdates">Student's new data</param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public StudentToUpdate ModifyStudent([FromRoute] int id, [FromBody] StudentToUpdate studUpdates)
        {
            DataLayer.ModifyStudentData(id, studUpdates.FirstName, studUpdates.LastName, studUpdates.Age);
            return studUpdates;
        }
    }
}
