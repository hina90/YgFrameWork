using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 放置模拟场景逻辑管理器
/// </summary>
public class IdleViewManager : LogicBehaviour
{
    public override void Initialize()
    {
        var npcMgr = ManagementCenter.GetManager<NpcManager>();

        var npcModule = moduleMgr.GetModule<NpcModule>();
        NpcData npcData = npcModule.CreateNpcData(1000);
        npcData.roleid = 1000;
        npcData.fsm = new NpcFSM();
        npcData.fsm.Initialize(npcData.npcid);
        npcModule.AddNpcData(npcData);

        var roleView = npcMgr.CreateNpc<NpcView>(npcData, IdleScene.transform);
        npcMgr.AddNpc(npcData.npcid, roleView);
        roleView.Initialize(npcData, new Vector3(-1, 1, 0));
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {
        
    }
}
