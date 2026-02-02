using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LocalizedString
{
    public string key;
    public string english;
    public string russian;
}

[CreateAssetMenu(fileName = "LocalizationData", menuName = "Game Data/Localization Data")]
public class LocalizationDataSO : ScriptableObject
{
    public List<LocalizedString> localizedStrings = new List<LocalizedString>();

    public string GetLocalizedString(string key, string language)
    {
        foreach (var item in localizedStrings)
        {
            if (item.key == key)
            {
                switch (language.ToLower())
                {
                    case "russian":
                    case "русский":
                        return item.russian;
                    default:
                        return item.english;
                }
            }
        }
        return $"[{key}]"; // Если ключ не найден
    }
}
