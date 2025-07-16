using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponData")]
    [System.Serializable]
    public class WeaponData : ScriptableObject
    {
        public int MaxAmmo;
        public float AmmoRecharge;
        public float Cooldown;
        public float FireCost;
        public float AltFireCost;
        public BulletData Bullet;
        public BulletData AltBullet;
    }
}