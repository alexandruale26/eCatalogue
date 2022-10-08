using Data;
using Data.Models;
using ECatalogueManager.DTOs;
using ECatalogueManager.Extensions;
using Microsoft.AspNetCore.Mvc;
using Data.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly DataLayer dataLayer;

        public CatalogueController(DataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        /// <summary>
        /// Creates or updates a subject
        /// </summary>
        /// <param name="newSubject">Subject's data</param>
        /// <returns>Result</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubjectToGet))]
        public IActionResult CreateSubject([FromBody] SubjectToCreate newSubject)
        {
            return Created("Successfully created", dataLayer.AddSubject(newSubject.ToEntity()).ToDto());
        }

        /// <summary>
        /// Adds a mark to a student
        /// </summary>
        /// <param name="newMark">Mark's data</param>
        /// <returns>Result</returns>
        [HttpPost("mark/add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult AddMarkToStudent([FromBody] MarkToCreate newMark)
        {
            Mark mark;
            try
            {
                mark = dataLayer.AddMark(newMark.ToEntity());
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            catch (SubjectDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok("Successfully added");
        }

        [HttpGet("{studentId}/mark/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarks([FromRoute][Range(1, int.MaxValue)] int studentId, [FromQuery][Optional][Range(1, int.MaxValue)] int subjectId)
        {
            List<MarkToGet> marks;
            try
            {
                marks = dataLayer.GetAllMarks(studentId, subjectId).Select(m => m.ToDto()).ToList();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            catch (SubjectDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok(marks);
        }
    }
}
