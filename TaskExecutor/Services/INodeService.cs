using TaskExecutor.Models;

namespace TaskExecutor.Services
{
    public interface INodeService
    {
        void AddNode(string name, string addres);
        Node RemoveNode(string name);
        void MakeNodeOffline(string name);
        List<Node> GetAllNodes();
    }
}
