using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 游戏数据模块管理器
/// </summary>
public class GameModuleManager : BaseManager
{
    public Dictionary<string, BaseModule> m_ModuleDic;


    public override void Initialize()
    {
        m_ModuleDic = new Dictionary<string, BaseModule>();
    }

    /// <summary>
    /// 系统模块帧事件
    /// </summary>
    public override void OnUpdate(float deltaTime)
    {
        foreach (var module in m_ModuleDic.Values)
        {
            module.OnUpdate();
        }
    }

    public override void OnDispose()
    {
        
    }

    /// <summary>
    /// 重置数据模块需要重置的数据
    /// </summary>
    private void ResetAllModuleData()
    {
        foreach (var module in m_ModuleDic.Values)
        {
            module.ResetData();
        }
    }

    /// <summary>
    /// 注册系统模块
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private T RegisterModule<T>() where T : BaseModule, new()
    {
        string key = typeof(T).ToString();
        if (m_ModuleDic.ContainsKey(key)) return null;
        m_ModuleDic[key] = new T();
        m_ModuleDic[key].Init(this);
        return m_ModuleDic[key] as T;
    }

    /// <summary>
    /// 获取系统模块
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>() where T : BaseModule, new()
    {
        string name = typeof(T).ToString();
        if (!m_ModuleDic.TryGetValue(name, out BaseModule module))
        {
            module = RegisterModule<T>();
        }
        return (T)module;
    }

    /// <summary>
    /// 保存模块数据
    /// </summary>
    public void SaveGameData<T>() where T : BaseModule
    {
        string name = typeof(T).ToString();
        if (!m_ModuleDic.TryGetValue(name, out BaseModule module))
        {
            TDDebug.DebugLogError("Please register " + name);
        }
        m_ModuleDic[name].SaveData();
    }

    /// <summary>
    /// 保存所有模块数据
    /// </summary>
    public void SaveGameData()
    {
        foreach (var module in m_ModuleDic.Values)
        {
            module.SaveData();
        }
    }
}
