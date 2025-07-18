using Entity;
using EventBus;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Encounter : MonoBehaviour
{
    [SerializeField] List<EntityType> entities = new List<EntityType>();
    [SerializeField] List<Transform> spawnPoints;
    int entityCount;
    ISpawnStrategy spawnStrategy;
    [SerializeField] SpawnStrategyType spawnStrategyType;
    [SerializeField] GameObject checkpoint;
    [SerializeField] protected UnityEvent onEncounterEnd, onEncounterStart, onEncounterReset;
    public bool Completed { get; private set; }
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
        onEncounterReset?.Invoke();
        Completed = false;
    }
    /// <summary>
    /// Begin spawning the entities in this encounter.
    /// </summary>
    public void StartEncounter()
    {
        if (!Completed)
        {
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
        onEncounterEnd?.Invoke();
        Completed = true;
        if (checkpoint)
        {
            Level.CurrentLevel.CheckpointReached();
            checkpoint.SetActive(true);
        }
    }
}
