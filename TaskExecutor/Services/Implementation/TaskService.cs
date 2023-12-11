using System.ComponentModel;
using TaskExecutor.Models;
using TaskExecutor.Repository;
using TaskExecutor.Repository.Implementation;

namespace TaskExecutor.Services.Implementation
{
    public class TaskService : ITaskService
    {
        private ITaskRepository _taskRepository;
        private INodeRepository _nodeRepository;
        private ITaskOrchestrator _taskOrchestrator;

        public TaskService(ITaskRepository taskRepository, INodeRepository nodeRepository, ITaskOrchestrator taskOrchestrator)
        {
            _taskRepository = taskRepository;
            _nodeRepository = nodeRepository;
            _taskOrchestrator = taskOrchestrator;
        }

        public Models.Task AddTask()
        {
            var task = new Models.Task();
            _taskOrchestrator.ExecuteNextTask();
            return task;
        }

        public List<Models.Task> GetTaskByStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("Status cannot be Null");
            }

            Models.TaskStatus statusAsEnum;
            if (!Models.TaskStatus.TryParse(status, out statusAsEnum))
            {
                throw new InvalidEnumArgumentException("Invalid Status passed");
            }

            return _taskRepository.GetTaskByStatus(statusAsEnum);
        }

        public List<Models.Task> GetNextTasks()
        {
            return _taskRepository.GetNextTasks();
        }

        public void ExecutedTask(Guid id, Models.TaskStatus status, string nodeName)
        {
            UpdateTask(id, status);
            _nodeRepository.ChangeStatusToAvailable(nodeName);
            _taskOrchestrator.ExecuteNextTask();
        }

        private void UpdateTask(Guid id, Models.TaskStatus status)
        {
            var taskToUpdate = _taskRepository.GetTaskById(id);

            if (taskToUpdate == null)
            {
                throw new InvalidDataException("Task with Id: " + id + " does not exists.");
            }

            taskToUpdate.SetStatus(status);
        }

        public Models.Task GetNextTaskToExecute()
        {
            return _taskRepository.GetNextTaskToExecute();
        }
    }
}
