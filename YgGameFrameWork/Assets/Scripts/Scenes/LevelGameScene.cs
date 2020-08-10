using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameScene : BaseScene
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    protected override void OnCreate(params object[] param)
    {
        base.OnCreate(param);
        ResName = "LevelGameScene";
    }

    protected override void Enter()
    {
        base.Enter();
        //AppStart.Instance.StartGameLevel();
    }

    public override void Quit()
    {
        base.Quit();
    }
}
