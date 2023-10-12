using Task = TaskExecutor.Models.Task;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor.Repository
{
    public class TaskRepository
    {
        private static TaskRepository _instance;
        private readonly List<Task> _task;

        private TaskRepository()
        {
            _task = new List<Task>();
        }

        public static TaskRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TaskRepository();
            }
            return _instance;
        }

        public Task AddTask(Task task)
        {
            _task.Add(task);
            return task;
        }

        public Task GetNextTaskToRun()
        {
            return _task.FirstOrDefault(_ => _.Status == Models.TaskStatus.Pending || _.Status == Models.TaskStatus.Abort);
        }

        public List<Task> GetNextSetOfTasksToRun()
        {
            return _task.Where(_ => _.Status == Models.TaskStatus.Pending).ToList();
        }

        public List<Task> GetTaskByStatus(string status)
        {
            return _task.Where(_ => _.Status.ToString().Equals(status)).ToList();
        }

        public void UpdateTask(Guid id, TaskStatus taskStatus)
        {
            var taskToUpdate = _task.FirstOrDefault(_ => _.Id.Equals(id));
            if (taskToUpdate != null)
            {
                taskToUpdate.Status = taskStatus;
            }
            else
            {
                throw new InvalidDataException("Task with Id: " + id + " does not exists.");
            }
        }
    }
}
