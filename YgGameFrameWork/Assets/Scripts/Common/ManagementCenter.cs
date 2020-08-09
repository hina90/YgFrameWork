using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏管理器对象
/// </summary>
public class ManagementCenter 
{
    private static GameObject _managerObject = null;
    public static GameObject managerObject
    {
        get
        {
            if(_managerObject == null)
                _managerObject = GameObject.FindWithTag("GameManager");
            return _managerObject;
        }
    }

    private static Main _main = null;

    public static Main main
    {
        get
        {
            if(_main == null)
                _main = managerObject.GetComponent<Main>();
            return _main;
        }
    }

    /// <summary>
    /// 获取管理器
    /// </summary>
    /// <param name="managerName"></param>
    /// <returns></returns>
    public static BaseManager GetManager(string managerName)
    {
        return BaseBeheviour.GetManager(managerName);
    }
    public static T GetManager<T>() where T : class
    {
        return BaseBeheviour.GetManager<T>();
    }
    /// <summary>
    /// 获取扩展管理器
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static object GetExtManager(string name)
    {
        return BaseBeheviour.GetExtManager(name);
    }
}
