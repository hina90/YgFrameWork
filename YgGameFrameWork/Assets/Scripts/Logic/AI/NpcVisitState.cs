using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 访问状态
/// </summary>
public class NpcVisitState : BaseState
{
    private NpcFSM npcFsm;
    private FsmVar<int> myNpcId;

    private Transform endPoint;

    /// <summary>
    /// 初始化状态
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        npcFsm = (NpcFSM)Machine;
    }
    /// <summary>
    /// 进入状态
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        myNpcId = npcFsm.GetVar<int>("myNpcId");

        var npcMgr = ManagementCenter.GetManager<NpcManager>();
        NpcView npc = npcMgr.GetNpc(myNpcId.value) as NpcView;
        npc.FindPath(myNpcId.value, GetEndPoint());
    }

    private Vector3 GetEndPoint()
    {
        GameObject endPoint = GameObject.Find("Scene_0/PathObject/PathPoint/endPoint");

        return endPoint.transform.position;
    }

    public override void Execute()
    {
        base.Execute();
    }
    /// <summary>
    /// 退出状态
    /// </summary>
    public override void Exit()
    {
        base.Exit();
    }
}
