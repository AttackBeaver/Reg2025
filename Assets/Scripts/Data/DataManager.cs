using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private string dataPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        // Создаем путь к папке данных
        dataPath = Path.Combine(Application.persistentDataPath, "GameData");

        // Создаем папку, если её нет
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
    }

    // Метод для сохранения любого объекта в JSON
    public void SaveData<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(dataPath, fileName + ".json");
        File.WriteAllText(filePath, json);
        Debug.Log($"Data saved to: {filePath}");
    }

    // Метод для загрузки любого объекта из JSON
    public T LoadData<T>(string fileName) where T : new()
    {
        string filePath = Path.Combine(dataPath, fileName + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.Log($"File not found: {filePath}. Creating default.");
            return new T();
        }
    }

    // Метод для сохранения массива объектов
    public void SaveArrayData<T>(T[] data, string fileName)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = data;
        SaveData(wrapper, fileName);
    }

    // Метод для загрузки массива объектов
    public T[] LoadArrayData<T>(string fileName)
    {
        string filePath = Path.Combine(dataPath, fileName + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }
        else
        {
            Debug.Log($"File not found: {filePath}. Creating default.");
            return new T[0];
        }
    }

    // Вспомогательный класс для сериализации массивов
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    // Получить путь к файлу (для отладки)
    public string GetFilePath(string fileName)
    {
        return Path.Combine(dataPath, fileName + ".json");
    }
}
