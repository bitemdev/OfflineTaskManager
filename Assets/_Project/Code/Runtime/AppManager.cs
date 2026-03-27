using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    [Header("Data")]
    public AppData AppData;
    public AppProject CurrentProject;

    [Header("UI Containers")]
    [SerializeField] private Transform _projectListContainer;
    [SerializeField] private Transform _todoContainer;
    [SerializeField] private Transform _inProgressContainer;
    [SerializeField] private Transform _doneContainer;

    [Header("Prefabs")]
    [SerializeField] private GameObject _projectButtonPrefab;
    [SerializeField] private GameObject _taskCardPrefab;

    private void Awake()
    {
        // Simple singleton pattern so other scripts can access it easily
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadApp();
    }

    public void LoadApp()
    {
        AppData = SaveManager.Load();

        if (AppData.Projects.Count > 0)
        {
            RefreshProjectSidebar();
            LoadProject(AppData.Projects[0]); // Load the first project by default
        }
    }

    public void CreateNewProject(string name)
    {
        AppProject newProject = new AppProject(name);
        
        AppData.Projects.Add(newProject);
        SaveData();
        
        RefreshProjectSidebar();
        LoadProject(newProject);
    }

    public void RefreshProjectSidebar()
    {
        // Clear old buttons
        foreach (Transform child in _projectListContainer)
        {
            Destroy(child.gameObject);
        }

        // Spawn new buttons
        foreach (var project in AppData.Projects)
        {
            GameObject btnObj = Instantiate(_projectButtonPrefab, _projectListContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = project.ProjectName;
            
            // Add click listener to load this project
            btnObj.GetComponent<Button>().onClick.AddListener(() => LoadProject(project));
        }
    }

    public void LoadProject(AppProject project)
    {
        CurrentProject = project;

        // Clear existing cards
        foreach (Transform child in _todoContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _inProgressContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _doneContainer)
        {
            Destroy(child.gameObject);
        }

        // Spawn tasks into their respective columns
        foreach (var task in project.Tasks)
        {
            Transform targetContainer = GetContainerForStatus(task.Status);
            GameObject cardObj = Instantiate(_taskCardPrefab, targetContainer);
            
            // Set UI Text
            TextMeshProUGUI[] texts = cardObj.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = task.Title;
            texts[1].text = task.Description;

            // Pass the data reference to the drag script so it knows what to save later
            cardObj.GetComponent<DraggableTask>().TaskData = task;
        }
    }

    public Transform GetContainerForStatus(TaskStatus status)
    {
        switch (status)
        {
            case TaskStatus.ToDo: return _todoContainer;
            case TaskStatus.InProgress: return _inProgressContainer;
            case TaskStatus.Done: return _doneContainer;
            default: return _todoContainer;
        }
    }

    public void SaveData()
    {
        SaveManager.Save(AppData);
    }
}