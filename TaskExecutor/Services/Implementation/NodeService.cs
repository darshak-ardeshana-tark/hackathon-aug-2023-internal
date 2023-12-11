using System.Xml.Linq;
using TaskExecutor.Models;
using TaskExecutor.Repository;

namespace TaskExecutor.Services.Implementation
{
    public class NodeService : INodeService
    {
        private INodeRepository _nodeRepository;
        private ITaskOrchestrator _taskOrchestrator;

        public NodeService(INodeRepository nodeRepository, ITaskOrchestrator taskOrchestrator)
        {
            _nodeRepository = nodeRepository;
            _taskOrchestrator = taskOrchestrator;
        }

        public void AddNode(string name, string address)
        {
            Node existingNode = _nodeRepository.GetNodeByName(name);
            if (existingNode != null)
            {
                throw new InvalidOperationException("Worker with the Name: " + name + " Already Exists.");
            }

            Node newNode = new Node(name, address);
            _nodeRepository.AddNode(newNode);

            _taskOrchestrator.ExecuteNextTask();
        }

        public Node RemoveNode(string name)
        {
            Node nodeToRemove = _nodeRepository.GetNodeByName(name);
            if (nodeToRemove == null)
            {
                throw new InvalidOperationException("Worker with the Name: " + name + " Not Found");
            }

            _nodeRepository.RemoveNode(nodeToRemove);
            return nodeToRemove;
        }

        public void MakeNodeOffline(string name)
        {
            Node nodeToMakeOffline = _nodeRepository.GetNodeByName(name);
            if (nodeToMakeOffline == null)
            {
                throw new InvalidOperationException("Worker with the Name: " + name + " Not Found");
            }

            Models.Task task = nodeToMakeOffline.GetRunningTask();
            task.Abort();
            nodeToMakeOffline.ChangeStatusToOffline();
            _taskOrchestrator.ExecuteNextTask();
        }

        public List<Node> GetAllNodes()
        {
            return _nodeRepository.GetAllNodes();
        }
    }
}
