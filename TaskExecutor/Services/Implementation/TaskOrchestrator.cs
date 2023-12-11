using System.Timers;
using TaskExecutor.DTOs;
using TaskExecutor.Models;
using TaskExecutor.Repository;
using TaskExecutor.Repository.Implementation;

namespace TaskExecutor.Services.Implementation
{
    public class TaskOrchestrator : ITaskOrchestrator
    {
        private System.Timers.Timer _timer;
        private readonly int timeToWaitForTaskToCompleteInSec = 5;

        private ITaskRepository _taskRepository;
        private ITaskService _taskService;
        private INodeRepository _nodeRepository;

        private INodeTaskService _nodeTaskService;

        public TaskOrchestrator(ITaskRepository taskRepository, ITaskService taskService, INodeRepository nodeRepository, INodeTaskService nodeTaskService)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _nodeRepository = nodeRepository;
            _nodeTaskService = nodeTaskService;
        }

        // REVIEW:
        //   async tasks should always have `Task` or `Task<>` as return type. Having them as void would create issues where parent object will get disposed before completion of this method.

        public async void ExecuteNextTask()
        {
            var nextTaskToExecute = _taskService.GetNextTaskToExecute();
            var availableNode = _nodeRepository.GetAvailableNode();

            if (nextTaskToExecute == null || availableNode == null)
            {
                return;
            }

            TaskExecutionRequest taskRequestObj = new TaskExecutionRequest(nextTaskToExecute.GetId(), nextTaskToExecute.GetStatus());

            availableNode.ChangeStatusToBusy();
            nextTaskToExecute.ChangeStatusToRunning();

            _nodeTaskService.AllocateTaskToNode(availableNode, nextTaskToExecute);

            // REVIEW:
            //   Create a separate method that should take care of invoking API for execution and timeout. Would make the code even more readable.
            SetTimeout(nextTaskToExecute, availableNode);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(availableNode.GetBaseURL() + "/api/Tasks/executetask", taskRequestObj);
        }



        // REVIEW:
        //   I see an issue with this timeout+abort mechanism:
        //     - When multiple tasks are scheduled, and more than one task timesout, only one of the timer would stop and rest will continue calling abort endpoint every 5 seconds
        public void SetTimeout(Models.Task task, Node node)
        {
            _timer = new System.Timers.Timer(timeToWaitForTaskToCompleteInSec * 1000);
            _timer.Elapsed += (sender, e) => InititateAbortTask(task, node);
            _timer.Enabled = true;
        }

        private async void InititateAbortTask(Models.Task task, Node node)
        {
            _timer.Enabled = false;
            if (task.IsRunning())
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(node.GetBaseURL() + "/api/Tasks/aborttask", "");
                response.EnsureSuccessStatusCode();

                task.Abort();
                node.ChangeStatusToAvailable();
                ExecuteNextTask();
            }
        }
    }
}
