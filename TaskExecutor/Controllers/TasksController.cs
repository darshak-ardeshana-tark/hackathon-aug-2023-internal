using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TaskExecutor.Models;
using TaskExecutor.Repository;
using Task = TaskExecutor.Models.Task;

namespace TaskExecutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private TaskRepository _taskRepository;

        public TasksController()
        {
            _taskRepository = TaskRepository.GetInstance();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTask()
        {
            var task = _taskRepository.AddTask(new Task());
            new TaskExecutor().ExecuteNextTask();
            return Ok(task);
        }

        [HttpGet]
        [Route("status/{status}")]
        public IActionResult GetTaskByStatus(string status)
        {
            if (status == null)
            {
                return BadRequest("Status cannot be Null.");
            }

            var task = _taskRepository.GetTaskByStatus(status);
            return Ok(task);
        }

        [HttpGet]
        [Route("nextsettorun")]
        public IActionResult GetNextSetOfTaskToRun()
        {
            var tasks = _taskRepository.GetNextSetOfTasksToRun();
            return Ok(tasks);
        }

        [HttpPut]
        [Route("executed")]
        public IActionResult ExecutedTaskUpdate([FromBody] Task task)
        {
            _taskRepository.UpdateTask(task);
            new TaskExecutor().ExecuteNextTask();
            return Ok();
        }
    }
}
