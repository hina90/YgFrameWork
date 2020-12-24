using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UObject = UnityEngine.Object;
using System.IO;

public class AssetBundleInfo
{
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle, int RefCount = 1)
    {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = RefCount;
    }
}
class LoadAssetRequest
{
    public Type assetType;
    public string[] assetNames;
    public Action<UObject[]> sharpFunc; 
}
class UnloadAssetBundleRequest
{
    public string abName;
    public bool unloadNow;
    public AssetBundleInfo abInfo;
}

public class AssetBundleManager
{
    ResourceManager mResMgr;
    string[] m_AllMainfest = null;
    AssetBundleManifest m_AssetBundleMainfest = null;
    Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
    Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
    Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();
    Dictionary<string, int> m_AssetBundleLoadingList = new Dictionary<string, int>();
    Dictionary<string, UnloadAssetBundleRequest> m_AssetBundleUnloadingList = new Dictionary<string, UnloadAssetBundleRequest>();


    public AssetBundleManager(ResourceManager manager)
    {
        mResMgr = manager;
    }

    public Dictionary<string, AssetBundleInfo> LoadedAssetBundles
    {
        get
        {
            return m_LoadedAssetBundles;
        }
    }


    /// <summary>
    /// 获取加载bundle
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    AssetBundleInfo GetLoadedAssetBundle(string abName)
    {
        AssetBundleInfo bundle = null;
        m_LoadedAssetBundles.TryGetValue(abName, out bundle);
        if (bundle == null) return null;

        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(abName, out dependencies))
            return bundle;

        for(int i = 0; i < dependencies.Length; i++)
        {
            AssetBundleInfo dependentBundle;
            m_LoadedAssetBundles.TryGetValue(dependencies[i], out dependentBundle);
            if (dependentBundle == null) return null;
        }

