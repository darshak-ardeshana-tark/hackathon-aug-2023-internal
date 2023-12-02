using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Task = Worker.Models.Task;

namespace Worker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        // REVIEW:
        //   While this implementation will work with assumptions provided in the problem statement, i.e. one task at a time on the agent, 
        //    it would not work if we change mind to support multiple parallel jobs on an agent. Try to solve that as a next iteration.
        
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
