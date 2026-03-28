using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableTask : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public AppTask TaskData;
    [HideInInspector] public Transform OriginalParent;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descText;
    
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector2 _lockedSize;
    private bool _isDragging = false;
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private Vector3 _targetScale = Vector3.one;
    private Vector2 _lastMousePosition;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void SetupTask(AppTask task)
    {
        TaskData = task;
        _titleText.text = task.Title;
        _descText.text = task.Description;

        UnityEngine.UI.Button deleteBtn = transform.Find("HeaderArea/DeleteButton").GetComponent<UnityEngine.UI.Button>();
        deleteBtn.onClick.AddListener(() => AppManager.Instance.RequestDeleteTask(TaskData, gameObject));
    }
    
    private void Update()
    {
        // If dragging, smoothly lerp position, rotation, and scale
        if (_isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * 25f);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * 15f);
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * 15f);
        }
        else
        {
            // When dropped, the Layout Group handles position, but we need to smoothly reset rotation and scale
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 15f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 15f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;
        _lockedSize = _rectTransform.rect.size;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); 

        // Force the card to stay its original size
        _rectTransform.sizeDelta = _lockedSize; 

        _canvasGroup.blocksRaycasts = false;
        _isDragging = true;
        _targetScale = new Vector3(1.05f, 1.05f, 1.05f);
        _lastMousePosition = eventData.position;
        _targetPosition = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetPosition = Input.mousePosition;
        Vector2 velocity = eventData.position - _lastMousePosition;
        _lastMousePosition = eventData.position;
        float tiltAmount = Mathf.Clamp(velocity.x * -0.2f, -15f, 15f);
        _targetRotation = Quaternion.Euler(0, 0, tiltAmount);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        
        if (transform.parent == transform.root)
        {
            transform.SetParent(OriginalParent);
        }
        
        _isDragging = false;
        _targetScale = Vector3.one;
        _targetRotation = Quaternion.identity;
    }
}