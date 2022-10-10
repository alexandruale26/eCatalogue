using Microsoft.AspNetCore.Mvc;
using Data;
namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateDataBaseController : ControllerBase
    {
        private readonly SeedDB seeder;

        public PopulateDataBaseController(SeedDB seeder)
        {
            this.seeder = seeder;
        }

        /// <summary>
        /// Populate Database
        /// </summary>
        /// <returns>Populated Database</returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult SeedDB()
        {
            seeder.PopulateDB();
            return Ok("Success");
        }
    }
}
