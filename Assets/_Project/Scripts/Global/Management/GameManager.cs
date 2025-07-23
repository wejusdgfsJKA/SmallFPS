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
    public static GameManager ManagerPrefab { get; set; }
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
    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void GoToMainMenu()
    {
        //probably save here as well
        SceneManager.LoadScene(0);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetTime()
    {
        Time.timeScale = 1;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureManagerExists()
    {
        EventBus<CheckpointReached>.AddBinding(0);
        EventBus<PlayerDeath>.AddBinding(0);
        if (Instance == null && ManagerPrefab != null)
        {
            Instance = Instantiate(ManagerPrefab);
        }
    }
    public void RestartLevel()
    {
        Debug.Log("Loading scene " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
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
    protected void Save(int nextLevel)
    {
        nextLevel = Mathf.Clamp(nextLevel, 1, SceneManager.sceneCountInBuildSettings - 1);
        string json = JsonUtility.ToJson(new GameData(nextLevel));
        File.WriteAllText(saveFilePath, json);
    }
    public void LoadProgress()
    {
        if (!File.Exists(saveFilePath))
        {
            SceneManager.LoadScene(1);
        }
        string json = File.ReadAllText(saveFilePath);
        int nextScene = Mathf.Clamp(JsonUtility.FromJson<GameData>(json).NextLevel, 1, SceneManager.sceneCountInBuildSettings - 1);
        SceneManager.LoadScene(nextScene);
    }
}
