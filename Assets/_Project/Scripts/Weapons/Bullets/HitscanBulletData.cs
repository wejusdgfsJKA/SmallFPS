using UnityEngine;

namespace Weapon
{

    [CreateAssetMenu(menuName = "ScriptableObjects/HitscanBulletData")]
    [System.Serializable]
    public class HitscanBulletData : BulletData
    {
        public LayerMask HitMask = 1 << 0 | 1 << 6;
        public int Range = 100;
        public float Duration = 1;
    }
}
