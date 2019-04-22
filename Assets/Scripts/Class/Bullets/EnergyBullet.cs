using UnityEngine;

namespace Assets.Scripts.Class.Bullets
{
    public class EnergyBullet : BaseClass.Energy
    {
        public float speed;

        protected override void Update()
        {
            base.Update();
            Move();
        }

        protected virtual void Move()
        {
            Vector3 deltaDis = transform.right * speed * Time.deltaTime;
            transform.Translate(deltaDis);
            // 子弹与镜头的水平差值
            if (ShouldDisappear())
            {
                Disappear();
            }
        }

        protected override bool ShouldDisappear()
        {
            return base.ShouldDisappear() || Tool.IsOutOfCameraX(transform.position.x) || Tool.IsOutOfCameraY(transform.position.y);
        }
    }
}
