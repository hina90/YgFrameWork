using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// 面板控制器
/// </summary>
public class PanelManager : BaseManager
{
    //面板列表
    private Dictionary<string, GameObject> mPanels = new Dictionary<string, GameObject>();

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Initialize()
    {
        isOnUpdate = false;
    }
    /// <summary>
    /// 打开面板
    /// </summary>
    public void OpenPanel<T>(UILayer layer, object[] param = null) where T : UIBase
    {
        var panelName = typeof(T).Name;
        var uiMgr = ManagementCenter.GetManager<UIManager>();
        var parent = uiMgr.GetLayer(layer).transform;
        if(parent.Find(panelName) != null)
            return;


        var resPath = "Prefabs/UI/" + panelName;
        var resMgr = ManagementCenter.GetManager<ResourceManager>();
        //resMgr.LoadResAsync
        resMgr.LoadAssetAsync<GameObject>(resPath, new string[] { panelName }, delegate (UnityEngine.Object[] prefabs)
        {
            if(prefabs != null && prefabs[0] != null)
            {
                GameObject panelObj = CreatePanelInternal<T>(panelName, prefabs[0] as GameObject, parent, param);
                panelObj.GetComponent<Canvas>().sortingOrder = (int)layer + parent.childCount;
            }
        });
    }
    /// <summary>
    /// 创建面板预制体
    /// </summary>
    private GameObject CreatePanelInternal<T>(string panelName, GameObject prefab, Transform parent, object[] param = null) where T : UIBase
    {
        var gameObj = Instantiate<GameObject>(prefab);
        gameObj.name = panelName;
        gameObj.layer = LayerMask.NameToLayer("UI");
        gameObj.transform.SetParent(parent);
        gameObj.transform.localScale = Vector3.one;
        gameObj.transform.localPosition = Vector3.zero;
        UIBase uiScript = gameObj.GetOrAddComponent<T>();
        uiScript.Initialize(param);
        uiScript.Open();

        mPanels[panelName] = gameObj;

        return gameObj;
    }
    /// <summary>
    /// 销毁面板
    /// </summary>
    /// <param name="name"></param>
    private void DestroyPanel(string name)
    {
        var panelName = name;
        if(mPanels.ContainsKey(panelName))
        {
            var panel = mPanels[panelName];
            mPanels.Remove(panelName);
            panel.GetComponent<UIBase>().Release();

            Destroy(panel, 0);
        }
    }
    /// <summary>
    /// 关闭面板
    /// </summary>
    /// <param name="name"></param>
    public void ClosePanel(string name)
    {
        DestroyPanel(name);
    }

    /// <summary>
    /// 发送UI事件
    /// </summary>
    /// <param name="param"></param>
    public void SendPanelEvent(GameEvent eventType, params object[] param)
    {
        foreach (var panel in mPanels.Values)
        {
            UIBase ui = panel.GetComponent<UIBase>();
            Dictionary<GameEvent, Callback<object[]>> callback = ui.CtorEvent();
            if (callback != null)
            {
                if (callback.ContainsKey(eventType))
                    callback[eventType](param);
                else
                    continue;
            }
        }
    }

    /// <summary>
    /// 帧更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {

    }
}
