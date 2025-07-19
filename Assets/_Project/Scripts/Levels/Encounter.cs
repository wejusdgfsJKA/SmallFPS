using Entity;
using EventBus;
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
    int entityCount;
    ISpawnStrategy spawnStrategy;
    [SerializeField] SpawnStrategyType spawnStrategyType;
    [SerializeField] GameObject checkpoint;
    [SerializeField] protected UnityEvent onEncounterEnd, onEncounterStart, onEncounterReset;
    public State CurrentState { get; private set; } = State.Waiting;
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
            ClearEntityBindings();
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
            for (int i = 0; i < entityCount; i++)
            {
                //spawn this entity
                var e = EntityManager.Instance.Spawn(entities[i], spawnStrategy.GetSpawnPoint());
                EventBus<OnDeath>.AddActions(e.transform.GetInstanceID(), null, OnEntityDeath);
            }
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
            ClearEntityBindings();
            CurrentState = State.Completed;
            onEncounterEnd?.Invoke();
            if (checkpoint)
            {
                Level.CurrentLevel.CheckpointReached();
                checkpoint.SetActive(true);
            }
        }
    }
    void ClearEntityBindings()
    {

    }
}