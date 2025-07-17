using KBCore.Refs;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(LineRenderer))]
    public class HitscanBullet : Bullet
    {
        /// <summary>
        /// The layers that will take damage from this bullet.
        /// </summary>
        protected LayerMask hitMask = 1 << 0 | 1 << 6;
        /// <summary>
        /// Raycast distance.
        /// </summary>
        protected int range = 100;
        protected RaycastHit hit;
        [SerializeField, Self] protected LineRenderer lineRenderer;
        public override void Init(BulletData data)
        {
            base.Init(data);
            var d = data as HitscanBulletData;
            if (d != null)
            {
                hitMask = d.HitMask;
                range = d.Range;
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = lineRenderer.endWidth = d.LineWidth;
                lineRenderer.material = d.Material;
            }
            else
            {
                Debug.LogError($"Invalid hitscan bullet data for bullet of Type {type}. Using default parameters.");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            lineRenderer?.SetPosition(0, transform.position);
            //fire a ray
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, hitMask))
            {
                OnHit(hit.collider.transform.root);
                OnHit(hit.point);
                lineRenderer?.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer?.SetPosition(1, transform.forward * range);
            }
        }
    }
}