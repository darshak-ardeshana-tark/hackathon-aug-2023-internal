using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor.DTOs
{
    public class TaskDTO
    {
        public Guid Id { get; set; }
        public TaskStatus Status { get; set; }

        public TaskDTO(Guid id, TaskStatus status)
        {
            Id = id;
            Status = status;
        }
    }
}
