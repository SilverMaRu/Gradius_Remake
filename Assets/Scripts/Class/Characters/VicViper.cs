using UnityEngine;
using Assets.Scripts.Class.BaseClass;
using Assets.Scripts.Class.Weapons;
using Assets.Scripts.GameObjectPool;

namespace Assets.Scripts.Class.Characters
{
    public class VicViper : WeaponFlyingObject
    {
        private const int NORMAL_WEAPON = 0;
        private const int MISSILE_WEAPON = 1;
        private const int DOUBLE_WEAPON = 2;
        private const int LASER_WEAPON = 3;
        private const float SPAW_USE_TIME = 3;
        private const float TWINKLING_FREQUENCY = 0.1f;

        public KeyCode shootKey = KeyCode.J;
        public KeyCode usePower = KeyCode.K;
        public int maxMissileLevel = 2;
        public int currentMissileLevel { get; protected set; }

        public int maxPowerBoxNum = 5;
        public int basePowerBoxNum = 0;
        public int currentPowerBoxNum { get; protected set; }

        public int revival = 1;

        public int maxOptionNum = 3;
        public int baseOptionNum = 0;
        private int currentOptionNum;
        private Option[] options;

        public GameObject spawPosGameObj;
        public GameObject freePosGameObj;
        private float spawStartTime;
        private float lastTwinklingTime;
        private bool isFree;

        private SpriteRenderer sr;

        protected override void Init()
        {
            base.Init();
            Respaw();
        }

        protected override void InitCurrentAttr()
        {
            base.InitCurrentAttr();
            currentPowerBoxNum = basePowerBoxNum;
            //usingDouble = false;
            //usingMissile = false;
            currentMissileLevel = 0;
            currentOptionNum = basePowerBoxNum;
        }

        protected override void InitComponents()
        {
            base.InitComponents();
            options = new Option[maxOptionNum];
            sr = GetComponent<SpriteRenderer>();
        }

        protected override void InitGameObjects()
        {
            base.InitGameObjects();
        }

        private void Respaw()
        {
            if(spawPosGameObj != null)
            {
                transform.position = spawPosGameObj.transform.position;
            }
            spawStartTime = Time.time;
            isInvincible = true;
            isFree = false;
        }

        private void Twinkling()
        {
            if (Time.time - lastTwinklingTime > TWINKLING_FREQUENCY)
            {
                sr.enabled = !sr.enabled;
                lastTwinklingTime = Time.time;
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

                if(Time.time - spawStartTime <= SPAW_USE_TIME)
                {
                    Twinkling();
                }
                else
                {
                    sr.enabled = true;
                    isInvincible = false;
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

        protected override void Move()
        {
            if (isFree)
            {
                base.Move();
            }
            else if(!isFree && freePosGameObj!=null)
            {
                transform.position = Vector3.MoveTowards(transform.position, freePosGameObj.transform.position, maxSpeed * Time.deltaTime);
                if(transform.position == freePosGameObj.transform.position)
                {
                    isFree = true;
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
            if (currentMissileLevel > 0)
            {
                weapons[MISSILE_WEAPON].TryShoot();
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

        private void SetOptionsWeapon()
        {
            for (int i = 0; i < currentOptionNum; i++)
            {
                options[i].UpdateUsingWeaponInfo(currentWeaponIdx, currentMissileLevel);
            }
        }

        private bool GroupUpMissile()
        {
            if(currentMissileLevel >= maxMissileLevel)
            {
                return false;
            }
            currentMissileLevel++;
            weaponInfos[MISSILE_WEAPON].shootFrequency = (float)(weaponInfos[MISSILE_WEAPON].shootFrequency - 0.75 * (currentMissileLevel - 1));
            SetOptionsWeapon();
            return true;
        }

        private bool GroupUpDouble()
        {
            //if (usingDouble)
            //{
            //    return false;
            //}
            //usingDouble = true;
            //ChangeWeapon("NormalBullet");
            if (currentWeaponIdx == DOUBLE_WEAPON)
            {
                return false;
            }
            currentWeaponIdx = DOUBLE_WEAPON;
            SetOptionsWeapon();
            return true;
        }

        private bool GroupUpLaser()
        {
            //string bulletName = "Laser";
            //string usingBulletName = weapons[currentWeaponIdx].bulletPre.name;
            //if (usingBulletName.IndexOf(bulletName) > 0)
            //{
            //    return false;
            //}
            //usingDouble = false;
            //ChangeWeapon(bulletName);
            if (currentWeaponIdx == LASER_WEAPON)
            {
                return false;
            }
            currentWeaponIdx = LASER_WEAPON;
            SetOptionsWeapon();
            return true;
        }

        private bool GroupUpOption()
        {
            if(currentOptionNum >= maxOptionNum)
            {
                return false;
            }
            GameObject optionInstance = PoolTool.GetGameObject("Option", transform.position, Quaternion.identity);
            AddOption(optionInstance.GetComponent<Option>());
            return true;
        }

        public void AddOption(Option newOption)
        {
            if(currentOptionNum >= maxOptionNum)
            {
                return;
            }
            if(currentOptionNum == 0)
            {
                newOption.GetTarget(gameObject);
            }
            else
            {
                newOption.GetTarget(options[currentOptionNum - 1].gameObject);
            }
            newOption.UpdateUsingWeaponInfo(currentWeaponIdx, currentMissileLevel);
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
                Respaw();
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
                Something something = collision.GetComponent<Something>();
                if(something != null)
                {
                    something.sourcePool.Recycling(collision.gameObject);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
            else if ("RevivalBox".Equals(collision.tag))
            {
                revival++;
                Something something = collision.GetComponent<Something>();
                if (something != null)
                {
                    something.sourcePool.Recycling(collision.gameObject);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
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
