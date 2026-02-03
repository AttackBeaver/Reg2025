using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    private UserData userData;

    private void Start()
    {
        LoadUserData();
    }

    public void LoadUserData()
    {
        if (DataManager.Instance != null)
        {
            userData = DataManager.Instance.LoadData<UserData>("UserData");
            ApplyUserData();
        }
        else
        {
            userData = new UserData();
            Debug.LogWarning("DataManager not found, using default UserData");
        }
    }

    public void SaveUserData()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveData(userData, "UserData");
        }
    }

    private void ApplyUserData()
    {
        // Применяем загруженные данные
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.SetCoins(userData.coins);
        }

        // Здесь можно применить другие данные
        // Например, выбранный фон, пройденные уровни и т.д.
    }

    // Методы для изменения данных
    public void AddCoins(int amount)
    {
        userData.coins += amount;
        SaveUserData();
    }

    public bool SpendCoins(int amount)
    {
        if (userData.coins >= amount)
        {
            userData.coins -= amount;
            SaveUserData();
            return true;
        }
        return false;
    }

    public int GetLastCompletedLevel()
    {
        return userData.lastCompletedLevel;
    }

    public void SetLastCompletedLevel(int level)
    {
        userData.lastCompletedLevel = level;
        SaveUserData();
    }

    public void UnlockBackground(int backgroundIndex)
    {
        if (backgroundIndex >= 0 && backgroundIndex < userData.purchasedBackgrounds.Length)
        {
            userData.purchasedBackgrounds[backgroundIndex] = true;
            SaveUserData();
        }
    }

    public bool IsBackgroundPurchased(int backgroundIndex)
    {
        if (backgroundIndex >= 0 && backgroundIndex < userData.purchasedBackgrounds.Length)
        {
            return userData.purchasedBackgrounds[backgroundIndex];
        }
        return false;
    }

    public void SelectBackground(int backgroundIndex)
    {
        if (backgroundIndex >= 0 && backgroundIndex < userData.purchasedBackgrounds.Length)
        {
            userData.selectedBackground = backgroundIndex;
            SaveUserData();
        }
    }

    public int GetSelectedBackground()
    {
        return userData.selectedBackground;
    }
}
