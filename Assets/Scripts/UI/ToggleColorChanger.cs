using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChanger : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Toggle toggle;

    private void Start()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        UpdateToggleAppearance(toggle.isOn);
        toggle.onValueChanged.AddListener(UpdateToggleAppearance);
    }

    private void UpdateToggleAppearance(bool isOn)
    {
        if (backgroundImage != null)
        {
            backgroundImage.sprite = isOn ? onSprite : offSprite;
        }
    }

    private void OnDestroy()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(UpdateToggleAppearance);
    }
}
