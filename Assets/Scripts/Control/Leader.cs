using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Class.BaseClass;
using Assets.Scripts.Class.Characters;
using Assets.Scripts.GameObjectPool;

namespace Assets.Scripts.Control
{
    public class Leader : MonoBehaviour
    {
        [Header("队员的对象池名称")]
        public string memberPoolName;
        public int memberCount = 5;
        [Header("队员之间的间隔")]
        public Vector3 spacing = Vector3.right * 0.2f;
        public bool useRoute = false;
        [Header("包含了移动路线点的游戏对象")]
        public GameObject routeGameObject;

        private GameObject[] memberInstances;
        private Transform lastMemberTrans;
        public bool isAce { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            SpawMember();
            if (useRoute)
            {
                for(int i = 0; i < memberCount; i++)
                {
                    AutoMove autoMove = memberInstances[i].GetComponent<AutoMove>();
                    autoMove.enabled = false;
                    OnRouteMove onRouteMove = memberInstances[i].GetComponent<OnRouteMove>();
                    onRouteMove.routeGameObject = routeGameObject;
                    onRouteMove.enabled = true;
                }
            }
            isAce = false;
        }

        private void SpawMember()
        {
            memberInstances = new GameObject[memberCount];
            for(int i = 0; i < memberCount; i++)
            {
                memberInstances[i] = PoolTool.GetGameObject(memberPoolName, transform.position + spacing * i, Quaternion.identity);
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckAliveMember();
            if (isAce)
            {
                SpawBox();
                Destroy(gameObject);
            }
        }

        public void CheckAliveMember()
        {
            int aliveCount = 0;
            int aliveMemberIdx = 0;
            for (int i = 0; i < memberCount; i++)
            {
                Entity entity = memberInstances[i].GetComponent<Entity>();
                if (entity != null && entity.isAlive)
                {
                    aliveCount++;
                    aliveMemberIdx = i;
                }
            }
            if (aliveCount == 1)
            {
                lastMemberTrans = memberInstances[aliveMemberIdx].transform;
            }
            else if (aliveCount <= 0)
            {
                isAce = true;
            }
        }

        private void SpawBox()
        {
            float randomF = Random.Range(0, 10);
            if (randomF < 0.1)
            {
                PoolTool.GetGameObject("RevivalBox", lastMemberTrans.position, Quaternion.identity);
            }
            else
            {
                PoolTool.GetGameObject("PowerBox", lastMemberTrans.position, Quaternion.identity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            for(int i = 0; i < memberCount; i++)
            {
                Gizmos.DrawSphere(transform.position + spacing * i, 0.2f);
            }

            if (useRoute)
            {
                Gizmos.color = Color.white;
                Transform[] routePointTrans = FindRoute();
                Gizmos.DrawLine(transform.position, routePointTrans[0].position);
                for (int i = 0;i< routePointTrans.Length; i++)
                {
                    Gizmos.DrawSphere(routePointTrans[i].position, 0.15f);
                    if (i > 0)
                    {
                        Gizmos.DrawLine(routePointTrans[i].position, routePointTrans[i - 1].position);
                    }
                }
            }
        }

        private Transform[] FindRoute()
        {
            Transform[] retTranss = new Transform[] { transform };
            if (routeGameObject != null)
            {
                int length = routeGameObject.transform.childCount;
                if (length > 0)
                {
                    retTranss = new Transform[length];
                    for (int i = 0; i < retTranss.Length; i++)
                    {
                        retTranss[i] = routeGameObject.transform.GetChild(i);
                    }
                }
            }
            return retTranss;
        }
    }
}
