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
    
    [Header("Task Creation UI")]
    [SerializeField] private GameObject _newTaskModal;
    [SerializeField] private TMP_InputField _titleInput;
    [SerializeField] private TMP_InputField _descInput;
    [SerializeField] private GameObject _addTaskButton;
    
    [Header("Project Creation UI")]
    [SerializeField] private GameObject _newProjectModal;
    [SerializeField] private TMP_InputField _projectNameInput;
    
    [Header("Confirmation Modal UI")]
    [SerializeField] private GameObject _confirmModal;
    [SerializeField] private TextMeshProUGUI _confirmMessageText;
    private System.Action _onConfirmAction;

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

    public void OpenNewProjectModal()
    {
        _projectNameInput.text = ""; // Clear out old text
        _newProjectModal.SetActive(true);
    }

    public void CloseNewProjectModal()
    {
        _newProjectModal.SetActive(false);
    }

    public void ConfirmCreateProject()
    {
        // Don't allow blank project names
        if (string.IsNullOrWhiteSpace(_projectNameInput.text))
        {
            return;
        }

        // Create the new project with the typed name
        AppProject newProject = new AppProject(_projectNameInput.text);
        
        AppData.Projects.Add(newProject);
        SaveData();

        RefreshProjectSidebar();
        LoadProject(newProject); // Automatically switch to the new project
        CloseNewProjectModal();
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
            
            UnityEngine.UI.Button deleteBtn = btnObj.transform.Find("DeleteButton").GetComponent<UnityEngine.UI.Button>();
            deleteBtn.onClick.AddListener(() => RequestDeleteProject(project));
        }
    }

    public void LoadProject(AppProject project)
    {
        CurrentProject = project;
        _addTaskButton.SetActive(true);

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
            cardObj.GetComponent<DraggableTask>().SetupTask(task);
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
    
    public void OpenNewTaskModal()
    {
        _titleInput.text = "";
        _descInput.text = "";
        _newTaskModal.SetActive(true);
    }

    public void CloseNewTaskModal()
    {
        _newTaskModal.SetActive(false);
    }

    public void ConfirmCreateTask()
    {
        // Don't create empty tasks
        if (string.IsNullOrWhiteSpace(_titleInput.text) || CurrentProject == null)
        {
            return;
        }

        AppTask newTask = new AppTask(_titleInput.text, _descInput.text);
        CurrentProject.Tasks.Add(newTask);
        SaveData();

        // Spawn the card visually in the To Do column
        GameObject cardObj = Instantiate(_taskCardPrefab, _todoContainer);
        
        cardObj.GetComponent<DraggableTask>().SetupTask(newTask);

        CloseNewTaskModal();
    }

    public void DeleteTask(AppTask task, GameObject uiCard)
    {
        if (CurrentProject != null)
        {
            CurrentProject.Tasks.Remove(task);
            SaveData();
            Destroy(uiCard); // Remove it from the screen
        }
    }
    
    public void ShowConfirmation(string message, System.Action actionToConfirm)
    {
        _confirmMessageText.text = message;
        _onConfirmAction = actionToConfirm;
        _confirmModal.SetActive(true);
    }

    public void ExecuteConfirm()
    {
        _onConfirmAction?.Invoke();
        _confirmModal.SetActive(false);
    }

    public void CancelConfirm()
    {
        _confirmModal.SetActive(false);
    }

    public void RequestDeleteTask(AppTask task, GameObject uiCard)
    {
        ShowConfirmation($"Are you sure you want to delete the task '{task.Title}'?", () => 
        {
            CurrentProject.Tasks.Remove(task);
            SaveData();
            Destroy(uiCard);
        });
    }

    public void RequestDeleteProject(AppProject project)
    {
        ShowConfirmation($"Are you sure you want to delete project '{project.ProjectName}' and ALL its tasks?", () => 
        {
            AppData.Projects.Remove(project);
            SaveData();
            RefreshProjectSidebar();
            
            // Load the first available project, or clear the board if none are left
            if (AppData.Projects.Count > 0)
            {
                LoadProject(AppData.Projects[0]);
            }
            else
            {
                ClearBoard();
            } 
        });
    }

    private void ClearBoard()
    {
        CurrentProject = null;
        _addTaskButton.SetActive(false);
        
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
    }
}