using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Npc离开状态
/// </summary>
public class NpcLeaveState : BaseState
{
    private NpcFSM npcFsm;
    private FsmVar<int> myNpcId;
    private NpcData myNpcData;

    /// <summary>
    /// 初始化状态数据
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
    }
    /// <summary>
    /// 进入状态
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        npcFsm = (NpcFSM)Machine;

        myNpcId = npcFsm.GetVar<int>("mynpcId");
        //myNpcData = 
    }
    /// <summary>
    /// 执行状态
    /// </summary>
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
