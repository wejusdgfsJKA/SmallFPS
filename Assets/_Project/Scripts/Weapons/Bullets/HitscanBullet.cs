using Entity;
using EventBus;
using UnityEngine;
using Utilities;

namespace Weapon
{
    public class HitscanBullet : Bullet
    {
        protected LayerMask hitMask = 1 << 0 | 1 << 6;
        protected int range = 100;
        protected RaycastHit hit;
        protected CountdownTimer timer;
        public override void Init(BulletData data)
        {
            base.Init(data);
            var d = data as HitscanBulletData;
            if (d != null)
            {
                hitMask = d.HitMask;
                range = d.Range;
                timer = new CountdownTimer(d.Duration);
            }
            else
            {
                timer = new CountdownTimer(1);
                Debug.LogError($"Invalid hitscan bullet data for bullet of Type {type}. Using default parameters.");
            }
        }
        protected void OnEnable()
        {
            //fire a ray
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, hitMask))
            {
                OnHit(hit.collider.transform.root);
            }
            timer.Start();
            timer.OnTimerStop += () => gameObject.SetActive(false);
        }
        protected void Update()
        {
            timer.Tick(Time.deltaTime);
        }
        public virtual void OnHit(Transform @object)
        {
            EventBus<TakeDamage>.Raise(@object.GetInstanceID(), new TakeDamage(dmgInfo));
        }
    }
}