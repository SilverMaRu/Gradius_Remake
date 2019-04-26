using UnityEngine;

namespace Assets.Scripts.Class.BaseClass
{
    public class FlyingObject : Entity
    {
        public float maxSpeed = 10;
        public float baseSpeed = 7;
        public float currentSpeed { get; protected set; }
        public bool isNormalized = false;
        public bool orientToDirection = false;
        
        protected override void InitCurrentAttr()
        {
            base.InitCurrentAttr();
            currentSpeed = baseSpeed;
        }

        protected virtual void Move()
        {
            Vector3 moveDirection = GetMoveDirection();
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
            if (orientToDirection)
            {
                transform.right = moveDirection;
            }
        }

        protected virtual Vector3 GetMoveDirection()
        {
            Vector3 direction = transform.right;
            return direction;
        }
    }
}
