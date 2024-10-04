using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
                instance = new ObjectPool();
            return instance;
        }
    }

    private GameObject pool;//作为所有对象池的根节点

    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();


    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;
        if(!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);
            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
                pool.AddComponent<monoObjectPool>();
            }
            GameObject prefabPool = GameObject.Find(prefab.name + "Pool");
            if(prefabPool == null)
            {
                prefabPool = new GameObject(prefab.name + "Pool");
                prefabPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(prefabPool.transform);
           
        }
        _object = objectPool[prefab.name].Dequeue();
        if(_object != null)
        _object.SetActive(true);
        return _object;
        
    }

    public void PushObject(GameObject prefab)
    {
        string name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(name))
            objectPool.Add(name, new Queue<GameObject>());
        objectPool[name].Enqueue(prefab);
        prefab.SetActive(false);
    }

    public void DequeueAll()
    {
       int len = pool.transform.childCount;
       Debug.Log(len);
       for(int i = 0;i<len;i++)
       {
           Transform cpool = pool.transform.GetChild(i);
           int len2 = cpool.childCount;
           for(int j = 0;j< len2;j++)
           {
               GameObject obj = cpool.GetChild(j).gameObject;
               PushObject(obj);
           }
       }
    }


}
