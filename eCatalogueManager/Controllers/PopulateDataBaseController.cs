using Microsoft.AspNetCore.Mvc;
using Data;
namespace ECatalogueManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateDataBaseController : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult SeedDB()
        {
            return Ok("success");
        }
    }
}
