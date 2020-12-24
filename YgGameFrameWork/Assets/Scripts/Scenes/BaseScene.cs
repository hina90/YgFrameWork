using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    protected object[] param;                       //参数
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
        panelMgr.OpenPanel<UI_Loading>(UILayer.Fixed);
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
                if (param.Length > 0 && param[0].ToString().Contains("Map"))
                {
                    LoadMap(param[0] as string, () =>
                    {
                        Enter();
                        panelMgr.SendPanelEvent(GameEvent.SCENE_LOAD_COMPLETE);
                    });
                }
                else
                {
                    Enter();
                    panelMgr.SendPanelEvent(GameEvent.SCENE_LOAD_COMPLETE);
                }

                //Enter();
                //panelMgr.SendPanelEvent(GameEvent.SCENE_LOAD_COMPLETE);
            },
            delegate(float value)
            {
                panelMgr.SendPanelEvent(GameEvent.SCENE_LOAD_PROGRESS, value);
            });
    }
    /// <summary>
    /// 加载地图
    /// </summary>
    protected void LoadMap(string mapName, Action callback)
    {
        var resPath = "Maps/" + mapName;
        var resMgr = ManagementCenter.GetManager<ResourceManager>();

        resMgr.LoadAssetAsync<GameObject>(resPath, new string[] { mapName }, delegate (UnityEngine.Object[] prefabs)
        {
            if (prefabs != null && prefabs[0] != null)
            {
                var gameObj = Instantiate<GameObject>(prefabs[0] as GameObject);
                gameObj.transform.SetParent(battleScene);
                gameObj.transform.localScale = Vector3.one;
                gameObj.transform.localPosition = Vector3.zero;

                callback?.Invoke();
            }
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
