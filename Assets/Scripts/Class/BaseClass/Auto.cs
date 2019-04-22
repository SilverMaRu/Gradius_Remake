﻿using UnityEngine;

namespace Assets.Scripts.Class.BaseClass
{
    public class Auto : FlyingObject
    {
        [Header("判断X轴超出屏幕的修正")]
        public float outOfXOffset = 1;

        protected override void Update()
        {
            if(Tool.IsOutOfCameraX(transform.position.x, outOfXOffset))
            {
                return;
            }
            base.Update();
            Move();
            Shoot();
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