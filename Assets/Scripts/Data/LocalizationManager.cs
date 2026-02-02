using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    [SerializeField] private LocalizationDataSO localizationData;
    private string currentLanguage = "English";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguage();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(string language)
    {
        currentLanguage = language;
        PlayerPrefs.SetString("GameLanguage", language);
        PlayerPrefs.Save();

        // Оповещаем все объекты об изменении языка
        // Можно реализовать систему событий
    }

    public string GetString(string key)
    {
        if (localizationData != null)
        {
            return localizationData.GetLocalizedString(key, currentLanguage);
        }
        return key;
    }

    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }

    private void LoadLanguage()
    {
        currentLanguage = PlayerPrefs.GetString("GameLanguage", "English");
    }
}
