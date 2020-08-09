using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 游戏入口主类
/// </summary>
public class Main :GameBehaviour
{
    protected override void OnAwake()
    {
        AppConst.AppState = AppState.IsPlaying;
        this.Initialize();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize()
    {
        BaseBeheviour.Initialize();
        DontDestroyOnLoad(gameObject);

        var gameMgr = ManagementCenter.GetManager<GameManager>();
        if(gameMgr != null)
        {
            gameMgr.Initialize();
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        BaseBeheviour.OnUpdate(Time.deltaTime);
    }

    protected override void OnDestroyMe()
    {
        base.OnDestroyMe();
        Debug.Log("~Main was destroyed");
    }

    private void OnApplicationQuit()
    {
        AppConst.AppState = AppState.Exiting;
    }
}
