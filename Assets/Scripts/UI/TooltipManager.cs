using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private float delayTime = 2f;
    
    private GameObject currentTooltip;
    private float hoverTime;
    private bool isHovering;
    private string currentTooltipText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isHovering)
        {
            hoverTime += Time.deltaTime;
            
            if (hoverTime >= delayTime && currentTooltip == null)
            {
                ShowTooltip();
            }
            
            if (currentTooltip != null)
            {
                // Позиционируем подсказку рядом с курсором
                Vector2 mousePos = Input.mousePosition;
                currentTooltip.transform.position = new Vector3(
                    mousePos.x + 20, 
                    mousePos.y - 20, 
                    0);
            }
        }
    }

    public void StartHover(string tooltipText)
    {
        isHovering = true;
        currentTooltipText = tooltipText;
        hoverTime = 0f;
    }

    public void EndHover()
    {
        isHovering = false;
        hoverTime = 0f;
        HideTooltip();
    }

    private void ShowTooltip()
    {
        if (tooltipPrefab != null)
        {
            currentTooltip = Instantiate(tooltipPrefab, transform);
            TextMeshProUGUI textComponent = currentTooltip.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = currentTooltipText;
            }
        }
    }

    private void HideTooltip()
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
        }
    }
}
