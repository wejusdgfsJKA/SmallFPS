using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Data for an explosion.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/ExplosionData")]
    [System.Serializable]
    public class ExplosionData : BulletData
    {
        /// <summary>
        /// The layers that will take damage from the explosion.
        /// </summary>
        public LayerMask HitMask = 1 << 0 | 1 << 6 | 1 << 7;
        /// <summary>
        /// The layers that will block the explosion.
        /// </summary>
        public LayerMask ObstructionMask = 1 << 0;
        /// <summary>
        /// Explosion distance.
        /// </summary>
        public int Range = 100;
    }
}
