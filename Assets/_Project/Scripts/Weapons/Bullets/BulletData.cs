using UnityEngine;

namespace Weapon
{
    public class BulletData : ScriptableObject
    {
        public BulletType Type;
        public Bullet Prefab;
        public int Damage;
        public AudioClip AudioClip;
    }
}
