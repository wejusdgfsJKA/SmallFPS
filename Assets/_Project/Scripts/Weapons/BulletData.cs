using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponData")]
    [System.Serializable]
    public class BulletData : ScriptableObject
    {
        public BulletType Type;
        public Bullet Prefab;
    }
}
