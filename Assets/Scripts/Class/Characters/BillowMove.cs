using UnityEngine;

namespace Assets.Scripts.Class.Characters
{
    public class BillowMove : BaseClass.AutoMove
    {
        [Header("变换频率(s)")]
        public float frequency = 1;
        [Header("变换幅度")]
        public float amplitude = 1;

        private float appearTime = 0;
        private const float doublePi = Mathf.PI * 2;

        protected override void Update()
        {
            base.Update();
            appearTime += Time.deltaTime;
        }

        protected override Vector3 GetMoveDirection()
        {
            float sinRet = Mathf.Sin(doublePi * appearTime / frequency);
            float cosRet = Mathf.Cos(doublePi * appearTime / frequency);
            float revise = cosRet * amplitude;
            if (sinRet < 0)
            {
                revise = -revise;
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
