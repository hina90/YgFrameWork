using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using Tool.Database;

public class UI_MainGame : UIBase
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
    }

    /// <summary>
    /// 进入战斗UI
    /// </summary>
    protected override void Enter()
    {
   
    }
    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        eventDic = base.CtorEvent();


        return eventDic;
    }
    /// <summary>
    /// 释放
    /// </summary>
    public override void Release()
    {
        base.Release();
    }

    public override void MainUpdate()
    {

    }
}

