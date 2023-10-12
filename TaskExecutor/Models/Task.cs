using System.Drawing;

namespace TaskExecutor.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public TaskStatus Status { get; set; }
        public List<NodeTask> NodeTasks { get; set; }

        public Task()
        {
            Id = Guid.NewGuid();
            Status = TaskStatus.Pending;
            NodeTasks = new List<NodeTask>();
        }

        public void ChangeStatusToRunning()
        {
            Status = TaskStatus.Running;
        }

        public void AddNodeTask(NodeTask nodeTask)
        {
            NodeTasks.Add(nodeTask);
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
