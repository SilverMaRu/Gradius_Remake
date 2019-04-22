using UnityEngine;

namespace Assets.Scripts.Class.Weapons
{
    public class DoubleWeapon: Weapon
    {
        public GameObject secondaryBulletPre { get; protected set; }
        public Transform secondaryBarrelTrans { get; protected set; }

        public DoubleWeapon(string bulletPath, Transform barrelTrans,Transform secondaryBarrelTrans,  float shootFrequency) :this(bulletPath, bulletPath, barrelTrans, secondaryBarrelTrans, shootFrequency)
        {
        }

        public DoubleWeapon(string bulletPath, string secondaryBulletPath, Transform barrelTrans, Transform secondaryBarrelTrans, float shootFrequency): base(bulletPath, barrelTrans, shootFrequency)
        {
            secondaryBulletPre = Resources.Load<GameObject>(secondaryBulletPath);
            this.secondaryBarrelTrans = secondaryBarrelTrans;
        }

        protected override void Shoot()
        {
            base.Shoot();
            Object.Instantiate(secondaryBulletPre, secondaryBarrelTrans.position, secondaryBarrelTrans.rotation);
        }
    }
}
