using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public event Action<int> OnCoinsChanged;

    private int currentCoins = 1000; // Начальное значение

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
        SaveCoins();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            OnCoinsChanged?.Invoke(currentCoins);
            SaveCoins();
            return true;
        }
        return false;
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
