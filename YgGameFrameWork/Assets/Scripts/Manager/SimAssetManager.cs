using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using UObject = UnityEngine.Object;

public class SimAssetManager
{
    ResourceManager mResMgr;
    public SimAssetManager(ResourceManager manager)
    {
        mResMgr = manager;
    }
    public void Initialize(Action initOK)
    {
        initOK?.Invoke();
    }
    /// <summary>
    /// 获取资源拓展名
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string GetExtName(Type type)
    {
        if (type == typeof(GameObject))
        {
            return ".prefab";
        }
        else if (type == typeof(Texture2D) || type == typeof(Sprite))
        {
            return ".png";
        }
        else if (type == typeof(AudioClip))
        {
            return ".mp3";
        }
        else if (type == typeof(Material))
        {
            return ".mat";
        }
        else if (type == typeof(Shader))
        {
            return ".shader";   
        }
        else if (type == typeof(Font))
        {
            return ".ttf";
        }
        return null;
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="abName">资源名字</param>
    /// <param name="assetNames">资源名数组</param>
    /// <param name="assetType">资源类型</param>
    /// <param name="action">回调函数</param>
    public void LoadAsset(string abName, string[] assetNames, Type assetType, Action<UObject[]> action = null)
    {
        var result = new List<UObject>();
#if UNITY_EDITOR
        var extName = GetExtName(assetType);
        if(assetNames == null)
        {
            UObject[] objs = null;
            var assetPath = Application.dataPath + "/Res/" + abName + extName;
            if(File.Exists(assetPath))
            {
                var path = "Assets/Res/" + abName + extName;
                objs = AssetDatabase.LoadAllAssetsAtPath(path);
            }
            else
            {
                var dirPath = Application.dataPath + "/Res/" + abName;
                var files = Directory.GetFiles(dirPath, "*" + extName, SearchOption.AllDirectories);
                objs = new UObject[files.Length];
                for(int i = 0; i < files.Length; i++)
                {
                    var path = files[i].Replace(Application.dataPath, "Assets");
                    objs[i] = AssetDatabase.LoadAssetAtPath(path, assetType);
                }
            }
            result = new List<UObject>(objs);
        }
        else
        {
            var dirName = abName.Substring(0, abName.LastIndexOf('/'));
            foreach(var name in assetNames)
            {
                var path = "Assets/Res/" + dirName + "/" + name + extName;
                var obj = AssetDatabase.LoadAssetAtPath(path, assetType);
                if (obj == null)
                {
                    Debug.LogError("LoadAsset:>" + path + " was null!~~");
                }
                result.Add(obj);
            }
        }
#endif
        action?.Invoke(result.ToArray());
    }

    public void Update(float deltaTime) { }
}
