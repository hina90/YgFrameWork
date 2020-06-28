using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

/// <summary>
/// 主界面
/// </summary>
public class UI_Main : UIBase
{
    private Button setting;             //设置
    private Button customer;            //顾客
    private Button menu;                //菜单
    private Button task;                //任务
    private Button propagandax;         //宣传x15
    private Button propaganda;          //宣传按钮
    private Button propagandaUp;        //宣传升级
    private Transform progress;         //宣传按钮进度条
    private Image progressBar;          //宣传按钮进度条
    private Button exit;                //退出游戏
    private Button exitPanel;
    private Button yesBtn;
    private Button noBtn;
    private Canvas canvas;
    private GameObject taskTip;          //完成任务提示
    private Button btnPrevious, btnNext, btnRestaurant, btnStore; //切换场景按钮
    private Image imgPrevious, imgNext, imgRestaurant, imgStore;

    private PlayerModule player;
    private CustomerModule customerModule;
    private int pointNum;                       //宣传当前点击的次数
    private int propagandaNum = 0;              //宣传次数
    private bool starPropaganda = false;        //是否开始宣传
    private CameraMove cameraMove;              //摄像机移动行为脚本
    private FacilitiesType curAreaType = FacilitiesType.Restaurant;         //当前聚焦的区域类型
    private Animator uiAnim;
    private GameObject drummer;
    private GameObject aimTaskObj;
    private Text taskDes;
    private Text rewardNum;
    private TextMeshProUGUI rewardTypeIcon;
    private Button btnTaskReceive;
    private Button aimTaskBtn;
    protected const string UI_FISH_PREFAB = "DriedFishImg";
    protected const string UI_Heart_PREFAB = "HeartImg";
    protected const string FISH_NAME = "DriedFish";
    protected Vector2 targetPos = default;
    private UI_Money uiMoney;
    private AreaContainer areaContainer;

    public override void Init()
    {
        Layer = LayerMenue.PUBLIC;
        player = GameModuleManager.Instance.GetModule<PlayerModule>();
        customerModule = GameModuleManager.Instance.GetModule<CustomerModule>();
        cameraMove = Camera.main.GetComponent<CameraMove>();
        areaContainer = GetComponent<AreaContainer>();
        pointNum = 0;
    }

