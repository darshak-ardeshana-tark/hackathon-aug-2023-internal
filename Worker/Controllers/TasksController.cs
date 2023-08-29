using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = Worker.Models.Task;

namespace Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpPost("executetask")]
        public IActionResult Executetask([FromBody] Task task)
        {

            return Ok();
        }
    }
}
