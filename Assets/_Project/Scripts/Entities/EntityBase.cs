using EventBus;
using Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Entity
{
    public class EntityBase : MonoBehaviour, Identifiable<EntityType>
    {
        #region Fields
        protected int maxHealth;
        public EntityType Type { get; protected set; }
        /// <summary>
        /// Set the entity's parameters.
        /// </summary>
        public EntityData Data
        {
            set
            {
                Type = value.Type;
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
                return Type;
            }
        }

        protected List<Type> eventBindings = new() { typeof(OnDamageTaken), typeof(OnDeath) };
        #endregion
        protected void OnEnable()
        {
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
        public void TakeDamage(TakeDamage @event)
        {
            EventBus<OnDamageTaken>.Raise(transform.GetInstanceID(), new OnDamageTaken(@event.DmgInfo, this));
            CurrentHealth -= @event.DmgInfo.Damage;
            if (CurrentHealth <= 0)
            {
                Die(@event.DmgInfo);
            }
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
        public void RegisterEventBindings()
        {
            EventBus<OnDamageTaken>.AddBinding(transform.GetInstanceID());
            EventBus<OnDeath>.AddBinding(transform.GetInstanceID());
            EventBus<TakeDamage>.AddBinding(transform.GetInstanceID());
            EventBus<TakeDamage>.AddActions(transform.GetInstanceID(), TakeDamage);
        }
        protected void OnDisable()
        {
            ClearEventBindings();
            if (EntityManager.Instance != null)
            {
                EntityManager.Instance.DeRegister(this);
            }
            else
            {
                Debug.LogError("No EntityManager instance found!");
            }
        }
        protected void ClearEventBindings()
        {
            EventBus<OnDamageTaken>.ClearBinding(transform.GetInstanceID());
            EventBus<OnDeath>.ClearBinding(transform.GetInstanceID());
            EventBus<TakeDamage>.ClearBinding(transform.GetInstanceID());
            EventBus<TakeDamage>.RemoveActions(transform.GetInstanceID(), TakeDamage);
        }
    }
}