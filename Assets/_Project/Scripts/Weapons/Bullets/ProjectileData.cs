using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ProjectileData")]
    [System.Serializable]
    public class ProjectileData : BulletData
    {
        public int Layer=0;
        public float Velocity=1;
    }
}