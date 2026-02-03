using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    private void Start()
    {
        InitializeAllData();
    }

    private void InitializeAllData()
    {
        // Создаем базовые настройки
        GameSettings baseSettings = new GameSettings();
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveData(baseSettings, "BaseGameSettings");
        }

        // Создаем дефолтные пользовательские данные
        UserData userData = new UserData();
        userData.coins = 1000;
        userData.lastCompletedLevel = 1;
        userData.purchasedBackgrounds = new bool[] { true, false, false }; // Первый фон куплен по умолчанию
        userData.selectedBackground = 0;

        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveData(userData, "UserData");
        }

        // Создаем конфигурацию магазина
        StoreConfigManager storeConfigManager = GetComponent<StoreConfigManager>();
        if (storeConfigManager != null)
        {
            storeConfigManager.SaveStoreConfig();
        }

        // Создаем настройки предметов
        ItemSettingsManager itemSettingsManager = GetComponent<ItemSettingsManager>();
        if (itemSettingsManager != null)
        {
            itemSettingsManager.SaveItemSettings();
        }

        Debug.Log("All data initialized!");
    }
}
