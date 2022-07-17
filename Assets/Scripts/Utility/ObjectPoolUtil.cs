using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ObjectPoolUtil : MonoBehaviour
{
    public static ObjectPoolUtil instance;

    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        if (ObjectPoolUtil.instance != null)
        {
            Destroy(this);
        }
        ObjectPoolUtil.instance = this;
    }

    public static ObjectPoolUtil GetInstance()
    {
        return ObjectPoolUtil.instance;
    }


    public GameObject GetObj(GameObject prefab, Transform parent)
    {
        if (pool == null)
        {
            pool = new ObjectPool<GameObject>
            (() =>
                {
                    if (parent == null)
                        return Instantiate(prefab);

                    return Instantiate(prefab, parent);
                }
                , GameObject =>
                {
                    GameObject.SetActive(true);
                }
                , GameObject =>
                {
                    GameObject.SetActive(false);
                }
                , GameObject =>
                {
                    Destroy(GameObject);
                }, false, 10, 20
            );
        }

        return pool.Get();
    }

    public void ReturnObj(GameObject obj)
    {
        pool.Release(obj);
    }

}
