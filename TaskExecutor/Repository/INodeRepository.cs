using TaskExecutor.Models;

namespace TaskExecutor.Repository
{
    public interface INodeRepository
    {
        Node GetNodeByName(string name);
        void AddNode(Node node);
        void ChangeStatusToAvailable(string name);
        List<Node> GetAllActiveNodes();
        List<Node> GetAllNodes();
        Node GetAvailableNode();
        void RemoveNode(Node node);
    }
}