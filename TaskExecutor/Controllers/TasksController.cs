using Microsoft.AspNetCore.Mvc;
using TaskExecutor.DTOs;
using TaskExecutor.Repository;
using TaskExecutor.Services;
using Worker.Models;
using Task = TaskExecutor.Models.Task;

namespace TaskExecutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private TaskRepository _taskRepository;
        private NodeRepository _nodeRepository;

        public TasksController()
        {
            _taskRepository = TaskRepository.GetInstance();
            _nodeRepository = NodeRepository.GetInstance();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTask()
        {
            var task = _taskRepository.AddTask(new Task());
            new TaskOrchestrator().ExecuteNextTask();
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
        public IActionResult ExecutedTaskUpdate([FromBody] TaskResponse taskResponse)
        {
            TaskDTO taskDTO = taskResponse.TaskDTO;
            _taskRepository.UpdateTask(taskDTO.Id, taskDTO.Status);
            _nodeRepository.ChangeStatusToAvailable(taskResponse.NodeName);
            new TaskOrchestrator().ExecuteNextTask();
            return Ok();
        }
    }
}
