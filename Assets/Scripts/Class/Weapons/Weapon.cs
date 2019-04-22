using UnityEngine;

namespace Assets.Scripts.Class.Weapons
{
    public class Weapon
    {
        [System.Serializable]
        public class WeaponInitInfo
        {
            public string bulletPath;
            public GameObject barrelGameObj;
            public float shootFrequency;

            public WeaponInitInfo(string bulletPath, GameObject barrelGameObj, float shootFrequency)
            {
                this.bulletPath = bulletPath;
                this.barrelGameObj = barrelGameObj;
                this.shootFrequency = shootFrequency;
            }

            public bool IsEffective()
            {
                return !string.Empty.Equals(bulletPath.Trim())
                    && barrelGameObj != null;
            }
        }

        public GameObject bulletPre { get; protected set; }
        public Transform barrelTrans { get; protected set; }
        public float shootFrequency { get; protected set; }
        public WeaponInitInfo info { get; protected set; }

        private float lastShootTime;

        public Weapon(string bulletPath, Transform barrelTrans) : this(bulletPath, barrelTrans, 0)
        {

        }

        public Weapon(string bulletPath, Transform barrelTrans, float shootFrequency)
        {
            info = new WeaponInitInfo(bulletPath, barrelTrans.gameObject, shootFrequency);
            bulletPre = Resources.Load<GameObject>(bulletPath);
            this.barrelTrans = barrelTrans;
            this.shootFrequency = shootFrequency;
            lastShootTime = -shootFrequency;
        }

        public Weapon(WeaponInitInfo info)
        {
            this.info = info;
            bulletPre = Resources.Load<GameObject>(info.bulletPath);
            barrelTrans = info.barrelGameObj.transform;
            shootFrequency = info.shootFrequency;
            lastShootTime = -shootFrequency;
        }

        public void TryShoot()
        {
            if(CanShoot())
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }

        protected virtual void Shoot()
        {
            Object.Instantiate(bulletPre, barrelTrans.position, barrelTrans.rotation);
        }

        protected virtual bool CanShoot()
        {
            return bulletPre != null && barrelTrans != null && Time.time - lastShootTime > shootFrequency;
        }
    }
}

