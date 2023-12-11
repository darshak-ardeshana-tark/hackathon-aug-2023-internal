using Task = TaskExecutor.Models.Task;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor.Repository.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<Task> _task;

        public TaskRepository()
        {
            _task = new List<Task>();
        }

        public Task GetTaskById(Guid id)
        {
            return _task.FirstOrDefault(_ => _.GetId() == id);
        }

        public Task AddTask(Task task)
        {
            _task.Add(task);
            return task;
        }

        public Task GetNextTaskToExecute()
        {
            return _task.FirstOrDefault(_ => _.GetStatus() == TaskStatus.Pending || _.GetStatus() == TaskStatus.Abort);
        }

        public List<Task> GetNextTasks()
        {
            return _task.Where(_ => _.GetStatus() == TaskStatus.Pending || _.GetStatus() == TaskStatus.Abort).ToList();
        }

        public List<Task> GetTaskByStatus(TaskStatus status)
        {
            return _task.Where(_ => _.GetStatus() == status).ToList();
        }

        public void UpdateTask(Guid id, TaskStatus taskStatus)
        {
            var taskToUpdate = _task.FirstOrDefault(_ => _.GetId().Equals(id));
            
            if (taskToUpdate == null)
            {
                throw new InvalidDataException("Task with Id: " + id + " does not exists.");
            }
            
            taskToUpdate.SetStatus(taskStatus);
        }
    }
}
