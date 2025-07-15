using Entity;
using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
        EntityManager.Instance.Spawn(EntityType.Player, transform.position, transform.rotation);
    }
}
