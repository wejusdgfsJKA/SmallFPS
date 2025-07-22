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
        public Bullet GetBullet(BulletData data)
        {
            var b = multiPool.Get(data.Type);
            if (b == null)
            {
                b = Instantiate(data.Prefab);
                b.Init(data);
            }
            b.transform.parent = null;
            return b;
        }
        public void Release(Bullet bullet)
        {
            multiPool.Release(bullet);
        }
    }
}