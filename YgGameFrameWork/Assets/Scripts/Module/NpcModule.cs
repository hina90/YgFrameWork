using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// NPC数据模块
/// </summary>
public class NpcModule : BaseModule
{
    private int npcIndex = 0;

    List<CustomerConfigData> customerDataList;

    private Dictionary<int, NpcData> mNpcDatas = new Dictionary<int, NpcData>();

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
    }
    /// <summary>
    /// 读取配置档数据
    /// </summary>
    internal override void ReadData()
    {
        customerDataList = ConfigDataManager.Instance.GetDatabase<CustomerConfigDatabase>().FindAll();
    }
    /// <summary>
    /// 创建新的NPC数据
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public NpcData CreateNpcData(int roleId)
    {
        var npcId = ++npcIndex;
        var npcData = new NpcData(npcId);
        npcData.roleid = roleId;

        return npcData;
    }
    /// <summary>
    /// 添加NPC数据
    /// </summary>
    /// <param name="data"></param>
    public void AddNpcData(NpcData data)
    {
        if (data == null) return;
        if(!mNpcDatas.ContainsKey(data.npcid))
        {
            mNpcDatas.Add(data.npcid, data);
        }
    }
    /// <summary>
    /// 删除NPC数据
    /// </summary>
    /// <param name="npcId"></param>
    public void RemoveNpcData(int npcId)
    {
        mNpcDatas.Remove(npcId);
    }
    /// <summary>
    /// 获取NPC数据
    /// </summary>
    /// <param name="npcid"></param>
    /// <returns></returns>
    public NpcData GetNpcData(int npcid)
    {
        NpcData npcData = null;
        mNpcDatas.TryGetValue(npcid, out npcData);
        return npcData;
    }
    /// <summary>
    /// 查找配置档数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CustomerConfigData GetConfigDataByID(int id)
    {
        return customerDataList.Find(x => x.Id == id);
    }

    /// <summary>
    /// 帧事件
    /// </summary>
    /// <param name="deltalTime"></param>
    internal override void OnUpdate(float deltalTime)
    {
        foreach(var npc in mNpcDatas)
        {
            if(npc.Value.fsm != null)
            {
                npc.Value.fsm.OnUpdate(deltalTime);
            }
        }
    }
}
