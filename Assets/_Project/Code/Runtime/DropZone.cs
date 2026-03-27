using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private TaskStatus _columnStatus; 
    [SerializeField] private Transform _contentContainer; 

    public void OnDrop(PointerEventData eventData)
    {
        // Check if the thing being dropped has the DraggableTask script
        if (eventData.pointerDrag != null)
        {
            DraggableTask draggedTask = eventData.pointerDrag.GetComponent<DraggableTask>();
            if (draggedTask != null)
            {
                // Re-parent the card to this column's content container
                draggedTask.transform.SetParent(_contentContainer);
                
                // Update the original parent so it doesn't snap back to the old column
                draggedTask.OriginalParent = _contentContainer;
                
                draggedTask.TaskData.Status = _columnStatus;
                AppManager.Instance.SaveData();

                Debug.Log($"Task moved to {_columnStatus}!");
            }
        }
    }
}