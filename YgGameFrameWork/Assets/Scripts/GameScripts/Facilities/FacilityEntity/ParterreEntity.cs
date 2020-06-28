using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tool.Database;
using System;
using Spine.Unity;
using Spine;

//花圃状态
public enum ParterreStatus
{
    /// <summary>
    /// 待种植
    /// </summary>
    Idle,
    /// <summary>
    /// 成苗
    /// </summary>
    Seeding,
    /// <summary>
    /// 成花
    /// </summary>
    Flowering,
    /// <summary>
    /// 观赏
    /// </summary>
    Show,
    /// <summary>
    /// 等待播种
    /// </summary>
    WaitSow,
    /// <summary>
    /// 等待变异
    /// </summary>
    WaitVariation,
}

/// <summary>
/// 花圃设施实体
/// </summary>
public class ParterreEntity : FacilityEntity
{
    private GameObject bubble;
    private GameObject slider;
    private Image imgProgress;
    private SpriteRenderer spriteSeed, flowerSprite;
    private TextMeshPro txtTimer, txtStage;
    private ParterreBubble parterreBubble;         //气泡组件
    private GardenConfigData flowerConfig;         //花朵配置数据
    private GardenModule gardenModule;             //花园数据模块
    private ParterreStoreData parterreStoreData;   //花圃持久数据
    private GameObject variationObj;               //变异警示图标
    private GameObject particle_Star;
    private SkeletonAnimation skeAnim;
    private bool isVariation;                      //是否变异

    private float curTime = 0;            //当前累计时间变量

    private int totalTime = 0;            //阶段总时间
    private int tempTime = 0;             //阶段临时时间

    public bool IsInSight = false;         //是否可以观赏

    /// <summary>
    /// 是否有客人
    /// </summary>
    public bool HaveGuest { get; set; }


    public ParterreStoreData ParterreStoreData
    {
        get
        {
            if (parterreStoreData == null)
            {
                //获取储存数据
                if (!gardenModule.ParterreDataDic.ContainsKey(facilitiesData.facilitiesConfigData.id))
                {
                    gardenModule.SetParterreDataValue(facilitiesData.facilitiesConfigData.id,
                    new ParterreStoreData()
                    {
                        parterreStatus = ParterreStatus.Idle,
                        visitTimes = 2,
                        remainVisitTimes = 2
                    });
                }
                parterreStoreData = gardenModule.ParterreDataDic[facilitiesData.facilitiesConfigData.id];
                SetFlowerData(parterreStoreData.flowerId);
                if (parterreStoreData.parterreStatus != ParterreStatus.Idle)
                {
                    //种子成长状态，UI显示
                    bubble.SetActive(false);
                    slider.SetActive(true);
                    string spritePath = string.Empty;
                    switch (parterreStoreData.parterreStatus)
                    {
                        case ParterreStatus.Seeding:
                            spritePath = flowerConfig.seedIcon;
                            break;
                        case ParterreStatus.Flowering:
                            spritePath = flowerConfig.flowerSeedingIcon;
                            break;
                        case ParterreStatus.Show:
                            spritePath = flowerConfig.icon;
                            break;
                        default:
                            break;
                    }
                    SetFlowerSprite(spritePath);
                }
                //判断是否处于待变异期间
                if (gardenModule.WaitVariationId == facilitiesData.facilitiesConfigData.id)
                {
                    ShowVariationWarning(true);
                    UI_SeedMutationAds.SetCallBack(WatchVideoSuccess, WatchVideoFail, CloseVideo, ShowVariationWarning);
                }
                ChangeStatus(parterreStoreData.parterreStatus);
            }
            return parterreStoreData;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        bubble = transform.Find("bubble").gameObject;
        spriteSeed = transform.Find("bubble/seed").GetComponent<SpriteRenderer>();
        flowerSprite = transform.Find("flower").GetComponent<SpriteRenderer>();
        slider = transform.Find("slider").gameObject;
        imgProgress = transform.Find("slider/progress/fill").GetComponent<Image>();
        txtTimer = transform.Find("slider/timer").GetComponent<TextMeshPro>();
        txtStage = transform.Find("slider/stage").GetComponent<TextMeshPro>();
        variationObj = transform.Find("variation").gameObject;
        particle_Star = transform.Find("Particle_Star").gameObject;
        bubble.SetActive(true);
        parterreBubble = bubble.GetOrAddComponent<ParterreBubble>();
        parterreBubble.SetPlantingAction(Planting);
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
    }

    private void Update()
    {
        if (facilitiesData == null) return;
        switch (ParterreStoreData.parterreStatus)
        {
            case ParterreStatus.Idle:
                IsInSight = false;
                break;
            case ParterreStatus.Seeding:
                IsInSight = false;
                Seeding();
                break;
            case ParterreStatus.Flowering:
                IsInSight = false;
                Flowering();
                break;
            case ParterreStatus.Show:
                IsInSight = true;
                Showing();
                break;
            case ParterreStatus.WaitSow:
                IsInSight = false;
                WaitSow();
                break;
            default:
                break;
        }

        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    Ornamental();
        //}
    }

    public override void ClickEffect()
    {
        if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
        if (ParterreStoreData.parterreStatus != ParterreStatus.Idle) return;
        UIManager.Instance.OpenUI<UI_Parterre>(facilitiesData);
        UI_Parterre.SetPlantingAction(Planting);
    }

    protected override void RefeshFacilityLogic()
    {
        parterreBubble.SetData(facilitiesData);
    }

    /// <summary>
    /// 种植
    /// </summary>
    private void Planting()
    {
        bubble.SetActive(false);
        slider.SetActive(true);
        RectTransform rect = txtStage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0.9f, 0.1f);
        txtStage.fontSize = 1.5f;
        TaskManager.Instance.CheckTask(TaskType.GROW_FLOWERS, 1);
        SetFlowerInfo(facilitiesData.itemId);
        ChangeStatus(ParterreStatus.Seeding);
    }

