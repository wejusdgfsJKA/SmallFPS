using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponData")]
    [System.Serializable]
    public class WeaponData : ScriptableObject
    {
        /// <summary>
        /// Maximum ammo the weapon can have.
        /// </summary>
        public int MaxAmmo;
        /// <summary>
        /// How much ammo is gained per second when not firing.
        /// </summary>
        public float AmmoRecharge;
        /// <summary>
        /// Cooldown between regular shots.
        /// </summary>
        public float Cooldown;
        /// <summary>
        /// How much ammo the regular fire costs per shot.
        /// </summary>
        public float FireCost;
        /// <summary>
        /// How much ammo the altfire costs per shot.
        /// </summary>
        public float AltFireCost;
        /// <summary>
        /// Normal fire bullet.
        /// </summary>
        public BulletData Bullet;
        /// <summary>
        /// Altfire bullet.
        /// </summary>
        public BulletData AltBullet;
        /// <summary>
        /// What layers should the weapon target?
        /// </summary>
        public LayerMask TargetMask = 1 << 0 | 1 << 6 | 1 << 7;
    }
}