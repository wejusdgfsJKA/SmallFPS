using Entity;
using EventBus;
using KBCore.Refs;
using Pooling;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
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
        protected CountdownTimer timer;
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
                timer = new CountdownTimer(data.Duration);
                timer.OnTimerStop += () => gameObject.SetActive(false);
            }
        }
        protected virtual void OnEnable()
        {
            audioSource.Play();
            timer?.Start();
        }
        protected void OnDisable()
        {
            BulletManager.Instance.Release(this);
        }
        protected void Update()
        {
            timer?.Tick(Time.deltaTime);
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