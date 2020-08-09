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


        Application.targetFrameRate = AppConst.GameFrameRate;
    }

    public override void OnUpdate(float deltaTime)
    {
       
    }

    public override void OnDispose()
    {
        
    }
}

