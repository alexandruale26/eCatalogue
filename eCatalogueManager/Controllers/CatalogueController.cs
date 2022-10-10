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
        /// Returns all marks for a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="subjectId">Subject's ID</param>
        /// <returns>Result</returns>
        [HttpGet("students/{id}/marks/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarks([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery][Optional][Range(0, int.MaxValue)] int subjectId)
        {
            List<MarkToGet> marks;
            try
            {
                marks = dataLayer.GetAllMarks(id, subjectId).Select(m => m.ToDto()).ToList();
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

        /// <summary>
        /// Retuns averages per subject for a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <returns>Result</returns>
        [HttpGet("students/{id}/marks/ordered-by-subject-averages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AveragesPerSubjectToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAveragesPerSubject([FromRoute][Range(1, int.MaxValue)] int id)
        {
            Student student;

            try
            {
                student = dataLayer.GetAveragesPerSubject(id);
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok(student.ToDtoByAverage());
        }

        /// <summary>
        /// Returns all students ordered by averages
        /// </summary>
        /// <param name="orderByAscending">If want to order students by ascending</param>
        /// <returns>Result</returns>
        [HttpGet("students/marks/ordered-by-averages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentOrderedToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllStudentsOrderedByAverages([FromQuery][Optional] bool orderByAscending)
        {
            // shouldn't have exposed student's ID??
            List<StudentOrderedToGet> result = dataLayer.GetStudentsOrderedByAverages(orderByAscending).Select(s => s.ToDtoOrdered()).ToList();

            // maybe should remove
            if (result.Count == 0)
            {
                return NotFound("No student found");
            }
            return Ok(result);
        }

        /// <summary>
        /// Returns all marks by a teacher
        /// </summary>
        /// <param name="id">Teachers ID</param>
        /// <returns>Result</returns>
        [HttpGet("teachers/{id}/marks/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkByTeacherToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarksByTeacher([FromRoute][Range(1, int.MaxValue)] int id)
        {
            List<MarkByTeacherToGet> marks;

            try
            {
                marks = dataLayer.GetMarksByTeacher(id).Select(m => m.ToDtoByTeacher()).ToList();
            }
            catch (TeacherDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok(marks);
        }

        /// <summary>
        /// Creates or updates a subject
        /// </summary>
        /// <param name="newSubject">Subject's data</param>
        /// <returns>Result</returns>
        [HttpPost("subjects/create")]
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
        [HttpPost("marks/add")]
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

        /// <summary>
        /// Removes a subject
        /// </summary>
        /// <param name="id">Subject's ID</param>
        /// <returns>Result</returns>
        [HttpDelete("subjects/{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveSubject([FromRoute][Range(1, int.MaxValue)] int id)
        {
            Subject subject;

            try
            {
                subject = dataLayer.RemoveSubject(id);
            }
            catch (SubjectDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok($"Subject {subject.Name} was successfully removed");
        }
    }
}
