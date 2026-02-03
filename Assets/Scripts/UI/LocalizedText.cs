using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;
    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateText();

        // Подписываемся на изменение языка
        // (нужно добавить событие в LocalizationManager)
    }

    private void UpdateText()
    {
        if (textComponent != null && LocalizationManager.Instance != null)
        {
            textComponent.text = LocalizationManager.Instance.GetString(key);
        }
    }

    public void SetKey(string newKey)
    {
        key = newKey;
        UpdateText();
    }
}
