using Data;
using ECatalogueManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using Data.Exceptions;
using ECatalogueManager.Extensions;
using System.ComponentModel.DataAnnotations;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly DataLayer dataLayer;
        private readonly ECatalogueContextDB context;

        public TeachersController(DataLayer dataLayer, ECatalogueContextDB context)
        {
            this.dataLayer = dataLayer;
            this.context = context;
        }


        /// <summary>
        /// Returns all teachers
        /// </summary>
        /// <returns>Result</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TeacherToGet>))]
        public IActionResult GetAllTeachersFromDB()
        {
            return Ok(context.Teachers.Include(t => t.Address).Include(t => t.Subject).Select(t => t.ToDto()).ToList());
        }

        /// <summary>
        /// Returns a teacher
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <returns>Result</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetTeacher([FromRoute][Range(1, int.MaxValue)] int id)
        {
            TeacherToGet teacher;
            try
            {
                teacher = dataLayer.GetTeacher(id).ToDto();
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok(teacher);
        }

        /// <summary>
        /// Creates a teacher
        /// </summary>
        /// <param name="newTeacher">Teacher's data</param>
        /// <returns>Result</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BasicTeacherToGet))]
        public IActionResult CreateTeacher([FromBody] TeacherToCreate newTeacher)
        {
            return Created("Successfully created", dataLayer.CreateTeacher(newTeacher.ToEntity()).ToDtoBasic());
        }

        /// <summary>
        /// Creates or updates a teachers's address
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <param name="removeAddress">If want to remove address from database if address is empty</param>
        /// <param name="newAddress">Address's data</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/address")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TeacherToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyAddress([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery] bool removeAddress, [FromBody] AddressToCreate newAddress)
        {
            TeacherToGet address;
            try
            {
                address = dataLayer.ModifyTeacherAddress(id, removeAddress, newAddress.ToEntity()).ToDto();
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully updated", address);
        }

        /// <summary>
        /// Creates or updates a teacher's subject
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <param name="newSubjectName">Subject's name</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/subject")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubjectToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyTeacherSubject([FromRoute][Range(1, int.MaxValue)] int id, [FromBody] string newSubjectName)
        {
            SubjectToGet subject;
            try
            {
                subject = dataLayer.ModifyTeacherSubject(id, newSubjectName).ToDto();
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully updated", subject);
        }

        /// <summary>
        /// Promotes a teacher
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/rank")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult PromoteTeacher([FromRoute][Range(1, int.MaxValue)] int id)
        {
            string result;
            try
            {
                result = dataLayer.PromoteTeacher(id).RankToName();
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok($"Successfully promoted to {result}");
        }

        /// <summary>
        /// Removes a teacher
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <returns>Result</returns>
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveTeacher([FromRoute][Range(1, int.MaxValue)] int id)
        {
            try
            {
                dataLayer.RemoveTeacher(id);
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok("Successfully removed");
        }
    }
}
