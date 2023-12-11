using System.Drawing;

namespace TaskExecutor.Models
{
    public class Task
    {
        private Guid Id;
        private TaskStatus Status;

        // REVIEW:
        //   Having to maintain this allocation list at two places - at node, and at task - creates data redundancy. Which comes with its own set of problems.
        //   Better idea would be to have a separate component (class) to track task allocations to the node

        public Task()
        {
            Id = Guid.NewGuid();
            Status = TaskStatus.Pending;
        }

        public Guid GetId()
        {
            return Id;
        }

        public TaskStatus GetStatus()
        {
            return Status;
        }

        public void SetStatus(TaskStatus status)
        {
            Status = status;
        }

        public void ChangeStatusToRunning()
        {
            Status = TaskStatus.Running;
        }

        public bool Abort()
        {
            return Status == TaskStatus.Abort;
        }

        public bool IsRunning()
        {
            return Status == TaskStatus.Running;
        }
    }
}
