using Entity;
using EventBus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        EntityManager.Instance.Spawn(EntityType.Player, CheckPoint.ActiveCheckpoint.transform.position);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
