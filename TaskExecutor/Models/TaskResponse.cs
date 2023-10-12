using TaskExecutor.DTOs;
using Task = TaskExecutor.Models.Task;

namespace Worker.Models
{
    public class TaskResponse
    {
        public TaskDTO TaskDTO { get; set; }
        public string NodeName { get; set; }

        public TaskResponse(TaskDTO taskDTO, string nodeName)
        {
            TaskDTO = taskDTO;
            NodeName = nodeName;
        }
    }
}
