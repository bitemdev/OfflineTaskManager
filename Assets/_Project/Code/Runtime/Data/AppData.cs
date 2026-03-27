using System;
using System.Collections.Generic;

public enum TaskStatus 
{ 
    ToDo, 
    InProgress, 
    Done 
}

[Serializable]
public class AppTask
{
    public string ID;
    public string Title;
    public string Description;
    public TaskStatus Status;

    // Constructor assigns a unique ID automatically when created
    public AppTask(string title, string description = "")
    {
        this.ID = Guid.NewGuid().ToString();
        this.Title = title;
        this.Description = description;
        this.Status = TaskStatus.ToDo; // Default to the first column
    }
}

[Serializable]
public class AppProject
{
    public string ID;
    public string ProjectName;
    public List<AppTask> Tasks;

    public AppProject(string name)
    {
        this.ID = Guid.NewGuid().ToString();
        this.ProjectName = name;
        this.Tasks = new List<AppTask>();
    }
}

[Serializable]
public class AppData
{
    // This is the master wrapper that JsonUtility will serialize
    public List<AppProject> Projects = new List<AppProject>();
}