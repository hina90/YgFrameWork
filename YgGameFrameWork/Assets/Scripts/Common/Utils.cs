using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
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
    /// 获得数字图片组
    /// </summary>
    /// <param name="count">数字大小</param>
    /// <param name="num">图片数量</param>
    /// <returns></returns>
    public static Sprite[] GetNumberSprites(string count, string gray = "")
    {
        char[] chars = count.ToCharArray();
        Sprite[] sprites = new Sprite[chars.Length];
        for (int i = 0; i < chars.Length; i++)
        {
            switch (chars[i])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + chars[i].ToString(), ResouceType.Font);
                    break;
                case 'a':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "10", ResouceType.Font);
                    break;
                case 'b':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "11", ResouceType.Font);
                    break;
                case '/':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "12", ResouceType.Font);
                    break;
                case 'c':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "13", ResouceType.Font);
                    break;
                case 'K':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "14", ResouceType.Font);
                    break;
                case 'G':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "15", ResouceType.Font);
                    break;
                case 'B':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "16", ResouceType.Font);
                    break;
                case 'M':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "17", ResouceType.Font);
                    break;
                case '.':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "18", ResouceType.Font);
                    break;
                case 'S':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "19", ResouceType.Font);
                    break;
                case 'X':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "20", ResouceType.Font);
                    break;
                case 'T':
                    sprites[i] = ResourceManager.Instance.GetSpriteResource(gray + "21", ResouceType.Font);
                    break;
            }
        }
        return sprites;
    }
}
