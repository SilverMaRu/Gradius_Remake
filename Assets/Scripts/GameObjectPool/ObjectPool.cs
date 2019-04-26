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
        protected int nextIndex;
        //// 回收方法的额外行为
        //protected System.Action recyclingOtherAction;

        public ObjectPool(string prefabPath, int capacity) : this(Resources.Load<GameObject>(prefabPath), capacity, () => { }) { }

        public ObjectPool(string prefabPath, int capacity, System.Action recyclingOtherAction) : this(Resources.Load<GameObject>(prefabPath), capacity, recyclingOtherAction) { }

        public ObjectPool(GameObject prefab, int capacity) : this(prefab, capacity, () => { }) { }

        public ObjectPool(GameObject prefab, int capacity, System.Action recyclingOtherAction)
        {
            this.prefab = prefab;
            this.capacity = capacity;
            gameObjects = new GameObject[capacity];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i] = Instantiate();
            }
            nextIndex = 0;
            //this.recyclingOtherAction = recyclingOtherAction;
        }

        private GameObject Instantiate()
        {
            GameObject retObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            Class.BaseClass.Something something = retObject.GetComponent<Class.BaseClass.Something>();
            if (something != null)
            {
                something.sourcePool = this;
            }
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
            gameObject.SetActive(false);
            //recyclingOtherAction?.Invoke();
        }
    }
}
