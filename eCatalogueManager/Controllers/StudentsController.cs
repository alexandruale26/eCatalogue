using Microsoft.AspNetCore.Mvc;
using Data;
using ECatalogueManager.DTOs;
using ECatalogueManager.Extensions;
using Data.Exceptions;
using System.ComponentModel.DataAnnotations;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataLayer dataLayer;
        private readonly ECatalogueContextDB context;

        public StudentsController(DataLayer dataLayer, ECatalogueContextDB context)
        {
            this.dataLayer = dataLayer;
            this.context = context;
        }


        /// <summary>
        /// Returns all students
        /// </summary>
        /// <returns>Result</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentToGet>))]
        public IActionResult GetAllStudentsFromDB()
        {
            return Ok(context.Students.Include(s => s.Address).Select(s => s.ToDto()).ToList());
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BasicStudentToGet))]
        public IActionResult CreateStudent([FromBody] StudentToCreate newStudent)
        {
            return Created("Successfully created", dataLayer.CreateStudent(newStudent.ToEntity()).ToDtoBasic());
        }

        /// <summary>
        /// Creates or updates a student's address
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="newAddress">Address's data</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyAddress([FromRoute][Range(1, int.MaxValue)] int id, [FromBody] AddressToCreate newAddress)
        {
            StudentToGet student;
            try
            {
                student = dataLayer.ModifyStudentAddress(id, newAddress.ToEntity()).ToDto();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok(student);
        }

        /// <summary>
        /// Updates student's data
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="newStudent">Student's new data</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasicStudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyStudent([FromRoute][Range(1, int.MaxValue)] int id, [FromBody] StudentToCreate newStudent)
        {
            BasicStudentToGet student;
            try
            {
                student = dataLayer.ModifyStudentData(id, newStudent.ToEntity()).ToDtoBasic();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok(student);
        }

        /// <summary>
        /// Removes a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <returns>Result</returns>
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveStudent([FromRoute][Range(1, int.MaxValue)] int id)
        {
            try
            {
                dataLayer.RemoveStudent(id);
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok("Successfully removed");
        }
    }
}
