using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;
using DG.Tweening;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : BaseManager
{
    public override void Initialize()
    {
        QualitySettings.vSyncCount = 2;
        DOTween.Init(true, true, LogBehaviour.Default);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.runInBackground = true;

        var settings = Utils.LoadGameSettings();
        if(settings != null)
        {
            AppConst.LogMode = settings.logMode;
            AppConst.DebugMode = settings.debugMode;
            AppConst.GameFrameRate = settings.GameFrameRate;
        }
        Utils.SetDebugState(AppConst.LogMode);
        Application.targetFrameRate = AppConst.GameFrameRate;
        //创建loading UI 
        ResInitialize();
    }

    public void ResInitialize()
    {
        if(AppConst.DebugMode)
        {
            OnResInitOK();
        }
    }

    private void OnResInitOK()
    {
        resMgr.Initialize();
        resMgr.InitMainfest(AppConst.ResIndexFile, delegate()
        {
            this.OnInitialzeOK();
        });
    }
    /// <summary>
    /// 初始化完成
    /// </summary>
    void OnInitialzeOK()
    {
        configMgr.Initialize();
        tableMgr.Initialize();
        soundMgr.Initialize();

        timerMgr.Initialize();
        moduleMgr.Initialize();
        uiMgr.Initialize();
        sceneMgr.Initialize();
        actorMgr.Initialize();

        Debug.Log("------------初始化完成-------------");

        panelMgr.OpenPanel<UI_MainGame>(UILayer.Fixed);
    }

    public override void OnUpdate(float deltaTime)
    {
       
    }

    public override void OnDispose()
    {
        
    }
}

