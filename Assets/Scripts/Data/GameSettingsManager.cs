using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    private GameSettings userSettings;
    private GameSettings baseSettings;
    
    private void Start()
    {
        LoadBaseSettings();
        LoadUserSettings();
    }

    private void LoadBaseSettings()
    {
        // Базовые настройки можно хранить в JSON или ScriptableObject
        // Для простоты создадим дефолтные
        baseSettings = new GameSettings();

        // Или загрузить из файла:
        // baseSettings = DataManager.Instance.LoadData<GameSettings>("BaseGameSettings");
    }

    public void LoadUserSettings()
    {
        if (DataManager.Instance != null)
        {
            userSettings = DataManager.Instance.LoadData<GameSettings>("UserGameSettings");
            ApplySettings();
        }
        else
        {
            // Если файла нет, используем базовые настройки
            userSettings = new GameSettings();
            Debug.LogWarning("DataManager not found, using default settings");
        }
    }

    public void SaveUserSettings()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveData(userSettings, "UserGameSettings");
        }
    }

    private void ApplySettings()
    {
        // Применяем настройки экрана
        Screen.SetResolution(userSettings.screenWidth, userSettings.screenHeight, userSettings.isFullscreen);

        // Применяем настройки звука
        AudioListener.volume = userSettings.soundEnabled ? 1f : 0f;

        // Применяем язык
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.SetLanguage(userSettings.language);
        }
    }

    public void ResetToDefault()
    {
        userSettings = new GameSettings()
        {
            screenWidth = baseSettings.screenWidth,
            screenHeight = baseSettings.screenHeight,
            isFullscreen = baseSettings.isFullscreen,
            soundEnabled = baseSettings.soundEnabled,
            language = baseSettings.language
        };

        SaveUserSettings();
        ApplySettings();
    }

    // Геттеры и сеттеры
    public GameSettings GetCurrentSettings()
    {
        return userSettings;
    }

    public void SetResolution(int width, int height)
    {
        userSettings.screenWidth = width;
        userSettings.screenHeight = height;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        userSettings.isFullscreen = isFullscreen;
    }

    public void SetSoundEnabled(bool enabled)
    {
        userSettings.soundEnabled = enabled;
    }

    public void SetLanguage(string language)
    {
        userSettings.language = language;
    }
}
