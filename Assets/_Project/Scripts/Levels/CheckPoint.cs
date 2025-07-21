using Entity;
using EventBus;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Levels
{
    public class Checkpoint : MonoBehaviour
    {
        public static Checkpoint ActiveCheckpoint { get; protected set; }
        [SerializeField] protected UnityEvent OnRespawn;
        private void Awake()
        {
            EventBus<PlayerDeath>.AddActions(0, null, Respawn);
        }
        private void OnEnable()
        {
            if (ActiveCheckpoint != this)
            {
                ActiveCheckpoint?.gameObject.SetActive(false);
                ActiveCheckpoint = this;
                EventBus<CheckpointReached>.Raise(0, new CheckpointReached());
            }
        }
        public static void Respawn()
        {
            if (!ActiveCheckpoint)
            {
                Debug.LogError("No active checkpoint!");
                return;
            }
            ActiveCheckpoint.OnRespawn.Invoke();
            ActiveCheckpoint.LocalRespawn();
        }
        void LocalRespawn()
        {
            StartCoroutine(RespawnCoroutine());
        }
        IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(1);
            EntityManager.Instance.Spawn(EntityType.Player, ActiveCheckpoint.transform).gameObject.SetActive(true);
        }
    }
}