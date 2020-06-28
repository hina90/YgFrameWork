using System.Collections.Generic;
using UnityEngine;

public class UGUISpriteAsset : ScriptableObject
{
    /// <summary>
    /// 图片资源
    /// </summary>
    public Texture texSource;
    /// <summary>
    /// 所有sprite信息 SpriteAssetInfor类为具体的信息类
    /// </summary>
    public List<SpriteAssetInfor> listSpriteAssetInfor;
}

[System.Serializable]
public class SpriteAssetInfor
{
    /// <summary>
    /// ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 中心点
    /// </summary>
    public Vector2 pivot;
    /// <summary>
    ///坐标&宽高
    /// </summary>
    public Rect rect;
    /// <summary>
    /// 精灵
    /// </summary>
    public Sprite sprite;
}
