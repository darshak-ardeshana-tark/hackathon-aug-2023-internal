using System.Xml.Linq;
using TaskExecutor.Models;
using Task = TaskExecutor.Models.Task;

namespace TaskExecutor.Repository.Implementation
{
    public class NodeRepository : INodeRepository
    {
        private readonly List<Node> _nodes;

        public NodeRepository()
        {
            _nodes = new List<Node>();
        }

        public Node GetNodeByName(string name)
        {
            return _nodes.FirstOrDefault(_ => _.GetName().Equals(name));
        }

        public void AddNode(Node node)
        {
            _nodes.Add(node);
        }

        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
        }

        public List<Node> GetAllNodes()
        {
            return _nodes;
        }

        public List<Node> GetAllActiveNodes()
        {
            return _nodes.Where(_ => _.GetStatus() == NodeStatus.Available || _.GetStatus() == NodeStatus.Busy).ToList();
        }

        public Node GetAvailableNode()
        {
            return _nodes.FirstOrDefault(_ => _.GetStatus() == NodeStatus.Available);
        }

        public void ChangeStatusToAvailable(string name)
        {
            var node = _nodes.FirstOrDefault(_ => _.GetName().Equals(name));
            node.ChangeStatusToAvailable();
        }
    }
}
