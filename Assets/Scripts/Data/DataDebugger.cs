using UnityEngine;
using TMPro;

public class DataDebugger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    public void ShowDataPaths()
    {
        if (DataManager.Instance != null)
        {
            string paths = "Data Paths:\n";
            paths += $"UserData: {DataManager.Instance.GetFilePath("UserData")}\n";
            paths += $"UserGameSettings: {DataManager.Instance.GetFilePath("UserGameSettings")}\n";
            paths += $"BaseGameSettings: {DataManager.Instance.GetFilePath("BaseGameSettings")}\n";
            paths += $"StoreConfig: {DataManager.Instance.GetFilePath("StoreConfig")}\n";
            paths += $"ItemSettings: {DataManager.Instance.GetFilePath("ItemSettings")}";

            if (debugText != null)
            {
                debugText.text = paths;
            }
            else
            {
                Debug.Log(paths);
            }
        }
    }
}
