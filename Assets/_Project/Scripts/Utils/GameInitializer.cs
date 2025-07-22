using EventBus;
using Levels;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameManager managerPrefab;
    private void Awake()
    {
        GameManager.ManagerPrefab = managerPrefab;
        Time.timeScale = 1;
        RegisterBindings();
    }
    void RegisterBindings()
    {
        EventBus<CheckpointReached>.AddBinding(0);
        EventBus<PlayerDeath>.AddBinding(0);
    }
}
