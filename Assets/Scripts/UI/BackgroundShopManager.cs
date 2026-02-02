using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class BackgroundItem
{
    public string backgroundName;
    public Sprite backgroundImage;
    public int price;
    public Color backgroundColor;
    public bool purchased;
    public bool selected;
}

public class BackgroundShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform backgroundsContainer;
    [SerializeField] private GameObject backgroundItemPrefab;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button closeButton;
    //[SerializeField] private Button videoRewardButton;

    [Header("Background Items")]
    [SerializeField] private BackgroundItem[] backgroundItems;

    [Header("Video Settings")]
    [SerializeField] private GameObject videoPlayerPanel;

    private void Start()
    {
        InitializeBackgrounds();
        UpdateCoinsDisplay();

        closeButton.onClick.AddListener(CloseShop);
        //videoRewardButton.onClick.AddListener(OnVideoRewardClicked);

        if (videoPlayerPanel != null)
            videoPlayerPanel.SetActive(false);

        // Загружаем состояние фонов
        LoadBackgroundsState();
    }

    private void InitializeBackgrounds()
    {
        foreach (Transform child in backgroundsContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < backgroundItems.Length; i++)
        {
            int index = i;
            GameObject item = Instantiate(backgroundItemPrefab, backgroundsContainer);
            SetupBackgroundItem(item, index);
        }
    }

    private void SetupBackgroundItem(GameObject item, int index)
    {
        BackgroundItem bgItem = backgroundItems[index];

        // Находим элементы
        Image bgImage = item.transform.Find("BackgroundImage").GetComponent<Image>();
        TextMeshProUGUI priceText = item.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
        GameObject checkmark = item.transform.Find("Checkmark").gameObject;
        Button actionButton = item.transform.Find("ActionButton").GetComponent<Button>();
        TextMeshProUGUI buttonText = actionButton.GetComponentInChildren<TextMeshProUGUI>();

        // Устанавливаем изображение
        bgImage.sprite = bgItem.backgroundImage;
        bgImage.color = bgItem.backgroundColor;

        // Проверяем куплен ли фон
        if (bgItem.purchased)
        {
            if (bgItem.selected)
            {
                checkmark.SetActive(true);
                priceText.text = "SELECTED";
                buttonText.text = "SELECT";
                actionButton.interactable = false;
            }
            else
            {
                checkmark.SetActive(false);
                priceText.text = "PURCHASED";
                buttonText.text = "SELECT";
                actionButton.interactable = true;
            }
            priceText.color = Color.white;
        }
        else
        {
            checkmark.SetActive(false);
            priceText.text = $"{bgItem.price} coins";
            buttonText.text = "BUY";

            // Проверяем, хватает ли монет
            if (CurrencyManager.Instance.GetCoins() >= bgItem.price)
                priceText.color = Color.white;
            else
                priceText.color = Color.red;
        }

        // Настраиваем кнопку
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => OnBackgroundButtonClicked(index));
    }

    private void OnBackgroundButtonClicked(int index)
    {
        BackgroundItem bgItem = backgroundItems[index];

        if (bgItem.purchased)
        {
            // Выбираем фон
            SelectBackground(index);
        }
        else
        {
            // Покупаем фон
            if (CurrencyManager.Instance.SpendCoins(bgItem.price))
            {
                bgItem.purchased = true;
                SelectBackground(index);
                SaveBackgroundsState();
                InitializeBackgrounds(); // Обновляем UI
                UpdateCoinsDisplay();
            }
            else
            {
                Debug.Log("Not enough coins!");
            }
        }
    }

    private void SelectBackground(int selectedIndex)
    {
        // Снимаем выделение со всех фонов
        for (int i = 0; i < backgroundItems.Length; i++)
        {
            backgroundItems[i].selected = (i == selectedIndex);
        }

        SaveBackgroundsState();
        InitializeBackgrounds(); // Обновляем UI

        // Здесь можно применить выбранный фон в игре
        ApplyBackground(selectedIndex);
    }

    private void ApplyBackground(int index)
    {
        // Применяем фон в игре
        // Это может быть изменение фона главного меню и т.д.
        Debug.Log($"Applied background: {backgroundItems[index].backgroundName}");
    }

    // private void OnVideoRewardClicked()
    // {
    //     if (videoPlayerPanel != null)
    //     {
    //         videoPlayerPanel.SetActive(true);
    //         StartCoroutine(SimulateVideoForCoins());
    //     }
    // }

    private IEnumerator SimulateVideoForCoins()
    {
        // Имитация просмотра видео
        yield return new WaitForSeconds(5f);

        if (videoPlayerPanel.activeSelf)
        {
            // Начисляем монеты за просмотр видео
            CurrencyManager.Instance.AddCoins(100);
            UpdateCoinsDisplay();
            videoPlayerPanel.SetActive(false);
        }
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

    private void SaveBackgroundsState()
    {
        for (int i = 0; i < backgroundItems.Length; i++)
        {
            PlayerPrefs.SetInt($"Background_{i}_Purchased", backgroundItems[i].purchased ? 1 : 0);
            PlayerPrefs.SetInt($"Background_{i}_Selected", backgroundItems[i].selected ? 1 : 0);
        }
    }

    private void LoadBackgroundsState()
    {
        for (int i = 0; i < backgroundItems.Length; i++)
        {
            backgroundItems[i].purchased = PlayerPrefs.GetInt($"Background_{i}_Purchased", 0) == 1;
            backgroundItems[i].selected = PlayerPrefs.GetInt($"Background_{i}_Selected", i == 0 ? 1 : 0) == 1;
        }
    }

    private void CloseShop()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged += OnCoinsChanged;
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int newCoins)
    {
        UpdateCoinsDisplay();
        InitializeBackgrounds(); // Обновляем цвета цен
    }
}
