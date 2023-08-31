using Microsoft.AspNetCore.Mvc;
using Task = Worker.Models.Task;

namespace Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly WorkerInfo workerInfo;
        public TasksController(WorkerInfo workerInfo)
        {
            this.workerInfo = workerInfo;
        }
        [HttpPost("executetask")]
        public IActionResult Executetask([FromBody] Task task)
        {
            Console.WriteLine(workerInfo.Name);
            Console.WriteLine(workerInfo.Port);
            Console.WriteLine(workerInfo.WorkDir);
            return Ok();
        }
    }
}
