using UnityEngine;
using Assets.Scripts.Others;

namespace Assets.Scripts.Class.BaseClass
{
    public class AutoMove : FlyingObject
    {
        [Header("判断X轴超出屏幕的修正")]
        public float outOfXOffset = 1;

        private float wakeTime = 0;

        protected override void Update()
        {
            if(Tool.IsOutOfCameraX(transform.position.x, outOfXOffset))
            {
                if (ShouldDisappear())
                {
                    Disappear();
                }
            }
            else
            {
                base.Update();
                Move();
                wakeTime += Time.deltaTime;
            }
        }

        protected override bool ShouldDisappear()
        {
            return base.ShouldDisappear() || wakeTime > 0 && Tool.IsOutOfCameraX(transform.position.x, outOfXOffset);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = Tool.GetCamera().transform.position;
            Vector3 size = Tool.GetCameraScale() + (Vector3.right * 2 * outOfXOffset);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
