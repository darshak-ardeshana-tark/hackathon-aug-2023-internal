using System.Drawing;

namespace TaskExecutor.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public TaskStatus Status { get; set; }

        public Task()
        {
            Id = Guid.NewGuid();
            Status = TaskStatus.Pending;
        }

        public void ChangeStatusToRunning()
        {
            Status = TaskStatus.Running;
        }
    }
}
