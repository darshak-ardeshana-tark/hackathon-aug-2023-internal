using System.Drawing;

namespace TaskExecutor.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public TaskStatus Status { get; set; }

        // REVIEW:
        //   Having to maintain this allocation list at two places - at node, and at task - creates data redundancy. Which comes with its own set of problems.
        //   Better idea would be to have a separate component (class) to track task allocations to the node
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
