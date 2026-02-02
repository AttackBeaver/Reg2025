using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsWindowPrefab;
    [SerializeField] private GameObject coinShopPrefab;
    [SerializeField] private GameObject backgroundShopPrefab;
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private TextMeshProUGUI coinsText;

    private GameObject currentWindow;

    private void Start()
    {
        UpdateCoinsDisplay();

        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged += OnCoinsChanged;
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= OnCoinsChanged;
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnLevelEditorButtonClicked()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void OnSettingsButtonClicked()
    {
        OpenWindow(settingsWindowPrefab);
    }

    public void OnShopButtonClicked()
    {
        OpenWindow(coinShopPrefab);
    }

    public void OnBackgroundShopButtonClicked()
    {
        OpenWindow(backgroundShopPrefab);
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OpenWindow(GameObject windowPrefab)
    {
        if (windowPrefab != null && currentWindow == null)
        {
            currentWindow = Instantiate(windowPrefab);

            // Делаем главное меню неактивным
            if (mainMenuCanvasGroup != null)
            {
                mainMenuCanvasGroup.interactable = false;
                mainMenuCanvasGroup.blocksRaycasts = false;
            }

            // Подписываемся на закрытие окна
            StartCoroutine(WaitForWindowClose(currentWindow));
        }
    }

    private System.Collections.IEnumerator WaitForWindowClose(GameObject window)
    {
        while (window != null)
        {
            yield return null;
        }

        // Восстанавливаем активность главного меню
        if (mainMenuCanvasGroup != null)
        {
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
        }

        currentWindow = null;
        UpdateCoinsDisplay();
    }

    private void UpdateCoinsDisplay()
    {
        if (coinsText != null && CurrencyManager.Instance != null)
        {
            int coins = CurrencyManager.Instance.GetCoins();
            coinsText.text = FormatCoins(coins);
        }
    }

    private string FormatCoins(int coins)
    {
        if (coins >= 1000)
        {
            float kCoins = coins / 1000f;
            return kCoins.ToString("0.0") + "k";
        }
        return coins.ToString();
    }

    private void OnCoinsChanged(int newCoins)
    {
        UpdateCoinsDisplay();
    }
}
