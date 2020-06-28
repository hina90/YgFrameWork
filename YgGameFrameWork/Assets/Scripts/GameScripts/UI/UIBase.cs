using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// UI层级类型
/// </summary>
public enum LayerMenue
{
    UI,
    PUBLIC,
    TIPS,
    LOADING,
    BLACK,
    MSGBOX,
    GUIDE,
    CLICK,
    MONEY,
    WARNINGTIP,
}

/// <summary>
/// UI基础类
/// </summary>
public class UIBase : MonoBehaviour
{
    public object[] param;                          //参数

    public UIBase BackUi { get; set; }              //上一层UI
    public LayerMenue Layer { get; set; }           //UI层级
    //UI上的监听事件列表
    Dictionary<GameEvent, Callback<object[]>> eventDic;

    private bool uiCamera;

    /// <summary>
    /// 设置UI摄像机
    /// </summary>
    public bool UICamera
    {
        get
        {
            return UICamera;
        }
        set
        {
            uiCamera = value;
            Canvas canvas = GetComponent<Canvas>();
            if (uiCamera) UIManager.Instance.AddUICamera(canvas);
            else UIManager.Instance.RemoveUICamera(canvas);
        }
    }
    /// <summary>
    /// UI初始化事件（初始化的数据处理放此处，如：配置档数据）
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// 打开UI
    /// </summary>
    public void Open()
    {
        //获取事件列表
        eventDic = CtorEvent();
        Show();
        Enter();
    }
    /// <summary>
    /// 进入UI
    /// </summary>
    protected virtual void Enter()
    {
        GuideManager.Instance.CheckGuide(name);
    }
    /// <summary>
    /// 播放UI关闭时动画
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public virtual bool PlayEndAnimation(Callback callback)
    {
        return false;
    }
    /// <summary>
    /// 播放UI打开动画
    /// </summary>
    /// <param name="callback"></param> 
    /// <returns></returns>
    public virtual bool PlayBeginAnimation(Callback callback)
    {
        return false;
    }
    public void PlayAnimation(GameObject gameObject)
    {
        gameObject.transform.DOScale(Vector3.one, 0.2f);
    }
    /// <summary>
    /// 查找子对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Find(GameObject obj, string name)
    {
        if (obj.name.Equals(name))
            return obj;

        int childCount = obj.transform.childCount;
        int i = 0;
        Transform child;
        GameObject childObj;
        while (i < childCount)
        {
            child = obj.transform.GetChild(i);
            childObj = Find(child.gameObject, name);
            if (childObj != null)
            {
                return childObj;
            }
            i++;
        }
        return null;
    }
    /// <summary>
    /// 查找子对象组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Find<T>(GameObject obj, string name) where T : Component
    {
        GameObject uiObj = Find(obj, name);

        return uiObj.GetOrAddComponent<T>();
    }
    /// <summary>
    /// 事件
    /// </summary>
    public virtual Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        return null;
    }
    /// <summary>
    /// UI上的帧事件
    /// </summary>
    public virtual void MainUpdate()
    {

    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 显示
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 返回到上一层UI
    /// </summary>
    public void BackToUI()
    {
        UIManager.Instance.BackUI(Layer);
    }
    /// <summary>
    /// 释放
    /// </summary>
    public virtual void Release()
    {
        UIManager.Instance.Release(this);
    }
}
