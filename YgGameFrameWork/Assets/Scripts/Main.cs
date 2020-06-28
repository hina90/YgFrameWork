using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏入口主类 
/// </summary>
public class Main : UnitySingleton<Main>
{
    /// <summary>
    /// 开始
    /// </summary>
    public void StartGame()
    {
        Caching.ClearCache();                                           //清理资源包缓存
        Application.targetFrameRate = 30;
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        SetDesignContentScale();
        //GuideManager.Instance.Init();
        //UIManager.Instance.Init();
        //CSceneManager.Instance.Init();
        //ConfigManager.Instance.Init();

        //GameModuleManager.Instance.Init();
        //AudioManager.Instance.Init();
        //TimerManager.Instance.StartGameTime();
        //LanguageManager.Init();
        //GameManager.Instance.Init();
        //TDDebug.GetInstance();

        //UIManager.Instance.OpenUI<UI_Black>();
    }
    private void SetDesignContentScale()
    {
#if UNITY_STANDALONE_WIN
        float designWidth = 720;
        float designHeight = 1280;
        float contentScale = Screen.currentResolution.height / designHeight;
        if (contentScale > 1) contentScale = 1;

        designWidth *= contentScale * 0.8f;

        Screen.SetResolution((int)designWidth, (int)designHeight, false);
#endif
    }
    /// <summary>
    /// 帧事件
    /// </summary>
    private void Update()
    {
        //UIManager.Instance.MainUpdate();
        //CSceneManager.Instance.MainUpdate();
        //GameModuleManager.Instance.MainUpdate();
        //TimerManager.Instance.UpdateGameTime();
    }
}
