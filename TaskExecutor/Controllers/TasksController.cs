using Microsoft.AspNetCore.Mvc;
using TaskExecutor.DTOs;
using TaskExecutor.Repository;
using TaskExecutor.Services;

namespace TaskExecutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTask()
        {
            var task = _taskService.AddTask();
            return Ok(task);
        }

        [HttpGet]
        [Route("status/{status}")]
        public IActionResult GetTaskByStatus(string status)
        {
            var task = _taskService.GetTaskByStatus(status);
            return Ok(task);
        }

        [HttpGet]
        [Route("next-tasks")]
        public IActionResult GetNextTasks()
        {
            var tasks = _taskService.GetNextTasks();
            return Ok(tasks);
        }

        [HttpPut]
        [Route("executed")]
        public IActionResult ExecutedTaskUpdate([FromBody] TaskExecutionResponse taskResponse)
        {
            _taskService.ExecutedTask(taskResponse.Id, taskResponse.Status, taskResponse.NodeName);
            return Ok();
        }
    }
}
