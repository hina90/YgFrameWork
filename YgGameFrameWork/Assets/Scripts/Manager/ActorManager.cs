﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : BaseManager
{
    private const int POOL_LIMITED = 0;                                                                             //角色缓存池大小
    private Dictionary<string, List<GameObject>> actorPool = new Dictionary<string, List<GameObject>>();            //缓存池



    public override void Initialize()
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {
        
    }



    /// <summary>
    /// 创建普通NPC
    /// </summary>
    /// <param name="type">角色类型</param>
    /// <param name="id">角色ID</param>
    /// <param name="name">角色资源名字</param>
    /// <param name="scale">缩放大小</param>
    /// <returns></returns>
    /// 
    //public BaseActor CreateActor<T>(CustomerConfigData configData, float scale = 1) where T:BaseActor
    //{
        //if (!actorPool.ContainsKey(configData.icon))
        //    actorPool.Add(configData.icon, new List<GameObject>());

        //GameObject actorObject = ResourceManager.Instance.GetResourceInstantiate(configData.icon, Instance.transform.transform, ResouceType.Actor);
        //actorPool[configData.icon].Add(actorObject);
        //BaseActor base_class = actorObject.GetOrAddComponent<T>();

        //return base_class;
        //return null;
    //}
    /// <summary>
    /// 销毁角色
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public bool DestroyActor(GameObject go)
    {
        if (go == null)
            return false;

        BaseActor actor = go.GetComponent<BaseActor>();
        if (actor != null)
        {
            actor.Release();
            //DestroyImmediate(actor);
        }

        string key = go.name.Replace("(Clone)", "");
        if(!actorPool.ContainsKey(key))
        {
            //Destroy(go);
            return false;
        }
        if(actorPool[key].Count > POOL_LIMITED)
        {
            actorPool[key].Remove(go);
            //Destroy(go);
            return true;
        }
        return true;
    }
    /// <summary>
    /// 释放角色缓存池
    /// </summary>
    public void ReleasePool()
    {
        foreach (var item in actorPool)
        {
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                //Destroy(item.Value[i]);
                item.Value.RemoveAt(i);
            }
        }
    }
}
