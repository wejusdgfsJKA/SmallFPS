using Pooling;
using UnityEngine;

namespace Weapon
{
    public class BulletManager : MonoBehaviour
    {
        public static BulletManager Instance { get; private set; }
        MultiPool<BulletType, Bullet> multiPool = new();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        /// <summary>
        /// Get a new bullet from a scriptable object. Will instantiate and initialize a new one if the respective pool is empty.
        /// </summary>
        /// <param name="data">The data to use to get the bullet.</param>
        /// <returns>The bullet object.</returns>
        public Bullet GetBullet(BulletData data)
        {
            var b = multiPool.Get(data.Type);
            if (b == null)
            {
                b = Instantiate(data.Prefab);
                b.Init(data);
            }
            return b;
        }
        /// <summary>
        /// Return a bullet to the pool.
        /// </summary>
        /// <param name="bullet">The bullet that must be returned to the pool.</param>
        public void Release(Bullet bullet)
        {
            multiPool.Release(bullet);
        }
    }
}