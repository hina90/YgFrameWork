using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// NPC状态机
/// </summary>
public class NpcFSM : BaseFSM
{

    public void Initialize(int npcId)
    {
        SetVar<int>("myNpcId", npcId);

        base.Initialize();

    }
    /// <summary>
    /// 添加状态
    /// </summary>
    public override void AddStates()
    {
        AddState<NpcVisitState>();

        SetInitialState<NpcVisitState>();
    }

    /// <summary>
    /// 帧函数
    /// </summary>
    /// <param name="deltaTime"></param>
    public void OnUpdate(float deltaTime)
    {
        OnExecute(deltaTime);
    }
}
