using UnityEngine;

namespace Assets.Scripts.Class.Characters
{
    public class ZigZagMove : BaseClass.AutoMove
    {
        [Header("变换频率(s)")]
        public float frequency = 1;

        private float appearTime = 0;
        private const float doublePi = Mathf.PI * 2;

        protected override void Update()
        {
            base.Update();
            appearTime += Time.deltaTime;
        }

        protected override Vector3 GetMoveDirection()
        {
            float sinRet = Mathf.Cos(doublePi * appearTime / frequency);
            int revise = 0;
            if (sinRet > 0)
            {
                revise = 1;
            }
            else if (sinRet < 0)
            {
                revise = -1;
            }
            Vector3 direction = Vector3.left + Vector3.up * revise;
            if (isNormalized)
            {
                direction = (Vector3.left + Vector3.up * revise).normalized;
            }
            return direction;
        }
    }
}
