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
            public BaseClass.Team team { get; set; }

            public WeaponInitInfo(string bulletPath, GameObject barrelGameObj, float shootFrequency, BaseClass.Team team)
            {
                this.bulletPath = bulletPath;
                this.barrelGameObj = barrelGameObj;
                this.shootFrequency = shootFrequency;
                this.team = team;
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
        public BaseClass.Team team { get; protected set; }
        public WeaponInitInfo info { get; protected set; }

        private float lastShootTime;

        public Weapon(string bulletPath, Transform barrelTrans, BaseClass.Team team) : this(bulletPath, barrelTrans, 0, team)
        {

        }

        public Weapon(string bulletPath, Transform barrelTrans, float shootFrequency, BaseClass.Team team)
        {
            info = new WeaponInitInfo(bulletPath, barrelTrans.gameObject, shootFrequency, team);
            bulletPre = Resources.Load<GameObject>(bulletPath);
            this.barrelTrans = barrelTrans;
            this.shootFrequency = shootFrequency;
            this.team = team;
            lastShootTime = -shootFrequency;
        }

        public Weapon(WeaponInitInfo info)
        {
            this.info = info;
            bulletPre = Resources.Load<GameObject>(info.bulletPath);
            barrelTrans = info.barrelGameObj.transform;
            shootFrequency = info.shootFrequency;
            team = info.team;
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
            GameObject instance = Object.Instantiate(bulletPre, barrelTrans.position, barrelTrans.rotation);
            BaseClass.Something something = instance.GetComponent<BaseClass.Something>();
            something.team = team;
        }

        protected virtual bool CanShoot()
        {
            return bulletPre != null && barrelTrans != null && Time.time - lastShootTime > shootFrequency;
        }

        public void SetShootFrequency(float shootFrequency)
        {
            this.shootFrequency = shootFrequency;
            info.shootFrequency = shootFrequency;
        }
    }
}

