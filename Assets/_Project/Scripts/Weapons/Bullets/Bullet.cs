using Entity;
using EventBus;
using KBCore.Refs;
using Levels;
using Pooling;
using System.Collections.Generic;
using UnityEngine;
namespace Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Bullet : ValidatedMonoBehaviour, Identifiable<BulletType>
    {
        #region Fields
        protected BulletType type;
        /// <summary>
        /// Whoever owns this bullet.
        /// </summary>
        public Transform Owner
        {
            set
            {
                dmgInfo.Source = value;
            }
        }
        public BulletType ID
        {
            get
            {
                return type;
            }
        }
        protected DmgInfo dmgInfo = new();
        /// <summary>
        /// What other bullets should this one spawn on hit?
        /// </summary>
        protected List<BulletData> spawnOnHit = new();
        [SerializeField, Self] protected AudioSource audioSource;
        protected float duration = 1;
        protected float timeActivated = -1;
        #endregion
        /// <summary>
        /// Initialize this bullet. Should only be called in BulletManager. Derived classed 
        /// better f*ckin cast the BulletData object instead of writing some other sh*t.
        /// </summary>
        /// <param name="data">Scriptable object containing all of the data pertaining to the bullet.</param>
        public virtual void Init(BulletData data)
        {
            type = data.Type;
            dmgInfo.Damage = data.Damage;
            spawnOnHit = data.SpawnOnHit;
            audioSource.clip = data.AudioClip;
            audioSource.playOnAwake = false;
            if (data.Duration > 0)
            {
                duration = data.Duration;
            }
        }
        protected void Deactivate()
        {
            gameObject?.SetActive(false);
        }
        protected virtual void OnEnable()
        {
            EventBus<PlayerDeath>.AddActions(0, null, Deactivate);
            if (audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
            timeActivated = Time.time;
        }
        protected void OnDisable()
        {
            if (BulletManager.Instance != null)
            {
                BulletManager.Instance.Release(this);
            }
            else
            {
                Debug.LogError("No BulletManager instance found!");
            }
            EventBus<PlayerDeath>.RemoveActions(0, null, Deactivate);
        }
        protected void Update()
        {
            if (Time.time - timeActivated >= duration)
            {
                gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// When hitting something, fire this at the point of contact.
        /// </summary>
        /// <param name="point">The point of contact for the bullet.</param>
        protected virtual void OnHit(Vector3 point)
        {
            for (int i = 0; i < spawnOnHit.Count; i++)
            {
                var b = BulletManager.Instance.GetBullet(spawnOnHit[i]);
                b.transform.position = point;
                b.transform.rotation = transform.rotation;
                b.Owner = dmgInfo.Source;
                b.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// When hitting an object, fire this for that object.
        /// </summary>
        /// <param name="object">The object we hit.</param>
        protected virtual void OnHit(Transform @object)
        {
            EventBus<TakeDamage>.Raise(@object.GetInstanceID(), new TakeDamage(dmgInfo));
        }
    }
}