namespace TaskExecutor.Models
{
    public class Node
    {

        // [DONE] REVIEW:
        //   Do we need all these members publically exposed and have public Read-Write Access? If not, they should be defined accordingly
        //     e.g. I see you have specific methods to change node's status, which is a good idea. With that, consumer's don't need Write access to the Status property and it can be made read-only
        //     Similarly, you probably don't need to expose NodeTasks publically, at all.

        private string Name;
        private string Address;
        private NodeStatus Status;

        public Node(string name, string address)
        {
            Name = name;
            Address = address;
            Status = NodeStatus.Available;
        }

        public string GetBaseURL()
        {
            return Address;
        }

        public string GetName()
        {
            return Name;
        }

        public NodeStatus GetStatus()
        {
            return Status;
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
    }
}
