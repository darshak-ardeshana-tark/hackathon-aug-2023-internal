namespace TaskExecutor.DTOs
{
    public class TaskExecutionRequest
    {
        public Guid Id { get; }
        public Models.TaskStatus Status { get; }

        public TaskExecutionRequest(Guid id, Models.TaskStatus status)
        {
            Id = id;
            Status = status;
        }
    }
}