        return bundle;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="mainfestName"></param>
    /// <param name="initOk"></param>
    public void Initialize(string mainfestName, Action initOk)
    {
        UnloadAssetBundle(mainfestName, true);
        LoadAsset(mainfestName, new string[] { "AssetBundleManifest" }, typeof(AssetBundleManifest), delegate(UObject[] objs)
        { 
            if(objs.Length > 0)
            {
                m_AssetBundleMainfest = objs[0] as AssetBundleManifest;
                m_AllMainfest = m_AssetBundleMainfest.GetAllAssetBundles();
            }
            initOk?.Invoke();
        });
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="assetNames"></param>
    /// <param name="assetType"></param>
    /// <param name="action"></param>
    public void LoadAsset(string abName, string[] assetNames, Type assetType, Action<UObject[]> action = null)
    {
        abName = GetRealAssetPath(abName);

        var request = new LoadAssetRequest();
        request.assetNames = assetNames;
        request.assetType = assetType;
        request.sharpFunc = action;

        List<LoadAssetRequest> requests = null;
        if(!m_LoadRequests.TryGetValue(abName, out requests))
        {
            requests = new List<LoadAssetRequest>();
            requests.Add(request);
            m_LoadRequests.Add(abName, requests);
            mResMgr.StartCoroutine(OnLoadAsset(abName, assetType));
        }
        else
        {
            requests.Add(request);
        }
    }

    IEnumerator OnLoadAsset(string abName, Type assetType)
    {
        AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
        if(bundleInfo == null)
        {
            yield return mResMgr.StartCoroutine(OnLoadAssetBundle(abName, assetType));

            bundleInfo = GetLoadedAssetBundle(abName);
            if(bundleInfo == null)
            {
                m_LoadRequests.Remove(abName);
                Debug.LogError("OnLoadAsset--->>>" + abName);
                yield break;
            }
        }
        List<LoadAssetRequest> list = null;
        if(!m_LoadRequests.TryGetValue(abName, out list))
        {
            m_LoadRequests.Remove(abName);
            yield break;
        }
        for(int i = 0; i < list.Count; i++)
        {
            string[] assetNames = list[i].assetNames;
            List<UObject> result = new List<UObject>();

            AssetBundle ab = bundleInfo.m_AssetBundle;
            if(assetNames != null)
            {
                for(int j = 0; j < assetNames.Length; j++)
                {
                    string assetPath = assetNames[j];
                    var request = ab.LoadAssetAsync(assetPath, assetType);
                    yield return request;
                    result.Add(request.asset);
                }
            }
            else
            {
                var request = ab.LoadAllAssetsAsync();
                yield return request;
                result = new List<UObject>(request.allAssets);
            }
            if(list[i].sharpFunc != null)
            {
                list[i].sharpFunc(result.ToArray());
                list[i].sharpFunc = null;
            }
            bundleInfo.m_ReferencedCount++;
        }
        m_LoadRequests.Remove(abName);
    }
    /// <summary>
    /// 加载bundle资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    IEnumerator OnLoadAssetBundle(string abName, Type type)
    {
        string url = GetAssetFullPath(abName);
        if(m_AssetBundleLoadingList.ContainsKey(url))
        {
            m_AssetBundleLoadingList[url]++;
            yield break;
        }
        m_AssetBundleLoadingList.Add(url, 1);
        var abUrl = Application.isEditor ? abName : url;
        var request = AssetBundle.LoadFromFileAsync(abUrl);
        if(abName != AppConst.ResIndexFile)
        {
            string[] dependences = m_AssetBundleMainfest.GetAllDependencies(abName);
            if (dependences.Length > 0)
            {
                m_Dependencies.Add(abName, dependences);
                for(int i = 0; i < dependences.Length; i++)
                {
                    string depName = dependences[i];
                    AssetBundleInfo bundleInfo = null;
                    if(m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                    {
                        bundleInfo.m_ReferencedCount++;
                    }
                    else if(!m_LoadRequests.ContainsKey(depName))
                    {
                        yield return mResMgr.StartCoroutine(OnLoadAssetBundle(depName, type));
                    }
                }
            }
        }
        yield return request;

        AssetBundle assetObj = request.assetBundle;
        if(assetObj != null)
        {
            var RefCount = m_AssetBundleLoadingList[url];
            var bundleInfo = new AssetBundleInfo(assetObj, RefCount);
            m_LoadedAssetBundles.Add(abName, bundleInfo);
        }
        m_AssetBundleLoadingList.Remove(url);
    }
    /// <summary>
    /// 卸载
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="isThorough"></param>
    public void UnloadAssetBundle(string abName, bool isThorough = false)
    {
        abName = GetRealAssetPath(abName);
        UnloadAssetBundleInternal(abName, isThorough);
        UnloadDependencies(abName, isThorough);
        Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
    }
    /// <summary>
    /// 获取素材全路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string GetAssetFullPath(string path)
    {
        string assetPath = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                assetPath = Path.Combine(FileUtils.AppContentPath(), AppConst.ResIndexFile);
                break;
            default:
                assetPath = Path.Combine(FileUtils.AppContentPath(), AppConst.ResIndexFile);
                break;
        }
        return Path.Combine(assetPath, path);
    }
    /// <summary>
    /// 获取bundle资源真实路径
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    string GetRealAssetPath(string abName)
    {
        if(abName.Equals(AppConst.ResIndexFile))
        {
            return abName;
        }
        abName = abName.ToLower();
        if(!abName.EndsWith(AppConst.ExtName))
        {
            abName += AppConst.ExtName;
        }
        if(abName.Contains("/"))
        {
            return abName;
        }
        for(int i = 0; i < m_AllMainfest.Length; i++)
        {
            int index = m_AllMainfest[i].LastIndexOf('/');
            string path = m_AllMainfest[i].Remove(0, index + 1);
            if(path.Equals(abName))
            {
                return m_AllMainfest[i];
            }
        }
        Debug.LogError("GetRealAssetPath Error:>>" + abName);
        return null;
    }

    public void Update(float deltaTime)
    {
        TryUnloadAssetBundle();
    }

    private void TryUnloadAssetBundle()
    {
        if (m_AssetBundleUnloadingList.Count == 0)
            return;

        foreach(var de in m_AssetBundleUnloadingList)
        {
            if (m_AssetBundleLoadingList.ContainsKey(de.Key))
                continue;

            var request = de.Value;
            if (request.abInfo != null && request.abInfo.m_AssetBundle != null)
            {
                request.abInfo.m_AssetBundle.Unload(true);
            }
            m_AssetBundleUnloadingList.Remove(de.Key);
            m_LoadedAssetBundles.Remove(de.Key);
            Debug.Log(de.Key + " has been unloaded successfully");
        }
    }

    /// <summary>
    /// 卸载依赖资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="isThorough"></param>
    void UnloadDependencies(string abName, bool isThorough)
    {
        string[] dependecies = null;
        if (!m_Dependencies.TryGetValue(abName, out dependecies))
            return;

        for(int i = 0; i < dependecies.Length; i++)
        {
            UnloadAssetBundleInternal(dependecies[i], isThorough);
        }
        m_Dependencies.Remove(abName  );
    }
    /// <summary>
    /// 卸载Bundle资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="unloadNow"></param>
    void UnloadAssetBundleInternal(string abName, bool unloadNow)
    {
        AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
        if (bundle == null) return;

        if(--bundle.m_ReferencedCount <= 0)
        {
            if(m_AssetBundleLoadingList.ContainsKey(abName))
            {
                var request = new UnloadAssetBundleRequest();
                request.abName = abName;
                request.abInfo = bundle;
                request.unloadNow = unloadNow;
                m_AssetBundleUnloadingList.Add(abName, request);
                return;
            }
            bundle.m_AssetBundle.Unload(unloadNow);
            m_LoadedAssetBundles.Remove(abName);
            Debug.Log(abName + " has been unloaded successfully");
        }
    }


}
