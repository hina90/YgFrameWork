using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Text;
using System;
using System.IO;
using UnityEngine.Profiling;


/// <summary>
/// 调试打印
/// </summary>
public class TDDebug : UnitySingleton<TDDebug>
{
    enum LOG_TYPE
    {
        LT_COMMON, LT_WARNING, LT_ERROR, LT_NUM
    }
    enum WINDOW_MODE
    {
        QUATER = 0,
        HALF = 1,
        THREE_FOURTHS = 2,
        MAX = 3,
    }
    WINDOW_MODE window_mode = WINDOW_MODE.HALF;
    private static GameObject ugui_root;
    private static TDDebug instance;
    private static GameObject tdDebug;         //根对象
    private static GameObject panel;           //日志面板
    private GameObject debug_switch;    //界面开关按钮
    private static int max_show_cout = 200;     //最大显示条目
    private static int max_memory_count = 200;  //最大存储条目
    private GameObject Content;         //列表内容滚动范围
    private Transform Content_tran;
    private bool pause_show = false;    //报错时暂停显示后面日志
    private GameObject drag_button;     //拖拽按钮
    private RectTransform PanelRectTran;//panel 区域对象
    private GameObject resize_button;   //重置尺寸
    private static Button []type_buttons = new Button[(int)LOG_TYPE.LT_NUM];
    
    private Text common_btn_text;
    private Text warn_btn_text;
    private Text error_btn_text;
    private Text system_text;
    private GameObject go_on;           //继续按钮
    private InputField copyfiled;
    private Text FPS_value;
    private static CanvasScaler uiScaler;

    bool is_auto_end = false; //添加新条目时，滚动条是否自动滚到底

    private static GameObject Item;            //用来克隆产生新的条目
    private static GameObject scrollview;      //四种列表
    private static Scrollbar scrollbar;         //滚动条

    private static GameObject scenebutton;     //场景开关按钮
    private static GameObject actorbutton;     //角色开关按钮
    private static GameObject uguibutton;      //日志界面内容开关按钮

    private static bool []showtype = new bool[(int)LOG_TYPE.LT_NUM]; //默认全部显示
    private static int []log_type_num = new int[(int)LOG_TYPE.LT_NUM];
    
    private static bool panel_visible = false;

    static GraphicRaycaster raycaster;
    static EventSystem eventsystem;
    static PointerEventData eventData;
    static private float addrowtime = 0;

    private static FileStream fs = null;
    public static string startDate = "unset";

    private Queue<GameObject> deaditem = new Queue<GameObject>();
    private static Dictionary<GameObject, DebugInfo> diclogs = new Dictionary<GameObject, DebugInfo>(); 
    private class DebugInfo
    {
        public LOG_TYPE logtype;
        public string  content;
    }

    static private Queue<DebugInfo> queue_logs = new Queue<DebugInfo>();

    static private void AddSingleLineQueue(string text, LOG_TYPE type)
    {
        DebugInfo newlog = new DebugInfo();
        newlog.logtype = type;
        newlog.content = text;
        queue_logs.Enqueue(newlog);
    }
    static public void Log(string txt)
    {
#if !DEBUG_OPEN
        return;
#endif
        if (diclogs.Count > max_memory_count) { return; }
		AddSingleLineQueue(txt, LOG_TYPE.LT_COMMON);
        log_type_num[(int)LOG_TYPE.LT_COMMON]++;
        WriteToFile(txt);
    }

    static public void Warn(string txt)
    {
#if !DEBUG_OPEN
        return;
#endif
        if (diclogs.Count > max_memory_count) { return; }
		AddSingleLineQueue(txt, LOG_TYPE.LT_WARNING);
        log_type_num[(int)LOG_TYPE.LT_WARNING]++;
        instance.warn_btn_text.text = string.Format("warn({0})", log_type_num[(int)LOG_TYPE.LT_WARNING]);
        WriteToFile(txt);
    }

