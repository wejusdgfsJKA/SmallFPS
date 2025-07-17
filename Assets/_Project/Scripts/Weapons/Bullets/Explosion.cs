using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// A damaging sphere.
    /// </summary>
    public class Explosion : Bullet
    {
        /// <summary>
        /// The layers that will take damage from the explosion.
        /// </summary>
        protected LayerMask hitMask = 1 << 0 | 1 << 6;
        /// <summary>
        /// The layers that will block the explosion.
        /// </summary>
        protected LayerMask obstructionMask = 1 << 0;
        /// <summary>
        /// Explosion distance.
        /// </summary>
        public int Range { get; protected set; } = 100;
        protected RaycastHit hit;
        /// <summary>
        /// All entities we have hit.
        /// </summary>
        protected HashSet<int> hits = new HashSet<int>();
        public override void Init(BulletData data)
        {
            base.Init(data);
            var d = data as ExplosionData;
            if (d != null)
            {
                hitMask = d.HitMask;
                Range = d.Range;
                obstructionMask = d.ObstructionMask;
            }
            else
            {
                Debug.LogError($"Invalid explosion data for explosion of Type {type}. Using default parameters.");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            hits.Clear();
            Collider[] cols = new Collider[10];
            int nrOfHits = Physics.OverlapSphereNonAlloc(transform.position, 5, cols, hitMask);
            for (int i = 0; i < nrOfHits; i++)
            {
                if (hits.Contains(cols[i].transform.root.GetInstanceID()))
                {
                    //make sure an explosion doesn't multi hit critters with multiple colliders
                    continue;
                }
                if (Physics.Linecast(transform.position, cols[i].transform.position, out hit, obstructionMask))
                {
                    if (hit.colliderInstanceID != cols[i].GetInstanceID())
                    {
                        //this collider is obstructed
                        continue;
                    }
                }
                base.OnHit(cols[i].transform.root);
                hits.Add(cols[i].transform.root.GetInstanceID());
            }
        }
    }
}