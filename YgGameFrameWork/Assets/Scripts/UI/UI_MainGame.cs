using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Tool.Database;
using TMPro;
using System;
using DG.Tweening;
using Tool.Database;
using Spine.Unity;
using Spine;

public class UI_MainGame : UIBase
{
    #region UI组件定义
    public Button settingBtn;
    public Button excavateBtn;
    public Button cultivateBtn;
    public DOTweenAnimation tweenCultivate;
    public Text costText;
    public GameObject mBtnUFO;
    public SkeletonGraphic mSkeletonUfo;
    private Bone mUfoBone = null;
    public Text mUfoText;

    public GameObject mEffectPanel;
    public GameObject mEffIconBig;
    public GameObject mEffGroup;
    public Text mEffectText;

    public Transform childTran;
    #endregion
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        Layer = LayerMenue.UI;
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

        eventDic[GameEvent.CULTIVATE_CAN_UPGRADE] = delegate (object[] param)
        {

        };

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

