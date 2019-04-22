using UnityEngine;

namespace Assets.Scripts.Class.Bullets
{
    public class RoamBullet : EntityBullet
    {
        // 导弹贴地面走时极限修正角度
        private float missileLimitAngle = 90;
        private bool isCloseToGround = false;

        protected override Vector3 GetMoveDirection()
        {
            RaycastHit2D preferredHit = Physics2D.Raycast(transform.position, -transform.up, .3f, LayerMask.GetMask("Ground"));
            RaycastHit2D backupHit = Physics2D.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Ground"));

            if (!isCloseToGround && preferredHit.transform == null)
            {
                return transform.right;
            }
            else if (!isCloseToGround && preferredHit.transform != null)
            {
                isCloseToGround = !isCloseToGround;
            }

            if (isCloseToGround && preferredHit.transform != null)
            {
                transform.up = preferredHit.normal;
            }
            else if (isCloseToGround && preferredHit.transform == null && backupHit.transform != null)
            {
                transform.up = backupHit.normal;
            }

            return transform.right;
        }

        protected override bool ShouldDisappear()
        {
            return base.ShouldDisappear() || IsHitGround();
        }

        private bool IsHitGround()
        {
            bool ret = true;
            RaycastHit2D hitGroundHit = Physics2D.Raycast(transform.position, transform.right, .2f, LayerMask.GetMask("Ground"));

            if (hitGroundHit.transform == null)
            {
                return false;
            }
            //// 导弹前方与法线的夹角
            float angleSN = Vector3.Angle(transform.up, hitGroundHit.normal);
            ret = angleSN >= missileLimitAngle;

            return ret;
        }
    }
}
