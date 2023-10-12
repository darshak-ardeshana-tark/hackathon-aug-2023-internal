using System.Timers;

namespace TaskExecutor.Models
{
    public class NodeTask
    {
        private System.Timers.Timer _timer;
        private readonly int timeToWaitForTaskToCompleteInSec = 5;

        public Node Node { get; set; }
        public Task Task { get; set; }

        public NodeTask(Node node, Task task)
        {
            Node = node;
            Task = task;
            SetTimeout();
        }

        public Task GetTask() { return Task; }

        public Node GetNode() { return Node; }

        public void SetTimeout()
        {
            _timer = new System.Timers.Timer(timeToWaitForTaskToCompleteInSec * 1000);
            _timer.Elapsed += InititateAbortTask;
            _timer.Enabled = true;
        }

        private async void InititateAbortTask(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            if (Task.IsRunning())
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(Node.NodeRegistrationRequest.Address + "/api/Tasks/aborttask", "");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
