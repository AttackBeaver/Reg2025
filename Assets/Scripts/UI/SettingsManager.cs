using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle windowModeToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button closeButton;

    [Header("Dialog")]
    [SerializeField] private GameObject confirmDialog;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TextMeshProUGUI noButtonText;

    private SettingsData currentSettings;
    private SettingsData tempSettings;
    private bool settingsChanged = false;
    private float dialogTimer = 10f;
    private bool dialogActive = false;

    private void Awake()
    {
        LoadSettings();
        InitializeUI();
    }

    private void Update()
    {
        if (dialogActive)
        {
            UpdateDialogTimer();
        }
    }

    private void LoadSettings()
    {
        // Пока загружаем дефолтные, потом добавим загрузку из файла
        currentSettings = new SettingsData();
        tempSettings = new SettingsData();
    }

    private void InitializeUI()
    {
        // Настройка Resolution Dropdown
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "1280:720",
            "1366:768",
            "1600:900",
            "1920:1080"
        });

        // Установка текущих значений
        resolutionDropdown.value = GetResolutionIndex(currentSettings.screenWidth, currentSettings.screenHeight);
        windowModeToggle.isOn = currentSettings.isFullscreen;
        soundToggle.isOn = currentSettings.soundEnabled;
        languageDropdown.value = currentSettings.language == "English" ? 0 : 1;

        // Подписка на изменения
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        windowModeToggle.onValueChanged.AddListener(OnWindowModeChanged);
        soundToggle.onValueChanged.AddListener(OnSoundChanged);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        resetButton.onClick.AddListener(OnResetButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        // Настройка диалога
        if (confirmDialog != null)
        {
            confirmDialog.SetActive(false);
            yesButton.onClick.AddListener(OnYesButtonClicked);
            noButton.onClick.AddListener(OnNoButtonClicked);
        }
    }

    private int GetResolutionIndex(int width, int height)
    {
        string res = width + ":" + height;
        switch (res)
        {
            case "1280:720": return 0;
            case "1366:768": return 1;
            case "1600:900": return 2;
            case "1920:1080": return 3;
            default: return 3;
        }
    }

    private void OnResolutionChanged(int index)
    {
        string[] res = resolutionDropdown.options[index].text.Split(':');
        tempSettings.screenWidth = int.Parse(res[0]);
        tempSettings.screenHeight = int.Parse(res[1]);
        settingsChanged = true;
    }

    private void OnWindowModeChanged(bool isOn)
    {
        tempSettings.isFullscreen = isOn;
        settingsChanged = true;
    }

    private void OnSoundChanged(bool isOn)
    {
        tempSettings.soundEnabled = isOn;
        settingsChanged = true;
    }

    private void OnLanguageChanged(int index)
    {
        tempSettings.language = languageDropdown.options[index].text;
        settingsChanged = true;
    }

    private void OnResetButtonClicked()
    {
        // Сброс к настройкам по умолчанию
        tempSettings = new SettingsData();
        settingsChanged = true;
        UpdateUIFromTempSettings();
    }

    private void UpdateUIFromTempSettings()
    {
        resolutionDropdown.value = GetResolutionIndex(tempSettings.screenWidth, tempSettings.screenHeight);
        windowModeToggle.isOn = tempSettings.isFullscreen;
        soundToggle.isOn = tempSettings.soundEnabled;
        languageDropdown.value = tempSettings.language == "English" ? 0 : 1;
    }

    private void OnCloseButtonClicked()
    {
        if (settingsChanged)
        {
            ShowConfirmDialog();
        }
        else
        {
            CloseSettings();
        }
    }

    private void ShowConfirmDialog()
    {
        if (confirmDialog != null)
        {
            confirmDialog.SetActive(true);
            dialogActive = true;
            dialogTimer = 10f;
            UpdateNoButtonText();
        }
    }

    private void UpdateDialogTimer()
    {
        if (dialogActive && dialogTimer > 0)
        {
            dialogTimer -= Time.deltaTime;
            UpdateNoButtonText();

            if (dialogTimer <= 0)
            {
                OnNoButtonClicked();
            }
        }
    }

    private void UpdateNoButtonText()
    {
        if (noButtonText != null)
        {
            noButtonText.text = $"No ({Mathf.CeilToInt(dialogTimer)})";
        }
    }

    private void OnYesButtonClicked()
    {
        ApplySettings();
        CloseDialog();
    }

    private void OnNoButtonClicked()
    {
        DiscardChanges();
        CloseDialog();
    }

    private void CloseDialog()
    {
        dialogActive = false;
        if (confirmDialog != null)
        {
            confirmDialog.SetActive(false);
        }
    }

    private void ApplySettings()
    {
        currentSettings = tempSettings;
        settingsChanged = false;

        // Применяем настройки экрана
        Screen.SetResolution(currentSettings.screenWidth, currentSettings.screenHeight, currentSettings.isFullscreen);

        // Применяем настройки звука
        AudioListener.volume = currentSettings.soundEnabled ? 1f : 0f;

        // Здесь позже добавим смену языка
        Debug.Log($"Settings applied: {currentSettings.screenWidth}x{currentSettings.screenHeight}, " +
                 $"Fullscreen: {currentSettings.isFullscreen}, Sound: {currentSettings.soundEnabled}, " +
                 $"Language: {currentSettings.language}");
    }

    private void DiscardChanges()
    {
        // Восстанавливаем предыдущие настройки
        tempSettings = new SettingsData()
        {
            screenWidth = currentSettings.screenWidth,
            screenHeight = currentSettings.screenHeight,
            isFullscreen = currentSettings.isFullscreen,
            soundEnabled = currentSettings.soundEnabled,
            language = currentSettings.language
        };

        UpdateUIFromTempSettings();
        settingsChanged = false;
    }

    public System.Action onSettingsClosed;

    private void CloseSettings()
    {
        if (onSettingsClosed != null)
            onSettingsClosed.Invoke();
        Destroy(gameObject);
    }
}
