using System;
using System.Net.Http;
using System.Timers;
using Task = TaskExecutor.Models.Task;
using TaskExecutor.Repository.Implementation;
using TaskExecutor.Repository;

namespace TaskExecutor.Services.Implementation
{
    public class HealthChecker : IHealthChecker
    {
        private System.Timers.Timer _timer;
        private readonly INodeRepository _nodeRepository;
        private readonly int healthCheckIntervalInSec = 10;

        public HealthChecker(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public void CheckWorkersHealth()
        {
            _timer = new System.Timers.Timer(healthCheckIntervalInSec * 1000);
            _timer.Elapsed += CheckWorkerHealth;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async void CheckWorkerHealth(object sender, ElapsedEventArgs e)
        {
            var nodes = _nodeRepository.GetAllNodes().Where(_ => _.GetStatus() != Models.NodeStatus.Offline).ToList();
            foreach (var node in nodes)
            {
                Console.WriteLine("Health Check Init");
                var workerHealthCheckUrl = node.GetBaseURL().ToString() + "/api/health";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var response = await client.GetAsync(workerHealthCheckUrl);

                        if (!response.IsSuccessStatusCode)
                        {
                            node.ChangeStatusToOffline();
                            Task task = node.GetRunningTask();
                            task.Abort();
                        }
                    }
                    catch (Exception ex)
                    {
                        node.ChangeStatusToOffline(); //Failed connection
                        Console.WriteLine($"Error checking worker health: {ex.Message}");
                    }
                }
            }
        }
    }
}
