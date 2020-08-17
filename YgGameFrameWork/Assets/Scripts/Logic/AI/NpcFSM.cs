using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC状态机
/// </summary>
public class NpcFSM : BaseFSM
{
    public void Initialize(int npcId)
    {
        SetVar<int>("myNpcId", npcId);
        SetVar<int>("targetId", 0);
        base.Initialize();
    }

    public override void AddStates()
    {
        
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
