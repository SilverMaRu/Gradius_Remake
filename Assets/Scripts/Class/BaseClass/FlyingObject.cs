using UnityEngine;

namespace Assets.Scripts.Class.BaseClass
{
    public class FlyingObject : Entity
    {
        public float maxSpeed = 10;
        public float baseSpeed = 7;
        public float currentSpeed { get; protected set; }
        public bool isNormalized = false;
        public bool orientToDirection = false;
        public Weapons.Weapon.WeaponInitInfo[] normalWeaponInfos;

        public Weapons.Weapon[] normalWeapons { get; protected set; }
        public int currentWeaponIdx { get; protected set; }

        protected override void InitCurrentAttr()
        {
            base.InitCurrentAttr();
            currentSpeed = baseSpeed;
            currentWeaponIdx = 0;
        }

        protected override void Init()
        {
            base.Init();
            InitWeapon();
        }

        protected virtual void InitWeapon()
        {
            if (normalWeaponInfos != null && normalWeaponInfos.Length > 0)
            {
                normalWeapons = new Weapons.Weapon[normalWeaponInfos.Length];
                for (int i = 0; i < normalWeapons.Length; i++)
                {
                    if (normalWeaponInfos[i].IsEffective())
                    {
                        normalWeaponInfos[i].team = team;
                        normalWeapons[i] = new Weapons.Weapon(normalWeaponInfos[i], 15);
                    }
                }
            }
        }

        protected virtual void Move()
        {
            Vector3 moveDirection = GetMoveDirection();
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
            if (orientToDirection)
            {
                transform.right = moveDirection;
            }
        }

        protected virtual Vector3 GetMoveDirection()
        {
            Vector3 direction = transform.right;
            return direction;
        }

        protected virtual void Shoot()
        {
            normalWeapons[currentWeaponIdx].TryShoot();
        }
    }
}
