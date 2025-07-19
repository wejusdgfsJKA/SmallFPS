using KBCore.Refs;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : Bullet
    {
        [SerializeField, Self] protected Rigidbody rb;
        [SerializeField, Self] protected Collider coll;
        protected float Velocity = 1;
        public override void Init(BulletData data)
        {
            base.Init(data);
            var d = data as ProjectileData;
            if (d != null)
            {
                Velocity = d.Velocity;
                gameObject.layer = d.Layer;
            }
            else
            {
                Debug.LogError($"Invalid projectile data for bullet of Type {type}. Using default parameters.");
            }
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.isKinematic = false;
            coll.isTrigger = true;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            rb.velocity = transform.forward * Velocity;
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform.root != dmgInfo.Source)
            {
                OnHit(transform.position);
                OnHit(other.transform.root);
                gameObject.SetActive(false);
            }
        }
    }
}