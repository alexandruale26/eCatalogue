﻿using Microsoft.AspNetCore.Mvc;
using Data;
using ECatalogueManager.DTOs;
using ECatalogueManager.Extensions;
using Data.Exceptions;

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
        /// <returns></returns>
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
        [HttpGet("{id}/student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetStudent([FromRoute] int id)
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
        /// <param name="newStudent">New Student</param>
        /// <returns>Result</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentToGet))]
        public IActionResult CreateStudent([FromBody] StudentToCreate newStudent)
        {
            return Created("Successfully created", dataLayer.CreateStudent(newStudent.ToEntity()).ToDto());
        }

        /// <summary>
        /// Removes a student
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <param name="removeAddress">If want to remove address from database if address has no students</param>
        /// <returns>Result</returns>
        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult RemoveStudent([FromRoute]int id, [FromQuery] bool removeAddress)
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

        /// <summary>
        /// Updates student's data
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="newStudent">Student's new data</param>
        /// <returns></returns>
        [HttpPut("{id}/update/data")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyStudent([FromRoute] int id, [FromBody] StudentToCreate newStudent)
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
        /// Creates or updates student's address
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeAddress">If want to remove address if has no students</param>
        /// <param name="addressToCreate">New address</param>
        /// <returns>Result</returns>
        [HttpPut("{id}/update/address")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ModifyAddress([FromRoute] int id, [FromQuery] bool removeAddress, [FromBody] AddressToCreate addressToCreate)
        {
            AddressToGet address;
            try
            {
                address = dataLayer.ModifyStudentAddress(id, removeAddress, addressToCreate.ToEntity()).ToDto();
            }
            catch (StudentDoesNotExistsException e)
            {
                return NotFound(e.message);
            }
            return Created("Successfully updated", address);
        }
    }
}
