using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池管理器
/// </summary>
public class ObjectManager : BaseManager
{
    private Transform m_PoolRootObject = null;
    private Dictionary<string, object> m_ObjectPools = new Dictionary<string, object>();
    private Dictionary<string, GameObjectPool> m_GameObjectPools = new Dictionary<string, GameObjectPool>();

    Transform PoolRootObject
    {
        get
        {
            if(m_PoolRootObject == null)
            {
                var objectPool = new GameObject("ObjectPool");
                objectPool.transform.SetParent(ManagementCenter.managerObject.transform);
                objectPool.transform.localScale = Vector3.one;
                objectPool.transform.localPosition = Vector3.zero;
                m_PoolRootObject = objectPool.transform;
            }
            return m_PoolRootObject;
        }
    }
    

    public override void Initialize()
    {
        //var abName1 = "Prefabs/Object/NpcObject";
        //var assetNames1 = new string[] { "NpcObject" };
        //resMgr.LoadAssetAsync<GameObject>(abName1, assetNames1, delegate (Object[] prefabs)
        //{
        //    var npcPrefab = prefabs[0] as GameObject;
        //    this.CreatePool(PoolNames.NPC, 5, 10, npcPrefab, true);
        //});

        //var abName2 = "Prefabs/Object/BulletObject";
        //var assetNames2 = new string[] { "BulletObject" };
        //resMgr.LoadAssetAsync<GameObject>(abName2, assetNames2, delegate (Object[] prefabs)
        //{
        //    var bulletPrefab = prefabs[0] as GameObject;
        //    this.CreatePool(PoolNames.BULLET, 5, 10, bulletPrefab);
        //});

        //var abName3 = "Prefabs/Object/EffectObject";
        //var assetNames3 = new string[] { "EffectObject" };
        //resMgr.LoadAssetAsync<GameObject>(abName3, assetNames3, delegate (Object[] prefabs)
        //{
        //    var effectPrefab = prefabs[0] as GameObject;
        //    this.CreatePool(PoolNames.EFFECT, 5, 10, effectPrefab);
        //});

    }
    /// <summary>
    /// 创建缓存池
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="initSize"></param>
    /// <param name="maxSize"></param>
    /// <param name="prefab"></param>
    /// <param name="selfGrowing"></param>
    /// <returns></returns>
    public GameObjectPool CreatePool(string poolName, int initSize, int maxSize, GameObject prefab, bool selfGrowing = false)
    {
        var pool = new GameObjectPool(poolName, prefab, initSize, maxSize, PoolRootObject, selfGrowing);
        m_GameObjectPools[poolName] = pool;

        return pool;
    }
    /// <summary>
    /// 获取缓存池
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
    public GameObjectPool GetPool(string poolName)
    {
        if(m_GameObjectPools.ContainsKey(poolName))
        {
            return m_GameObjectPools[poolName];
        }
        return null;
    }
    /// <summary>
    /// 获取对象池对象
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
    public GameObject Get(string poolName)
    {
        GameObject result = null;
        if(m_GameObjectPools.ContainsKey(poolName))
        {
            GameObjectPool pool = m_GameObjectPools[poolName];
            var poolObj = pool.NextAvailableObject();
            if (poolObj == null)
            {
                Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
            }
            else
            {
                result = poolObj.gameObject;
            }
        }
        else
        {
            Debug.LogError("Invalid pool name specified: " + poolName);
        }
        return result;
    }
    /// <summary>
    /// 释放对象回缓存池
    /// </summary>
    /// <param name="gameObj"></param>
    public void Release(GameObject gameObj)
    {
        if (gameObj == null || AppConst.AppState == AppState.Exiting)
            return;

        var poolObject = gameObj.GetComponent<PoolObject>();
        if(poolObject != null)
        {
            var poolName = poolObject.poolName;
            if(m_GameObjectPools.ContainsKey(poolName))
            {
                var pool = m_GameObjectPools[poolName];
                pool.ReturnObjectToPool(poolObject);
            }
            else
            {
                Debug.LogWarning("No pool available with name: " + poolName);
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {
        
    }
}
