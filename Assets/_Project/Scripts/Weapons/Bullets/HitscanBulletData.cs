using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Data for a hitscan bullet.
    /// </summary>

    [CreateAssetMenu(menuName = "ScriptableObjects/HitscanBulletData")]
    [System.Serializable]
    public class HitscanBulletData : BulletData
    {
        /// <summary>
        /// The layers that will be damaged by this bullet.
        /// </summary>
        public LayerMask HitMask = 1 << 0 | 1 << 6;
        /// <summary>
        /// Raycast distance.
        /// </summary>
        public int Range = 100;
        /// <summary>
        /// LineRenderer line width.
        /// </summary>
        public float LineWidth = 0.05f;
        /// <summary>
        /// LineRenderer material.
        /// </summary>
        public Material Material;
    }
}
