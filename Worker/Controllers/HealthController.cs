using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("I'm Healthy :)");
        }
    }
}
