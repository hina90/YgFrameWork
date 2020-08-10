using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
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
}
