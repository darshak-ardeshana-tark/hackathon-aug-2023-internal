using TaskExecutor.Repository;

namespace TaskExecutor
{
    public class TaskExecutor
    {
        private readonly TaskRepository _taskRepository;
        private readonly NodeRepository _nodeRepository;

        public TaskExecutor()
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
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(availableNode.NodeRegistrationRequest.Address + "/executetask", nextTaskToExecute);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
