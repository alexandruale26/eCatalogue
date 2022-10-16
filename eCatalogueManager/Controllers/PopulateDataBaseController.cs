using Microsoft.AspNetCore.Mvc;
using Data;
namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateDatabaseController : ControllerBase
    {
        private readonly SeedDB seeder;

        public PopulateDatabaseController(SeedDB seeder)
        {
            this.seeder = seeder;
        }

        /// <summary>
        /// Populate Database
        /// </summary>
        /// <returns>Populated Database</returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SeedDB()
        {
            seeder.PopulateDB();
            return Ok("Success");
        }
    }
}
