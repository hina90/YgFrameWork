using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class FileUtils
{
    /// <summary>
    /// 获取可写目录路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetWritePath(string path)
    {
        StringBuilder sb = new StringBuilder();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            sb.Append(Application.streamingAssetsPath).Append("/").Append(path);
        }
        else
        {
            sb.Append(Application.persistentDataPath).Append("/").Append(path);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取打包资源路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetResPath(string path)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Application.persistentDataPath).Append("/Resources/").Append(path);

        return sb.ToString();
    }
    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + AppConst.AppName + "/";
            }
            if (AppConst.DebugMode)
            {
                return Application.dataPath + "/res/";
            }
            return "c:/" + AppConst.AppName + "/";
        }
    }
    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = Application.streamingAssetsPath + "/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.streamingAssetsPath + "/";
                break;
        }
        return path;
    }

}
