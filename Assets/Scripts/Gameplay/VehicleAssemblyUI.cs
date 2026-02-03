using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class VehicleAssemblyUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button homeButton;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Transform vehicleContainer;
    [SerializeField] private Transform attachmentItemsContainer;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject attachmentItemPrefab;

    [Header("Vehicle Data")]
    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private GameObject[] attachmentPrefabs;

    private GameObject currentVehicle;
    private List<GameObject> placedAttachments = new List<GameObject>();
    private List<AttachmentItemUI> attachmentItems = new List<AttachmentItemUI>();

    private int currentLevel = 1;
    private int requiredAttachmentsCount = 3;

    private void Start()
    {
        InitializeUI();
        LoadVehicle();
        CreateAttachmentItems();

        homeButton.onClick.AddListener(OnHomeButtonClicked);
        startButton.onClick.AddListener(OnStartButtonClicked);
        startButton.interactable = false;

        UpdateCoinsDisplay();
        levelText.text = $"LEVEL {currentLevel}";
    }

    private void InitializeUI()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += OnCoinsChanged;
        }
    }

    private void LoadVehicle()
    {
        // Загружаем выбранный транспорт (пока первый)
        if (vehiclePrefabs.Length > 0)
        {
            currentVehicle = Instantiate(vehiclePrefabs[0], vehicleContainer);
            currentVehicle.transform.localPosition = Vector3.zero;

            // Добавляем компонент для управления сборкой
            VehicleAssemblyController assemblyController = currentVehicle.AddComponent<VehicleAssemblyController>();
            assemblyController.Initialize(this);
        }
    }

    private void CreateAttachmentItems()
    {
        // Очищаем контейнер
        foreach (Transform child in attachmentItemsContainer)
        {
            Destroy(child.gameObject);
        }

        // Создаем предметы для установки
        for (int i = 0; i < requiredAttachmentsCount; i++)
        {
            if (i < attachmentPrefabs.Length)
            {
                GameObject itemUI = Instantiate(attachmentItemPrefab, attachmentItemsContainer);
                AttachmentItemUI attachmentUI = itemUI.GetComponent<AttachmentItemUI>();

                if (attachmentUI != null)
                {
                    attachmentUI.Initialize(attachmentPrefabs[i], i);
                    attachmentItems.Add(attachmentUI);
                }
            }
        }
    }

    public void OnAttachmentPlaced(GameObject attachment)
    {
        placedAttachments.Add(attachment);

        // Проверяем, все ли предметы установлены
        if (placedAttachments.Count >= requiredAttachmentsCount)
        {
            startButton.interactable = true;
        }
    }

    public void OnAttachmentRemoved(GameObject attachment)
    {
        if (placedAttachments.Contains(attachment))
        {
            placedAttachments.Remove(attachment);
            startButton.interactable = false;
        }
    }

    private void OnHomeButtonClicked()
    {
        // Возвращаемся в главное меню
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void OnStartButtonClicked()
    {
        // Начинаем уровень
        StartGame();
    }

    private void StartGame()
    {
        // Отключаем UI сборки
        gameObject.SetActive(false);

        // Включаем игровой процесс
        GameManager.Instance.StartLevel(currentLevel, currentVehicle, placedAttachments.ToArray());
    }

    private void UpdateCoinsDisplay()
    {
        if (coinsText != null && CurrencyManager.Instance != null)
        {
            int coins = CurrencyManager.Instance.GetCoins();
            coinsText.text = coins >= 1000 ? $"{(coins / 1000f):0.0}k" : coins.ToString();
        }
    }

    private void OnCoinsChanged(int newCoins)
    {
        UpdateCoinsDisplay();
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= OnCoinsChanged;
        }
    }
}
