using EventBus;
using Levels;
using System.Collections;
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
        EventBus<CheckpointReached>.AddBinding(0);
        EventBus<PlayerDeath>.AddBinding(0);
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
    public void SpawnPlayer()
    {
        StartCoroutine(SpawnPlayerCoroutine());
    }
    IEnumerator SpawnPlayerCoroutine()
    {
        yield return new WaitUntil(() => Checkpoint.ActiveCheckpoint != null);
        Checkpoint.Respawn();
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
