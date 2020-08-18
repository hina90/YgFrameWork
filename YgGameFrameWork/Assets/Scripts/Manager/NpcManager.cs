using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 角色管理器
/// </summary>
public class NpcManager : BaseManager
{
    private readonly object npcLock = new object();
    private Dictionary<int, INpcView> mNpcs = new Dictionary<int, INpcView>();

    public Dictionary<int, INpcView> Npcs
    {
        get
        {
            return mNpcs;
        }
    }
    /// <summary>
    /// 添加NPC
    /// </summary>
    /// <param name="npcId"></param>
    /// <param name="view"></param>
    public void AddNpc(int npcId, INpcView view)
    {
        lock(npcLock)
        {
            if(!Npcs.ContainsKey(npcId))
            {
                Npcs.Add(npcId, view);
            }
        }
    }
    /// <summary>
    /// 获取NPC
    /// </summary>
    /// <param name="npcid"></param>
    /// <returns></returns>
    public INpcView GetNpc(int npcid)
    {
        lock(npcLock)
        {
            if(Npcs.ContainsKey(npcid))
            {
                return Npcs[npcid];
            }
            return null;
        }
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="npcid"></param>
    public void RemoveNpc(int npcid)
    {
        lock (npcLock)
        {
            Npcs.Remove(npcid);
        }
    }
    /// <summary>
    /// 创建NPC
    /// </summary>
    public T CreateNpc<T>(NpcData npcData, Transform parent) where T : NpcView, new()
    {
        var gameObj = objMgr.Get(PoolNames.NPC);     //一个客户端对象
        gameObj.transform.SetParent(parent);
        gameObj.transform.localScale = Vector3.one;
        gameObj.transform.localPosition = Vector3.zero;
        gameObj.gameObject.SetActive(true);

        var npcView = new T();
        npcView.NpcData = npcData;
        gameObj.GetComponent<ViewObject>().BindView(npcView);
        return npcView;
    }
    /// <summary>
    /// 移除角色
    /// </summary>
    public void RemoveNpc<T>(int npcid) where T : NpcView
    {
        var view = GetNpc(npcid);
        if (view != null)
        {
            var npcView = view as NpcView;
            if (npcView != null)
            {
                npcView.OnDispose();
                objMgr.Release(npcView.gameObject);
            }
        }
        RemoveNpc(npcid);
    }



    public override void Initialize()
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {
        
    }

   
}
