using Microsoft.AspNetCore.Mvc;
using Task = Worker.Models.Task;

namespace Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly WorkerInfo _workerInfo;
        public TasksController(WorkerInfo workerInfo)
        {
            _workerInfo = workerInfo;
        }

        [HttpPost("executetask")]
        public IActionResult Executetask([FromBody] Task task)
        {
            new Executor(_workerInfo).ExecuteTask(task);
            return Ok();
        }
    }
}
