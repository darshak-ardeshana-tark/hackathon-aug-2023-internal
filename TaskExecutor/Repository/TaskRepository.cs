using Task = TaskExecutor.Models.Task;

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
            return _task.FirstOrDefault(_ => _.Status == Models.TaskStatus.Pending);
        }

        public List<Task> GetNextSetOfTasksToRun()
        {
            var nodeCount = NodeRepository.GetInstance().GetAllActiveNodes().Count();

            return _task.Where(_ => _.Status == Models.TaskStatus.Pending).Take(nodeCount).ToList();
        }

        public List<Task> GetTaskByStatus(string status)
        {
            return _task.Where(_ => _.Status.Equals(status)).ToList();
        }

        public void UpdateTask(Task task)
        {
            var taskToUpdate = _task.FirstOrDefault(_ => _.Id.Equals(task.Id));
            if (taskToUpdate != null)
            {
                taskToUpdate.Status = task.Status;
            }
            else
            {
                throw new InvalidDataException("Task with Id: " + task.Id + " does not exists.");
            }
        }
    }
}
