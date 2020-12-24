using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using System.IO;

/// <summary>
/// Resource本地包加载器
/// </summary>
/// 
//资源类型
public enum AssetsType
{
    UI,
    EFFECT,
    AUDIO,
    SPRITE,
    ATLAS,
    MAP,
    Count,
}
public class ResAssetManager
{
    //资源管理器
    ResourceManager mResMgr;
	/// 容器类
	private class PrefabContainer
    {
        public Dictionary<string, GameObject> details = new Dictionary<string, GameObject>();
    }
    //预设资源缓存数组
    private PrefabContainer[] prefabPool = new PrefabContainer[(int)AssetsType.Count];
    //图集缓存数组
    private Dictionary<AssetsType, SpriteAtlas> spriteAtlasDic = new Dictionary<AssetsType, SpriteAtlas>();

    public ResAssetManager(ResourceManager manager)
    {
        mResMgr = manager;

        for (int i = 0; i < (int)AssetsType.Count; i++)
        {
            prefabPool[i] = new PrefabContainer();
        }
    }
    public void Initialize(Action initOK)
    {
        initOK?.Invoke();


        InitAtlas();
        
    }
    /// <summary>
    /// 初始化图集资源
    /// </summary>
    private void InitAtlas()
    {
        //SpriteAtlas iconAtlas = Resources.Load<SpriteAtlas>(Path.Combine("Sprite/Atlas", AssetsType.ATLAS.ToString()));
        //spriteAtlasDic.Add(AssetsType.ATLAS, iconAtlas);`
    }
    /// <summary>
    /// 添加资源
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject AddAsset(string prefabName, AssetsType type)
    {
        string prefabPath = type.ToString() + "/" + prefabName;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab) prefabPool[(int)type].details.Add(prefabName, prefab);

        return prefab;
    }
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public void LoadAseet(string prefabName, AssetsType type, Action<UnityEngine.Object> func)
    {
        GameObject prefab = prefabPool[(int)type].details.TryGet(prefabName);
        if (prefab == null) prefab = AddAsset(prefabName, type);

        func?.Invoke(prefab);
    }
    /// <summary>
    /// 加载获取动态图集图片
    /// </summary>
    /// <param name="spriteName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Sprite LoadAtlasSprite(string spriteName, AssetsType type)
    {
        SpriteAtlas atlas;
        spriteAtlasDic.TryGetValue(AssetsType.ATLAS, out atlas);
        //如果图集不为空则从图集里面获取对应图片
        if (null == atlas)
            return null;

        return atlas.GetSprite(spriteName);
    }
}
