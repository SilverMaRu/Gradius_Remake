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

        public static void AddObjectPool(string poolName, string prefabPath, int capacity, Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            AddObjectPool(poolName, prefab, capacity, parent);
        }

        public static void AddObjectPool(string poolName, GameObject prefab, int capacity, Transform parent)
        {
            bool containsKey = namePoolPairs.ContainsKey(poolName);
            if (containsKey)
            {
                namePoolPairs[poolName].Expand(capacity);
            }
            else
            {
                namePoolPairs.Add(poolName, new ObjectPool(prefab, capacity, parent));
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

        public static void Recycling(GameObject gameObject)
        {
            Recycling(gameObject, () => { });
        }

        public static void Recycling(GameObject gameObject, Action otherAction)
        {
            ObjectPoolAttr objectPoolAttr = gameObject.GetComponent<ObjectPoolAttr>();
            if (objectPoolAttr != null)
            {
                objectPoolAttr.sourcePool.Recycling(gameObject, otherAction);
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}
