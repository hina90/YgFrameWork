using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

public static class Utils
{
    /// <summary>
    /// 获取随机整数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Random(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    /// <summary>
    /// 获取随机浮点数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Random(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    /// <summary>
    /// 获取key对应值
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static V TryGet<K, V>(this Dictionary<K, V> dic, K key)
    {
        dic.TryGetValue(key, out V value);
        return value;
    }
    /// <summary>
    /// 获取类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>   
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = default;
        component = go.gameObject.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    //得到1970年的时间戳
    public static DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static DateTime GetDateTime(long second)
    {
        DateTime dt = new DateTime(second * 1000 * 1000 * 10 + startTime.Ticks);
        return dt;
    }
    // 获取1970年以来的秒数,最常用
    public static long GetTimeSeconds()
    {
        DateTime nowTime = DateTime.Now;
        return (long)((nowTime - startTime).TotalSeconds);
    }
    /// <summary>
    /// 秒转分钟数
    /// </summary>
    /// <returns></returns>
    public static string Second2Minute(int totalSecond)
    {
        StringBuilder sb = new StringBuilder();
        string m = (totalSecond / 60).ToString("D2");
        string s = (totalSecond % 60).ToString("D2");
        return sb.AppendFormat("{0}:{1}", m, s).ToString();
    }
    /// <summary>
    /// 秒转分钟数
    /// </summary>
    /// <returns></returns>
    public static string Second2Hours(int totalSecond)
    {
        StringBuilder sb = new StringBuilder();
        int hour = totalSecond / 3600;
        string h = hour == 0 ? "" : hour + "小时";  //.ToString("D2")
        int minute = totalSecond / 60 % 60;
        string m = minute == 0 ? "" : minute + "分";  //.ToString("D2")
        int second = totalSecond % 60;
        string s = (second).ToString();
        return sb.AppendFormat("{0}{1}{2}秒", h, m, s).ToString();
    }
    /// <summary>
    /// Base64编码
    /// </summary>
    public static string Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    public static string Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }
    /// <summary>
    /// 判断数字
    /// </summary>
    public static bool IsNumeric(string str)
    {
        if (str == null || str.Length == 0) return false;
        for (int i = 0; i < str.Length; i++)
        {
            if (!Char.IsNumber(str[i])) { return false; }
        }
        return true;
    }
    /// <summary>
    /// 是否为数字
    /// </summary>
    public static bool IsNumber(string strNumber)
    {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }
    /// <summary>
    /// HashToMD5Hex
    /// </summary>
    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] result = md5.ComputeHash(Bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));
            return builder.ToString();
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
    /// <summary>
    /// 设置打印模式
    /// </summary>
    /// <param name="state"></param>
    public static void SetDebugState(bool state)
    {
        Debug.unityLogger.logEnabled = state;
    }

    /// <summary>
    /// 生成一个Key名
    /// </summary>
    public static string GetKey(string key)
    {
        return AppConst.AppPrefix + "_" + key;
    }

    /// <summary>
    /// 取得整型
    /// </summary>
    public static int GetInt(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.GetInt(name);
    }

    /// <summary>
    /// 有没有值
    /// </summary>
    public static bool HasKey(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.HasKey(name);
    }

    /// <summary>
    /// 保存整型
    /// </summary>
    public static void SetInt(string key, int value)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetInt(name, value);
    }

    /// <summary>
    /// 取得数据
    /// </summary>
    public static string GetString(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.GetString(name);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public static void SetString(string key, string value)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetString(name, value);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public static void RemoveData(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }

    /// <summary>
    /// 清理内存
    /// </summary>
    public static void ClearMemory()
    {
        GC.Collect(); Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 网络可用
    /// </summary>
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    /// <summary>
    /// 是否是无线
    /// </summary>
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }
    /// <summary>
    /// 加载游戏设置
    /// </summary>
    /// <returns></returns>
    public static GameSetting LoadGameSettings()
    {
        return Resources.Load<GameSetting>(AppConst.GameSettingPath);
    }

    /// <summary>
    /// 相机边距
    /// </summary>
    /// <returns></returns>
    public static float CameraHalfWidth()
    {
        return Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
    }
}
