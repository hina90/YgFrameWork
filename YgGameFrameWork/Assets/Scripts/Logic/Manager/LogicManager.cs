using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏逻辑管理器
/// </summary>
public class LogicManager : LogicBehaviour
{
    public override void Initialize()
    {
        AddManager<IdleViewManager>();
        AddManager<LogicManager>(this);


        idleMgr.Initialize();
    }

    public override void OnUpdate(float deltaTime)
    {
         
    }

    public override void OnDispose()
    {
         
    }
}
