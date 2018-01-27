using System.Collections.Generic;

/// <summary>
/// Manages the players current task and past tasks.
/// </summary>
public class TaskManager {

	public List<Task> PastTasks { get; private set;}
    public Task CurrentTask { get; private set; }
    public bool CurrentIsDirty = true;
    public bool PastIsDirty = false;

    public TaskManager(Task firstTask)
    {
        PastTasks = new List<Task>();
        CurrentTask = firstTask;
    }

    /// <summary>
    /// Set the current task. Will move the current task to the history and alter dirtys.
    /// </summary>
    /// <param name="newTask">The task to assign as the new Current Task.</param>
    /// <param name="moveCurrentToPastTasks">Should the old Current Task be moved to PastTasks?</param>
    public void SetCurrentTask(Task newTask, bool moveCurrentToPastTasks = true)
    {
        if (newTask == null) return;

        if (newTask.Description != string.Empty)
        {
            PastTasks.Add(CurrentTask);
            CurrentTask = newTask;
            PastIsDirty = true;
            CurrentIsDirty = true;
        }
    }
}
