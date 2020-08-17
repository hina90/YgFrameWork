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
    /// <summary>
    /// 初始化游戏管理器
    /// </summary>
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

        //初始化资源
        ResInitialize();
    }
    /// <summary>
    /// 初始化资源
    /// </summary>
    public void ResInitialize()
    {
        if(AppConst.DebugMode)
        {
            OnResInitOK();
        }
    }
    /// <summary>
    /// 初始化资源完成
    /// </summary>
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
        panelMgr.Initialize();
        sceneMgr.Initialize();
        objMgr.Initialize();
        npcMgr.Initialize();

        sceneMgr.ChangeScene("MainScene");

        //panelMgr.OpenPanel<UI_MainGame>(UILayer.Common);
    }

    public override void OnUpdate(float deltaTime)
    {
       
    }

    public override void OnDispose()
    {
        
    }
}

