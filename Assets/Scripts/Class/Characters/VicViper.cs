using UnityEngine;
using Assets.Scripts.Class.BaseClass;
using Assets.Scripts.Class.Weapons;

namespace Assets.Scripts.Class.Characters
{
    public class VicViper : FlyingObject
    {
        public KeyCode shootKey = KeyCode.J;
        public KeyCode usePower = KeyCode.K;
        
        public Weapon.WeaponInitInfo doubleWeaponInfo;
        public Weapon.WeaponInitInfo missileWeaponInfo;
        
        private Weapon doubleWeapon;
        private bool usingDouble;
        private Weapon missile;
        private bool usingMissile;


        public int maxPowerBoxNum = 5;
        public int basePowerBoxNum = 0;
        private int currentPowerBoxNum;

        public int revival = 1;

        protected override void Init()
        {
            base.Init();
        }

        protected override void InitCurrentAttr()
        {
            base.InitCurrentAttr();
            currentPowerBoxNum = basePowerBoxNum;
            usingDouble = false;
            usingMissile = false;
        }

        protected override void InitWeapon()
        {
            base.InitWeapon();
            if (doubleWeaponInfo.IsEffective())
            {
                doubleWeapon = new Weapon(doubleWeaponInfo);
            }
            if (missileWeaponInfo.IsEffective())
            {
                missile = new Weapon(missileWeaponInfo);
            }
        }

        protected override void Update()
        {
            if (isAlive)
            {
                base.Update();
                Move();
                if (Input.GetKeyDown(shootKey))
                {
                    Shoot();
                }

                if (Input.GetKeyDown(usePower))
                {
                    GroupUp();
                }
            }
        }

        protected override Vector3 GetMoveDirection()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 direction = (Vector3.right * h + Vector3.up * v).normalized;
            return direction;
        }

        protected override void Shoot()
        {
            base.Shoot();
            if (usingDouble)
            {
                doubleWeapon.TryShoot();
            }
            if (usingMissile)
            {
                missile.TryShoot();
            }
        }

        private void GroupUp()
        {
            bool GroupUpSuccess = false;
            switch (currentPowerBoxNum)
            {
                case 1:
                    GroupUpSuccess = GroupUpSpeed();
                    break;
                case 2:
                    GroupUpSuccess = GroupUpMissile();
                    break;
                case 3:
                    GroupUpSuccess = GroupUpDouble();
                    break;
                case 4:
                    GroupUpSuccess = GroupUpLaser();
                    break;
                case 5:
                    GroupUpSuccess = GroupUpOption();
                    break;
            }
            if (GroupUpSuccess)
            {
                currentPowerBoxNum = 0;
            }
        }

        private bool GroupUpSpeed()
        {
            if(currentSpeed >= maxSpeed)
            {
                return false;
            }
            currentSpeed++;
            return true;
        }

        private void ChangeWeapon(string bulletName)
        {
            for(int i = 0; i < normalWeapons.Length; i++)
            {
                if (normalWeapons[i].bulletPre.name.IndexOf(bulletName) > 0)
                {
                    currentWeaponIdx = i;
                }
            }
        }

        private void ChangeToDoubleWeapon()
        {
            for (int i = 0; i < normalWeapons.Length; i++)
            {
                if (normalWeapons[i] is DoubleWeapon)
                {
                    currentWeaponIdx = i;
                }
            }
        }

        private bool GroupUpMissile()
        {
            if (usingMissile)
            {
                return false;
            }
            usingMissile = true;
            return true;
        }

        private bool GroupUpDouble()
        {
            if (usingDouble)
            {
                return false;
            }
            usingDouble = true;
            ChangeWeapon("NormalBullet");
            return true;
        }

        private bool GroupUpLaser()
        {
            string bulletName = "Laser";
            string usingBulletName = normalWeapons[currentWeaponIdx].bulletPre.name;
            if (usingBulletName.IndexOf(bulletName) > 0)
            {
                return false;
            }
            usingDouble = false;
            ChangeWeapon(bulletName);
            return true;
        }

        private bool GroupUpOption()
        {
            return false;
        }



        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if ("PowerBox".Equals(collision.tag))
            {
                currentPowerBoxNum++;
                if (currentPowerBoxNum > maxPowerBoxNum)
                {
                    currentPowerBoxNum = 1;
                }
                Destroy(collision.gameObject);
            }
            else if ("RevivalBox".Equals(collision.tag))
            {
                revival++;
                Destroy(collision.gameObject);
            }
        }
    }
}
