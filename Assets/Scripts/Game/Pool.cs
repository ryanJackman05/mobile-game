using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public readonly GameObject objectToPool;
    private int preferredMax;
    public bool isCapped;
    List<GameObject> pool;

    public Pool(GameObject objectToPool)
    {
        this.objectToPool = objectToPool;
    }
    
    public void CreatePool(int num)
    {
        pool = new List<GameObject>(num);
        for (int i = 0; i < num; i++)
        {
            pool.Add(GameObject.Instantiate(objectToPool));
        }

        //foreach (var g in pool) Debug.Log("?????");
    }

    public GameObject spawn(Vector3 position, Quaternion rotation)
    {
        foreach (var p_object in pool)
        {
            if (p_object != null)
            {
                if (!p_object.activeInHierarchy)
                {
                    p_object.transform.position = position;
                    p_object.transform.rotation = rotation;
                    p_object.SetActive(true);
                    return p_object;
                }
            }
        }

        GameObject o = GameObject.Instantiate(objectToPool);
        pool.Add(o);
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
