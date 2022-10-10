using Data;
using ECatalogueManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using Data.Exceptions;
using ECatalogueManager.Extensions;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly DataLayer dataLayer;

        public TeacherController(DataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }


        /// <summary>
        /// Creates a teacher
        /// </summary>
        /// <param name="newTeacher">Teacher's data</param>
        /// <returns>Result</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TeacherToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult CreateStudent([FromBody] TeacherToCreate newTeacher)
        {
            TeacherToGet teacher;

            try
            {
                teacher = dataLayer.CreateTeacher(newTeacher.ToEntity()).ToDto();
            }
            catch (SubjectDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully created",teacher);
        }

        /// <summary>
        /// Creates or updates a teachers's address
        /// </summary>
        /// <param name="id">Teacher's ID</param>
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
                address = dataLayer.ModifyTeacherAddress(id, removeAddress, newAddress.ToEntity()).ToDto();
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully updated", address);
        }



        /// <summary>
        /// Removes a teacher
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <param name="removeAddress">If want to remove address from database if address is empty</param>
        /// <returns>Result</returns>
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveTeacher([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery] bool removeAddress)
        {
            try
            {
                dataLayer.RemoveTeacher(id, removeAddress);
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok("Successfully removed");
        }
    }
}
