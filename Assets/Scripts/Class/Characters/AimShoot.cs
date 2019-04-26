using UnityEngine;

namespace Assets.Scripts.Class.Characters
{
    public class AimShoot : BaseClass.Auto
    {
        public GameObject aimTarget;
        // 可瞄准的最大角度
        public float minAimAngle = 0;
        public float maxAimAngle = 180;
        // 原定瞄准敌人各个角度的Sprite
        public Sprite[] aimSprites;

        private SpriteRenderer sr;
        private float aimAngle = 0;
        private float halfRemainder = 0;
        private float angleMVXFlatAngle = 0;
        private float angleMVXWeekAngle = 0;
        private int currentSpriteIdx = 0;

        protected override void Init()
        {
            base.Init();
            aimAngle = maxAimAngle - minAimAngle;
            halfRemainder = (360 - aimAngle) / 2;
        }

        protected override void InitComponents()
        {
            base.InitComponents();
            sr = GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();
            Aim();
        }

        private void Aim()
        {
            if (aimTarget == null)
            {
                return;
            }
            float eachAngle = aimAngle / aimSprites.Length;
            // 当前对象到aimTarget的向量
            Vector3 mt = aimTarget.transform.position - transform.position;
            // mv与x轴正方向的夹角
            float signedAngle = Vector3.SignedAngle(Quaternion.Euler(0, 0, minAimAngle) * transform.right, mt, transform.forward);
            angleMVXFlatAngle = signedAngle;
            // 转换为0~360
            angleMVXWeekAngle = angleMVXFlatAngle < 0 ? 360 + angleMVXFlatAngle : angleMVXFlatAngle;
            // 设定该使用的Sprite
            currentSpriteIdx = Mathf.Clamp((int)((angleMVXWeekAngle - eachAngle / 2) / eachAngle), 0, aimSprites.Length - 1);
            if (angleMVXFlatAngle >= minAimAngle - halfRemainder && angleMVXFlatAngle < minAimAngle)
            {
                currentSpriteIdx = 0;
            }
            else if (angleMVXWeekAngle > maxAimAngle && angleMVXFlatAngle < maxAimAngle + halfRemainder)
            {
                currentSpriteIdx = aimSprites.Length - 1;
            }
            sr.sprite = aimSprites[currentSpriteIdx];
        }

        protected override void Shoot()
        {
            if (aimTarget == null)
            {
                return;
            }
            for (int i = 0; i < weaponInfos[currentWeaponIdx].barrelGameObjs.Length; i++)
            {
                // 当前对象发射点到aimTarget的向量
                Vector3 bt = aimTarget.transform.position - weaponInfos[currentWeaponIdx].barrelGameObjs[i].transform.position;
                Vector3 direction = bt;

                if (angleMVXFlatAngle >= minAimAngle - halfRemainder && angleMVXFlatAngle < minAimAngle)
                {
                    direction = Quaternion.Euler(0, 0, minAimAngle) * transform.right;
                }
                else if (angleMVXWeekAngle > maxAimAngle && angleMVXFlatAngle < maxAimAngle + halfRemainder)
                {
                    direction = Quaternion.Euler(0, 0, maxAimAngle) * transform.right;
                }
                weaponInfos[currentWeaponIdx].barrelGameObjs[i].transform.right = direction;
            }
            base.Shoot();
        }
    }

}