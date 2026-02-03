using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Assembly,
        Playing,
        Paused,
        Win,
        Lose
    }

    public GameState currentState = GameState.Assembly;
    private GameObject currentVehicle;
    private GameObject[] vehicleAttachments;
    private int currentLevel = 1;

    [Header("Level Settings")]
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject[] levelPrefabs;

    [Header("UI References")]
    [SerializeField] private GameObject assemblyUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    private GameObject currentLevelInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartLevel(int level, GameObject vehicle, GameObject[] attachments)
    {
        currentLevel = level;
        currentVehicle = vehicle;
        vehicleAttachments = attachments;

        // Загружаем уровень
        LoadLevel(level);

        // Активируем транспорт
        ActivateVehicle();

        // Переключаем UI
        assemblyUI.SetActive(false);
        gameplayUI.SetActive(true);

        // Меняем состояние
        currentState = GameState.Playing;
    }

    private void LoadLevel(int levelIndex)
    {
        // Удаляем предыдущий уровень
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // Загружаем новый уровень
        if (levelIndex <= levelPrefabs.Length)
        {
            currentLevelInstance = Instantiate(levelPrefabs[levelIndex - 1], levelContainer);
        }
        else
        {
            // Если уровней нет, создаем простой тестовый
            CreateTestLevel();
        }
    }

    private void CreateTestLevel()
    {
        currentLevelInstance = new GameObject("TestLevel");
        currentLevelInstance.transform.SetParent(levelContainer);

        // Создаем простую трассу
        GameObject track = GameObject.CreatePrimitive(PrimitiveType.Cube);
        track.transform.SetParent(currentLevelInstance.transform);
        track.transform.localScale = new Vector3(20, 0.2f, 100);
        track.transform.position = new Vector3(0, 0, 50);

        // Добавляем финиш
        GameObject finish = Instantiate(Resources.Load<GameObject>("FinishTape"));
        finish.transform.SetParent(currentLevelInstance.transform);
        finish.transform.position = new Vector3(0, 0.5f, 95);
    }

    private void ActivateVehicle()
    {
        if (currentVehicle != null)
        {
            // Активируем физику
            Rigidbody rb = currentVehicle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Активируем управление
            VehicleController vehicleController = currentVehicle.AddComponent<VehicleController>();
            vehicleController.Initialize(vehicleAttachments);

            // Позиционируем транспорт на старте
            currentVehicle.transform.position = new Vector3(0, 2, -5);
        }
    }

    public void OnVehicleReachedFinish()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Win;
            ShowWinScreen();
        }
    }

    public void OnVehicleFailed()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Lose;
            ShowLoseScreen();
        }
    }

    private void ShowWinScreen()
    {
        gameplayUI.SetActive(false);
        winUI.SetActive(true);

        // Начисляем награду
        int reward = CalculateReward();
        CurrencyManager.Instance.AddCoins(reward);
    }

    private void ShowLoseScreen()
    {
        gameplayUI.SetActive(false);
        loseUI.SetActive(true);
    }

    private int CalculateReward()
    {
        // Базовая награда + бонусы
        int baseReward = 100 * currentLevel;

        // Можно добавить бонусы за время, собранные монеты и т.д.

        return baseReward;
    }

    public void RestartLevel()
    {
        // Перезагружаем текущий уровень
        StartLevel(currentLevel, currentVehicle, vehicleAttachments);
        loseUI.SetActive(false);
        gameplayUI.SetActive(true);
        currentState = GameState.Playing;
    }

    public void NextLevel()
    {
        currentLevel++;
        // Здесь нужно перезагрузить сцену или сбросить состояние
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
