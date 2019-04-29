using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameObjectPool
{
    public class ObjectPool
    {
        public int capacity { get; set; }
        public GameObject prefab { get; protected set; }
        public GameObject[] gameObjects { get; protected set; }
        protected Transform parent;
        protected int nextIndex;

        public ObjectPool(string prefabPath, int capacity, Transform parent) : this(Resources.Load<GameObject>(prefabPath), capacity, parent) { }

        public ObjectPool(GameObject prefab, int capacity, Transform parent)
        {
            this.prefab = prefab;
            this.capacity = capacity;
            gameObjects = new GameObject[capacity];
            this.parent = parent;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i] = Instantiate();
            }
            nextIndex = 0;
        }

        private GameObject Instantiate()
        {
            GameObject retObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            ObjectPoolAttr attr = retObject.GetComponent<ObjectPoolAttr>();
            if (attr == null)
            {
                attr = retObject.AddComponent<ObjectPoolAttr>();
            }
            attr.sourcePool = this;
            retObject.SetActive(false);
            return retObject;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject ret = null;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                int tempIndex = (nextIndex + i) % gameObjects.Length;
                if (!gameObjects[tempIndex].activeSelf)
                {
                    nextIndex = tempIndex + 1;
                    ret = gameObjects[tempIndex];
                }
            }
            if (ret == null)
            {
                nextIndex = gameObjects.Length;
                Expand(capacity);
                ret = gameObjects[nextIndex];
            }
            ret.transform.position = position;
            ret.transform.rotation = rotation;
            ret.SetActive(true);
            return ret;
        }

        public void Expand(int length)
        {
            GameObject[] tempGameObjects = new GameObject[length];
            for (int i = 0; i < tempGameObjects.Length; i++)
            {
                tempGameObjects[i] = Instantiate();
            }
            gameObjects = Others.Tool.Append(tempGameObjects, gameObjects);
        }

        public void Recycling(GameObject gameObject)
        {
            Recycling(gameObject, () => { });
        }

        public void Recycling(GameObject gameObject, System.Action otherAction)
        {
            gameObject.SetActive(false);
            otherAction();
        }
    }
}
