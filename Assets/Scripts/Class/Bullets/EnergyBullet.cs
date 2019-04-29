using UnityEngine;
using Assets.Scripts.Others;

namespace Assets.Scripts.Class.Bullets
{
    public class EnergyBullet : BaseClass.Energy
    {
        public float speed;

        protected override void Update()
        {
            if (ShouldDisappear())
            {
                Disappear();
            }
            else
            {
                base.Update();
                Move();
            }
        }

        protected virtual void Move()
        {
            Vector3 deltaDis = transform.right * speed * Time.deltaTime;
            transform.Translate(deltaDis);
        }

        protected override bool ShouldDisappear()
        {
            return base.ShouldDisappear() || Tool.IsOutOfCameraX(transform.position.x) || Tool.IsOutOfCameraY(transform.position.y);
        }
    }
}
