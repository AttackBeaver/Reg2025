using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDatas", menuName = "Game Data/Level Datas")]
public class LevelDatasSO : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();

    public LevelData GetLevelData(int levelNumber)
    {
        foreach (var level in levels)
        {
            if (level.levelNumber == levelNumber)
                return level;
        }
        return null;
    }

    public void MarkLevelCompleted(int levelNumber, int stars)
    {
        var level = GetLevelData(levelNumber);
        if (level != null)
        {
            level.isCompleted = true;
            level.starsEarned = Mathf.Max(level.starsEarned, stars);
        }
    }
}
