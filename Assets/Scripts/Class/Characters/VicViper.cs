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
        
        public Weapon doubleWeapon { get; protected set; }
        public bool usingDouble { get; protected set; }
        public Weapon missile { get; protected set; }
        public bool usingMissile { get; protected set; }
        public int maxMissileLevel = 2;
        public int currentMissileLevel { get; protected set; }


        public int maxPowerBoxNum = 5;
        public int basePowerBoxNum = 0;
        private int currentPowerBoxNum;

        public int revival = 1;

        public int maxOptionNum = 3;
        public int baseOptionNum = 0;
        private int currentOptionNum;
        private Option[] options;
        private GameObject optionPrefab;

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
            currentMissileLevel = 0;
            currentOptionNum = basePowerBoxNum;
        }

        protected override void InitComponents()
        {
            base.InitComponents();
            options = new Option[maxOptionNum];
        }

        protected override void InitGameObjects()
        {
            base.InitGameObjects();
            optionPrefab = Resources.Load<GameObject>("Prefabs/VicViper/Remake/Option");
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

                //测试用
                if (Input.GetKeyDown(KeyCode.C))
                {
                    currentPowerBoxNum++;
                    if (currentPowerBoxNum > maxPowerBoxNum)
                    {
                        currentPowerBoxNum = 1;
                    }
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
            //if (usingMissile)
            //{
            //    missile.TryShoot();
            //}
            if (currentMissileLevel > 0)
            {
                missile.TryShoot();
            }
            for (int i = 0; i < currentOptionNum; i++)
            {
                options[i].OptionShoot();
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

        private void SetOptionsWeapon()
        {
            for (int i = 0; i < currentOptionNum; i++)
            {
                options[i].ResetUsingWeapons();
            }
        }

        private bool GroupUpMissile()
        {
            //if (usingMissile)
            //{
            //    return false;
            //}
            //usingMissile = true;
            //SetOptionsWeapon();
            //return true;
            if(currentMissileLevel >= maxMissileLevel)
            {
                return false;
            }
            currentMissileLevel++;
            missile.SetShootFrequency((float)(missile.shootFrequency - 0.5 * (currentMissileLevel - 1)));
            SetOptionsWeapon();
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
            SetOptionsWeapon();
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
            SetOptionsWeapon();
            return true;
        }

        private bool GroupUpOption()
        {
            if(currentOptionNum >= maxOptionNum)
            {
                return false;
            }
            GameObject optionInstance = Instantiate(optionPrefab, transform.position, Quaternion.identity);
            AddOption(optionInstance.GetComponent<Option>());
            return true;
        }

        public void AddOption(Option newOption)
        {
            if(currentOptionNum == 0)
            {
                newOption.GetTarget(gameObject);
            }
            else
            {
                newOption.GetTarget(options[currentOptionNum - 1].gameObject);
            }
            options[currentOptionNum] = newOption;
            currentOptionNum++;
        }

        protected override void Die()
        {
            revival--;
            for (int i = 0; i < currentOptionNum; i++)
            {
                options[i].LostTarget();
            }
            if (revival <= 0)
            {
                base.Die();
            }
            else
            {
                InitCurrentAttr();
            }
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
            else if ("Option".Equals(collision.tag))
            {
                Option option = collision.GetComponent<Option>();
                if (option != null)
                {
                    AddOption(option);
                }
            }
        }
    }
}
