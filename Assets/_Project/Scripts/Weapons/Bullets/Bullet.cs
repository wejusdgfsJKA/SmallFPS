using Pooling;
using UnityEngine;
namespace Weapon
{
    public abstract class Bullet : MonoBehaviour, Identifiable<BulletType>
    {
        #region Fields
        protected BulletType type;
        public Transform Owner
        {
            set
            {
                dmgInfo.Source = value;
            }
        }
        public BulletType ID
        {
            get
            {
                return type;
            }
        }
        protected DmgInfo dmgInfo = new();
        #endregion
        public virtual void Init(BulletData data)
        {
            type = data.Type;
            dmgInfo.Damage = data.Damage;
        }
        protected void OnDisable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            BulletManager.Instance.Release(this);
        }
    }
}