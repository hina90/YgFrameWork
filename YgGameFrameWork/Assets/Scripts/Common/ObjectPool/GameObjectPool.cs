using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池
/// </summary>
public class GameObjectPool
{
    //缓存池的最大数量
    private int maxSize;
    //缓存池的初始化数量
    private int poolSize;
    //缓存池名字
    private string poolName;
    //显示容器
    private Transform poolRoot;
    //预制体
    private GameObject poolObjectPrefab;
    //是否自增长
    private bool selfGrowing;

    private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

    public GameObjectPool(string poolName, GameObject poolObjectPrefab, int initCount, int maxsize, Transform pool, bool selfGrowing = false)
    {
        this.poolName = poolName;
        this.poolSize = initCount;
        this.maxSize = maxsize;
        this.poolRoot = pool;
        this.poolObjectPrefab = poolObjectPrefab;
        this.selfGrowing = selfGrowing;

        for (int i = 0; i < initCount; i++)
        {
            AddObjectToPool(NewObjectInstance());
        }
    }
    /// <summary>
    /// 添加到缓存池
    /// </summary>
    /// <param name="po"></param>
    private void AddObjectToPool(PoolObject po)
    {
        po.name = poolName;
        po.gameObject.SetActive(false);
        availableObjStack.Push(po);
        po.isPooled = true;
        po.transform.SetParent(poolRoot, false);
    }
    /// <summary>
    /// 创建对象
    /// </summary>
    /// <returns></returns>
    private PoolObject NewObjectInstance()
    {
        var go = GameObject.Instantiate<GameObject>(poolObjectPrefab);
        go.name = poolName;
        var po = go.GetOrAddComponent<PoolObject>();
        po.poolName = poolName;

        return po;
    }
    /// <summary>
    /// 获取下一个有效缓存对象
    /// </summary>
    /// <returns></returns>
    public PoolObject NextAvailableObject()
    {
        PoolObject po = null;
        if(availableObjStack.Count > 0)
        {
            po = availableObjStack.Pop();
        }
        else if(poolSize < maxSize)
        {
            poolSize++;
            po = NewObjectInstance();
            Debug.Log(string.Format("Growing pool {0}. New size: {1}", poolName, poolSize));
        }
        else if(selfGrowing)
        {
            poolSize++;
            maxSize++;
            po = NewObjectInstance();
            Debug.LogWarning(string.Format("Growing pool {0}. New size: {1}", poolName, poolSize));
        }
        else
        {
            Debug.LogError("No object available & cannot grow pool: " + poolName);
        }

        return po;
    }
    /// <summary>
    /// 回收缓存对象
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnObjectToPool(PoolObject obj)
    {
        if(poolName.Equals(obj.poolName))
        {
            AddObjectToPool(obj);
        }
        else
        {
            Debug.LogError(string.Format("Trying to add object to incorrect pool {0} ", poolName));
        }
    }
}
