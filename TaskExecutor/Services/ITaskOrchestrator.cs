using TaskExecutor.Models;

namespace TaskExecutor.Services
{
    public interface ITaskOrchestrator
    {
        void ExecuteNextTask();
        void SetTimeout(Models.Task task, Node node);
    }
}