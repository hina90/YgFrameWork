using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//上一个场景的处理状态
public enum SceneState
{
    ReleasePre,
    HidePre
}

/// <summary>
/// 场景基础类
/// </summary>
public class BaseScene:BaseBeheviour
{
    protected object[] param;                        //参数
    private int preState;                           //处理上一个场景的状态（销毁or隐藏）
    private string resName;                         //资源名字
    private float loadProgress;                     //加载进度

    //UI上的监听事件列表
    Dictionary<GameEvent, Callback<object[]>> eventDic;


    public string ResName { get; set; }
    public float LoadProgress { get; set; }
    public SceneState PreState { get; set; }

    /// <summary>
    /// 场景初始化
    /// </summary>
    /// <param name="param"></param>
    protected virtual void OnCreate(params object[] param)
    {
        this.param = param;

        resName = "";
        //获取事件列表
        eventDic = CtorEvent();
        //UIManager.Instance.OpenUI<UI_Loading>();
    }

    /// <summary>
    /// 打开场景
    /// </summary>
    /// <param name="param"></param>
    public void Open(params object[] param)
    {
        PreState = SceneState.ReleasePre;

        OnCreate(param);       
        sceneMgr.LoadScene(ResName, PreState, 
            delegate()
            {
                Enter();
                //uiMgr.SendUIEvent(GameEvent.SCENE_LOAD_COMPLETE);
            },
            delegate(float value)
            {
                //uiMgr.SendUIEvent(GameEvent.SCENE_LOAD_PROGRESS, value);
            });
    }
    /// <summary>
    /// 进入场景
    /// </summary>
    protected virtual void Enter()
    {

    }
    /// <summary>
    /// 场景帧事件
    /// </summary>
    public new virtual void OnUpdate(float deltaTime)
    {
        
    }
    /// <summary>
    /// 场景事件
    /// </summary>
    public virtual Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        return null;
    }

    public virtual void Quit()
    {

    }
}
