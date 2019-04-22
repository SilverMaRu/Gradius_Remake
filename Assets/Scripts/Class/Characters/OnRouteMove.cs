using UnityEngine;

namespace Assets.Scripts.Class.Characters
{
    public class OnRouteMove : BaseClass.Auto
    {
        [Header("包含了移动路线点的游戏对象")]
        public GameObject routeGameObject;

        private Transform[] routePoints;
        private int pointIdx = 0;

        protected override void Init()
        {
            base.Init();
            InitRoutePoints();
        }

        protected void InitRoutePoints()
        {
            if(routeGameObject == null)
            {
                return;
            }
            int length = routeGameObject.transform.childCount;
            if (length > 0)
            {
                routePoints = new Transform[length];
                for (int i = 0; i < routePoints.Length; i++)
                {
                    routePoints[i] = routeGameObject.transform.GetChild(i);
                }
            }
        }

        protected override void InitWeapon()
        {

        }

        protected override void Move()
        {
            if (routePoints == null || routePoints.Length <= 0 || pointIdx >= routePoints.Length)
            {
                base.Move();
            }
            else
            {
                if (transform.position != routePoints[pointIdx].position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, routePoints[pointIdx].position, currentSpeed * Time.deltaTime);
                }
                if (orientToDirection)
                {
                    transform.right = (routePoints[pointIdx].position - transform.position).normalized;
                }
                if (routePoints[pointIdx].position == transform.position)
                {
                    pointIdx++;
                }
            }
        }

        protected override void Shoot()
        {

        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            InitRoutePoints();
            if (routePoints == null)
            {
                return;
            }
            Gizmos.color = Color.white;
            for (int i = 0; i < routePoints.Length; i++)
            {
                Gizmos.DrawSphere(routePoints[i].position, .15f);
                if (i > 0)
                {
                    Gizmos.DrawLine(routePoints[i - 1].position, routePoints[i].position);
                }
            }
        }
    }
}
