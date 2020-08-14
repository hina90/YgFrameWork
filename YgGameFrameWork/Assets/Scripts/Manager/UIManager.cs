
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI层级管理器
/// </summary>
public class UIManager : BaseManager
{
    //private Dictionary<string, UIBase> uiDic = new Dictionary<string, UIBase>();
    //private Dictionary<LayerMenue, UIBase> openUIDic = new Dictionary<LayerMenue, UIBase>();
    //private Dictionary<string, GameObject> uiPrefabDic = new Dictionary<string, GameObject>();
    private Dictionary<UILayer, GameObject> layerDic = new Dictionary<UILayer, GameObject>();


    /// <summary>
    /// 添加UI摄像机
    /// </summary>
    public void AddUICamera(Canvas canvas)
    {
        if (uiCamera != null)
        {
            canvas.worldCamera = uiCamera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            uiCamera.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 隐藏UI摄像机
    /// </summary>
    public void RemoveUICamera(Canvas canvas)
    {
        uiCamera.gameObject.SetActive(false);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Initialize()
    {
        InitUILayers();
    }
    /// <summary>
    /// 初始化层级
    /// </summary>
    private void InitUILayers()
    {
        CreateLayer("Common", UILayer.Common);
        CreateLayer("Fixed", UILayer.Fixed);
        CreateLayer("Movie", UILayer.Movie);
        CreateLayer("Effect", UILayer.Effect);
        CreateLayer("Top", UILayer.Top);
    }
    /// <summary>
    /// 创建各层级
    /// </summary>
    /// <param name="name"></param>
    /// <param name="layerType"></param>
    private void CreateLayer(string name, UILayer layerType)
    {
        string layerName = name + "_Layer";
        GameObject layerObj = new GameObject(layerName);
        layerObj.layer = LayerMask.NameToLayer("UI");
        layerObj.transform.SetParent(uiCanvas.transform);
        layerObj.transform.localScale = Vector3.one;

        RectTransform rect = layerObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition3D = Vector3.zero;
        rect.SetSiblingIndex((int)layerType);

        layerDic[layerType] = layerObj;
    }
    /// <summary>
    /// 获取层级
    /// </summary>
    /// <param name="layerType"></param>
    /// <returns></returns>
    public GameObject GetLayer(UILayer layerType)
    {
        return layerDic[layerType];
    }

    public override void OnUpdate(float deltaTime)
    {
         
    }


    public override void OnDispose()
    {
        
    }

    ///// <summary>
    ///// 注册UI
    ///// </summary>
    ///// <param name="uiName">UI名字</param>
    //private void RegisterUI(string uiName)
    //{
    //    //ResourceManager.Instance.AddResource(uiName, ResouceType.UI);
    //}
    ///// <summary>
    ///// 打开UI
    ///// </summary>
    ///// <param name="names">参数组</param>
    //public UIBase OpenUI<T>(params object[] param) where T : UIBase
    //{
    //    string uiName = typeof(T).ToString();
    //    UIBase ui = uiDic.TryGet(uiName);
    //    if (ui == null)
    //    {
    //        ui = GetUIBase<T>(param);
    //    }

    //    var beginAni = false;
    //    UIBase openUi = openUIDic.TryGet(ui.Layer);
    //    if (openUi != null)
    //    {
    //        if (ui.name != openUi.name)
    //        {
    //            ui.BackUi = openUi;
    //            openUi.Hide();
    //            beginAni = ui.PlayBeginAnimation(() =>
    //            {
    //                openUIDic[ui.Layer] = ui;
    //                openUIDic[ui.Layer].Open();
    //            });
    //        }
    //    }

    //    if (!beginAni)
    //    {
    //        openUIDic[ui.Layer] = ui;
    //        openUIDic[ui.Layer].Open();
    //    }
    //    else
    //    {
    //        ui.Hide();
    //    }
    //    return ui;
    //}

    ///// <summary>
    ///// 获取UI
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //private UIBase GetUIBase<T>(params object[] param) where T : UIBase
    //{
    //    //string uiName = typeof(T).ToString();
    //    //GameObject uiObject = ResourceManager.Instance.GetResourceInstantiate(uiName, transform, ResouceType.UI);
    //    //uiObject.name = uiName;
    //    //UIBase uibase = uiObject.GetOrAddComponent<T>();
    //    //uibase.param = param;
    //    //uibase.Init();
    //    //uiDic.Add(uiName, uibase);

    //    //return uibase;
    //    return null;
    //}
    ///// <summary>
    ///// 根据资源名获取UI
    ///// </summary>
    ///// <param name="uiName"></param>
    ///// <returns></returns>
    //public UIBase GetResUI(string uiResName)
    //{
    //    foreach (UIBase ui in openUIDic.Values)
    //    {
    //        if (ui.name == uiResName)
    //        {
    //            return ui;
    //        }
    //    }
    //    return null;
    //}
    ///// <summary>
    ///// 获取UI
    ///// </summary>
    ///// <param name="uiName"></param>
    ///// <returns></returns>
    //public UIBase GetUI<T>(string uiName) where T : UIBase
    //{
    //    if (!uiDic.ContainsKey(uiName))
    //        return null;
    //    return uiDic[uiName] as T;
    //}
    ///// <summary>
    ///// 返回上一层UI
    ///// </summary>
    ///// <param name="layer"></param>
    //public void BackUI(LayerMenue layer)
    //{
    //    UIBase ui = openUIDic.TryGet(layer);
    //    if (ui == null) return;

    //    if (ui.BackUi == null)
    //    {
    //        var endAni2 = ui.PlayEndAnimation(() =>
    //        {
    //            CloseUI(layer);
    //        });
    //        if (!endAni2)
    //        {
    //            CloseUI(layer);
    //        }
    //        return;
    //    }
    //    var endAni = ui.PlayEndAnimation(() =>
    //    {
    //        openUIDic[layer].Release();
    //        openUIDic[layer] = ui.BackUi;
    //        openUIDic[layer].Open();
    //        ui.BackUi = null;
    //    });
    //    if (!endAni)
    //    {
    //        openUIDic[layer].Release();
    //        openUIDic[layer] = ui.BackUi;
    //        openUIDic[layer].Open();
    //        ui.BackUi = null;
    //    }
    //}

    ///// <summary>
    ///// 关闭UI
    ///// </summary>
    ///// <param name="layer"></param>
    ///// 
    //public void CloseUI(LayerMenue layer)
    //{
    //    UIBase ui = openUIDic.TryGet(layer);
    //    if (ui == null) return;

    //    ui.Release();
    //    openUIDic.Remove(layer);
    //}
    ///// <summary>
    ///// 关闭所有UI
    ///// </summary>
    ///// <param name="layer"></param>
    //public void CloseAllUI()
    //{
    //    foreach (UIBase ui in openUIDic.Values)
    //    {
    //        ui.Release();
    //    }
    //    openUIDic.Clear();
    //}
    ///// <summary>
    ///// 销毁UI
    ///// </summary>
    ///// <param name="ui"></param>
    //public void Release(UIBase ui)
    //{
    //    if (uiDic.ContainsKey(ui.name))
    //        uiDic.Remove(ui.name);

    //     Destroy(ui.gameObject, 0.1f);
    //}
    ///// <summary>
    ///// 发送UI事件
    ///// </summary>
    ///// <param name="param"></param>
    //public void SendUIEvent(GameEvent eventType, params object[] param)
    //{
    //    foreach (UIBase ui in openUIDic.Values)
    //    {
    //        Dictionary<GameEvent, Callback<object[]>> callback = ui.CtorEvent();
    //        if (callback != null)
    //        {
    //            if (callback.ContainsKey(eventType))
    //                callback[eventType](param);
    //            else
    //                continue;
    //        }
    //    }
    //}
    ///// <summary>
    ///// UI帧事件
    ///// </summary>
    //public void MainUpdate()
    //{
    //    foreach (UIBase ui in openUIDic.Values)
    //    {
    //        ui.MainUpdate();
    //    }
    //}
    
    //public bool DoUIAnimation(GameObject inUI, eUIAnimation inType, Callback callback = null)
    //{
    //    switch (inType)
    //    {
    //        case eUIAnimation.PopUp:
    //            {
    //                inUI.transform.localScale = Vector3.one * 0.8f;
    //                inUI.transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() =>
    //                 {
    //                     inUI.transform.DOScale(Vector3.one * 1f, 0.05f);
    //                 });
    //            }
    //            break;
    //    }
    //    return true;
    //}
    //public void DoPopUpAnimationWithValue(GameObject inUI, float start,float end,float during,Callback callback = null)
    //{
    //    inUI.transform.localScale = Vector3.one * start;
    //    inUI.transform.DOScale(Vector3.one * 1.1f * end, during).OnComplete(() =>
    //    {
    //        inUI.transform.DOScale(Vector3.one * end, during / 2f);
    //    });
    //}
    //public void DoMoveAndScaleWithValue(GameObject inUI, Vector3 startPos, Vector3 endPos, 
    //    float startScale,float endScale,float during, Callback callback = null)
    //{
    //    inUI.transform.localPosition = startPos;
    //    inUI.transform.DOLocalMove(endPos, during);
    //    inUI.transform.localScale = Vector3.one * startScale;
    //    inUI.transform.DOScale(Vector3.one * endScale, during).OnComplete(() =>
    //    {
    //        if (callback != null)
    //            callback();
    //    });
    //}
}
