using Task = Worker.Models.Task;

namespace Worker.Models
{
    public class TaskResponse
    {
        public Task Task { get; set; }
        public string NodeName { get; set; }

        public TaskResponse(Task task, string nodeName)
        {
            Task = task;
            NodeName = nodeName;
        }
    }
}
