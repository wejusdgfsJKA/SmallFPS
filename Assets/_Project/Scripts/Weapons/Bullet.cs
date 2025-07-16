using Pooling;
using UnityEngine;
namespace Weapon
{
    public abstract class Bullet : MonoBehaviour, Identifiable<BulletType>
    {
        #region Fields
        protected BulletType type;
        Transform owner;
        public Transform Owner
        {
            set
            {
                owner = value;
            }
        }
        public BulletData Parameters
        {
            set
            {
                type = value.Type;
            }
        }
        public BulletType ID
        {
            get
            {
                return type;
            }
        }
        #endregion
        protected void OnDisable()
        {
            BulletManager.Instance.Release(this);
        }
    }
}