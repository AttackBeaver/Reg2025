using UnityEngine;
using System;

[Serializable]
public class UserData
{
    public int lastCompletedLevel = 1; // Последний пройденный уровень
    public int coins = 1000; // Количество монет
    public bool[] purchasedBackgrounds = new bool[3]; // Купленные фоны
    public int selectedBackground = 0; // Выбранный фон
    public int[] purchasedItems = new int[0]; // Купленные предметы (можно расширить)
}
