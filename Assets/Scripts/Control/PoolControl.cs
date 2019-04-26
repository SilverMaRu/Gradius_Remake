using UnityEngine;

namespace Assets.Scripts.Control
{
    [System.Serializable]
    public class PoolInfo
    {
        public string poolName;
        public string prefabPath;
        public int capacity;
        //public System.Action recyclingOtherAction { get; set; }
    }

    public class PoolControl : MonoBehaviour
    {
        public PoolInfo[] poolInfos;

        private void Awake()
        {
            for (int i = 0; i < poolInfos.Length; i++)
            {
                //poolInfos[i].recyclingOtherAction = () => { };
                GameObjectPool.PoolTool.AddObjectPool(poolInfos[i]);
            }
        }

        private void Start()
        {
            //for (int i = 0; i < poolInfos.Length; i++)
            //{
            //    poolInfos[i].recyclingOtherAction = () => { };
            //    GameObjectPool.PoolTool.AddObjectPool(poolInfos[i]);
            //}
        }
    }
}
