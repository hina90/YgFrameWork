using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.EventSystems;

public static class Utils
{
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


    /// <summary>
    /// 修改层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <param name="isAll"></param>
	public static void ChangeLayer(GameObject go, int layer, bool isAll)
    {
        go.layer = layer;
        if (isAll)
        {
            int tcc = go.transform.childCount;
            for (int i = 0; i < tcc; i++)
            {
                ChangeLayer(go.transform.GetChild(i).gameObject, layer, isAll);
            }
        }
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
    /// 设置图片灰色
    /// </summary>
    /// <param name="img"></param>
    public static void SetImageGray(UnityEngine.UI.Image img)
    {
        Shader shader = Shader.Find("Sprites/Gray");
        Material material = new Material(shader);
        Texture2D texture = Resources.Load("Shader/Sprite_Gray") as Texture2D;
        material.SetTexture("_mat", texture);
        img.material = material;
    }

    /// <summary>
    /// 十六进制颜色转换Color结构体
    /// </summary>
    /// <param name="colorStr">十六进制颜色字符</param>
    /// <returns></returns>
    public static Color ParseHtmlColor(this string colorStr)
    {
        ColorUtility.TryParseHtmlString(colorStr, out Color color);
        return color;
    }

    /// <summary>
    /// 获取次元空间格式时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string GetDimensionTimeFormat(this DateTime time)
    {
        string tempStr = time.ToString("yyyy-MM-dd HH:mm");
        string[] strs = tempStr.Split('-');
        return tempStr.Replace(strs[0], (int.Parse(strs[0]) + 1000).ToString());
    }
    /// <summary>
    /// 数组转List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrayData"></param>
    /// <returns></returns>
    public static List<T> ArrayToList<T>(T[] arrayData)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < arrayData.Length; i++)
        {
            list.Add(arrayData[i]);
        }
        return list;
    }

    /// <summary>
    /// 世界坐标转成UI中父节点的坐标, 并设置子节点的位置
    /// </summary>
    /// <param name="wpos">3D物体坐标</param>
    /// <param name="uiTarget">UI物体位置组件</param>
    /// <param name="cam">摄像机</param>
    /// <param name="parentTrans">ui物体父物体</param>
    public static void World2UIPos(Vector3 wpos, RectTransform uiTarget, Camera cam, Transform parentTrans = null)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(wpos);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(uiTarget, screenPos, cam, out Vector3 mousePos))
        {
            uiTarget.position = mousePos;
            if (parentTrans != null) uiTarget.SetParent(parentTrans);
        }
    }

    /// <summary>
    /// 世界坐标转屏幕坐标
    /// </summary>
    /// <param name="wpos">3D物体坐标</param>
    /// <param name="uiTarget">UI物体位置组件</param>
    /// <param name="parentTrans">ui物体父物体</param>
    public static void World2ScreenPos(Vector3 wpos, RectTransform uiTarget, Transform parentTrans = null)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(wpos);
        uiTarget.position = screenPos;
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
    /// 判断是否点击在UI上
    /// </summary>
    /// <returns></returns>
    public static bool CheckIsClickOnUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
#endif
#if UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount > 0)
        {
            eventData.pressPosition = Input.GetTouch(0).position;
            eventData.position = Input.GetTouch(0).position;
        }
#endif
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }
}
