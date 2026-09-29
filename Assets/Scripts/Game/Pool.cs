using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public readonly GameObject[] objectsToPool;
    private int preferredMax;
    public bool isCapped;
    List<GameObject> pool;

    public Pool(GameObject objectToPool)
    {
        this.objectsToPool = new GameObject[1];
        this.objectsToPool[0] = objectToPool;
    }

    public Pool(GameObject[] objectsToPool)
    {
        for (int i = 0; i < objectsToPool.Length; i++)
        {
            if (objectsToPool[i] == null)
            {
                Debug.LogError("Pool object "+i+" is null");
                return;
            }
        }
        this.objectsToPool = objectsToPool;
    }
    
    public void CreatePool(int num)
    {
        pool = new List<GameObject>(num);
        preferredMax = num;
        for (int i = 0; i < num; i++)
        {
            GameObject o = GameObject.Instantiate(objectsToPool[Random.Range(0, objectsToPool.Length)]);
            pool.Add(o);
            o.SetActive(false);
        }

        //foreach (var g in pool) Debug.Log("?????");
    }

    public GameObject spawn(Vector3 position, Quaternion rotation)
    {
        foreach (var p_object in pool) // check for inactive instances first
        {
            if (p_object != null)
            {
                if (!p_object.activeInHierarchy)
                {
                    p_object.transform.position = position;
                    p_object.transform.rotation = rotation;
                    p_object.SetActive(true);
                    Debug.Log("Spawned " + p_object.name);
                    return p_object;
                }
            }
        }
        // all instances active
        if(isCapped) return null; // capped?

        // pool not capped. Create new
        GameObject o = GameObject.Instantiate(objectsToPool[Random.Range(0, objectsToPool.Length)]);
        pool.Add(o);
        o.transform.position = position;
        o.transform.rotation = rotation;
        return o;
    }

    public void Reset() // reset to preferred max
    {
        if (pool.Count > preferredMax)
        {
            //pool.RemoveRange(pool.Count, preferredMax - pool.Count);
            pool.TrimExcess();
        }
    }
}
