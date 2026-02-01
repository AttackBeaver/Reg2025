using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsWindowPrefab;
    private GameObject currentSettingsWindow;

    public void OnPlayButtonClicked()
    {
        // Будем загружать игровую сцену
        SceneManager.LoadScene("GameScene");
    }

    public void OnLevelEditorButtonClicked()
    {
        // Будем загружать редактор уровней
        SceneManager.LoadScene("LevelEditor");
    }

    public void OnSettingsButtonClicked()
    {
        if (settingsWindowPrefab != null && currentSettingsWindow == null)
        {
            currentSettingsWindow = Instantiate(settingsWindowPrefab);
        }
    }

    public void OnShopButtonClicked()
    {
        Debug.Log("Shop opened");
        // Здесь будет открытие магазина
    }

    public void OnBackgroundShopButtonClicked()
    {
        Debug.Log("Background shop opened");
        // Здесь будет открытие магазина фонов
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
