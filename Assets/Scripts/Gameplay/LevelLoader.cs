using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelElementData
{
    public string prefabName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

[System.Serializable]
public class LevelLayout
{
    public int levelNumber;
    public List<LevelElementData> elements;
    public List<LevelElementData> decorations;
    public Vector3 startPosition;
    public Vector3 finishPosition;
}

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] elementPrefabs;
    [SerializeField] private GameObject[] decorationPrefabs;
    [SerializeField] private GameObject finishPrefab;

    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        BuildPrefabDictionary();
    }

    private void BuildPrefabDictionary()
    {
        foreach (GameObject prefab in elementPrefabs)
        {
            prefabDictionary[prefab.name] = prefab;
        }

        foreach (GameObject prefab in decorationPrefabs)
        {
            prefabDictionary[prefab.name] = prefab;
        }
    }

    public void LoadLevelFromJSON(string jsonData)
    {
        LevelLayout levelLayout = JsonUtility.FromJson<LevelLayout>(jsonData);

        if (levelLayout != null)
        {
            LoadLevel(levelLayout);
        }
    }

    public void LoadLevel(LevelLayout levelLayout)
    {
        // Создаем контейнер для уровня
        GameObject levelContainer = new GameObject($"Level_{levelLayout.levelNumber}");

        // Создаем элементы
        foreach (LevelElementData elementData in levelLayout.elements)
        {
            CreateElement(elementData, levelContainer.transform);
        }

        // Создаем декорации
        foreach (LevelElementData decorationData in levelLayout.decorations)
        {
            CreateDecoration(decorationData, levelContainer.transform);
        }

        // Создаем финиш
        if (finishPrefab != null)
        {
            GameObject finish = Instantiate(finishPrefab, levelContainer.transform);
            finish.transform.position = levelLayout.finishPosition;
        }
    }

    private void CreateElement(LevelElementData data, Transform parent)
    {
        if (prefabDictionary.ContainsKey(data.prefabName))
        {
            GameObject element = Instantiate(prefabDictionary[data.prefabName], parent);
            element.transform.position = data.position;
            element.transform.eulerAngles = data.rotation;
            element.transform.localScale = data.scale;

            // Настраиваем физику если нужно
            SetupElementPhysics(element);
        }
    }

    private void CreateDecoration(LevelElementData data, Transform parent)
    {
        if (prefabDictionary.ContainsKey(data.prefabName))
        {
            GameObject decoration = Instantiate(prefabDictionary[data.prefabName], parent);
            decoration.transform.position = data.position;
            decoration.transform.eulerAngles = data.rotation;
            decoration.transform.localScale = data.scale;

            // Декорации не имеют физики
            Collider collider = decoration.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
    }

    private void SetupElementPhysics(GameObject element)
    {
        // Добавляем коллайдер если нет
        if (element.GetComponent<Collider>() == null)
        {
            MeshCollider meshCollider = element.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }

        // Настраиваем теги
        if (element.name.Contains("Obstacle"))
        {
            element.tag = "Obstacle";
        }
    }
}
