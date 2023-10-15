using System.Timers;

namespace TaskExecutor.Models
{
    public class NodeTask
    {
        public Node Node { get; set; }
        public Task Task { get; set; }

        public NodeTask(Node node, Task task)
        {
            Node = node;
            Task = task;
        }

        public Task GetTask() { return Task; }

        public Node GetNode() { return Node; }
    }
}
