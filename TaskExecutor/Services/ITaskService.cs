namespace TaskExecutor.Services
{
    public interface ITaskService
    {
        public Models.Task AddTask();
        public List<Models.Task> GetTaskByStatus(string status);
        public List<Models.Task> GetNextTasks();
        public void ExecutedTask(Guid id, Models.TaskStatus status, string nodeName);
        public Models.Task GetNextTaskToExecute();
    }
}
