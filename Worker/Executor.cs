using System.IO;
using Worker.Models;
using Task = Worker.Models.Task;

namespace Worker
{
    public class Executor
    {
        public async System.Threading.Tasks.Task<Task> ExecuteTask(Task task)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://meme-api.com/gimme/wholesomememes");
            var meme = await response.Content.ReadFromJsonAsync<Meme>();
            // do nsfw check
            return null;
        }
    }
}