    static public void Error(string txt)
    {
#if !DEBUG_OPEN
        return;
#endif
        if (diclogs.Count > max_memory_count) { return; }
        AddSingleLineQueue(txt + "\n", LOG_TYPE.LT_ERROR);

        log_type_num[(int)LOG_TYPE.LT_ERROR]++;
        instance.error_btn_text.text = string.Format("error({0})", log_type_num[(int)LOG_TYPE.LT_ERROR]);
        panel_visible = true;
        panel.SetActive(panel_visible);
        WriteToFile(txt);
    }

    private static void WriteToFile(string stlog)
    {
#if !DEBUG_OPEN
                return;
#endif
        byte[] data = null;
        if (fs == null)
        {
            DateTime dt = Utils.GetDateTime(Utils.GetTimeSeconds());
            startDate = string.Format("{0:yyyy-MM-dd_hh-mm}", dt);
            string path = "log_" + startDate + ".txt";
            // windows 下检查本地资源目录
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android ||
                UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
            {
                path = FileUtils.Instance.GetWritePath(path);
            }
            fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            data = Encoding.UTF8.GetBytes("===================================start game=============================\n");
            fs.Write(data, 0, data.Length);
        }
        
        data = Encoding.UTF8.GetBytes(stlog + "\n");
        fs.Write(data, 0, data.Length);
    }

