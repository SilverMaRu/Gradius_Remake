using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameObjectPool
{
    public class PoolTool
    {
        public static string[] poolNames
        {
            get
            {
                string[] ret = null;
                namePoolPairs.Keys.CopyTo(ret, 0);
                return ret;
            }
        }
        protected static Dictionary<string, ObjectPool> namePoolPairs = new Dictionary<string, ObjectPool>();

        public static void AddObjectPool(string poolName, string prefabPath, int capacity)
        {
            AddObjectPool(poolName, prefabPath, capacity, () => { });
        }

        public static void AddObjectPool(string poolName, string prefabPath, int capacity, Action recyclingOtherAction)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            AddObjectPool(poolName, prefab, capacity, recyclingOtherAction);
        }

        public static void AddObjectPool(string poolName, GameObject prefab, int capacity)
        {
            AddObjectPool(poolName, prefab, capacity, () => { });
        }

        public static void AddObjectPool(string poolName, GameObject prefab, int capacity, Action recyclingOtherAction)
        {
            bool containsKey = namePoolPairs.ContainsKey(poolName);
            if (containsKey)
            {
                namePoolPairs[poolName].Expand(capacity);
            }
            else
            {
                namePoolPairs.Add(poolName, new ObjectPool(prefab, capacity, recyclingOtherAction));
            }
        }

        public static void AddObjectPool(Control.PoolInfo info)
        {
            GameObject prefab = Resources.Load<GameObject>(info.prefabPath);
            bool containsKey = namePoolPairs.ContainsKey(info.poolName);
            if (containsKey)
            {
                namePoolPairs[info.poolName].Expand(info.capacity);
            }
            else
            {
                //namePoolPairs.Add(info.poolName, new ObjectPool(prefab, info.capacity, info.recyclingOtherAction));
                namePoolPairs.Add(info.poolName, new ObjectPool(prefab, info.capacity));
            }
        }

        public static GameObject GetGameObject(string poolName, Vector3 position, Quaternion rotation)
        {
            GameObject ret = null;
            bool containsKey = namePoolPairs.ContainsKey(poolName);
            if (containsKey)
            {
                ret = namePoolPairs[poolName].Get(position, rotation);
            }
            return ret;
        }
    }
}
