using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicBehaviour
{
    static Dictionary<string, LogicBehaviour> logicManagers = new Dictionary<string, LogicBehaviour>();

    private static LogicManager _logicMgr;

    private GameObject _idleScene;

    /// <summary>
    /// 放置经营场景
    /// </summary>
    protected GameObject IdleScene
    {
        get
        {
            if(_idleScene == null)
            {
                _idleScene = GameObject.Find("/MainGame/IdleScene");
            }
            return _idleScene;
        }
    }
    public static LogicManager logicMgr
    {
        get
        {
            if (_logicMgr == null)
            {
                _logicMgr = GetManager<LogicManager>();
            }
            return _logicMgr;
        }
    }

    private static GameModuleManager _moduleMgr;
    protected static GameModuleManager moduleMgr
    {
        get
        {
            if (_moduleMgr == null)
            {
                _moduleMgr = GameModuleManager.Create();
            }
            return _moduleMgr;
        }
    }

    private static ConfigManager _configMgr;
    protected static ConfigManager configMgr
    {
        get
        {
            if (_configMgr == null)
            {
                _configMgr = ConfigManager.Create();
            }
            return _configMgr;
        }
    }

    private static CTimer _timerMgr;
    public static CTimer timerMgr
    {
        get
        {
            if (_timerMgr == null)
            {
                _timerMgr = CTimer.Create();
            }
            return _timerMgr;
        }
    }
    private static IdleViewManager _idleManger;
    public static IdleViewManager idleMgr
    {
        get
        {
            if(_idleManger == null)
            {
                _idleManger = GetManager<IdleViewManager>();
            }
            return _idleManger;
        }
    }


    public virtual void Initialize()
    {
    }

    /// <summary>
    /// 添加管理器
    /// </summary>
    protected static T AddManager<T>() where T : LogicBehaviour, new()
    {
        var type = typeof(T);
        var obj = new T();
        logicManagers.Add(type.Name, obj);
        return obj;
    }

    /// <summary>
    /// 添加管理器
    /// </summary>
    protected static void AddManager<T>(LogicBehaviour obj) where T : LogicBehaviour
    {
        var type = typeof(T);
        logicManagers.Add(type.Name, obj);
    }

    /// <summary>
    /// 获取管理器
    /// </summary>
    public static T GetManager<T>() where T : class
    {
        var type = typeof(T);
        if (!logicManagers.ContainsKey(type.Name))
        {
            return null;
        }
        return logicManagers[type.Name] as T;
    }

    public virtual void OnUpdate(float deltaTime)
    {
        foreach(var mgr in logicManagers)
        {
            if(mgr.Value != null)
            {
                mgr.Value.OnUpdate(deltaTime);
            }
        }
    }

    public virtual void OnDispose()
    {
    }
}
