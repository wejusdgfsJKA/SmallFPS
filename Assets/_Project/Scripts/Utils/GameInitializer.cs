using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameManager managerPrefab;
    private void Awake()
    {
        GameManager.ManagerPrefab = managerPrefab;
        Time.timeScale = 1;
    }
}
