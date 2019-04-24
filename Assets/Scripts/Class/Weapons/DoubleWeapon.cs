using UnityEngine;

namespace Assets.Scripts.Class.Weapons
{
    public class DoubleWeapon: Weapon
    {
        public GameObject secondaryBulletPre { get; protected set; }
        public Transform secondaryBarrelTrans { get; protected set; }

        public DoubleWeapon(string bulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency, BaseClass.Team team
            , int initCapacity) : this(bulletPath, barrelTrans, secondaryBarrelTrans, shootFrequency, team
            , initCapacity, () => { })
        { }

        public DoubleWeapon(string bulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency, BaseClass.Team team
            , int initCapacity, System.Action recyclingOtherAction)
            : this(bulletPath, bulletPath, barrelTrans, secondaryBarrelTrans, shootFrequency, team, initCapacity, recyclingOtherAction)
        {
        }

        public DoubleWeapon(string bulletPath, string secondaryBulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency, BaseClass.Team team
            , int initCapacity) : this(bulletPath, secondaryBulletPath, barrelTrans, secondaryBarrelTrans, shootFrequency, team
            , initCapacity, () => { })
        { }

        public DoubleWeapon(string bulletPath, string secondaryBulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency, BaseClass.Team team
            , int initCapacity, System.Action recyclingOtherAction)
            : base(bulletPath, barrelTrans, shootFrequency, team, initCapacity, recyclingOtherAction)
        {
            secondaryBulletPre = Resources.Load<GameObject>(secondaryBulletPath);
            this.secondaryBarrelTrans = secondaryBarrelTrans;
        }

        protected override void Shoot()
        {
            base.Shoot();
            //GameObject instance = Object.Instantiate(secondaryBulletPre, secondaryBarrelTrans.position, secondaryBarrelTrans.rotation);
            GameObject instance = bulletPool.Get(barrelTrans.position, barrelTrans.rotation);
            BaseClass.Something something = instance.GetComponent<BaseClass.Something>();
            something.team = team;
            //something.objectPool = bulletPool;
        }
    }
}
