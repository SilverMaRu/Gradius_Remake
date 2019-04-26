using Assets.Scripts.Others;

namespace Assets.Scripts.Class.Bullets
{
    public class EntityBullet: BaseClass.FlyingObject
    {
        protected override void Update()
        {
            base.Update();
            if (ShouldDisappear())
            {
                Disappear();
            }
            else
            {
                Move();
            }
        }

        protected virtual bool ShouldDisappear()
        {
            return Tool.IsOutOfCameraX(transform.position.x) || Tool.IsOutOfCameraY(transform.position.y);
        }

        protected virtual void Disappear()
        {
            if(sourcePool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                sourcePool.Recycling(gameObject);
            }
        }
    }
}
