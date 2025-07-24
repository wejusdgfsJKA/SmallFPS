using Entity;
using EventBus;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Levels
{
    public class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// The checkpoint which is currently active. If more than one checkpoint is active 
        /// in the scene, something is wrong.
        /// </summary>
        public static Checkpoint ActiveCheckpoint { get; protected set; }
        /// <summary>
        /// Fires right before the player respawns from this checkpoint.
        /// </summary>
        [SerializeField] protected UnityEvent OnRespawn;
        private void Awake()
        {
            EventBus<PlayerDeath>.AddActions(0, null, Respawn);
            if (ActiveCheckpoint == null || ActiveCheckpoint.gameObject == null)
            {
                ActiveCheckpoint = null;
            }
        }
        /// <summary>
        /// Sets ActiveCheckpoint to null.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetCheckpoint()
        {
            ActiveCheckpoint = null;
        }
        private void OnEnable()
        {
            if (EntityManager.Instance.Player == null)
            {
                LocalRespawn();
            }
            if (ActiveCheckpoint != this)
            {
                ActiveCheckpoint?.gameObject.SetActive(false);
                ActiveCheckpoint = this;
            }
            EventBus<CheckpointReached>.Raise(0, new CheckpointReached());
        }
        private void OnDisable()
        {
            EventBus<PlayerDeath>.RemoveActions(0, null, Respawn);
        }
        /// <summary>
        /// Respawn the player at the active checkpoint.
        /// </summary>
        public static void Respawn()
        {
            if (!ActiveCheckpoint)
            {
                Debug.LogError("No active checkpoint!");
                return;
            }
            ActiveCheckpoint.LocalRespawn();
        }
        /// <summary>
        /// Respawn the player at this checkpoint.
        /// </summary>
        void LocalRespawn()
        {
            StartCoroutine(RespawnCoroutine());
        }
        IEnumerator RespawnCoroutine()
        {
            yield return null;
            OnRespawn.Invoke();
            yield return null;
            var p = EntityManager.Instance.Spawn(EntityType.Player, ActiveCheckpoint.transform);
            yield return null;
            p?.gameObject.SetActive(true);
        }
    }
}