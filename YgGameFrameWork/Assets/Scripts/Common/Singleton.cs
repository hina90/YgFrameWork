using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例数据类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    private static readonly object  _lock = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new T();
                }
            }
            return _instance;
        }
    }
}
/// <summary>
/// Unity单例类
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if(_instance == null)
                {
                    var name = typeof(T).Name;
                    GameObject _gameObject = new GameObject(name);
                    _instance = (T) _gameObject.AddComponent(typeof(T));
}
            }
            return _instance;
        }
    }
    ///初始化虚函数
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
