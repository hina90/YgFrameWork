using System.Collections.Generic;
using UnityEngine;

public class UIManager : UnitySingleton<UIManager>
{
    private Dictionary<string, UIBase> uiDic = new Dictionary<string, UIBase>();
    private Dictionary<LayerMenue, UIBase> openUIDic = new Dictionary<LayerMenue, UIBase>();
    private Dictionary<string, GameObject> uiPrefabDic = new Dictionary<string, GameObject>();

    private Camera uiCamera;

    public Camera UiCamera { get => uiCamera; }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        RegisterUI("UI_Loading");
        RegisterUI("UI_ToolTip");
        RegisterUI("UI_TipMsg");
        RegisterUI("UI_Main");
        RegisterUI("UI_Black");
        RegisterUI("UI_Customer");
        RegisterUI("UI_Menu");
        RegisterUI("UI_Money");
        RegisterUI("UI_Setting");
        RegisterUI("UI_Task");
        RegisterUI("UI_TipsControl");
        RegisterUI("UI_Parterre");
        RegisterUI("UI_WishingPool");
        RegisterUI("UI_Cashier");
        RegisterUI("UI_Goods");
        RegisterUI("UI_Guide");
        RegisterUI("UI_SeedMutationAds");
        RegisterUI("UI_DeliveryAds");
        RegisterUI("UI_GuestProfile");
        //RegisterUI("UI_ClickEffect");

        GameObject ugui = GameObject.Find("ugui_root");
        uiCamera = ugui.transform.Find("UI_Camera").GetComponent<Camera>();
        uiCamera.gameObject.SetActive(false);
    }
    /// <summary>
    /// 注册UI
    /// </summary>
    /// <param name="uiName">UI名字</param>
    private void RegisterUI(string uiName)
    {
        ResourceManager.Instance.AddResource(uiName, ResouceType.UI);
    }
    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="names">参数组</param>
    public void OpenUI<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        UIBase ui = uiDic.TryGet(uiName);
        if (ui == null)
        {
            ui = GetUIBase<T>(param);
        }
        ui.param = param;

        var beginAni = false;
        UIBase openUi = openUIDic.TryGet(ui.Layer);
        if (openUi != null)
        {
            if (ui.name != openUi.name)
            {
                ui.BackUi = openUi;
                openUi.Hide();
                beginAni = ui.PlayBeginAnimation(() =>
                {

                    openUIDic[ui.Layer] = ui;
                    openUIDic[ui.Layer].Open();
                });
            }
        }

        if (!beginAni)
        {
            openUIDic[ui.Layer] = ui;
            openUIDic[ui.Layer].Open();
        }
    }
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
    /// 获取UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private UIBase GetUIBase<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        GameObject uiObject = ResourceManager.Instance.GetResourceInstantiate(uiName, transform, ResouceType.UI);
        uiObject.name = uiName;
        UIBase uibase = uiObject.GetOrAddComponent<T>();
        uibase.param = param;
        uibase.Init();
        uiDic.Add(uiName, uibase);

        return uibase;
    }
    /// <summary>
    /// 根据资源名获取UI
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase GetResUI(string uiResName)
    {
        foreach (UIBase ui in openUIDic.Values)
        {
            if (ui.name == uiResName)
            {
                return ui;
            }
        }
        return null;
    }
    /// <summary>
    /// 获取UI
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public T GetUI<T>(string uiName) where T : UIBase
    {
        return uiDic[uiName] as T;
    }
    /// <summary>
    /// 返回上一层UI
    /// </summary>
    /// <param name="layer"></param>
    public void BackUI(LayerMenue layer)
    {
        UIBase ui = openUIDic.TryGet(layer);

        if (ui == null) return;

        if (ui.BackUi == null)
        {
            CloseUI(layer);
            return;
        }
        var endAni = ui.PlayEndAnimation(() =>
        {
            openUIDic[layer].Release();
            openUIDic[layer] = ui.BackUi;
            openUIDic[layer].Open();
            ui.BackUi = null;
        });
        if (!endAni)
        {
            openUIDic[layer].Release();
            openUIDic[layer] = ui.BackUi;
            openUIDic[layer].Open();
            ui.BackUi = null;
        }
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="layer"></param>
    /// 
    public void CloseUI(LayerMenue layer)
    {
        UIBase ui = openUIDic.TryGet(layer);
        if (ui == null) return;

        ui.Release();
        openUIDic.Remove(layer);
    }
    /// <summary>
    /// 关闭所有UI
    /// </summary>
    /// <param name="layer"></param>
    public void CloseAllUI()
    {
        foreach (UIBase ui in openUIDic.Values)
        {
            ui.Release();
        }
        openUIDic.Clear();
    }
    /// <summary>
    /// 销毁UI
    /// </summary>
    /// <param name="ui"></param>
    public void Release(UIBase ui)
    {
        if (uiDic.ContainsKey(ui.name))
            uiDic.Remove(ui.name);

        Destroy(ui.gameObject);
    }
    /// <summary>
    /// 发送UI事件
    /// </summary>
    /// <param name="param"></param>
    public void SendUIEvent(GameEvent eventType, params object[] param)
    {
        foreach (UIBase ui in openUIDic.Values)
        {
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
    /// UI帧事件
    /// </summary>
    public void MainUpdate()
    {
        foreach (UIBase ui in openUIDic.Values)
        {
            ui.MainUpdate();
        }
    }
}
