using System;

[Serializable]
public class Task
{
    public string Name;
    public string Description;
    public bool Completed = false;

    public Task(string name, string description)
    {
        Name = name;
        Description = description;
    }

}

