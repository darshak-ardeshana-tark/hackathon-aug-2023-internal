using System.Timers;
using System.Xml.Linq;
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

        // REVIEW:
        //   async tasks should always have `Task` or `Task<>` as return type. Having them as void would create issues where parent object will get disposed before completion of this method.

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

                // REVIEW:
                //   Create a separate method that should take care of invoking API for execution and timeout. Would make the code even more readable.
                SetTimeout(nextTaskToExecute, availableNode);

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(availableNode.NodeRegistrationRequest.Address + "/api/Tasks/executetask", taskDTO);
                response.EnsureSuccessStatusCode();
            }
        }



        // REVIEW:
        //   I see an issue with this timeout+abort mechanism:
        //     - When multiple tasks are scheduled, and more than one task timesout, only one of the timer would stop and rest will continue calling abort endpoint every 5 seconds
        public void SetTimeout(Models.Task task, Node node)
        {
            _timer = new System.Timers.Timer(timeToWaitForTaskToCompleteInSec * 1000);
            _timer.Elapsed += (sender, e) => InititateAbortTask(sender, e, task, node);
            _timer.Enabled = true;
        }

        private async void InititateAbortTask(object sender, ElapsedEventArgs e, Models.Task task, Node node)
        {
            _timer.Enabled = false;
            if (task.IsRunning())
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(node.NodeRegistrationRequest.Address + "/api/Tasks/aborttask", "");
                response.EnsureSuccessStatusCode();

                task.Abort();
                node.ChangeStatusToAvailable();
                ExecuteNextTask();
            }
        }
    }
}
