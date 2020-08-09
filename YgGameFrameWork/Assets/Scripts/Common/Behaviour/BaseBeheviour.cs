using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBeheviour
{
    static Dictionary<string, BaseManager> Managers = new Dictionary<string, BaseManager>();
    static Dictionary<string, BaseObject> ExtManagers = new Dictionary<string, BaseObject>();

    private Canvas _uiCanvas;

    protected Canvas uiCanvas
    {
        get
        {
            if (_uiCanvas == null)
            {
                _uiCanvas = GameObject.Find("/MainGame/UICanvas").GetComponent<Canvas>();
            }
            return _uiCanvas;
        }
    }

    private Camera _battleCamera;
    protected Camera battleCamera
    {
        get
        { 
            if(_battleCamera == null)
            {
                _battleCamera = Camera.main;
            }
            return _battleCamera;
        }
    }

    public T Instantiate<T>(T original) where T : UnityEngine.Object
    {
        return GameObject.Instantiate<T>(original);
    }

    public static void Destroy(UnityEngine.Object obj, float t)
    {
        if(obj != null)
        {
            GameObject.Destroy(obj, t);
        }
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return ManagementCenter.main.StartCoroutine(routine);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Initialize()
    {
        InitManagers();
        InitExtManager();
        InitComponent();
    }
    /// <summary>
    /// 初始化组件
    /// </summary>
    private static void InitComponent()
    {

    }
    /// <summary>
    /// 初始化扩展管理器
    /// </summary>
    private static void InitExtManager()
    {

    }

    private static void InitManagers()
    {
        AddManager<GameManager>();
    }
    /// <summary>
    /// 添加管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static T AddManager<T>() where T : BaseManager, new()
    {
        var type = typeof(T);
        var obj = new T();
        Managers.Add(type.Name, obj);

        return obj;
    }
    /// <summary>
    /// 获取管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetManager<T>() where T : class
    {
        var type = typeof(T);
        if(!Managers.ContainsKey(type.Name))
        {
            return null;
        }
        return Managers[type.Name] as T;
    }

    public static BaseManager GetManager(string managerName)
    {
        if(!Managers.ContainsKey(managerName))
        {
            return null;
        }
        return Managers[managerName];
    }
    /// <summary>
    /// 获取扩展管理器
    /// </summary>
    /// <param name="componentName"></param>
    /// <returns></returns>
    public static object GetExtManager(string componentName)
    {
        if (!ExtManagers.ContainsKey(componentName))
        {
            return null;
        }
        return ExtManagers[componentName];
    }
    /// <summary>
    /// 控制器更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public static void OnUpdate(float deltaTime)
    {
        //驱动所有管理器
        foreach(var mgr in Managers)
        {
            if(mgr.Value != null && mgr.Value.isOnUpdate)
            {
                mgr.Value.OnUpdate(deltaTime);
            }
        }
        //驱动所有组件
        foreach(var com in ExtManagers)
        {
            if(com.Value != null && com.Value.isOnUpdate)
            {
                com.Value.OnUpdate(deltaTime);
            }
        }
    }
}
