using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class BulletData : ScriptableObject
    {
        public BulletType Type;
        public Bullet Prefab;
        public int Damage;
        public AudioClip AudioClip;
        /// <summary>
        /// What other bullets to spawn on hit?
        /// </summary>
        public List<BulletData> SpawnOnHit = new();
        /// <summary>
        /// Maximum life time of the bullet. Set this to a high number for projectiles.
        /// </summary>
        public float Duration = 1;
    }
}
