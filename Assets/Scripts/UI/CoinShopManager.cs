using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public int coinAmount;
    public int price;
    public Sprite icon;
    public bool isVideoReward;
    public bool isTimeReward;
    public float cooldownTime;
}

public class CoinShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject[] shopItems;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private GameObject paymentPanel;
    [SerializeField] private Button closeButton;

    [Header("Shop Items Data")]
    [SerializeField] private ShopItem[] itemsData;

    [Header("Video Settings")]
    [SerializeField] private GameObject videoPlayerPanel;
    [SerializeField] private Button videoCloseButton;

    private float[] cooldownTimers;
    private bool[] cooldownActive;

    private void Start()
    {
        InitializeShop();
        UpdateCoinsDisplay();

        closeButton.onClick.AddListener(CloseShop);

        if (paymentPanel != null)
            paymentPanel.SetActive(false);

        if (videoPlayerPanel != null)
        {
            videoPlayerPanel.SetActive(false);
            videoCloseButton.onClick.AddListener(CloseVideo);
        }

        cooldownTimers = new float[itemsData.Length];
        cooldownActive = new bool[itemsData.Length];
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    private void InitializeShop()
    {
        for (int i = 0; i < shopItems.Length && i < itemsData.Length; i++)
        {
            int index = i;
            GameObject item = shopItems[i];

            // Находим элементы UI
            Image iconImage = item.transform.Find("ItemIcon").GetComponent<Image>();
            TextMeshProUGUI itemText = item.transform.Find("ItemText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = item.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
            Button buyButton = item.transform.Find("BuyButton").GetComponent<Button>();

            // Устанавливаем данные
            iconImage.sprite = itemsData[i].icon;
            itemText.text = itemsData[i].itemName;

            if (itemsData[i].price == 0)
                priceText.text = $"FREE\n({itemsData[i].coinAmount} coins)";
            else
                priceText.text = $"{itemsData[i].price} coins";

            // Настраиваем кнопку
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => OnItemBuyClicked(index));

            // Проверяем кулдаун
            if ((itemsData[i].isTimeReward || itemsData[i].isVideoReward) &&
                PlayerPrefs.HasKey($"ShopItemCooldown_{i}"))
            {
                float remainingTime = PlayerPrefs.GetFloat($"ShopItemCooldown_{i}") - Time.time;
                if (remainingTime > 0)
                {
                    cooldownTimers[i] = remainingTime;
                    cooldownActive[i] = true;
                    buyButton.interactable = false;
                }
            }
        }
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < cooldownActive.Length; i++)
        {
            if (cooldownActive[i])
            {
                cooldownTimers[i] -= Time.deltaTime;

                if (cooldownTimers[i] <= 0)
                {
                    cooldownActive[i] = false;
                    shopItems[i].transform.Find("BuyButton").GetComponent<Button>().interactable = true;

                    // Обновляем текст
                    TextMeshProUGUI priceText = shopItems[i].transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
                    if (itemsData[i].price == 0)
                        priceText.text = $"FREE\n({itemsData[i].coinAmount} coins)";
                }
                else
                {
                    // Обновляем текст с таймером
                    TextMeshProUGUI priceText = shopItems[i].transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
                    priceText.text = $"Wait {Mathf.CeilToInt(cooldownTimers[i])}s";
                }
            }
        }
    }

    private void OnItemBuyClicked(int itemIndex)
    {
        if (itemIndex >= itemsData.Length) return;

        ShopItem item = itemsData[itemIndex];

        if (item.isVideoReward)
        {
            PlayVideoForReward(itemIndex);
            return;
        }

        if (item.price > 0)
        {
            if (CurrencyManager.Instance.SpendCoins(item.price))
            {
                StartCoroutine(ProcessPayment(itemIndex));
            }
            else
            {
                Debug.Log("Not enough coins!");
            }
        }
        else if (item.isTimeReward)
        {
            GiveReward(itemIndex);

            // Устанавливаем кулдаун
            cooldownTimers[itemIndex] = item.cooldownTime;
            cooldownActive[itemIndex] = true;
            PlayerPrefs.SetFloat($"ShopItemCooldown_{itemIndex}", Time.time + item.cooldownTime);

            shopItems[itemIndex].transform.Find("BuyButton").GetComponent<Button>().interactable = false;
        }
        else
        {
            GiveReward(itemIndex);
        }
    }

    private IEnumerator ProcessPayment(int itemIndex)
    {
        // Показываем панель оплаты
        if (paymentPanel != null)
            paymentPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        // Начисляем монеты
        GiveReward(itemIndex);

        // Скрываем панель оплаты
        if (paymentPanel != null)
            paymentPanel.SetActive(false);
    }

    private void GiveReward(int itemIndex)
    {
        CurrencyManager.Instance.AddCoins(itemsData[itemIndex].coinAmount);
        UpdateCoinsDisplay();
        Debug.Log($"Reward received: {itemsData[itemIndex].coinAmount} coins");
    }

    private void PlayVideoForReward(int itemIndex)
    {
        if (videoPlayerPanel != null)
        {
            videoPlayerPanel.SetActive(true);

            // Имитация просмотра видео
            StartCoroutine(SimulateVideoPlayback(itemIndex));
        }
    }

    private IEnumerator SimulateVideoPlayback(int itemIndex)
    {
        // В реальном проекте здесь будет воспроизведение видео
        yield return new WaitForSeconds(5f); // Имитация 5 секунд видео

        // Проверяем, что видео не было закрыто
        if (videoPlayerPanel.activeSelf)
        {
            // Начисляем награду
            GiveReward(itemIndex);

            // Устанавливаем кулдаун
            cooldownTimers[itemIndex] = itemsData[itemIndex].cooldownTime;
            cooldownActive[itemIndex] = true;
            PlayerPrefs.SetFloat($"ShopItemCooldown_{itemIndex}", Time.time + itemsData[itemIndex].cooldownTime);

            shopItems[itemIndex].transform.Find("BuyButton").GetComponent<Button>().interactable = false;

            CloseVideo();
        }
    }

    private void CloseVideo()
    {
        if (videoPlayerPanel != null)
        {
            videoPlayerPanel.SetActive(false);
            StopAllCoroutines();
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
    }
}
