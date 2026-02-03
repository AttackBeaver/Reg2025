using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [Header("Control Buttons")]
    [SerializeField] private Button propellerButton;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button spikedWheelsButton;
    [SerializeField] private Button wingsButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI speedText;

    [Header("Cooldown Indicators")]
    [SerializeField] private Image rocketCooldownImage;
    [SerializeField] private Image wingsCooldownImage;

    private VehicleController vehicleController;
    private bool propellerActive = false;

    private void Start()
    {
        FindVehicleController();
        SetupButtons();
        UpdateUI();

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += OnCoinsChanged;
        }
    }

    private void FindVehicleController()
    {
        GameObject vehicle = GameObject.FindGameObjectWithTag("Player");
        if (vehicle != null)
        {
            vehicleController = vehicle.GetComponent<VehicleController>();
        }
    }

    private void SetupButtons()
    {
        // Propeller - зажатие/отпускание
        EventTrigger propellerTrigger = propellerButton.gameObject.AddComponent<EventTrigger>();

        // Rocket - одиночное нажатие
        rocketButton.onClick.AddListener(OnRocketButtonClicked);

        // Spiked Wheels - переключатель
        spikedWheelsButton.onClick.AddListener(OnSpikedWheelsButtonClicked);

        // Wings - одиночное нажатие с кулдауном
        wingsButton.onClick.AddListener(OnWingsButtonClicked);

        // Рестарт и домой
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        homeButton.onClick.AddListener(OnHomeButtonClicked);
    }

    [System.Obsolete]
    private void Update()
    {
        UpdateCooldownIndicators();
        UpdateSpeedDisplay();

        // Обработка зажатия пропеллера
        if (propellerActive && vehicleController != null)
        {
            // Удерживается для ускорения
        }
    }

    private void UpdateCooldownIndicators()
    {
        // Здесь можно обновлять индикаторы кулдаунов
        // Например, заполнение Image.fillAmount в зависимости от оставшегося времени
    }

    [System.Obsolete]
    private void UpdateSpeedDisplay()
    {
        if (vehicleController != null)
        {
            Rigidbody rb = vehicleController.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float speed = rb.velocity.magnitude;
                speedText.text = $"Speed: {speed:0.0}";
            }
        }
    }

    private void OnPropellerButtonPressed()
    {
        propellerActive = true;
        if (vehicleController != null)
        {
            vehicleController.ActivatePropeller(true);
        }
    }

    private void OnPropellerButtonReleased()
    {
        propellerActive = false;
        if (vehicleController != null)
        {
            vehicleController.ActivatePropeller(false);
        }
    }

    private void OnRocketButtonClicked()
    {
        if (vehicleController != null)
        {
            vehicleController.FireRocket();
        }
    }

    private void OnSpikedWheelsButtonClicked()
    {
        if (vehicleController != null)
        {
            vehicleController.ToggleSpikedWheels();
        }
    }

    private void OnWingsButtonClicked()
    {
        if (vehicleController != null)
        {
            vehicleController.UseWings();
        }
    }

    private void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartLevel();
    }

    private void OnHomeButtonClicked()
    {
        GameManager.Instance.ReturnToMenu();
    }

    private void UpdateUI()
    {
        UpdateCoinsDisplay();
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
