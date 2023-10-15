using System.Threading;
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

        public async System.Threading.Tasks.Task ExecuteTask(Task task, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage memeResponse = await client.GetAsync("https://meme-api.com/gimme/wholesomememes", cancellationToken);
            memeResponse.EnsureSuccessStatusCode();

            var meme = await memeResponse.Content.ReadFromJsonAsync<Meme>(options: null, cancellationToken);
            if (meme == null || meme.NSFW)
            {
                task.ChangeStatusToFailed();
            }
            else
            {
                await DownloadImage(meme.URL, cancellationToken);
                task.ChangeStatusToCompleted();
            }

            TaskResponse executedTask = new TaskResponse(task, _workerInfo.Name);
            HttpResponseMessage updateTaskResponse = await client.PutAsJsonAsync("http://localhost:5000/api/Tasks/executed", executedTask, cancellationToken);
            updateTaskResponse.EnsureSuccessStatusCode();
        }

        public async System.Threading.Tasks.Task DownloadImage(string imageUrl, CancellationToken cancellationToken)
        {
            var fileName = GetFileName(imageUrl);
            HttpClient httpClient = new HttpClient();
            byte[] fileBytes = await httpClient.GetByteArrayAsync(imageUrl, cancellationToken);
            await File.WriteAllBytesAsync(_workerInfo.WorkDir + "/" + fileName, fileBytes, cancellationToken);
        }

        private string GetFileName(string url)
        {
            return url.Split('/').LastOrDefault();
        }
    }
}
