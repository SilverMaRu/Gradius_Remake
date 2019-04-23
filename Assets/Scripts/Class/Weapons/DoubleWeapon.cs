using UnityEngine;

namespace Assets.Scripts.Class.Weapons
{
    public class DoubleWeapon: Weapon
    {
        public GameObject secondaryBulletPre { get; protected set; }
        public Transform secondaryBarrelTrans { get; protected set; }

        public DoubleWeapon(string bulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency, BaseClass.Team team)
            : this(bulletPath, bulletPath, barrelTrans, secondaryBarrelTrans, shootFrequency, team)
        {
        }

        public DoubleWeapon(string bulletPath, string secondaryBulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency, BaseClass.Team team)
            : base(bulletPath, barrelTrans, shootFrequency, team)
        {
            secondaryBulletPre = Resources.Load<GameObject>(secondaryBulletPath);
            this.secondaryBarrelTrans = secondaryBarrelTrans;
        }

        protected override void Shoot()
        {
            base.Shoot();
            GameObject instance = Object.Instantiate(secondaryBulletPre, secondaryBarrelTrans.position, secondaryBarrelTrans.rotation);
            BaseClass.Something something = instance.GetComponent<BaseClass.Something>();
            something.team = team;
        }
    }
}
