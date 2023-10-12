using System.Timers;
using TaskExecutor.DTOs;
using TaskExecutor.Models;
using TaskExecutor.Repository;

namespace TaskExecutor.Services
{
    public class TaskOrchestrator
    {
        private System.Timers.Timer _timer;
        private readonly int timeToWaitForTaskToCompleteInSec = 5;

        private readonly TaskRepository _taskRepository;
        private readonly NodeRepository _nodeRepository;

        public TaskOrchestrator()
        {
            _taskRepository = TaskRepository.GetInstance();
            _nodeRepository = NodeRepository.GetInstance();
        }

        public async void ExecuteNextTask()
        {
            var nextTaskToExecute = _taskRepository.GetNextTaskToRun();
            var availableNode = _nodeRepository.GetAvailableNode();

            if (nextTaskToExecute != null && availableNode != null)
            {
                TaskDTO taskDTO = new TaskDTO(nextTaskToExecute.Id, nextTaskToExecute.Status);

                availableNode.ChangeStatusToBusy();
                nextTaskToExecute.ChangeStatusToRunning();

                NodeTask nodeTask = new NodeTask(availableNode, nextTaskToExecute);
                availableNode.AddNodeTask(nodeTask);
                nextTaskToExecute.AddNodeTask(nodeTask);

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(availableNode.NodeRegistrationRequest.Address + "/api/Tasks/executetask", taskDTO);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
