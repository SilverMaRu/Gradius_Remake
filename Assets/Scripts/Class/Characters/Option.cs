using UnityEngine;
using Assets.Scripts.Class.BaseClass;
using Assets.Scripts.Others;

namespace Assets.Scripts.Class.Characters
{
    public class Option : Auto
    {
        private const int MISSILE_WEAPON = 1;

        public bool isActive = false;
        public GameObject followTargetGameObj;
        public float followDistance = 3;

        private Vector3[] track;
        private int trackLength;

        private int currentMissileLevel;
        private Collider2D coll;
        private SpriteRenderer sr;
        private WeaponFlyingObject followTarget;

        protected override void Init()
        {
            base.Init();
            InitTrack();
        }

        protected override void InitComponents()
        {
            base.InitComponents();
            coll = GetComponent<Collider2D>();
            sr = GetComponent<SpriteRenderer>();
            followTarget = followTargetGameObj.GetComponent<WeaponFlyingObject>();
        }

        protected override void InitWeapon()
        {
            for (int i = 0; i < followTarget.weaponInfos.Length; i++)
            {
                weaponInfos[i].poolName = followTarget.weaponInfos[i].poolName;
                weaponInfos[i].shootFrequency = followTarget.weaponInfos[i].shootFrequency;
            }
            base.InitWeapon();
        }

        public void UpdateUsingWeaponInfo(int currentWeaponIdx, int currentMissileLevel)
        {
            this.currentWeaponIdx = currentWeaponIdx;
            this.currentMissileLevel = currentMissileLevel;
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
            if (currentMissileLevel > 0)
            {
                weapons[MISSILE_WEAPON].TryShoot();
            }
        }

        public void GetTarget(GameObject target)
        {
            if(target != null)
            {
                isActive = true;
                followTargetGameObj = target;
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


        protected override void OnDrawGizmos()
        {
            for (int i = 0; track != null && i < track.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(track[i], 0.1f);
            }
        }
    }
}
