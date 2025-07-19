using Entity;
using EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Encounter : MonoBehaviour
{
    public enum State
    {
        Waiting,
        Running,
        Completed
    }
    [SerializeField] List<EntityType> entities = new List<EntityType>();
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] int entityCount;
    ISpawnStrategy spawnStrategy;
    [SerializeField] SpawnStrategyType spawnStrategyType;
    [SerializeField] GameObject checkpoint;
    [SerializeField] protected UnityEvent onEncounterEnd, onEncounterStart, onEncounterReset;
    public State CurrentState { get; private set; } = State.Waiting;
    [SerializeField] float initialDelay = 1, inBetweenDelay = 0;
    private void Awake()
    {
        spawnStrategy = spawnStrategyType switch
        {
            SpawnStrategyType.Linear => new LinearSpawnStrategy(spawnPoints),
            _ => spawnStrategy
        };
    }
    private void OnEnable()
    {
        Level.CurrentLevel.RegisterEncounter(this);
    }
    public void ResetEncounter()
    {
        if (CurrentState != State.Waiting)
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
    IEnumerator SpawnEntities()
    {
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < entityCount; i++)
        {
            //spawn this entity
            var e = EntityManager.Instance.Spawn(entities[i], spawnStrategy.GetSpawnPoint());
            EventBus<OnDeath>.AddActions(e.transform.GetInstanceID(), null, OnEntityDeath);
            yield return new WaitForSeconds(inBetweenDelay);
        }
    }
    public void OnEntityDeath()
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
            if (checkpoint)
            {
                Level.CurrentLevel.CheckpointReached();
                checkpoint.SetActive(true);
            }
        }
    }
}