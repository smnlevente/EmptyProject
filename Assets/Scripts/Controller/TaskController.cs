using System.Collections.Generic;
using MVC;
using TaskManager;

public class TaskController : Controller<TaskController>
{
    private readonly List<Task> tasks = new List<Task>();

    public bool IsWorking
    {
        get
        {
            return this.tasks.Count > 0;
        }
    }

    public Task WaitForNextFrame()
    {
        Task task = new Task();
        this.tasks.Add(task);
        return task.ThenWaitFor(0.01f);
    }

    public Task WaitFor(float seconds)
    {
        Task task = new Task();
        this.tasks.Add(task);
        return task.ThenWaitFor(seconds);
    }

    public Task WaitUntil(UntilTaskPredicate condition)
    {
        Task task = new Task();
        this.tasks.Add(task);
        return task.ThenWaitUntil(condition);
    }

    public void Stop()
    {
        foreach (Task task in this.tasks) 
        {
            task.Stop();
        }

        this.tasks.Clear();
    }

    private void Update()
    {
        if (!this.IsWorking)
        {
            return;
        }

        foreach (Task task in this.tasks.ToArray())
        {
            task.Update();
            if (task.IsComplete)
            {
                this.tasks.Remove(task);
            }
        }
    }
}