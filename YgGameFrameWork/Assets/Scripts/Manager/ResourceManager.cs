using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;


//资源类型
public enum ResouceType
{
    UI,
    Actor,
    Effect,
    Audio,
    Scene,
    Battle,
    PrefabItem,
    Icon,
    Face,
    Game,
    BaseWidget,
    Exhibits,
    ExhibitsUI,
    Decoration,
    DigSpineUI,
    Cultivate,
    Font,
    Count,
}

/// <summary>
/// 资源管理器
/// </summary>
public class ResourceManager : UnitySingleton<ResourceManager>
{
    /// <summary>
	/// 容器类
	/// </summary>
	private class PrefabContainer
    {
        public Dictionary<string, GameObject> details = new Dictionary<string, GameObject>();
    }
    //预设资源缓存数组
    private PrefabContainer[] prefabPool = new PrefabContainer[(int)ResouceType.Count];
    //图集缓存
    Dictionary<string, SpriteAtlas> mDicSpriteAtlas = new Dictionary<string, SpriteAtlas>();

    /// <summary>
    /// 初始化
    /// </summary>
    public ResourceManager()
    {
        for (int i = 0; i < (int)ResouceType.Count; i++)
        {
            prefabPool[i] = new PrefabContainer();
        }
    }
    /// <summary>
    /// 添加资源
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="type"></param>
    public GameObject AddResource(string prefabName, ResouceType type)
    {
        string prefabPath = "Prefab/" + type.ToString() + "/" + prefabName;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab) prefabPool[(int)type].details.Add(prefabName, prefab);

        return prefab;
    }
    /// <summary>
    /// 获取预设资源 
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetResource(string prefabName, ResouceType type)
    {
        GameObject prefab = prefabPool[(int)type].details.TryGet(prefabName);
        if (prefab == null)
            prefab = AddResource(prefabName, type);

        return prefab;
    }
    /// <summary>
    /// 获取创建资源
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="parent"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetResourceInstantiate(string prefabName, Transform parent, ResouceType type)
    {
        GameObject prefab = GetResource(prefabName, type);
        GameObject prefabObj = Instantiate(prefab, parent);
        prefabObj.transform.eulerAngles = Vector3.zero;
        prefabObj.transform.localPosition = Vector3.zero;

        return prefabObj;
    }
    public GameObject GetResourceInit(string prefabName, Transform parent, ResouceType type)
    {
        GameObject prefab = GetResource(prefabName, type);
        GameObject prefabObj = Instantiate(prefab, parent);

        return prefabObj;
    }
    /// <summary>
    /// 获取图片资源
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite GetSpriteResource(string spriteName, ResouceType type)
    {
        StringBuilder prefabPath = new StringBuilder();
        //#if UNITY_IOS || UNITY_ANDROID
        //        prefabPath.Append("Atlas/").Append(type.ToString());
        //        SpriteAtlas nowAtlas;
        //        if (mDicSpriteAtlas.ContainsKey(prefabPath.ToString()))
        //            nowAtlas = mDicSpriteAtlas[type.ToString()];
        //        else
        //        {
        //            nowAtlas = Resources.Load<SpriteAtlas>(prefabPath.ToString());
        //            mDicSpriteAtlas[type.ToString()] = nowAtlas;
        //        }
        //        Sprite sprite = nowAtlas.GetSprite(spriteName);
        //#else
        prefabPath.Append("Sprite/").Append(type.ToString()).Append("/").Append(spriteName);
        Sprite sprite = Resources.Load<Sprite>(prefabPath.ToString());
        //#endif
        return sprite;
    }
}
