using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI类管理器
/// </summary>
public class CtrManager : BaseManager
{
    private Dictionary<string, UIBase> ctrlDic = new Dictionary<string, UIBase>();

    public override void Initialize()
    {
        AddCtrl<UI_MainGame>();
    }

    /// <summary>
    /// 添加UI类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="ctrl"></param>
    private void AddCtrl<T>() where T : UIBase, new()
    {
        var name = typeof(T).Name;
        ctrlDic[name] = new T();
    }
    /// <summary>
    /// 获取UI类
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UIBase GetCtrl(string name)
    {
        return ctrlDic[name];
    }
    /// <summary>
    /// 移除UI类
    /// </summary>
    /// <param name="name"></param>
    public void RemoveCtrl(string name)
    {
        if(ctrlDic.ContainsKey(name))
        {
            ctrlDic.Remove(name);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
       
    }

    public override void OnDispose()
    {
        
    }
}
