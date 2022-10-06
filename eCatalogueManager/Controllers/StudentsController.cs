using Microsoft.AspNetCore.Mvc;
using EStudentsManager.DTOs;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllStudentsFromDB()
        {
            var students = DataLayer.GetAllStudents();
            var result = new List<StudentToGet>();

            students.ForEach(s => result.Add(s.ToDto()));

            if (result.Count == 0)
            {
                return NotFound("No student found");
            }
            return Ok(result);
        }

        /// <summary>
        /// Returns a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns></returns>
        [HttpGet("student/{id}")]
        //////////
        public StudentToGet GetStudent([FromRoute] int id)
        {
            var student = DataLayer.GetStudent(id);
            return student.ToDto();
        }

        /// <summary>
        /// Creates a student
        /// </summary>
        /// <param name="newStudent">Student data</param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentToGet))]
        //////////////////
        public IActionResult CreateStudent([FromBody] StudentToCreate newStudent)
        {
            return Created("Success", DataLayer.CreateStudent(newStudent.FirstName, newStudent.LastName, newStudent.Age, newStudent.City, newStudent.Street, newStudent.StreetNumber).ToDto());
        }

        /// <summary>
        /// Removes a student
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
        /// Updates student's data
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="studentUpdates">Student's new data</param>
        /// <returns></returns>
        [HttpPut("update/data{id}")]
        public StudentToGet ModifyStudent([FromRoute] int id, [FromBody] StudentToUpdate studentUpdates)
        {
            return DataLayer.ModifyStudentData(id, studentUpdates.FirstName, studentUpdates.LastName, studentUpdates.Age).ToDto();
        }

        /// <summary>
        /// Updates student's address
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeAddress">If want to remove address if has no students</param>
        /// <param name="addressToUpdate">New address</param>
        /// <returns></returns>
        [HttpPut("update/address{id}")]
        public StudentToGet ModifyAddress([FromRoute] int id, [FromQuery] bool removeAddress, [FromBody] AddressToUpdate addressToUpdate)
        {
            return DataLayer.ModifyStudentAddress(id, removeAddress, addressToUpdate.City, addressToUpdate.Street, addressToUpdate.StreetNumber).ToDto();
        }
    }
}
