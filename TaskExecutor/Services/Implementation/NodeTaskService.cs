using TaskExecutor.Models;
using TaskExecutor.Repository;
using TaskExecutor.Repository.Implementation;

namespace TaskExecutor.Services.Implementation
{
    public class NodeTaskService : INodeTaskService
    {
        private INodeTaskRepository _nodeTaskRepository;

        public NodeTaskService(INodeTaskRepository nodeTaskRepository)
        {
            _nodeTaskRepository = nodeTaskRepository;
        }

        public void AllocateTaskToNode(Node node, Models.Task task)
        {
            _nodeTaskRepository.AllocateTaskToNode(node, task);
        }
    }
}
