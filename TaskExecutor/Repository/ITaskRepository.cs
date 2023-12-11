namespace TaskExecutor.Repository
{
    public interface ITaskRepository
    {
        public Models.Task GetTaskById(Guid id);
        Models.Task AddTask(Models.Task task);
        List<Models.Task> GetNextTasks();
        Models.Task GetNextTaskToExecute();
        List<Models.Task> GetTaskByStatus(Models.TaskStatus status);
        void UpdateTask(Guid id, Models.TaskStatus taskStatus);
    }
}