using System.Collections.Generic;

public class TaskManager {

	public List<Task> PastTasks { get; private set;}
    public Task CurrentTask { get; private set; }
    public bool CurrentIsDirty = true;
    public bool PastIsDirty = true;

    public TaskManager(Task firstTask)
    {
        PastTasks = new List<Task>();
        CurrentTask = firstTask;
    }
}
