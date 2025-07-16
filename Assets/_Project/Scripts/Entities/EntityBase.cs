using Detection;
using EventBus;
using Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Entity
{
    public class EntityBase : DetectableTarget, Identifiable<EntityType>
    {
        #region Fields
        protected int maxHealth;
        protected EntityType type;
        /// <summary>
        /// Set the entity's parameters.
        /// </summary>
        public EntityData Data
        {
            set
            {
                type = value.Type;
                maxHealth = value.MaxHealth;
            }
        }
        /// <summary>
        /// How much health the entity currently has.
        /// </summary>
        public int CurrentHealth { get; protected set; }
        public EntityType ID
        {
            get
            {
                return type;
            }
        }

        protected List<Type> eventBindings = new() { typeof(OnDamageTaken), typeof(OnDeath) };
        #endregion
        protected override void OnEnable()
        {
            base.OnEnable();
            //register events
            RegisterEventBindings();
            //register entity
            if (EntityManager.Instance != null)
            {
                if (!EntityManager.Instance.Register(this))
                {
                    Debug.LogError($"Entity {transform} unable to register.");
                }
            }
            else
            {
                Debug.LogError("No EntityManager instance found!");
            }
            CurrentHealth = maxHealth;
        }
        /// <summary>
        /// Receive an attack. Fires OnDamageTaken event binding.
        /// </summary>
        /// <param name="dmgInfo">Damage package.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void TakeDamage(DmgInfo dmgInfo)
        {
            EventBus<OnDamageTaken>.Raise(transform.GetInstanceID(), new OnDamageTaken(dmgInfo, this));
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// This entity just died. Fire OnDeath(this, DmgInfo).
        /// </summary>
        /// <param name="dmgInfo">The damage package that caused the death.</param>
        protected void Die(DmgInfo dmgInfo)
        {
            EventBus<OnDeath>.Raise(transform.GetInstanceID(), new OnDeath(dmgInfo, this));
            transform.root.gameObject.SetActive(false);
        }
        public override void RegisterEventBindings()
        {
            EventBus<OnDamageTaken>.AddBinding(transform.GetInstanceID());
            EventBus<OnDeath>.AddBinding(transform.GetInstanceID());
            EventBus<TakeDamage>.AddBinding(transform.GetInstanceID());
        }
        protected void OnDisable()
        {
            if (EntityManager.Instance != null)
            {
                EntityManager.Instance.DeRegister(this);
            }
            else
            {
                Debug.LogError("No EntityManager instance found!");
            }
        }
        protected void OnDestroy()
        {
            OnDisable();
        }
    }
}