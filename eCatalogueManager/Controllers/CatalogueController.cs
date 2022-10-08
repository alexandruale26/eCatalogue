using Data;
using ECatalogueManager.DTOs;
using ECatalogueManager.Extensions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
    }
}
