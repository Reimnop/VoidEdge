namespace TAB2.Api;

public delegate Task TaskDelegate();

public interface ITaskScheduler
{
    Task Run(TaskDelegate taskDelegate);
}