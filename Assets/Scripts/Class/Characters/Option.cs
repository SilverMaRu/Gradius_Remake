using UnityEngine;
using Assets.Scripts.Class.BaseClass;
using Assets.Scripts.Class.Weapons;

namespace Assets.Scripts.Class.Characters
{
    public class Option : FlyingObject
    {
        public bool isActive = false;

        public GameObject followTarget;
        public float followDistance = 3;
        private Vector3[] track;
        private int trackLength;

        public GameObject normalBarrel;
        public GameObject doubleBarrel;
        public GameObject missileBarrel;

        public Weapon.WeaponInitInfo doubleWeaponInfo;
        public Weapon.WeaponInitInfo missileWeaponInfo;

        public Weapon doubleWeapon { get; protected set; }
        public bool usingDouble { get; protected set; }
        public Weapon missile { get; protected set; }
        public bool usingMissile { get; protected set; }
        public int maxMissileLevel = 2;
        public int currentMissileLevel { get; protected set; }

        private Collider2D coll;
        private SpriteRenderer sr;

        protected override void Init()
        {
            base.Init();
            InitTrack();
        }

        protected override void InitComponents()
        {
            base.InitComponents();
            coll = gameObject.GetComponent<Collider2D>();
            sr = gameObject.GetComponent<SpriteRenderer>();
        }

        protected override void InitWeapon()
        {
            // 获取目标的武器信息
            VicViper vicViper = followTarget.GetComponent<VicViper>();
            if (vicViper != null)
            {
                normalWeaponInfos = vicViper.normalWeaponInfos;
                doubleWeaponInfo = vicViper.doubleWeaponInfo;
                missileWeaponInfo = vicViper.missileWeaponInfo;
                maxMissileLevel = vicViper.maxMissileLevel;
            }
            else
            {
                Option option = followTarget.GetComponent<Option>();
                if (option != null)
                {
                    normalWeaponInfos = option.normalWeaponInfos;
                    doubleWeaponInfo = option.doubleWeaponInfo;
                    missileWeaponInfo = option.missileWeaponInfo;
                    maxMissileLevel = option.maxMissileLevel;
                }
            }
            // 修改发射点
            for (int i = 0; i < normalWeaponInfos.Length; i++)
            {
                normalWeaponInfos[i].barrelGameObj = normalBarrel;
            }
            doubleWeaponInfo.barrelGameObj = doubleBarrel;
            missileWeaponInfo.barrelGameObj = missileBarrel;

            base.InitWeapon();
            doubleWeapon = new Weapon(doubleWeaponInfo, 15);
            missile = new Weapon(missileWeaponInfo, 10);
            ResetUsingWeapons();
        }

        public void ResetUsingWeapons()
        {
            VicViper vicViper = followTarget.GetComponent<VicViper>();
            if (vicViper != null)
            {
                currentWeaponIdx = vicViper.currentWeaponIdx;
                usingDouble = vicViper.usingDouble;
                usingMissile = vicViper.usingMissile;
                currentMissileLevel = vicViper.currentMissileLevel;
            }
            else
            {
                Option option = followTarget.GetComponent<Option>();
                if (option != null)
                {
                    currentWeaponIdx = option.currentWeaponIdx;
                    usingDouble = option.usingDouble;
                    usingMissile = option.usingMissile;
                    currentMissileLevel = option.currentMissileLevel;
                }
            }
            missile.SetShootFrequency(missileWeaponInfo.shootFrequency);
        }

        private void InitTrack()
        {
            // 设定移动速度
            if (followTarget != null)
            {
                FlyingObject flyingObject = followTarget.GetComponent<FlyingObject>();
                if (flyingObject != null)
                {
                    currentSpeed = flyingObject.maxSpeed;
                }
            }
            trackLength = (int)(followDistance / currentSpeed / 0.015);
            // 初始化跟踪数组
            track = new Vector3[trackLength];
            ResetTrack();
        }

        private FlyingObject GetFlyingObject(GameObject tryGetGameObject)
        {
            FlyingObject ret = null;
            FlyingObject flyingObject = tryGetGameObject.GetComponent<FlyingObject>();
            if(flyingObject == null)
            {
                Option option = tryGetGameObject.GetComponent<Option>();
                if (option != null && option.followTarget != null)
                {
                    ret = GetFlyingObject(option.followTarget);
                }
            }
            else
            {
                ret = flyingObject;
            }
            return ret;
        }

        private void ResetTrack()
        {
            for (int i = 0; i < track.Length; i++)
            {
                track[i] = followTarget.transform.position;
            }
        }

        protected override void Update()
        {
            if (isActive)
            {
                //base.Update();
                Move();
            }
        }

        protected override void Move()
        {
            if (followTarget == null || track == null)
            {
                return;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (h != 0 || v != 0)
            {
                RememberTrack();
            }
            else
            {
                RefreshTrack();
            }
            Vector3 limitPoint = track[track.Length - 1];
            if (limitPoint != Vector3.zero)
            {
                transform.position = limitPoint;
            }
        }

        private void RememberTrack()
        {
            if (followTarget.transform.position != track[0])
            {
                track = Tool.Prepend(followTarget.transform.position, track, false);
            }
        }

        private void RefreshTrack()
        {
            Vector3 deltaVec = followTarget.transform.position - track[0];
            for (int i = 0; i < track.Length; i++)
            {
                track[i] = track[i] + deltaVec;
            }
        }

        public void OptionShoot()
        {
            if (isActive)
            {
                Shoot();
            }
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
        }

        public void GetTarget(GameObject target)
        {
            if(target != null)
            {
                isActive = true;
                followTarget = target;
                Init();
                coll.enabled = false;
                sr.enabled = true;
            }
        }

        public void LostTarget()
        {
            followTarget = null;
            isActive = false;
            coll.enabled = true;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
        }


        private void OnDrawGizmos()
        {
            for (int i = 0; track != null && i < track.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(track[i], 0.1f);
            }
        }
    }
}
