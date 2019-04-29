using UnityEngine;

namespace Assets.Scripts.Control
{
    [System.Serializable]
    public class PoolInfo
    {
        public string poolName;
        public string prefabPath;
        public int capacity;
    }

    public class PoolControl : MonoBehaviour
    {
        public PoolInfo[] poolInfos;

        private void Awake()
        {
            for (int i = 0; i < poolInfos.Length; i++)
            {
                GameObjectPool.PoolTool.AddObjectPool(poolInfos[i].poolName, poolInfos[i].prefabPath, poolInfos[i].capacity, transform);
            }
        }
    }
}
