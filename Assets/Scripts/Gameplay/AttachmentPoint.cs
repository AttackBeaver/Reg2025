using UnityEngine;
using UnityEngine.EventSystems;

public class AttachmentPoint : MonoBehaviour
{
    [SerializeField] private string[] allowedAttachmentTypes;
    [SerializeField] private GameObject currentAttachment;

    public bool CanAttach(GameObject attachment)
    {
        if (currentAttachment != null) return false;

        // Проверяем тип предмета (можно добавить теги или компоненты)
        string attachmentType = attachment.GetComponent<AttachmentItemData>().type;

        foreach (string type in allowedAttachmentTypes)
        {
            if (type == attachmentType) return true;
        }

        return false;
    }

    public void Attach(GameObject attachment)
    {
        if (CanAttach(attachment))
        {
            currentAttachment = attachment;
            attachment.transform.SetParent(transform);
            attachment.transform.localPosition = Vector3.zero;
            attachment.transform.localRotation = Quaternion.identity;
        }
    }

    public void Detach()
    {
        if (currentAttachment != null)
        {
            Destroy(currentAttachment);
            currentAttachment = null;
        }
    }

    public bool HasAttachment()
    {
        return currentAttachment != null;
    }

    public GameObject GetAttachment()
    {
        return currentAttachment;
    }
}
