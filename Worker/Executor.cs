using System.IO;
using System.Runtime.CompilerServices;
using Worker.Models;
using Task = Worker.Models.Task;

namespace Worker
{
    public class Executor
    {
        private readonly WorkerInfo _workerInfo;

        public Executor(WorkerInfo workerInfo)
        {
            _workerInfo = workerInfo;
        }

        public async void ExecuteTask(Task task)
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://meme-api.com/gimme/wholesomememes");
            var meme = await response.Content.ReadFromJsonAsync<Meme>();
            if (meme == null || meme.NSFW)
            {
                task.Status = Models.TaskStatus.Failed;
            }
            else
            {
                // Download Meme
                task.Status = Models.TaskStatus.Completed;
                Console.WriteLine(_workerInfo.Name);
            }

            client.PutAsJsonAsync("https://localhost:5000/executed", task);
        }
    }
}
