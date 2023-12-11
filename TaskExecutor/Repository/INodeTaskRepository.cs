using TaskExecutor.Models;

namespace TaskExecutor.Repository
{
    public interface INodeTaskRepository
    {
        void AllocateTaskToNode(Node node, Models.Task task);
        List<Node> GetNodesForTask(Models.Task task);
        List<Models.Task> GetTasksForNode(Node node);
    }
}