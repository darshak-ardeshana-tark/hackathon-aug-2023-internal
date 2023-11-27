namespace TaskExecutor.Models
{
    public class Node
    {

        // REVIEW:
        //   Do we need all these members publically exposed and have public Read-Write Access? If not, they should be defined accordingly
        //     e.g. I see you have specific methods to change node's status, which is a good idea. With that, consumer's don't need Write access to the Status property and it can be made read-only
        //     Similarly, you probably don't need to expose NodeTasks publically, at all.
        
        public NodeRegistrationRequest NodeRegistrationRequest { get; set; }
        public NodeStatus Status { get; set; }
        public List<NodeTask> NodeTasks { get; set; }

        public Node(NodeRegistrationRequest nodeRegistrationRequest)
        {
            NodeRegistrationRequest = nodeRegistrationRequest;
            Status = NodeStatus.Available;
            NodeTasks = new List<NodeTask>();
        }

        public void ChangeStatusToOffline()
        {
            Status = NodeStatus.Offline;
        }

        public void ChangeStatusToBusy()
        {
            Status = NodeStatus.Busy;
        }

        public void ChangeStatusToAvailable()
        {
            Status = NodeStatus.Available;
        }

        public void AddNodeTask(NodeTask nodeTask)
        {
            NodeTasks.Add(nodeTask);
        }

        public Task GetRunningTask()
        {
            return NodeTasks.Where(_ => _.GetTask().IsRunning()).FirstOrDefault().GetTask();
        }
    }
}
