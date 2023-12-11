using TaskExecutor.Models;

namespace TaskExecutor.Services
{
    public interface INodeTaskService
    {
        void AllocateTaskToNode(Node node, Models.Task task);
    }
}
