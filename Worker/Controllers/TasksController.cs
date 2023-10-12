using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Task = Worker.Models.Task;

namespace Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        static CancellationTokenSource cancellationTokenSource;
        static System.Threading.Tasks.Task _task;
        private readonly Executor _executor;

        public TasksController(Executor executor)
        {
            _executor = executor;
        }

        [HttpPost("executetask")]
        public IActionResult Executetask([FromBody] Task task)
        {
            cancellationTokenSource = new CancellationTokenSource();
            System.Threading.Tasks.Task.Run(() => _task = _executor.ExecuteTask(task, cancellationTokenSource.Token), cancellationTokenSource.Token);
            return Ok();
        }

        [HttpPost("aborttask")]
        public IActionResult AbortTask()
        {
            cancellationTokenSource.Cancel();
            return Ok();
        }
    }
}
