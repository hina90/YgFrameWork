using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 角色管理器
/// </summary>
public class ActorManager : UnitySingleton<ActorManager>
{
    private const int POOL_LIMITED = 0;                                                                             //角色缓存池大小
    private Dictionary<string, List<GameObject>> actorPool = new Dictionary<string, List<GameObject>>();            //缓存池
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="type">角色类型</param>
    /// <param name="id">角色ID</param>
    /// <param name="name">角色资源名字</param>
    /// <param name="scale">缩放大小</param>
    /// <returns></returns>
    public BaseActor CreateActor<T>(CustomerConfigData configData, float scale = 1) where T:BaseActor
    {
        //GameObject base_go = new GameObject(name);
        //base_go.transform.SetParent(Instance.transform, false);
        //base_go.transform.localScale = new Vector3(scale, scale, scale);
        //base_go.transform.position = Vector3.zero;
        if (!actorPool.ContainsKey(configData.guest))
            actorPool.Add(configData.guest, new List<GameObject>());

        //actorPool[name].Add(base_go);

        GameObject actorObject = ResourceManager.Instance.GetResourceInstantiate(configData.guest, Instance.transform.transform, ResouceType.Actor);
        actorPool[configData.guest].Add(actorObject);
        BaseActor base_class = actorObject.GetOrAddComponent<T>();
        base_class.ConfigData = configData;
        base_class.Init();

        return base_class;
    }
    /// <summary>
    /// 销毁角色
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public bool DestroyActor(GameObject go)
    {
        if (go == null)
            return false;

        //if (!actorPool.ContainsKey(go.name))
        //    actorPool.Add(go.name, new List<GameObject>());

        //go.SetActive(false);
        //List<GameObject> goList = actorPool[go.name];
        //for (int i = 0; i < goList.Count; i++)
        //{
        //    if (goList[i] == go)
        //        return false;
        //}

        //if (go.transform.childCount == 0)
        //{    
        //    DestroyImmediate(go);
        //    return false;
        //}
        BaseActor actor = go.GetComponent<BaseActor>();
        if (actor != null)
        {
            actor.Release();
            DestroyImmediate(actor);
        }

        string key = go.name.Replace("(Clone)", "");
        if(!actorPool.ContainsKey(key))
        {
            Destroy(go);
            return false;
        }
        if(actorPool[key].Count > POOL_LIMITED)
        {
            actorPool[key].Remove(go);
            Destroy(go);
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
            TDDebug.DebugLogFormat("---------------------release actor res:{0}", item.Key);
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                Destroy(item.Value[i]);
                item.Value.RemoveAt(i);
            }
        }
    }
}
