using UnityEngine;
using UnityEngine.EventSystems;

public class VehicleAssemblyController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
private VehicleAssemblyUI assemblyUI;
    private GameObject currentDraggedItem;
    private Vector3 originalPosition;
    private Transform originalParent;
    
    private AttachmentPoint[] attachmentPoints;
    
    public void Initialize(VehicleAssemblyUI ui)
    {
        assemblyUI = ui;
        
        // Находим все точки крепления
        attachmentPoints = GetComponentsInChildren<AttachmentPoint>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Находим предмет под курсором
        if (eventData.pointerEnter != null)
        {
            AttachmentItemUI attachmentUI = eventData.pointerEnter.GetComponent<AttachmentItemUI>();
            if (attachmentUI != null)
            {
                currentDraggedItem = attachmentUI.GetAttachmentPrefab();
                originalPosition = attachmentUI.transform.position;
                originalParent = attachmentUI.transform.parent;
                
                // Создаем визуализацию перетаскивания
                GameObject dragVisual = Instantiate(currentDraggedItem, transform);
                dragVisual.transform.position = eventData.position;
                // Настроить Layer и т.д.
            }
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (currentDraggedItem != null)
        {
            // Обновляем позицию перетаскиваемого объекта
            Vector3 screenPoint = new Vector3(eventData.position.x, eventData.position.y, 10);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
            // Обновляем позицию визуализации
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDraggedItem != null)
        {
            // Проверяем, попал ли предмет на точку крепления
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                AttachmentPoint point = hit.collider.GetComponent<AttachmentPoint>();
                if (point != null && point.CanAttach(currentDraggedItem))
                {
                    // Прикрепляем предмет
                    GameObject attachment = Instantiate(currentDraggedItem, point.transform);
                    attachment.transform.localPosition = Vector3.zero;
                    
                    // Сообщаем UI о размещении
                    assemblyUI.OnAttachmentPlaced(attachment);
                }
            }
            
            // Возвращаем предмет на место или уничтожаем визуализацию
            currentDraggedItem = null;
        }
    }
}
