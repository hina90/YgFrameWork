using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// 场景管理类
/// </summary>
public class CSceneManager : UnitySingleton<CSceneManager>
{
    //场景加载器
    private SceneLoader sceneLoader = null;
    //场景加载完成回调
    private Callback OnBaseLoaded = null;
    //场景加载过程回调
    private Callback<float> OnBaseProgress = null;
    //场景是否加载完成
    private bool isSceneLoadOK = true;


    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="mode">0.销毁上一个场景 1.隐藏上一个场景</param>
    /// <param name="baseLoadedCal">加载函数回调</param>
    /// <param name="baseProgressCal">加载更新函数</param>
    /// <returns></returns>
    public bool LoadScene(string sceneName, SceneState mode = SceneState.ReleasePre, Callback baseLoadedCal = null, Callback<float> baseProgressCal = null)
    {
        if(isSceneLoadOK)
        {
            isSceneLoadOK = false;
            OnBaseLoaded = baseLoadedCal;
            OnBaseProgress = baseProgressCal;

            Scene scene = SceneManager.GetSceneByName(sceneName);
            Scene preScene = SceneManager.GetActiveScene();
            if (preScene == null && mode == SceneState.HidePre)
            {
                mode = SceneState.ReleasePre;
            }
            if (preScene != null)                        //处理上一个场景
            {
                if (mode == SceneState.ReleasePre)       //销毁
                {
                    SceneManager.UnloadSceneAsync(preScene.name);
                }
                else if (mode == SceneState.HidePre)     //隐藏
                {
                    GameObject[] goList = preScene.GetRootGameObjects();
                    for (int i = 0; i < goList.Length; i++)
                    {
                        goList[i].SetActive(false);
                    }
                }
            }
            //如果此场景已经加载则显示出来
            if (scene != null && scene.isLoaded)
            {
                GameObject[] goList = scene.GetRootGameObjects();
                for (int i = 0; i < goList.Length; i++)
                {
                    goList[i].SetActive(true);
                }
                SceneManager.SetActiveScene(scene);

                isSceneLoadOK = true;
                OnLevelBaseLoaded();

                return true;
            }
            GetSceneLoader().LoadScene(sceneName, (LoadSceneMode)mode);
        }
        
        return true;
    }

   /// <summary>
   /// 查找场景中的物体
   /// </summary>
   /// <param name="name"></param>
   /// <returns></returns>
    public GameObject GetSceneObjByName(string name)
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] goList = scene.GetRootGameObjects();
        Transform tr = null;
        for (int i = 0; i < goList.Length; i++)
        {
            if (goList[i].name == name)
            {
                return goList[i].gameObject;
            }
            else
            {
                tr = goList[i].gameObject.transform.Find(name);
                if (tr != null)
                    return tr.gameObject;
            }
        }

        return null;
    }
    /// <summary>
    /// 获取场景加载器
    /// </summary>
    /// <returns></returns>
    public SceneLoader GetSceneLoader()
    {
        if (sceneLoader == null)
        {
            sceneLoader = gameObject.GetComponent<SceneLoader>();
            if (sceneLoader == null)
            {
                sceneLoader = gameObject.AddComponent<SceneLoader>();
            }
        }

        return sceneLoader;
    }
    /// <summary>
    /// 场景加载完后回调
    /// </summary>
    public void OnLevelBaseLoaded()
    {
        isSceneLoadOK = true;
        if (OnBaseLoaded != null)
        {
            OnBaseLoaded();
        }
    }
    /// <summary>
    /// 加载过程回调
    /// </summary>
    /// <param name="value"></param>
    public void OnLoadProgress(float value)
    {
        if (OnBaseProgress != null)
        {
            OnBaseProgress(value);
        }
    }
    /// <summary>
    /// 设置相机适配系数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="size"></param>
    public void SetCamera(float x, float y, float z, float size)
    {
        float factWidth = Screen.width;
        float factHeight = Screen.height;
        float factAspectRatio = factWidth / factHeight;
        float factOrthoSize = (size * 0.56f) / factAspectRatio;

        GameObject cameraObj = GameObject.Find("Main Camera");
        Camera camera = cameraObj.GetComponent<Camera>();
        camera.orthographicSize = factOrthoSize;
        camera.transform.position = new Vector3(x, y, z);
    }

    //当前场景
    private BaseScene currentScene;
    private Dictionary<string, BaseScene> sceneDic = new Dictionary<string, BaseScene>();
    /// <summary>
    /// 初始化所有场景数据
    /// </summary>
    public void Init()
    {
        sceneDic["LevelGameScene"] = new LevelGameScene();
        //sceneDic["BattleScene"] = new BattleScene();
    }
    /// <summary>
    /// 场景帧事件
    /// </summary>
    public void MainUpdate()
    {
        if(currentScene != null)
        {
            currentScene.MainUpdate();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void ChangeScene(string name, params object[] param)
    {
        BaseScene scene = sceneDic.TryGet(name);
        if(scene == null)
        {
            Debug.LogError("scene is null");
            return;
        }

        if (currentScene != null)
            currentScene.Quit();

        currentScene = scene;
        currentScene.Open(param);
    }
    /// <summary>
    /// 发送场景事件
    /// </summary>
    /// <param name="param"></param>
    public void SendSceneEvent(GameEvent eventType, params object[] param)
    {
        foreach (BaseScene scene in sceneDic.Values)
        {
            Dictionary<GameEvent, Callback<object[]>> callback = scene.CtorEvent();
            if (callback != null)
            {
                callback[eventType](param);
            }
        }
    }
}
