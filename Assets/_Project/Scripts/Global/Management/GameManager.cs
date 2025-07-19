using Entity;
using EventBus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
    }
    protected void OnEnable()
    {
        //make sure we have an EntityManager and InteractableManager
        if (EntityManager.Instance == null)
        {
            transform.AddComponent<EntityManager>();
        }
        if (InteractableManager.Instance == null)
        {
            transform.AddComponent<InteractableManager>();
        }
        if (EntityManager.Instance == null)
        {
            transform.AddComponent<EntityManager>();
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
    public void EnableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void GoToMainMenu()
    {
        //probably save here as well
        SceneManager.LoadScene(0);
    }
    public void RespawnPlayer()
    {
        Level.CurrentLevel?.ResetEncounters();
        EntityManager.Instance.TerminateAll();
        EntityManager.Instance?.Spawn(EntityType.Player, CheckPoint.ActiveCheckpoint.transform);
        CheckPoint.ActiveCheckpoint?.OnRespawn?.Invoke();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            GoToMainMenu();
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
        Debug.LogError("Keep in mind that the current way of handling levels is brittle as fuck.");
    }
}
