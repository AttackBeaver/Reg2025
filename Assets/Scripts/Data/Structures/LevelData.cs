using UnityEngine;
using System;

[Serializable]
public class LevelReward
{
    public int coins = 100;
    public int bonusCoins = 50; // Бонус за 3 звезды
}

[Serializable]
public class LevelRequirements
{
    public string[] requiredItems; // ID необходимых предметов
    public int timeLimit = 60; // Лимит времени в секундах
}

[Serializable]
public class LevelData
{
    public int levelNumber;
    public string levelName;
    public LevelReward reward;
    public LevelRequirements requirements;
    public bool isCompleted = false;
    public int starsEarned = 0;
}