    /// <summary>
    /// 育苗
    /// </summary>
    private void Seeding()
    {
        //花种子成苗计时
        curTime += Time.deltaTime;
        if (curTime >= 1)
        {
            totalTime--;
            txtTimer.text = Utils.Second2Minute(totalTime);
            curTime = 0;
            imgProgress.fillAmount = 1 - (float)totalTime / tempTime;
            //保存花苗成长时剩余时间
            ParterreStoreData.remainTime = totalTime;
        }
        if (totalTime <= 0)
        {
            //概率变异
            Mutation();
        }
    }

    /// <summary>
    /// 成花
    /// </summary>
    private void Flowering()
    {
        //花苗成花计时
        curTime += Time.deltaTime;
        if (curTime >= 1)
        {
            totalTime--;
            txtTimer.text = Utils.Second2Minute(totalTime);
            curTime = 0;
            imgProgress.fillAmount = 1 - (float)totalTime / tempTime;
            //保存花苗成花剩余时间
            ParterreStoreData.remainTime = totalTime;
        }
        if (totalTime <= 0)
        {
            ChangeStatus(ParterreStatus.Show);
        }
    }

    /// <summary>
    /// 展示
    /// </summary>
    private void Showing()
    {
        //展示倒计时
        curTime += Time.deltaTime;
        if (curTime >= 1)
        {
            totalTime--;
            txtTimer.text = Utils.Second2Minute(totalTime);
            curTime = 0;
            imgProgress.fillAmount = 1 - (float)totalTime / tempTime;
            //保存展示剩余时间
            parterreStoreData.remainTime = totalTime;
        }
        if (totalTime <= 0)
        {
            //花朵凋谢
            ChangeStatus(ParterreStatus.WaitSow);
            //重置精灵图片
            flowerSprite.sprite = null;  //ResourceManager.Instance.GetSpriteResource(facilitiesData.itemConfigData.icon, ResouceType.Icon);
            //重置可观赏次数
            parterreStoreData.remainVisitTimes = 0;
        }
    }

    /// <summary>
    /// 等待播种
    /// </summary>
    private void WaitSow()
    {
        //等待播种倒计时
        curTime += Time.deltaTime;
        if (curTime > 1)
        {
            totalTime--;
            curTime = 0;
        }
        if (totalTime <= 0)
        {
            ChangeStatus(ParterreStatus.Idle);
        }
    }

    /// <summary>
    /// 种子突变，突变成功返回True,否则False
    /// </summary>
    private void Mutation()
    {
        ///突变CD期间不可再次发生突变
        if (gardenModule.MutationCDTime > 0 || gardenModule.WaitVariationId != 0)
        {
            ChangeStatus(ParterreStatus.Flowering);
            return;
        }
        int section = (int)(flowerConfig.mutationRate * 100);
        if (UnityEngine.Random.Range(0, 101) <= section)
        {
            //提供玩家操作的接口界面TODO
            ShowVariationWarning(true);
            UI_SeedMutationAds.SetCallBack(WatchVideoSuccess, WatchVideoFail, CloseVideo, ShowVariationWarning);
            //WatchVideoSuccess("", "", .1f);
        }
        else
        {
            //改变状态为成花期
            ChangeStatus(ParterreStatus.Flowering);
        }
    }

    /// <summary>
    /// 顾客观赏行为
    /// </summary>
    public void Ornamental()
    {
        if (ParterreStoreData.remainVisitTimes <= 0) return;
        ParterreStoreData.remainVisitTimes--;
        //变异花朵观赏获取双倍收益
        int value = isVariation ? (int)facilitiesData.itemConfigData.funParam_2[1] * 2 : (int)facilitiesData.itemConfigData.funParam_2[1];
        //产生小鱼干
        CreateDriedFish(value);
        if (parterreStoreData.remainVisitTimes <= 0)
        {
            //重置精灵图片
            flowerSprite.sprite = null; //ResourceManager.Instance.GetSpriteResource(facilitiesData.itemConfigData.icon, ResouceType.Icon);
            //花朵凋谢
            ChangeStatus(ParterreStatus.WaitSow);
            //重置变异
            isVariation = false;
        }
        //Debug.Log("观赏了一次花");
    }

