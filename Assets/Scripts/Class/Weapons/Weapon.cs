using UnityEngine;
using Assets.Scripts.GameObjectPool;

namespace Assets.Scripts.Class.Weapons
{
    [System.Serializable]
    public class WeaponInfo
    {
        public GameObject[] barrelGameObjs;
        public float shootFrequency;
        public string poolName;
    }

    public class Weapon
    {
        public GameObject[] barrelGameObjs;
        public float shootFrequency;
        public BaseClass.Team team { get; set; }
        public string poolName;

        protected int usingBulletIdx;
        private float lastShootTime;
        
        public Weapon(GameObject[] barrelGameObjs, float shootFrequency, BaseClass.Team team, string poolName)
        {
            this.barrelGameObjs = barrelGameObjs;
            this.shootFrequency = shootFrequency;
            this.team = team;
            this.poolName = poolName;
            lastShootTime = -shootFrequency;
            usingBulletIdx = 0;
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
            for(int i = 0; i < barrelGameObjs.Length; i++)
            {
                GameObject instance = PoolTool.GetGameObject(poolName, barrelGameObjs[i].transform.position, barrelGameObjs[i].transform.rotation);
                BaseClass.Something something = instance.GetComponent<BaseClass.Something>();
                something.team = team;
            }
        }

        protected virtual bool CanShoot()
        {
            return barrelGameObjs != null && Time.time - lastShootTime > shootFrequency;
        }

        //public void ChangeBullet()
        //{
        //    usingBulletIdx = (usingBulletIdx + 1) % bulletInfos.Length;
        //}
    }
}

