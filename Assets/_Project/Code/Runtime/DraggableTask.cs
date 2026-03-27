using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableTask : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public AppTask TaskData;
    [HideInInspector] public Transform OriginalParent;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Remember where it came from in case it is dropped in an invalid spot
        OriginalParent = transform.parent;

        // Move the card to the very top level of the Canvas so it doesn't get clipped by the ScrollView
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); // Renders on top of everything else

        // Turn off raycasts so the DropZone behind the card can detect the mouse
        _canvasGroup.blocksRaycasts = false;
        
        _canvasGroup.alpha = 0.8f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Snap the card to the mouse position
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable raycasts and reset alpha
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        // If the parent is still the root canvas, it means it wasn't dropped on a valid DropZone.
        // Snap it back to its original column.
        if (transform.parent == transform.root)
        {
            transform.SetParent(OriginalParent);
        }
    }
}