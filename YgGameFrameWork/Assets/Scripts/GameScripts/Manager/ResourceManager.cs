using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//资源类型
public enum ResouceType
{
    UI,
    Actor,
    Effect,
    Audio,
    Sound,
    PrefabItem,
    Button,
    Icon,
    FacilitiesTitle,
    Facility,
    Goods,
    Guide,
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
        string prefabPath = type.ToString() + "/" + prefabName;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab) prefabPool[(int)type].details.Add(prefabName, prefab);

        return prefab;
    }
    /// <summary>
    /// 获取图片资源
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite GetSpriteResource(string spriteName, ResouceType type)
    {
        StringBuilder prefabPath = new StringBuilder();
        prefabPath.Append("Sprite/").Append(type.ToString()).Append("/").Append(spriteName);
        Sprite sprite = Resources.Load<Sprite>(prefabPath.ToString());

        return sprite;
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
        if (prefab == null) prefab = AddResource(prefabName, type);

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
        //TDDebug.DebugLog("----------------prefabName:" + prefabName);
        GameObject prefab = GetResource(prefabName, type);
        GameObject prefabObj = Instantiate(prefab, parent);
        prefabObj.transform.eulerAngles = Vector3.zero;
        prefabObj.transform.localPosition = Vector3.zero;

        return prefabObj;
    }
    /// <summary>
    /// 获取音频
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(string clipName)
    {
        StringBuilder prefabPath = new StringBuilder();
        prefabPath.Append("Sound/").Append(clipName);

        return Resources.Load<AudioClip>(prefabPath.ToString());
    }
}
