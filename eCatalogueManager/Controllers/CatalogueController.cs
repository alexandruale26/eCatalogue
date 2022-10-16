using Data;
using Data.Data;
using Data.Models;
using ECatalogueManager.DTOs;
using ECatalogueManager.Extensions;
using Microsoft.AspNetCore.Mvc;
using Data.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly DataLayer dataLayer;
        private readonly ECatalogueContextDB context;

        public CatalogueController(DataLayer dataLayer, ECatalogueContextDB context)
        {
            this.dataLayer = dataLayer;
            this.context = context;
        }


        /// <summary>
        /// Returns all marks for a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="subjectId">Subject's ID</param>
        /// <returns>Result</returns>
        [HttpGet("students/{id}/marks/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarksForStudent([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery][Optional][Range(0, int.MaxValue)] int subjectId)
        {
            List<MarkToGet> marks;
            try
            {
                marks = dataLayer.GetAllMarksForStudent(id, subjectId).Select(m => m.ToDto()).ToList();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok(marks);
        }

        /// <summary>
        /// Returns averages per subject for a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="subjectId">Subject's ID</param>
        /// <returns>Result</returns>
        [HttpGet("students/{id}/subjects/averages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AveragesPerSubjectToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAveragesPerSubject([FromRoute][Range(1, int.MaxValue)] int id, [FromQuery][Optional][Range(0, int.MaxValue)] int subjectId)
        {
            List<AveragesPerSubjectToGet> averages;
            try
            {
                averages = dataLayer.GetAllMarksForStudent(id, subjectId).ToDtoByAverage();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Ok(averages);
        }

        /// <summary>
        /// Returns all students ordered by averages
        /// </summary>
        /// <param name="orderAscending">Order students ascending</param>
        /// <returns>Result</returns>
        [HttpGet("students/ordered-by-averages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentOrderedToGet>))]
        public IActionResult GetStudentsOrderedByAverages([FromQuery] bool orderAscending)
        {
            if (orderAscending)
            {
                return Ok(context.Students.Include(s => s.Marks).OrderBy(s => s.Marks.Average(m => m.Value)).Select(s => s.ToDtoOrdered()).ToList());
            }
            return Ok(context.Students.Include(s => s.Marks).OrderByDescending(s => s.Marks.Average(m => m.Value)).Select(s => s.ToDtoOrdered()).ToList());
        }

        /// <summary>
        /// Returns all marks given by a teacher
        /// </summary>
        /// <param name="id">Teacher's ID</param>
        /// <returns>Result</returns>
        [HttpGet("teachers/{id}/marks/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkByTeacherToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarksByTeacher([FromRoute][Range(1, int.MaxValue)] int id)
        {
            Teacher teacher = context.Teachers.FirstOrDefault(t => t.TeacherId == id);
            if (teacher == null)
            {
                return NotFound($"Teacher with ID {id} does not exists");
            }
            return Ok(context.Marks.Where(m => m.SubjectId == context.Subjects.First(s => s.TeacherId == id).TeacherId).Select(m => m.ToDtoByTeacher()).ToList());
        }

        /// <summary>
        /// Adds a mark to a student
        /// </summary>
        /// <param name="newMark">Mark's data</param>
        /// <returns>Result</returns>
        [HttpPost("marks/create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MarkToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult AddMarkToStudent([FromBody] MarkToCreate newMark)
        {
            MarkToGet mark;
            try
            {
                mark = dataLayer.AddMark(newMark.ToEntity()).ToDto();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            catch (SubjectDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully added", mark);
        }

        /// <summary>
        /// Removes a subject
        /// </summary>
        /// <param name="id">Subject's ID</param>
        /// <returns>Result</returns>
        [HttpDelete("subjects/{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveSubject([FromRoute][Range(1, int.MaxValue)] int id)
        {
            try
            {
                dataLayer.RemoveSubject(id);
            }
            catch (SubjectDoesNotExistException e)
            {
                return NotFound(e.message);
            }
            return Ok("Successfully removed");
        }
    }
}
