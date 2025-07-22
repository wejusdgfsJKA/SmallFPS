using Entity;
using EventBus;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    public static GameManager ManagerPrefab { get; set; }
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
}
