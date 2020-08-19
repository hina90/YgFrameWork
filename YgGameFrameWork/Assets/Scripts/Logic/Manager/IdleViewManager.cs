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
        GameObject startPoint = GameObject.Find("Scene_0/PathObject/PathPoint/startPoint");
        
        var npcMgr = ManagementCenter.GetManager<NpcManager>();

        var npcModule = moduleMgr.GetModule<NpcModule>();
        NpcData npcData = npcModule.CreateNpcData(1000);
        npcData.roleid = 1000;
        npcData.fsm = new NpcFSM();
        npcModule.AddNpcData(npcData);

        var roleView = npcMgr.CreateNpc<NpcView>(npcData, IdleScene.transform);
        npcMgr.AddNpc(npcData.npcid, roleView);
        roleView.Initialize(npcData, startPoint.transform.position, ()=>
        {
            npcData.fsm.Initialize(npcData.npcid);
        });
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {
        
    }
}
