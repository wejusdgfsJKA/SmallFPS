using Entity;
using EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Levels
{
    public class Encounter : MonoBehaviour
    {
        #region Fields
        public enum State
        {
            Waiting,
            Running,
            Completed,
            Finished
        }
        [SerializeField] List<EntityType> entities = new List<EntityType>();
        [SerializeField] List<Transform> spawnPoints;
        [SerializeField] int entityCount;
        ISpawnStrategy spawnStrategy;
        [SerializeField] SpawnStrategyType spawnStrategyType;
        [SerializeField] protected UnityEvent onEncounterEnd, onEncounterStart, onEncounterReset;
        [field: SerializeField] public State CurrentState { get; private set; } = State.Waiting;
        [SerializeField] float initialDelay = 1, inBetweenDelay = 0;
        #endregion
        private void Awake()
        {
            spawnStrategy = spawnStrategyType switch
            {
                SpawnStrategyType.Linear => new LinearSpawnStrategy(spawnPoints),
                _ => spawnStrategy
            };
            RegisterActions();
        }
        protected void RegisterActions()
        {
            if (!EventBus<CheckpointReached>.AddActions(0, null, FinishEncounter))
            {
                throw new System.Exception($"{this} unable to add action to CheckpointReached EventBus!");
            }
            if (!EventBus<PlayerDeath>.AddActions(0, null, ResetEncounter))
            {
                throw new System.Exception($"{this} unable to add action to PlayerDeath EventBus!");
            }
        }
        void ClearActions()
        {
            if (!EventBus<CheckpointReached>.RemoveActions(0, null, FinishEncounter))
            {
                Debug.LogError($"{this} unable to remove action from CheckpointReached EventBus!");
            }
            if (!EventBus<PlayerDeath>.RemoveActions(0, null, ResetEncounter))
            {
                Debug.LogError($"{this} unable to remove action from PlayerDeath EventBus!");
            }
        }
        public void ResetEncounter()
        {
            if (CurrentState != State.Waiting && CurrentState != State.Finished)
            {
                CurrentState = State.Waiting;
                onEncounterReset?.Invoke();
            }
        }
        /// <summary>
        /// Begin spawning the entities in this encounter.
        /// </summary>
        public void StartEncounter()
        {
            if (CurrentState == State.Waiting)
            {
                CurrentState = State.Running;
                onEncounterStart?.Invoke();
                entityCount = entities.Count;
                StartCoroutine(SpawnEntities());
            }
        }
        IEnumerator SpawnEntities(bool enable = true)
        {
            yield return new WaitForSeconds(initialDelay);
            for (int i = 0; i < entityCount; i++)
            {
                //spawn this entity
                var e = EntityManager.Instance.Spawn(entities[i], spawnStrategy.GetSpawnPoint());
                if (enable)
                {
                    EventBus<OnDeath>.AddBinding(e.transform.GetInstanceID());
                    EventBus<OnDeath>.AddActions(e.transform.GetInstanceID(), OnEntityDeath);
                    e.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(inBetweenDelay);
            }
        }
        public void OnEntityDeath(OnDeath @event)
        {
            entityCount--;
            if (entityCount <= 0)
            {
                OnEncounterEnd();
            }
        }
        public void OnEncounterEnd()
        {
            if (CurrentState == State.Running)
            {
                CurrentState = State.Completed;
                onEncounterEnd?.Invoke();
            }
        }
        protected void FinishEncounter()
        {
            if (CurrentState == State.Completed)
            {
                CurrentState = State.Finished;
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            ClearActions();
        }
    }
}