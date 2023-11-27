using System.Xml.Linq;
using TaskExecutor.Models;
using Task = TaskExecutor.Models.Task;

namespace TaskExecutor.Repository
{
    public class NodeRepository
    {
        private static NodeRepository _instance;
        private readonly List<Node> _nodes;

        private NodeRepository()
        {
            _nodes = new List<Node>();
        }

        public static NodeRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NodeRepository();
            }
            return _instance;
        }

        public void AddNode(Node node)
        {
            Node existingNode = _nodes.FirstOrDefault(_ => _.NodeRegistrationRequest.Name.Equals(node.NodeRegistrationRequest.Name));
            if (existingNode == null)
            {
                _nodes.Add(node);
            }
        }

        public Node RemoveNode(string name)
        {
            Node nodeToRemove = _nodes.FirstOrDefault(_ => _.NodeRegistrationRequest.Name.Equals(name));
            
            // REVIEW:
            //   It's good practice to implement validations to handle bad data. :kudos:
            //   One common practice, for better readability, is to short circuit the method in case of bad input, instead of having implementation inside the if block. Something like:
            //   if (nodeToRemove == null)
            //   {
            //       throw new ArgumentException($"Worker with name {name} does not exist!");  
            //   }
            //   
            //   // <Rest implementation goes here>
            //
            //   Benefit of this approach is, it's easier to read. When reading, the if block, I'd know that we are short circuiting in case of bad data.
            //   Having thrown exception at the end will require reader to figure out when code will, or will not reach to this point. It'd have more cognitive load.

            if (nodeToRemove != null)
            {
                _nodes.Remove(nodeToRemove);
                return nodeToRemove;
            }

            
            throw new InvalidOperationException("Worker with the Name: " + name + " Not Found");
        }

        public void MakeNodeOffline(string name)
        {
            Node nodeToMakeOffline = _nodes.FirstOrDefault(_ => _.NodeRegistrationRequest.Name.Equals(name));
            if (nodeToMakeOffline != null)
            {
                Task task = nodeToMakeOffline.GetRunningTask();
                task.Abort();
                nodeToMakeOffline.ChangeStatusToOffline();
            }

            throw new InvalidOperationException("Worker with the Name: " + name + " Not Found");
        }

        public List<Node> GetAllNodes()
        {
            return _nodes;
        }

        public List<Node> GetAllActiveNodes()
        {
            return _nodes.Where(_ => _.Status == NodeStatus.Available || _.Status == NodeStatus.Busy).ToList();
        }

        public Node GetAvailableNode()
        {
            return _nodes.FirstOrDefault(_ => _.Status == NodeStatus.Available);
        }

        public void ChangeStatusToAvailable(string name)
        {
            var node = _nodes.FirstOrDefault(_ => _.NodeRegistrationRequest.Name.Equals(name));
            node.ChangeStatusToAvailable();
        }
    }
}