    /// <summary>
    /// 进入界面
    /// </summary>
    protected override void Enter()
    {
        base.Enter();
        setting = Find<Button>(gameObject, "Setting");
        customer = Find<Button>(gameObject, "Customer");
        menu = Find<Button>(gameObject, "Menu");
        task = Find<Button>(gameObject, "Task");
        propagandax = Find<Button>(gameObject, "PropagandaX15");
        propaganda = Find<Button>(gameObject, "Propaganda");
        propagandaUp = Find<Button>(gameObject, "ProUpgrade");
        progress = Find(gameObject, "Progress").transform;
        progressBar = Find<Image>(gameObject, "Bar");
        exit = Find<Button>(gameObject, "Exit");
        exitPanel = Find<Button>(gameObject, "exitPanel");
        yesBtn = Find<Button>(gameObject, "Yes");
        noBtn = Find<Button>(gameObject, "No");
        btnPrevious = Find<Button>(gameObject, "btnPrevious");
        btnNext = Find<Button>(gameObject, "btnNext");
        btnRestaurant = Find<Button>(gameObject, "btnRestaurant");
        btnStore = Find<Button>(gameObject, "btnStore");
        imgPrevious = Find<Image>(btnPrevious.gameObject, "areaImg");
        imgNext = Find<Image>(btnNext.gameObject, "areaImg");
        imgRestaurant = Find<Image>(btnRestaurant.gameObject, "areaImg");
        imgStore = Find<Image>(btnStore.gameObject, "areaImg");
        canvas = GetComponent<Canvas>();
        taskTip = Find(gameObject, "tip");
        drummer = GameObject.Find("PathMap/Restaurant/drumTeam");
        aimTaskObj = Find(gameObject, "aimTask");
        aimTaskBtn = aimTaskObj.GetComponent<Button>();
        taskDes = Find<Text>(aimTaskObj, "taskDes");
        rewardTypeIcon = Find<TextMeshProUGUI>(aimTaskObj, "rewardIcon");
        rewardNum = Find<Text>(aimTaskObj, "rewardNum");
        btnTaskReceive = Find<Button>(aimTaskObj, "btnReceive");
        uiAnim = GetComponent<Animator>(); AddEventLiseners();
        TaskManager.Instance.SetReceivedTips();
        uiMoney = UIManager.Instance.GetUI<UI_Money>("UI_Money");
        //if (Global.OPEN_GUIDE && !GuideManager.Instance.IsFinishGuide())
        //{
        //    Hide();
        //}
        //UIManager.Instance.SendUIEvent(GameEvent.START_AIMTASK);
    }
    public override void MainUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))//点击返回键有弹窗
        {
            if (!GuideManager.Instance.IsFinishGuide()) return;
            exitPanel.gameObject.SetActive(true);
            canvas.sortingOrder = 4;
        }
        if (propagandaNum > 0 && !GameManager.Instance.CheckDinnerLimited())
        {
            starPropaganda = true;
            progress.gameObject.SetActive(true);
            propagandax.interactable = false;
            if (GuideManager.Instance.IsFinishGuide())
            {
                propaganda.interactable = false;
            }
        }
        if (!starPropaganda)//是否可以开始宣传15连
            return;
        progressBar.fillAmount += 0.02f;
        if (progressBar.fillAmount >= 1)
        {
            if (GameManager.Instance.CheckDinnerLimited())
            {
                drummer.SetActive(false);
                starPropaganda = false;
                progressBar.fillAmount = 0;
                propagandax.interactable = true;
                propaganda.interactable = true;
                //propagandaNum = 0;
                progress.gameObject.SetActive(false);
                return;
            }
            progressBar.fillAmount = 0;
            GameManager.Instance.AdvertiseGuest();
            propagandaNum += 1;
        }
        if (propagandaNum >= 14)
        {
            drummer.SetActive(false);
            starPropaganda = false;
            progress.gameObject.SetActive(false);
            propagandax.interactable = true;
            propaganda.interactable = true;
            propagandaNum = 0;
        }
    }

    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        Dictionary<GameEvent, Callback<object[]>> eventDic = new Dictionary<GameEvent, Callback<object[]>>
        {
            //宣传按钮*15事件
            [GameEvent.UPDATE_PROPAGANDA] = delegate (object[] param)
            {
                if (param.Length > 0 && (bool)param[0] == false)
                    drummer.SetActive(false);
                else
                    drummer.SetActive(true);

                starPropaganda = true;
                progress.gameObject.SetActive(true);
                propagandax.interactable = false;
                propaganda.interactable = false;
            },
            [GameEvent.OPEN_TASKTIP] = delegate (object[] param)
            {
                taskTip.SetActive(true);
            },
            [GameEvent.CLOSE_TASKTIP] = delegate (object[] param)
            {
                taskTip.SetActive(false);
            },
            ///摄像机聚焦行为
            [GameEvent.CAMERA_FOCUS] = delegate (object[] param)
            {
                ShowGuideArrow((FacilitiesType)param[0]);
            },
            ///摄像机远景模式
            [GameEvent.CAMERA_PROSPECT] = delegate (object[] param)
            {
                btnPrevious.gameObject.SetActive(false);
                btnNext.gameObject.SetActive(false);
                btnRestaurant.gameObject.SetActive(false);
                btnStore.gameObject.SetActive(false);
            },
            //显示UI按钮
            [GameEvent.SHOW_MAINUI] = delegate (object[] param)
            {
                Show();
            },
            //开始引导任务
            [GameEvent.START_AIMTASK] = delegate (object[] param)
            {
                aimTaskObj.SetActive(true);
                RefreshAimTask(GameModuleManager.Instance.GetModule<AimTaskModule>().AimTaskData);
            },
            //刷新引导任务
            [GameEvent.REFRESH_AIMTASK] = delegate (object[] param)
              {
                  RefreshAimTask((AimTaskData)param[0]);
              },
            //完成引导任务
            [GameEvent.FINISH_AIMTASK] = delegate (object[] param)
            {
                aimTaskObj.SetActive(false);
            },
        };
        return eventDic;
    }

    /// <summary>
    /// 添加按钮事件
    /// </summary>
    private void AddEventLiseners()
    {
        setting.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.OpenUI<UI_Setting>();
        });
        customer.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.OpenUI<UI_Customer>();
        });
        menu.onClick.AddListener(() =>
        {
            GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_WEAKGUIDE);
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.OpenUI<UI_Menu>();

        });
        task.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.OpenUI<UI_Task>();
        });
        propagandax.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            if (GameManager.Instance.CheckDinnerLimited())
            {
                TipManager.Instance.ShowMsg("排队的客人有些多\n先让他们吃上美食吧~");
                return;
            }
            SDKManager.Instance.LogEvent(EventId.CC_Propaganda.ToString(), "ProgressWatchAd", "Button");
            UIManager.Instance.OpenUI<UI_Progress>();
        });
        propaganda.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");

            if (GameManager.Instance.CheckDinnerLimited())
            {
                TipManager.Instance.ShowMsg("排队的客人有些多\n先让他们吃上美食吧~");
                return;
            }
            progress.gameObject.SetActive(true);
            progressBar.fillAmount += (1f / 7);
            pointNum += 1;
            if (pointNum >= 7)
            {
                progressBar.fillAmount = 0;
                progress.gameObject.SetActive(false);
                GameManager.Instance.ClickGuest();
                TaskManager.Instance.CheckTask(TaskType.PROPAGANDA, 1);
                pointNum = 0;
                //宣传按钮引导
                GuideManager.Instance.BroadcastGuideEvent(GameEvent.AUTO_FINISH);
            }
        });
        propagandaUp.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
        });
        exit.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            CameraMove.ToggleProspect();
        });
        yesBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            Application.Quit();
        });
        noBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            exitPanel.gameObject.SetActive(false);
            canvas.sortingOrder = 0;
        });
        btnPrevious.onClick.AddListener(() =>
        {
            switch (curAreaType)
            {
                case FacilitiesType.Restaurant:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Garden);
                    break;
                case FacilitiesType.Kitchen:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Restaurant);
                    break;
                case FacilitiesType.Store:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Cafe);
                    break;
                case FacilitiesType.Farm:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Store);
                    break;
                default:
                    break;
            }
        });
        btnNext.onClick.AddListener(() =>
        {
            switch (curAreaType)
            {
                case FacilitiesType.Restaurant:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Kitchen);
                    break;
                case FacilitiesType.Garden:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Restaurant);
                    break;
                case FacilitiesType.Cafe:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Store);
                    break;
                case FacilitiesType.Store:
                    cameraMove.MoveDesignatedArea(FacilitiesType.Farm);
                    break;
                default:
                    break;
            }
            GuideManager.Instance.RegisterBtn(menu);
        });
        btnRestaurant.onClick.AddListener(() =>
        {
            cameraMove.MoveDesignatedArea(FacilitiesType.Restaurant);
        });
        btnStore.onClick.AddListener(() =>
        {
            cameraMove.MoveDesignatedArea(FacilitiesType.Store);
        });

        btnTaskReceive.onClick.AddListener(() =>
        {
            GameEvent eventType = aimTaskData.taskConfig.rewardType == 1 ? GameEvent.UPDATE_FISH : GameEvent.UPDATE_STAR;
            string rewardType = string.Empty;
            switch (eventType)
            {

                case GameEvent.UPDATE_FISH:
                    rewardType = UI_FISH_PREFAB;
                    targetPos = new Vector2(-296, 585);
                    break;
                case GameEvent.UPDATE_STAR:
                    rewardType = UI_Heart_PREFAB;
                    targetPos = new Vector2(-300, 533);
                    break;
                default:
                    break;
            }
            GameObject rewardObj = ResourceManager.Instance.GetResourceInstantiate(rewardType, uiMoney.transform, ResouceType.PrefabItem);
            rewardObj.transform.position = aimTaskObj.transform.position;
            int rewardValue = aimTaskData.taskConfig.rewardValue;
            rewardObj.transform.DOLocalMove(targetPos, 0.8f).OnComplete(() =>
            {
                UIManager.Instance.SendUIEvent(eventType, rewardValue);
                Destroy(rewardObj.gameObject);
            });
            TaskManager.Instance.FinishAimTask(taskId);

        });

        aimTaskBtn.onClick.AddListener(() =>
        {
            ShowAimTaskGuideLogic();
        });

        GuideManager.Instance.RegisterBtn(menu);
        GuideManager.Instance.RegisterBtn(propaganda);
        GuideManager.Instance.RegisterBtn(propagandax);
        GuideManager.Instance.RegisterBtn(btnPrevious);
        GuideManager.Instance.RegisterBtn(btnNext);
        GuideManager.Instance.RegisterBtn(exit);
    }

    /// <summary>
    /// 显示指引箭头逻辑
    /// </summary>
    private void ShowGuideArrow(FacilitiesType areaType)
    {
        btnPrevious.gameObject.SetActive(false);
        btnNext.gameObject.SetActive(false);
        btnStore.gameObject.SetActive(false);
        btnRestaurant.gameObject.SetActive(false);

        switch (areaType)
        {
            case FacilitiesType.Restaurant:
                btnPrevious.gameObject.SetActive(true);
                btnNext.gameObject.SetActive(true);
                btnStore.gameObject.SetActive(true);
                imgPrevious.overrideSprite = areaContainer.areaSprites[3];
                imgNext.overrideSprite = areaContainer.areaSprites[2];
                imgStore.overrideSprite = areaContainer.areaSprites[5];
                break;
            case FacilitiesType.Store:
                btnPrevious.gameObject.SetActive(true);
                btnNext.gameObject.SetActive(true);
                btnRestaurant.gameObject.SetActive(true);
                imgRestaurant.overrideSprite = areaContainer.areaSprites[1];
                imgPrevious.overrideSprite = areaContainer.areaSprites[6];
                imgNext.overrideSprite = areaContainer.areaSprites[6];
                break;
            case FacilitiesType.Kitchen:
                btnPrevious.gameObject.SetActive(true);
                imgPrevious.overrideSprite = areaContainer.areaSprites[0];
                break;
            case FacilitiesType.Farm:
                btnPrevious.gameObject.SetActive(true);
                imgPrevious.overrideSprite = areaContainer.areaSprites[4];
                break;
            case FacilitiesType.Garden:
                btnNext.gameObject.SetActive(true);
                imgNext.overrideSprite = areaContainer.areaSprites[0];
                break;
            case FacilitiesType.Cafe:
                btnNext.gameObject.SetActive(true);
                imgNext.overrideSprite = areaContainer.areaSprites[4];
                break;
            default:
                break;
        }
        curAreaType = areaType;
    }


    private int taskId;
    private AimTaskData aimTaskData;
    /// <summary>
    /// 刷新当前指引任务
    /// </summary>
    private void RefreshAimTask(AimTaskData aimTaskData)
    {
        this.aimTaskData = aimTaskData;
        taskId = aimTaskData.taskConfig.taskId;
        if (aimTaskData.taskConfig.targetValue == 0)
        {
            taskDes.text = aimTaskData.taskConfig.des;
        }
        else
        {
            taskDes.text = string.Format(aimTaskData.taskConfig.des, aimTaskData.taskProgressValue);
        }
        int value = aimTaskData.taskConfig.rewardValue;
        rewardNum.text = "+" + value.ToString();
        rewardTypeIcon.text = aimTaskData.taskConfig.rewardType == 1 ? "<sprite=7>" : "<sprite=5>";
        btnTaskReceive.gameObject.SetActive(aimTaskData.isFinish);
    }

    /// <summary>
    /// 显示引导任务指引逻辑
    /// </summary>
    private GameObject claw;
    private void ShowAimTaskGuideLogic()
    {
        if (aimTaskData.isFinish || claw != null) return;
        AimTaskAreaType areaType = (AimTaskAreaType)aimTaskData.taskConfig.uiType;
        switch (areaType)
        {
            case AimTaskAreaType.RestaurantPanel:
            case AimTaskAreaType.KitchenPanel:
            case AimTaskAreaType.StorePanel:
            case AimTaskAreaType.GardenPanel:
            case AimTaskAreaType.MenuPanel:
                UIManager.Instance.OpenUI<UI_Menu>(aimTaskData);
                break;
            case AimTaskAreaType.CustomerPanel:
                UIManager.Instance.OpenUI<UI_Customer>();
                break;
            case AimTaskAreaType.GoodsPanel:
                UIManager.Instance.OpenUI<UI_Goods>(1);
                break;
            case AimTaskAreaType.RestaurantArea:
                if (curAreaType != FacilitiesType.Restaurant)
                {
                    cameraMove.MoveDesignatedArea(FacilitiesType.Restaurant);

                }
                break;
            case AimTaskAreaType.GardenArea:
                if (curAreaType != FacilitiesType.Garden)
                {
                    cameraMove.MoveDesignatedArea(FacilitiesType.Garden);
                }
                break;
            case AimTaskAreaType.StoreArea:
                if (curAreaType != FacilitiesType.Store)
                {
                    cameraMove.MoveDesignatedArea(FacilitiesType.Store);
                }
                break;
            default:
                break;
        }

        if (aimTaskData.taskConfig.showFinger == 1)
        {
            string clawName = string.Empty;
            Transform parentTrans = default;
            switch (aimTaskData.taskConfig.guideType)
            {
                case 1:
                    //界面指引
                    clawName = "Claw";
                    parentTrans = UIManager.Instance.GetResUI(aimTaskData.taskConfig.parentName).transform;
                    break;
                case 2:
                    //场景指引
                    clawName = "WorldClaw";
                    if (aimTaskData.taskId == 1014)
                    {
                        parentTrans = FacilitiesManager.Instance.GetFreeParterre().gameObject.transform;
                    }
                    else
                    {
                        parentTrans = FacilitiesManager.Instance.FacilitiesBuidingDic[int.Parse(aimTaskData.taskConfig.parentName)].gameObject.transform;
                    }
                    break;
                default:
                    break;
            }
            if (parentTrans == default) return;
            GameObject clawPrefab = ResourceManager.Instance.GetResource(clawName, ResouceType.PrefabItem);
            claw = Instantiate(clawPrefab, parentTrans);
            claw.transform.localPosition = new Vector3(aimTaskData.taskConfig.pos[0], aimTaskData.taskConfig.pos[1], 0);
            claw.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, aimTaskData.taskConfig.angle));
            Destroy(claw, 2f);
        }
    }
}