    /// <summary>
    /// 重置当前花培育阶段的时间
    /// </summary>
    private void ResetTime(int time, int stageTime)
    {
        tempTime = stageTime;
        totalTime = time;
        txtTimer.text = Utils.Second2Minute(time);
    }

    /// <summary>
    /// 状态切换
    /// </summary>
    /// <param name="status"></param>
    private void ChangeStatus(ParterreStatus status)
    {
        int remainTime = ParterreStoreData.remainTime;
        float growthRate = 1 - gardenModule.GrowthRateBonus;
        switch (status)
        {
            //待种植
            case ParterreStatus.Idle:
                bubble.SetActive(true);
                txtStage.text = null;
                break;
            //育苗期
            case ParterreStatus.Seeding:
                txtStage.text = "育苗期";
                int stageTime = (int)(flowerConfig.GrowthTime[0] * growthRate);
                ResetTime(remainTime <= 0 ? stageTime : remainTime, stageTime);
                SetFlowerSprite(flowerConfig.seedIcon);
                break;
            //成花期
            case ParterreStatus.Flowering:
                txtStage.text = "成熟期";
                int stageTime1 = (int)(flowerConfig.GrowthTime[1] * growthRate);
                ResetTime(remainTime <= 0 ? stageTime1 : remainTime, stageTime1);
                SetFlowerSprite(flowerConfig.flowerSeedingIcon);
                break;
            //展示期
            case ParterreStatus.Show:
                txtStage.text = "展示期";
                ResetTime(remainTime <= 0 ? flowerConfig.GrowthTime[2] : remainTime, flowerConfig.GrowthTime[2]);
                ParterreStoreData.remainVisitTimes = ParterreStoreData.visitTimes;
                SetFlowerSprite(flowerConfig.icon);
                //显示特效
                particle_Star.SetActive(true);
                break;
            //等待播种
            case ParterreStatus.WaitSow:
                imgProgress.fillAmount = 0;
                slider.SetActive(false);
                ResetTime(flowerConfig.sowCDTime, flowerConfig.sowCDTime);
                //特效消失
                particle_Star.SetActive(false);
                break;
            default:
                break;
        }
        ParterreStoreData.parterreStatus = status;
    }

    /// <summary>
    /// 更新花种信息
    /// </summary>
    /// <param name="itemId">当前设施Id</param>
    private void SetFlowerInfo(int itemId)
    {
        ItemConfigData itemConfig = ConfigDataManager.Instance.GetDatabase<ItemConfigDatabase>().GetDataByKey(itemId.ToString());
        SetFlowerData((int)itemConfig.funParam_1[0]);
    }

    /// <summary>
    /// 获取花种信息
    /// </summary>
    /// <param name="flowerId"></param>
    private void SetFlowerData(int flowerId)
    {
        ParterreStoreData.flowerId = flowerId;
        flowerConfig = ConfigDataManager.Instance.GetDatabase<GardenConfigDatabase>().GetDataByKey(flowerId.ToString());
    }

    /// <summary>
    /// 设置花品精灵图片
    /// </summary>
    private void SetFlowerSprite(string spritePath)
    {
        flowerSprite.sprite = ResourceManager.Instance.GetSpriteResource(spritePath, ResouceType.Icon);
    }

    /// <summary>
    /// 显示蝴蝶特效
    /// </summary>
    //private void ShowButterfly()
    //{
    //    butterflyEffect.SetActive(true);
    //    skeAnim.AnimationState.SetAnimation(0, "animation", false).Complete += delegate
    //    {
    //        skeAnim.AnimationState.SetAnimation(0, "animation2", true);
    //    };
    //}

    /// <summary>
    /// 显示变异警告
    /// </summary>
    private void ShowVariationWarning(bool isVariation)
    {
        variationObj.SetActive(isVariation);
        slider.SetActive(!isVariation);
        //重置变异设施Id
        gardenModule.WaitVariationId = isVariation ? facilitiesData.facilitiesConfigData.id : 0;
        ChangeStatus(isVariation ? ParterreStatus.WaitVariation : ParterreStatus.Flowering);
    }

    private void WatchVideoSuccess(string arg1, string arg2, float arg3)
    {
        //变异成功展示变异花朵相关表现，并重置花朵数据
        SetFlowerData(flowerConfig.mutationID);
        gardenModule.MutationCDTime = flowerConfig.mutationCDTime;
        ShowVariationWarning(false);
        //花朵变异
        isVariation = true;
        ChangeStatus(ParterreStatus.Flowering);
    }

    private void WatchVideoFail(string arg1, string arg2) { }

    private void CloseVideo(string obj)
    {
        TDDebug.Log("关闭了激励视频!");
    }
}