    void OnDestroy()
    {
        if (fs != null)
        {
            byte[] data = Encoding.UTF8.GetBytes("===================================end game=============================\n\n\n\n\n");
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        fs = null;
        instance = null;
        diclogs.Clear();
        deaditem.Clear();
    }
    public static TDDebug GetInstance()
    {
#if !DEBUG_OPEN
        ugui_root = GameObject.Find("ugui_root");
        if(ugui_root == null)
        {
            GameObject ugui_root_asset = Resources.Load("ugui_root") as GameObject;
            ugui_root = GameObject.Instantiate(ugui_root_asset);
            ugui_root.name = "ugui_root";
            ugui_root_asset = null;
            Resources.UnloadUnusedAssets();
        }
        GameObject.DontDestroyOnLoad(ugui_root);
        return null; 
#endif
        if (instance == null)
        {
            ugui_root = GameObject.Find("ugui_root");
            if (ugui_root == null)
            {
                GameObject ugui_root_asset = Resources.Load("Debug/ugui_root") as GameObject;
                ugui_root = GameObject.Instantiate(ugui_root_asset);
                ugui_root.name = "ugui_root";
                ugui_root_asset = null;
                Resources.UnloadUnusedAssets();
            }
            GameObject.DontDestroyOnLoad(ugui_root);
            uiScaler = Find(ugui_root, "Canvas").GetComponent<CanvasScaler>();
            GameObject tdasset = Resources.Load("Debug/TdDebug") as GameObject;
            if (tdasset == null)
            {
                UnityEngine.Debug.LogError("Check Resouce prefab : 'TdDebug' is not exist!");
                return null;
            }
            GameObject tdins = GameObject.Instantiate(tdasset);
            GameObject.DontDestroyOnLoad(tdins);
            GameObject canvas = Find(ugui_root, "Canvas");
            tdins.transform.SetParent(canvas.transform, false);
            instance = tdins.GetOrAddComponent<TDDebug>();
            tdDebug = tdins;
            instance.Init();

            UnityEngine.Application.logMessageReceived += logCallBack;
        }
        return instance;
    }

    static void logCallBack(string condition, string stackTrace, LogType type)
    {
        if (LogType.Log == type) 
        {
            Log(condition);
        }
        else if (LogType.Warning == type)
        {
            Warn(condition);
        } 
        else if (LogType.Exception == type) 
        {
            Error(condition + "###\n--------c#堆栈-------------\n" + stackTrace + "\n------------------lua堆栈-------------------\n" );
        }
    }

    private void OnSwitchClick(PointerEventData data)
    {
        panel_visible = !panel_visible;
        panel.SetActive(panel_visible);
    }

    static private GameObject Find(GameObject obj, string target)
    {
        if (obj.name == target)
        {
            return obj;
        }

        int childcount = obj.transform.childCount;
        int i = 0;
        while (i < childcount)
        {
            Transform child = obj.transform.GetChild(i);
            GameObject childobj = Find(child.gameObject, target);
            if (childobj != null)
            {
                return childobj;
            }
            i++;
        }
        return null;
    }

    public static void DebugLog(string log)
    {
#if DEBUG_OPEN
        Debug.Log(log);
#endif
    }
    public static void DebugLogFormat(string log, params object[] args)
    {
#if DEBUG_OPEN
        Debug.LogFormat(log, args);
#endif
    }
    public static void DebugLogError(string log)
    {
#if DEBUG_OPEN
        Debug.LogError(log);
#endif
    }
    public static void DebugLogWarning(string log)
    {
#if DEBUG_OPEN
        Debug.LogWarning(log);
#endif
    }

    private void OnDragEnd(PointerEventData data)
    {

    }
    private void OnDrag(PointerEventData data)
    {
        int width = Screen.width;
        int height = Screen.height;
        
        float designWidth = uiScaler.referenceResolution.x;//开发时分辨率宽
        float designHeight = uiScaler.referenceResolution.y;//开发时分辨率高
        float s1 = designWidth / (float)width;
        float s2 = designHeight / (float)height;
        Vector2 offmax = new Vector2(PanelRectTran.offsetMax.x, PanelRectTran.offsetMax.y);
        Vector2 offmin = new Vector2(PanelRectTran.offsetMin.x, PanelRectTran.offsetMin.y);
        offmax.x += data.delta.x * s1;
        offmin.y += data.delta.y * s2;
        PanelRectTran.offsetMax = offmax;
        PanelRectTran.offsetMin = offmin;
    }

    private void OnResize(PointerEventData data)
    {
        float[] mode = new float[] { 0.25f, 0.5f, 0.75f, 1 };
        string[] smode = new string[] { "1/4", "1/2", "3/4", "MAX" };
        
        float designWidth = uiScaler.referenceResolution.x;//开发时分辨率宽
        float designHeight = uiScaler.referenceResolution.y;//开发时分辨率高
        PanelRectTran.offsetMin = new Vector2(0, -designHeight * mode[(int)window_mode]);
        PanelRectTran.offsetMax = new Vector2(designWidth * mode[(int)window_mode], 0);

        resize_button.transform.GetChild(0).GetComponent<Text>().text = smode[(int)window_mode];
        window_mode++;
        if (window_mode > WINDOW_MODE.MAX)
            window_mode = WINDOW_MODE.QUATER;
        addrowtime = Time.realtimeSinceStartup;
    }

    private void UpdatePauseBtnShow()
    {
        string showtext = pause_show ? "Go" : "Pause";
        go_on.transform.GetChild(0).GetComponent<Text>().text = showtext;
    }
    private void OnGoClick(PointerEventData data)
    {
        pause_show = !pause_show;
        UpdatePauseBtnShow();
    }

    private void RefreshList(LOG_TYPE type)
    {
        showtype[(int)type] = !showtype[(int)type];
        Color normalcolor = showtype[(int)type] ? Color.white : new Color(0.44f,0.44f,0.44f,1.0f);
        ColorBlock colblock = type_buttons[(int)type].colors;
        colblock.normalColor = normalcolor;
        type_buttons[(int)type].colors = colblock;
        foreach (KeyValuePair<GameObject, DebugInfo> kvp in diclogs)
        {
            bool visible = showtype[(int)type];
            if (kvp.Value.logtype == type)
            {
                kvp.Key.SetActive(visible);
            }
        }
        addrowtime = Time.realtimeSinceStartup;
    }
    private void OnCommonClick(PointerEventData data)
    {
        RefreshList(LOG_TYPE.LT_COMMON);
    }

    private void OnWarnClick(PointerEventData data)
    {
        RefreshList(LOG_TYPE.LT_WARNING);
    }

    private void OnErrorClick(PointerEventData data)
    {
        RefreshList(LOG_TYPE.LT_ERROR);
    }

    private void OnScrollBarDragEnd(PointerEventData data)
    {
        for (int i = 0; i <= (int)LOG_TYPE.LT_NUM; i++)
        {
            if(scrollbar.value == 0)
            {
                is_auto_end = true;
            }
            else
            {
                is_auto_end = false;
            }
        }
    }

    private void ScrollViewOnDoubleClick(PointerEventData data)
    {
        GameObject click = GetTopTouchObject();
        if(click != null)
        {
            copyfiled.gameObject.SetActive(true);
            copyfiled.text += click.GetComponent<Text>().text + '\n';
        }
    }

    private void OnCopyHideBtnClick(PointerEventData data)
    {
        copyfiled.text = "";
        copyfiled.gameObject.SetActive(false);
    }

    static bool defaultshow = true;
    static bool actorshow = true;
    static private GameObject sceneObj;
    
    static public void FindScene()
    {
        sceneObj = GameObject.Find("taizi");
        if(sceneObj == null)
        {
            sceneObj = GameObject.Find("S001_01_02");
        }
    }
    private void OnSceneClick(PointerEventData data)
    {
        defaultshow = !defaultshow;
        if(sceneObj != null)
        {
            for (int i = 0; i < sceneObj.transform.childCount; ++i)
            {
                sceneObj.transform.GetChild(i).gameObject.SetActive(defaultshow);
            }
        }
    }

    static void GetChildComponetsEx<T>(Transform tran, List<T> list)
    {
        T comp = tran.gameObject.GetComponent<T>();
        if (comp != null && !comp.ToString().Equals("null"))
        {
            list.Add(comp);
        }
        int count = tran.childCount;
        for (int i = 0; i < count; ++i)
        {
            Transform child = tran.GetChild(i);
            GetChildComponetsEx<T>(child, list);
        }
    }

    private void OnActorClick(PointerEventData data)
    {
        actorshow = !actorshow;
        GameObject gamecamera = GameObject.Find("GameCamera");
        if(gamecamera != null)
        {
            int actor = 1 << 8;
            int actor2 = 1 << 9;
            int allactor = actor | actor2;
        }

        GameObject reallight = GameObject.Find("LayerMover/Effects");
        if(reallight != null)
        {
            List<Renderer> allrender = new List<Renderer>();
            GetChildComponetsEx<Renderer>(reallight.transform, allrender);
            for(int i = 0; i < allrender.Count; ++i)
            {
                allrender[i].enabled = actorshow;
            }
        }
        
    }

    private void OnUguiClick(PointerEventData data)
    {
        scrollview.SetActive(!scrollview.activeSelf);
    }

    private void OnPrintSystem(PointerEventData data)
    {
        Queue<string> sb = new Queue<string>();
        sb.Enqueue("\nprint system info:");
        sb.Enqueue(string.Format("分辨率:<color=#00ffffff>{0}x{1}</color>,", Screen.width, Screen.height));
        sb.Enqueue(string.Format("内存:<color=#00ffffff>{0}</color>,", SystemInfo.systemMemorySize));
        sb.Enqueue(string.Format("显存:<color=#00ffffff>{0}</color>,", SystemInfo.graphicsMemorySize));
        sb.Enqueue(string.Format("deltaTime:<color=#00ffffff>{0}</color>,", Time.deltaTime.ToString("f4")));
        sb.Enqueue(string.Format("游戏运行时间:<color=#00ffffff>{0}</color>,", Time.realtimeSinceStartup.ToString("f1")));

        float iSize = 0;
        float big = 0;
        string bigName = "";
        var textures = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
        foreach (UnityEngine.Object t in textures)
        {
            int s = Profiler.GetRuntimeMemorySize(t);
            iSize += s;
            if (s > big)
            {
                big = s;
                bigName = t.name;
            }
            float ss = s / 1024f / 1024f;
            if (ss > 1.0f)
            {
                sb.Enqueue(string.Format("object:<color=#ffff00ff>{0}</color>(<color=#00ffffff>{1}</color>Mb)", t.name, ss));
            }
        }
        big = big / 1024f / 1024f;
        iSize = iSize / 1024f / 1024f;
        
        sb.Enqueue(string.Format("设备唯一id:<color=#ff00ffff>{0}</color>", SystemInfo.deviceUniqueIdentifier));

        sb.Enqueue(string.Format("显卡:<color=#ffff00ff>{0}</color>,", SystemInfo.graphicsDeviceName));
        sb.Enqueue(string.Format("显卡版本:<color=#ffff00ff>{0}</color>,", SystemInfo.graphicsDeviceVersion));
        sb.Enqueue(string.Format("Shader版本:<color=#ffff00ff>{0}</color>", SystemInfo.graphicsShaderLevel));

        sb.Enqueue(string.Format("操作系统:<color=#3ca5acff>{0}</color>", SystemInfo.operatingSystem));

        sb.Enqueue(string.Format("CPU架构:<color=#3b9fe5ff>{0}</color>,", SystemInfo.processorType));
        sb.Enqueue(string.Format("CPU核心数:<color=#3b9fe5ff>{0}</color>", SystemInfo.processorCount));

        sb.Enqueue(string.Format("持久保存路径:<color=#8b7decff>{0}</color>", Application.persistentDataPath));
        sb.Enqueue(string.Format("内部资源路径:<color=#8b7decff>{0}</color>", Application.streamingAssetsPath));

        //sb.Enqueue(string.Format("ip:<color=#96aa2a>{0}</color>,", Network.player.ipAddress));

        sb.Enqueue(string.Format("model:<color=#96aa2a>{0}</color>,", SystemInfo.deviceModel));
        sb.Enqueue(string.Format("name:<color=#96aa2a>{0}</color>,", SystemInfo.deviceName));
        sb.Enqueue(string.Format("type:<color=#96aa2a>{0}</color>,", SystemInfo.deviceType));

        sb.Enqueue(string.Format("内存占用:<color=#87ba66>{0} MB</color>", (System.GC.GetTotalMemory(true)) / 1024 / 1024));

        // 所有对象的内存占用
        int byteCount = 0;
        int objCount = 0;
        UnityEngine.Object[] allObject = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
        foreach (UnityEngine.Object obj in allObject)
        {
            byteCount += Profiler.GetRuntimeMemorySize(obj);
            objCount++;
        }
        sb.Enqueue(string.Format("对象数量:<color=#6F60D0>{0}</color>,", objCount));
        sb.Enqueue(string.Format("内存占用:<color=#87ba66>{0:F2}MB</color>,", byteCount / 1024.0f / 1024.0f));

        uint monoHeap = Profiler.GetMonoHeapSize();
        sb.Enqueue(string.Format("分配Mono堆:<color=#87ba66>{0:F2}MB</color>,", monoHeap / 1024.0f / 1024.0f));

        uint monoUsedHeap = Profiler.GetMonoUsedSize();
        sb.Enqueue(string.Format("Mono使用内存:<color=#87ba66>{0:F2}MB</color>,", monoUsedHeap / 1024.0f / 1024.0f));

        while(sb.Count != 0)
        {
            DebugInfo info = new DebugInfo();
            info.content = sb.Dequeue();
            info.logtype = LOG_TYPE.LT_COMMON;
            AddNewRow(info);
        }
    }
    // Use this for initialization
    void Init ()
    {
        if (!(UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer ||
           UnityEngine.Application.platform == RuntimePlatform.WindowsEditor))
        {
            max_show_cout = 100;
        }
        panel = Find(tdDebug, "Panel");
        PanelRectTran = panel.GetComponent<RectTransform>();
        debug_switch = Find(tdDebug, "Switch");
        
        scrollview = Find(panel, "Scroll View");
        drag_button = Find(panel, "drag_button");
        resize_button = Find(panel, "resize_button");
        scenebutton = Find(panel, "scene_button");
        actorbutton = Find(panel, "actor_button");
        uguibutton = Find(panel, "ugui_button");
        showtype[(int)LOG_TYPE.LT_COMMON] = showtype[(int)LOG_TYPE.LT_WARNING] = showtype[(int)LOG_TYPE.LT_ERROR] = true;
        type_buttons[(int)LOG_TYPE.LT_COMMON] = Find(panel, "common_button").GetComponent<Button>();
        type_buttons[(int)LOG_TYPE.LT_WARNING] = Find(panel, "warn_button").GetComponent<Button>();
        type_buttons[(int)LOG_TYPE.LT_ERROR] = Find(panel, "error_button").GetComponent<Button>();
        common_btn_text = Find(panel, "common_btn_text").GetComponent<Text>();
        warn_btn_text = Find(panel, "warn_btn_text").GetComponent<Text>();
        error_btn_text = Find(panel, "error_btn_text").GetComponent<Text>();
        system_text = Find(panel, "system_text").GetComponent<Text>();
        go_on = Find(panel, "go_on");
        copyfiled = Find(panel, "InputFieldForCopy").GetComponent<InputField>();
        FPS_value = Find(tdDebug, "FPS_value").GetComponent<Text>();

        EventTriggerListener.Get(Find(copyfiled.gameObject, "Button")).onClick += OnCopyHideBtnClick;

        EventTriggerListener.Get(debug_switch).onClick += OnSwitchClick;
        EventTriggerListener.Get(drag_button).onDrag += OnDrag;
        EventTriggerListener.Get(drag_button).onEndDrag += OnDragEnd;
        EventTriggerListener.Get(resize_button).onClick += OnResize;
        EventTriggerListener.Get(go_on).onClick += OnGoClick;

        EventTriggerListener.Get(common_btn_text.gameObject).onClick += OnCommonClick;
        EventTriggerListener.Get(warn_btn_text.gameObject).onClick += OnWarnClick;
        EventTriggerListener.Get(error_btn_text.gameObject).onClick += OnErrorClick;
        EventTriggerListener.Get(system_text.gameObject).onClick += OnPrintSystem;
        
        EventTriggerListener.Get(scrollview).onDBClick += ScrollViewOnDoubleClick;
        Content = Find(scrollview, "Content");
        EventTriggerListener.Get(scrollview).onEndDrag += OnScrollBarDragEnd;
        EventTriggerListener.Get(scenebutton).onClick += OnSceneClick;
        EventTriggerListener.Get(actorbutton).onClick += OnActorClick;
        EventTriggerListener.Get(uguibutton).onClick += OnUguiClick;

        Content_tran = Content.transform;
        Item = Find(Content, "Item");
        scrollbar = Find(scrollview, "Scrollbar Vertical").GetComponent<Scrollbar>();
        EventTriggerListener.Get(scrollbar.gameObject).onEndDrag += OnScrollBarDragEnd;
        is_auto_end = true;
        
        raycaster = Find(ugui_root, "Canvas").GetComponent<GraphicRaycaster>();
        eventsystem = Find(ugui_root, "EventSystem") .GetComponent<EventSystem>();
        eventData = new PointerEventData(eventsystem);
        OnResize(eventData);
    }

    private void AddNewRow(DebugInfo info)
    {
        GameObject newItem = null;
        if (deaditem.Count > 0)
        {
            newItem = deaditem.Dequeue();
        }
        else
        {
            newItem = GameObject.Instantiate(Item);
        }
         
        Text comtxt = newItem.transform.GetComponent<Text>();
		int len = info.content.Length;
		if(len>1000)
			len = 1000;
		comtxt.text = "<b><color=#88ff88>" + DateTime.Now.ToString("HH:mm:ss") + ":</color></b> " + info.content.Substring(0,len);
        switch(info.logtype)
        {
            case LOG_TYPE.LT_COMMON: comtxt.color = Color.white; break;
            case LOG_TYPE.LT_WARNING: comtxt.color = Color.yellow; break;
            case LOG_TYPE.LT_ERROR: comtxt.color = new Color(1, 0.4f, 0.4f); /*Outline ot = newItem.AddComponent<Outline>(); ot.effectColor = Color.black;*/ break;
            default:break;
        }
        newItem.transform.SetParent(Content.transform, false);
        newItem.SetActive(true);
        diclogs[newItem] = info;
        if (addrowtime == 0)
        {
            addrowtime = Time.realtimeSinceStartup;
        }
    }

    private void UpdateScrollBar()
    {
        if (is_auto_end)
        {
            scrollbar.value = 0;
        }
    }

    private void RemoveRow(int type)
    {
        Transform firstitem = Content_tran.GetChild(1); //索引0克隆用
        diclogs.Remove(firstitem.gameObject);
        
        if (deaditem.Count > 10)
            GameObject.Destroy(firstitem.gameObject);
        else
        {
            deaditem.Enqueue(firstitem.gameObject);
            firstitem.SetParent(tdDebug.transform);
            firstitem.gameObject.SetActive(false);
        }
    }
    
	// Update is called once per frame
	void Update () {
        UpdateTick();
        if (Input.GetKeyDown(KeyCode.L))
        {
            Log("this is a \n 换行了 \rcommon log");
        }
        //else if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Warn("this is a warning");
        //}
        //else if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Error("this is a error");
        //}
        
        if (queue_logs.Count > 0 && !pause_show)
        {
            DebugInfo info = queue_logs.Dequeue();
            if (info.logtype == LOG_TYPE.LT_ERROR)
            {
                pause_show = true;
                UpdatePauseBtnShow();
            }
            AddNewRow(info);
        }

        for(int i = 0; i <= (int)LOG_TYPE.LT_NUM; ++i)
        {
            if (Content_tran && Content_tran.childCount + 1 > max_show_cout)
            {
                RemoveRow(i);
            }
        }

        if (panel_visible && is_auto_end && addrowtime != 0 && Time.realtimeSinceStartup - addrowtime > 0.3)
        {
            //ExecuteEvents.Execute<IDragHandler>(scrollview, new PointerEventData(EventSystem.current), ExecuteEvents.dragHandler);
            UpdateScrollBar();
            addrowtime = 0;
        }
        
        FPS_value.color = Color.green;
        if (mLastFps < 15)
            FPS_value.color = Color.red;
        else if (mLastFps >= 15 && mLastFps < 30)
            FPS_value.color = Color.yellow;
        FPS_value.text = mLastFps.ToString();
    }

    private long mFrameCount = 0;
    private long mLastFrameTime = 0;
    static long mLastFps = 0;
    private void UpdateTick()
    {
        if (true)
        {
            mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (mLastFrameTime == 0)
            {
                mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - mLastFrameTime) >= 1000)
            {
                long fps = (long)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));
                mLastFps = fps;
                mFrameCount = 0;
                mLastFrameTime = nCurTime;
            }
        }
    }

    public static long TickToMilliSec(long tick)
    {
        return tick / (10 * 1000);
    }

    static public bool CheckUGuiRaycastObjects()     //专业事件拦截
    {
#if !DEBUG_OPEN
        return false;
#endif
        if (instance == null) GetInstance();
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        raycaster.Raycast(eventData, list);
        return list.Count > 0;
    }

    static public GameObject GetTopTouchObject()
    {
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        raycaster.Raycast(eventData, list);
        return list[0].gameObject;
    }
}
