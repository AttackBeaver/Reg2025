using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttachmentItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject attachmentPrefab;

    private Vector3 originalPosition;
    private Transform originalParent;
    private GameObject dragVisual;

    private int itemIndex;

    public void Initialize(GameObject prefab, int index)
    {
        attachmentPrefab = prefab;
        itemIndex = index;

        // Устанавливаем иконку
        // Можно добавить компонент для получения иконки из префаба
    }

    public GameObject GetAttachmentPrefab()
    {
        return attachmentPrefab;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;

        // Создаем визуализацию для перетаскивания
        dragVisual = new GameObject("DragVisual");
        dragVisual.transform.SetParent(transform.root);

        Image visualImage = dragVisual.AddComponent<Image>();
        visualImage.sprite = itemIcon.sprite;
        visualImage.raycastTarget = false;

        RectTransform rectTransform = dragVisual.GetComponent<RectTransform>();
        rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;

        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Возвращаем на место
        transform.SetParent(originalParent);
        transform.position = originalPosition;

        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }

        // Проверяем, был ли предмет помещен на транспорт
        // Это обрабатывается в VehicleAssemblyController
    }
}
