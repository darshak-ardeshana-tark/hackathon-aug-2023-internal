using System.Drawing;

namespace Worker.Models
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

        public void ChangeStatusToFailed()
        {
            Status = TaskStatus.Failed;
        }

        public void ChangeStatusToCompleted()
        {
            Status = TaskStatus.Completed;
        }
    }
}
