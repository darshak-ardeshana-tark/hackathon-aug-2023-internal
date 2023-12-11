using TaskExecutor.Models;

namespace TaskExecutor.Repository.Implementation
{
    public class NodeTaskRepository : INodeTaskRepository
    {
        private Dictionary<Node, List<Models.Task>> nodeTaskAllocations = new Dictionary<Node, List<Models.Task>>();
        private Dictionary<Models.Task, List<Node>> taskNodeAllocations = new Dictionary<Models.Task, List<Node>>();

        public void AllocateTaskToNode(Node node, Models.Task task)
        {
            if (!nodeTaskAllocations.ContainsKey(node))
            {
                nodeTaskAllocations[node] = new List<Models.Task>();
            }
            nodeTaskAllocations[node].Add(task);

            if (!taskNodeAllocations.ContainsKey(task))
            {
                taskNodeAllocations[task] = new List<Node>();
            }
            taskNodeAllocations[task].Add(node);
        }

        public List<Models.Task> GetTasksForNode(Node node)
        {
            return nodeTaskAllocations.ContainsKey(node) ? nodeTaskAllocations[node] : new List<Models.Task>();
        }

        public List<Node> GetNodesForTask(Models.Task task)
        {
            return taskNodeAllocations.ContainsKey(task) ? taskNodeAllocations[task] : new List<Node>();
        }
    }
}
