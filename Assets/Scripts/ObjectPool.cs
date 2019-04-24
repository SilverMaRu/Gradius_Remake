using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    protected GameObject prefab;
    protected GameObject[] gameObjects;
    protected int initCapacity;
    protected int nextIndex;
    // 回收方法的额外行为
    protected System.Action recyclingOtherAction;

    public ObjectPool(string prefabPath, int initCapacity) : this(Resources.Load<GameObject>(prefabPath), initCapacity, () => { }) { }

    public ObjectPool(string prefabPath, int initCapacity, System.Action recyclingOtherAction) : this(Resources.Load<GameObject>(prefabPath), initCapacity, recyclingOtherAction) { }

    public ObjectPool(GameObject prefab, int initCapacity) : this(prefab, initCapacity, () => { }) { }
    
    public ObjectPool(GameObject prefab, int initCapacity, System.Action recyclingOtherAction)
    {
        this.prefab = prefab;
        this.initCapacity = initCapacity;
        gameObjects = new GameObject[initCapacity];
        for(int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i] = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            Assets.Scripts.Class.BaseClass.Something something = gameObjects[i].GetComponent<Assets.Scripts.Class.BaseClass.Something>();
            if(something != null)
            {
                something.objectPool = this;
            }
            gameObjects[i].SetActive(false);
        }
        nextIndex = 0;
        this.recyclingOtherAction = recyclingOtherAction;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject ret = null;
        for(int i = 0; i < gameObjects.Length; i++)
        {
            int tempIndex = (nextIndex + i) % gameObjects.Length;
            if (!gameObjects[tempIndex].activeSelf)
            {
                ret = gameObjects[tempIndex];
            }
        }
        if (ret == null)
        {
            GameObject[] tempGameObjects = new GameObject[initCapacity];
            for (int i = 0; i < tempGameObjects.Length; i++)
            {
                tempGameObjects[i] = Object.Instantiate(prefab, position, rotation);
                tempGameObjects[i].SetActive(false);
            }
            ret = tempGameObjects[0];
        }
        ret.transform.position = position;
        ret.transform.rotation = rotation;
        ret.SetActive(true);
        return ret;
    }

    public void Recycling(GameObject gameObject)
    {
        gameObject.SetActive(false);
        recyclingOtherAction();
    }
}
