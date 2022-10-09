using Data;
using ECatalogueManager.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStudent">Teacher's data</param>
        /// <returns>Result</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentToGet))]
        public IActionResult CreateStudent([FromBody] StudentToCreate newStudent)/// teacher to create
        {
            return Created("", "");
        }


    }
}
