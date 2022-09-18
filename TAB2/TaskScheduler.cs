using TAB2.Api;

namespace TAB2;

public class TaskScheduler : ITaskScheduler
{
    private readonly List<Task> runningTasks = new List<Task>();

    public Task Run(TaskDelegate taskDelegate)
    {
        Task task = Task.Run(() => taskDelegate());
        runningTasks.Add(task);
        return task;
    }

    public void Tick()
    {
        // Remove all finished tasks
        runningTasks.RemoveAll(x =>
        {
            if (x.IsCompleted)
            {
                // If task had exception, rethrow it
                if (x.Exception != null)
                {
                    throw x.Exception;
                }
                
                return true;
            }

            return false;
        });
    }
}