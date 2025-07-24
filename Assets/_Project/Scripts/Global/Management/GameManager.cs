using Entity;
using EventBus;
using Levels;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    /// <summary>
    /// Manager prefab to be instantiated if none is found in a scene.
    /// </summary>
    public static GameManager ManagerPrefab { get; set; }
    /// <summary>
    /// Path to the save file.
    /// </summary>
    string saveFilePath => Path.Combine(Application.persistentDataPath, "save.dat");
    protected void Awake()
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
    protected void OnEnable()
    {
        if (InteractableManager.Instance == null)
        {
            transform.AddComponent<InteractableManager>();
        }
        if (BulletManager.Instance == null)
        {
            transform.AddComponent<BulletManager>();
        }
    }
    void ClearBuses()
    {
        EventBusUtil.ClearAllBuses();
    }
    private void OnApplicationQuit()
    {
        //we need to clear all buses
        ClearBuses();
    }
    /// <summary>
    /// Locks and hides the cursor.
    /// </summary>
    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary>
    /// Unlocks and shows the cursor.
    /// </summary>
    public void EnableMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// Load the MainMenu scene.
    /// </summary>
    public void GoToMainMenu()
    {
        //probably save here as well
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Resets the timescale to 1.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetTime()
    {
        Time.timeScale = 1;
    }
    /// <summary>
    /// Attempts to create a new manager in the scene if none is found, using managerPrefab.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureManagerExists()
    {
        EventBus<CheckpointReached>.AddBinding(0);
        EventBus<PlayerDeath>.AddBinding(0);
        if (Instance == null)
        {
            if (ManagerPrefab != null)
            {
                Instance = Instantiate(ManagerPrefab);
            }
            else
            {
                Debug.LogError("No manager prefab found!");
            }
        }
    }
    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void RestartLevel()
    {
        Debug.Log("Loading scene " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Saves progress and loads the next scene in the build order, looping back if it reached the end.
    /// </summary>
    public void NextLevel()
    {
        Save(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(ChangeLevel());
    }
    IEnumerator ChangeLevel()
    {
        EntityManager.Instance.TerminateAll();
        yield return null;
        int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
        Debug.LogError("Keep in mind that the current way of handling levels is brittle as fuck.");
    }
    /// <summary>
    /// Store in the save file the level the player should be loaded in when 'continue' is pressed in the main menu.
    /// </summary>
    /// <param name="nextLevel">The build index of the next level.</param>
    protected void Save(int nextLevel)
    {
        nextLevel = Mathf.Clamp(nextLevel, 1, SceneManager.sceneCountInBuildSettings - 1);
        string json = JsonUtility.ToJson(new GameData(nextLevel));
        File.WriteAllText(saveFilePath, json);
    }
    /// <summary>
    /// Loads the level found in the save file, or the first level if no save file is found.
    /// </summary>
    public void LoadProgress()
    {
        if (!File.Exists(saveFilePath))
        {
            SceneManager.LoadScene(1);
            Debug.LogError("No save file found!");
        }
        string json = File.ReadAllText(saveFilePath);
        int nextScene = Mathf.Clamp(JsonUtility.FromJson<GameData>(json).NextLevel, 1, SceneManager.sceneCountInBuildSettings - 1);
        SceneManager.LoadScene(nextScene);
    }
}
