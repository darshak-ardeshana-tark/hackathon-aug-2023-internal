namespace TaskExecutor.DTOs
{
    public class TaskExecutionResponse
    {
        public Guid Id { get; }
        public Models.TaskStatus Status { get; }
        public string NodeName { get; }

        public TaskExecutionResponse(Guid id, Models.TaskStatus status, string nodeName)
        {
            Id = id;
            Status = status;
            NodeName = nodeName;
        }
    }
}
