using Assets.Scripts.Others;

namespace Assets.Scripts.Class.Bullets
{
    public class EntityBullet: BaseClass.FlyingObject
    {
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

        protected override bool ShouldDisappear()
        {
            return Tool.IsOutOfCameraX(transform.position.x) || Tool.IsOutOfCameraY(transform.position.y);
        }
    }
}
