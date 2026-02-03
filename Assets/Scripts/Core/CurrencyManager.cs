using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public event Action<int> OnCoinsChanged;

    private int currentCoins = 1000; // Начальное значение

    private UserDataManager userDataManager;

    [Obsolete]
    private void Start()
    {
        userDataManager = FindObjectOfType<UserDataManager>();
        if (userDataManager != null)
        {
            currentCoins = GetCoins();
        }
        else
        {
            LoadCoins();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Здесь позже добавим загрузку из сохранений
    }

    public int GetCoins()
    {
        return currentCoins;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        OnCoinsChanged?.Invoke(currentCoins);

        if (userDataManager != null)
        {
            userDataManager.AddCoins(amount);
        }
        else
        {
            SaveCoins();
        }
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            OnCoinsChanged?.Invoke(currentCoins);

            if (userDataManager != null)
            {
                // В UserDataManager должен быть метод SpendCoins
                // Пока просто вычитаем через AddCoins с отрицательным значением
                userDataManager.AddCoins(-amount);
            }
            else
            {
                SaveCoins();
            }
            return true;
        }
        return false;
    }

    // Добавим метод для установки монет из UserData
    public void SetCoins(int coins)
    {
        currentCoins = coins;
        OnCoinsChanged?.Invoke(currentCoins);
    }


    private void SaveCoins()
    {
        // Здесь позже добавим сохранение
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
    }

    private void LoadCoins()
    {
        currentCoins = PlayerPrefs.GetInt("PlayerCoins", 1000);
    }

}
