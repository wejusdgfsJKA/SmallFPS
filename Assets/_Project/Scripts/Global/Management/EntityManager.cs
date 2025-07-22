using EventBus;
using Levels;
using Pooling;
using System.Collections.Generic;
using UnityEngine;
namespace Entity
{
    /// <summary>
    /// Manages entity pooling, spawning, damage and death.
    /// </summary>
    public class EntityManager : MonoBehaviour
    {
        public static EntityManager Instance { get; private set; }
        /// <summary>
        /// Active entities, identified by their root InstanceID.
        /// </summary>
        public Dictionary<int, EntityBase> Entities { get; } = new();
        /// <summary>
        /// All entity pools.
        /// </summary>
        protected MultiPool<EntityType, EntityBase> multiPool = new();
        /// <summary>
        /// All entities the manager can spawn.
        /// </summary>
        protected Dictionary<EntityType, EntityData> roster = new();
        /// <summary>
        /// Transform of currently active player.
        /// </summary>
        public Transform Player { get; protected set; }
        [SerializeField] List<EntityData> rosterEntries = new();
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            for (int i = 0; i < rosterEntries.Count; i++)
            {
                roster.Add(rosterEntries[i].Type, rosterEntries[i]);
            }
            EventBus<PlayerDeath>.AddActions(0, null, TerminateAll);
        }
        /// <summary>
        /// Register an existing entity. This should only be called in OnEnable in EntityBase.
        /// </summary>
        /// <param name="entity">The entity to be registered.</param>
        /// <returns>True if registration successfull.</returns>
        public bool Register(EntityBase entity)
        {
            if (Entities.TryAdd(entity.transform.GetInstanceID(), entity))
            {
                if (entity.Type == EntityType.Player)
                {
                    Player = entity.transform;
                }
                return true;
            }
            return false;
        }
        public EntityBase Spawn(EntityType entityType, Transform transform)
        {
            return Spawn(entityType, transform.position, transform.rotation);
        }
        /// <summary>
        /// Spawn a new enemy, or get one from the entity pool. The spawned enemy will be inactive.
        /// </summary>
        /// <param name="id">The id of the enemy (it's Type, not its unique identifier).</param>
        /// <param name="position">Spawn position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <returns>An instance of the enemy if existing pool/roster entry was found, null otherwise</returns>
        public EntityBase Spawn(EntityType id, Vector3 position, Quaternion rotation)
        {
            if (id == EntityType.Player && Player != null) return null;
            EntityBase e = multiPool.Get(id);
            if (e == null)
            {
                EntityData data;
                if (roster.TryGetValue(id, out data))
                {
                    e = Instantiate(data.Prefab, position, rotation);
                    //we need to set the entity's data for the first time.
                    e.Data = data;
                }
                else
                {
                    Debug.LogError($"Id {id} not found in entity roster!");
                    return null;
                }
            }
            else
            {
                e.transform.position = position;
                e.transform.rotation = rotation;
            }
            return e;
        }
        /// <summary>
        /// Add an entity to its coresponding pool, identified by its EntityType field.
        /// </summary>
        /// <param name="entity">The entity will be added to the pool.</param>
        protected void AddToPool(EntityBase entity)
        {
            multiPool.Release(entity);
        }
        /// <summary>
        /// This entity just died, will be added to the pool.
        /// </summary>
        /// <param name="entity">The entity that died.</param>
        public void DeRegister(EntityBase entity)
        {
            AddToPool(entity);
            Entities.Remove(entity.transform.GetInstanceID());
            if (entity.Type == EntityType.Player)
            {
                Player = null;
                EventBus<PlayerDeath>.Raise(0, new());
            }
        }
        public void TerminateAll()
        {
            EntityBase[] entities = new EntityBase[Entities.Count];
            Entities.Values.CopyTo(entities, 0);
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i].gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            Entities.Clear();
            multiPool.Clear();
            roster.Clear();
        }
    }
}