using Microsoft.AspNetCore.Mvc;
using Data;
using ECatalogueManager.DTOs;
using ECatalogueManager.Extensions;
using Data.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataLayer dataLayer;

        public StudentsController(DataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }


        /// <summary>
        /// Returns all students
        /// </summary>
        /// <returns>Result</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllStudentsFromDB()
        {
            List<StudentToGet> result = dataLayer.GetAllStudents().Select(s => s.ToDto()).ToList();

            // maybe should remove
            if (result.Count == 0)
            {
                return NotFound("No student found");
            }
            return Ok(result);
        }

        /// <summary>
        /// Returns a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <returns>Result</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetStudent([FromRoute][Range(1, int.MaxValue)] int id)
        {
            StudentToGet student;
            try
            {
                student = dataLayer.GetStudent(id).ToDto();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok(student);
        }

        /// <summary>
        /// Creates a student
        /// </summary>
        /// <param name="newStudent">Student's data</param>
        /// <returns>Result</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentToGet))]
        public IActionResult CreateStudent([FromBody] StudentToCreate newStudent)
        {
            return Created("Successfully created", dataLayer.CreateStudent(newStudent.ToEntity()).ToDto());
        }

        /// <summary>
        /// Creates or updates a student's address
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="removeAddress">If want to remove address from database if address is empty</param>
        /// <param name="newAddress">Address's data</param>
        /// <returns>Result</returns>
        [HttpPost("{id}/update/address")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyAddress([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery] bool removeAddress, [FromBody] AddressToCreate newAddress)
        {
            AddressToGet address;
            try
            {
                address = dataLayer.ModifyStudentAddress(id, removeAddress, newAddress.ToEntity()).ToDto();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully updated", address);
        }

        /// <summary>
        /// Updates student's data
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="newStudent">Student's new data</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/data")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyStudent([FromRoute][Range(1, int.MaxValue)] int id, [FromBody] StudentToCreate newStudent)
        {
            StudentToGet student;
            try
            {
                student = dataLayer.ModifyStudentData(id, newStudent.ToEntity()).ToDto();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully updated", student);
        }

        /// <summary>
        /// Removes a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="removeAddress">If want to remove address from database if address is empty</param>
        /// <returns>Result</returns>
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveStudent([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery] bool removeAddress)
        {
            try
            {
                dataLayer.RemoveStudent(id, removeAddress);
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok("Successfully removed");
        }
    }
}
