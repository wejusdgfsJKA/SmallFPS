using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ProjectileData")]
    [System.Serializable]
    public class ProjectileData : BulletData
    {
        /// <summary>
        /// The layer the bullet object should be on.
        /// </summary>
        public int Layer = 0;
        /// <summary>
        /// Starting velocity of the bullet.
        /// </summary>
        public float Velocity = 1;
    }
}