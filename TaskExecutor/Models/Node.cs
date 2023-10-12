namespace TaskExecutor.Models
{
    public class Node
    {
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
