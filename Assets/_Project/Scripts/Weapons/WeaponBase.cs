using UnityEngine;

namespace Weapon
{
    public class WeaponBase : MonoBehaviour
    {

        #region Fields
        protected RaycastHit hit;
        protected LayerMask targetMask = 1 << 0 | 1 << 6;
        [SerializeField] protected WeaponData weaponData;
        protected float cooldown;
        public WeaponData Parameters
        {
            set
            {
                cooldown = value.Cooldown;
                maxAmmo = value.MaxAmmo;
                ammoRecharge = value.AmmoRecharge;
                fireCost = value.FireCost;
                altFireCost = value.AltFireCost;
                bullet = value.Bullet;
                altBullet = value.AltBullet;
                targetMask = value.TargetMask;
                ResetWeapon();
            }
        }
        protected BulletData bullet, altBullet;
        /// <summary>
        /// Where the shots are coming from.
        /// </summary>
        [SerializeField] protected Transform shootPoint;
        /// <summary>
        /// Maximum ammo of the weapon.
        /// </summary>
        protected float maxAmmo;
        /// <summary>
        /// Maximum ammo of the weapon.
        /// </summary>
        public float MaxAmmo
        {
            get
            {
                return maxAmmo;
            }
        }
        protected float ammo;
        /// <summary>
        /// Current ammo of the weapon.
        /// </summary>
        public float Ammo
        {
            get
            {
                return ammo;
            }
            set
            {
                float newValue = Mathf.Clamp(value, 0, maxAmmo);
                if (ammo != newValue)
                {
                    ammo = newValue;
                    OnAmmoValueChanged.Invoke(ammo / maxAmmo);
                }
            }
        }
        /// <summary>
        /// Fires every time the current ammo of the weapon changes.
        /// </summary>
        public event System.Action<float> OnAmmoValueChanged = delegate { };
        /// <summary>
        /// How many ammo units are replenished per second.
        /// </summary>
        protected float ammoRecharge;
        /// <summary>
        /// Are we currently firing?
        /// </summary>
        [field: SerializeField] public bool Firing { get; protected set; }
        /// <summary>
        /// How much ammo does it cost per regular shot.
        /// </summary>
        protected float fireCost;
        /// <summary>
        /// How much ammo does it cost per alternate shot.
        /// </summary>
        protected float altFireCost;
        protected AudioSource audioSource;
        protected float timeLastShot = -1;
        #endregion
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (shootPoint == null)
            {
                shootPoint = transform;
            }
            Parameters = weaponData;
        }
        /// <summary>
        /// Reset the ammo of the weapon.
        /// </summary>
        public void ResetWeapon()
        {
            Ammo = maxAmmo;
        }
        protected void Update()
        {
            if (Firing)
            {
                if (Time.time - timeLastShot >= cooldown)
                {
                    Fire();
                }
            }
            else
            {
                ReplenishAmmo(ammoRecharge * Time.deltaTime);
            }
        }
        /// <summary>
        /// Start firing normally.
        /// </summary>
        public void StartFiring()
        {
            if (Ammo >= fireCost)
            {
                Firing = true;
            }
        }
        /// <summary>
        /// Stop normal fire.
        /// </summary>
        public void StopFiring()
        {
            Firing = false;
        }
        /// <summary>
        /// Fire normally once. Calls StopFiring if it runs out of ammo.
        /// </summary>
        protected void Fire()
        {
            if (Ammo >= fireCost)
            {
                timeLastShot = Time.time;
                Ammo -= fireCost;
                OnFire();
            }
            else
            {
                StopFiring();
            }
        }
        /// <summary>
        /// Recharge ammo.
        /// </summary>
        /// <param name="recharge">By how much should the ammo recharge.</param>
        protected void ReplenishAmmo(float recharge)
        {
            Ammo += recharge;
        }
        /// <summary>
        /// Execute alternate fire once.
        /// </summary>
        public void AltFire()
        {
            if (Ammo >= altFireCost)
            {
                Ammo -= altFireCost;
                OnAltFire();
            }
        }
        /// <summary>
        /// Execute regular shooting.
        /// </summary>
        protected void OnFire()
        {
            FireBullet(bullet);
        }
        /// <summary>
        /// Execute alternate fire.
        /// </summary>
        protected void OnAltFire()
        {
            FireBullet(altBullet);
        }
        protected void OnDisable()
        {
            OnAmmoValueChanged = delegate { };
            StopFiring();
        }
        protected void OnDestroy()
        {
            OnDisable();
        }
        /// <summary>
        /// Fire a bullet from the shootPoint. Target orientation is given by the weapon's 
        /// transform.forward.
        /// </summary>
        /// <param name="bulletData">The data for the bullet to be fired.</param>
        protected void FireBullet(BulletData bulletData)
        {
            var b = BulletManager.Instance.GetBullet(bulletData);
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, targetMask))
            {
                shootPoint.LookAt(hit.point);
            }
            else
            {
                shootPoint.LookAt(transform.forward * 100);
            }
            b.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
            b.Owner = transform;
            b.gameObject.SetActive(true);
        }
    }
}