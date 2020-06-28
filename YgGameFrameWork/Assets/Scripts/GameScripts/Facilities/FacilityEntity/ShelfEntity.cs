using UnityEngine;
using Tool.Database;
using System;
using System.Collections;

/// <summary>
/// 货架状态
///// </summary>
public enum ShelfStatus
{
    /// <summary>
    /// 休闲
    /// </summary>
    Idle,
    /// <summary>
    /// 上货
    /// </summary>
    Load,
    /// <summary>
    /// 售货
    /// </summary>
    Sell,
    /// <summary>
    /// 等待上货状态
    /// </summary>
    Wait,
}

/// <summary>
/// 货架设施实体
/// </summary>
public class ShelfEntity : FacilityEntity
{
    private SpriteRenderer goodsSprite;
    private SpriteRenderer goodsProgressSprite;

    private StoreConfigData goodsData;
    private StoreModule storeModule;
    private float curTime;
    private ShelfData shelfData;
    private ShelfBubble shelfBubble;
    private GameObject bubble;
    private GameObject timer;

    public bool HaveGuest { set; get; }         //是否正在卖货

    public ShelfData ShelfData
    {
        get
        {
            if (shelfData == null)
            {
                if (!storeModule.ShelfDic.ContainsKey(facilitiesData.facilitiesConfigData.id))
                {
                    storeModule.AddGoodsData(facilitiesData.facilitiesConfigData.id, new ShelfData());
                }
                shelfData = storeModule.ShelfDic[facilitiesData.facilitiesConfigData.id];
                if (shelfData.shelfStatus != ShelfStatus.Idle && shelfData.shelfStatus != ShelfStatus.Wait)
                {
                    InitOfflineData();
                }
                else
                {
                    shelfData.shelfStatus = ShelfStatus.Idle;
                }
            }
            return shelfData;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        bubble = transform.Find("bubble").gameObject;
        timer = transform.Find("timer").gameObject;
        goodsSprite = transform.Find("goods").GetComponent<SpriteRenderer>();
        goodsProgressSprite = transform.Find("goodsProgress").GetComponent<SpriteRenderer>();
        storeModule = GameModuleManager.Instance.GetModule<StoreModule>();
        shelfBubble = bubble.GetOrAddComponent<ShelfBubble>();
        ShelfBubble.SetLoadingAction(OnDelivery);
        bubble.SetActive(true);
    }

    /// <summary>
    /// 初始化离线数据
    /// </summary>
    private void InitOfflineData()
    {
        bubble.SetActive(false);
        goodsData = ConfigDataManager.Instance.GetDatabase<StoreConfigDatabase>().GetDataByKey(ShelfData.goodsId.ToString());
        goodsSprite.sprite = ResourceManager.Instance.GetSpriteResource(goodsData.icon, ResouceType.Goods);
        if (ShelfData.shelfStatus == ShelfStatus.Sell) return;
        //显示加载进度时钟
        timer.SetActive(true);
        goodsProgressSprite.sprite = goodsSprite.sprite;
    }

    private void Update()
    {
        switch (ShelfData.shelfStatus)
        {
            case ShelfStatus.Load:
                OnLoading();
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Sales();
        }
    }

    /// <summary>
    /// 上货
    /// </summary>
    internal void OnDelivery(StoreConfigData goodsData)
    {
        this.goodsData = goodsData;
        ShelfData.goodsId = goodsData.Id;
        SetGoodsSprite(goodsData.icon);
        ShelfData.remainLoadTime = goodsData.loadTime * (1 - storeModule.LoadRate);
        ShelfData.remainSellTimes = goodsData.soldTimes + (int)facilityModule.FacilityAddValueCache[facilitiesData.facilitiesConfigData.id];
        ChangeStatus(ShelfStatus.Load);
        //显示加载进度时钟
        timer.SetActive(true);
        bubble.SetActive(false);
    }

    /// <summary>
    /// 上货进度
    /// </summary>
    private void OnLoading()
    {
        curTime += Time.deltaTime;
        if (curTime >= 1f)
        {
            curTime = 0;
            ShelfData.remainLoadTime -= 1f;
            goodsProgressSprite.material.SetFloat("_ProgressValue", ShelfData.remainLoadTime / goodsData.loadTime);
            if (ShelfData.remainLoadTime <= 0)
            {
                ChangeStatus(ShelfStatus.Sell);
                //显示加载进度时钟
                timer.SetActive(false);
            }
        }
    }

    /// <summary>s
    /// 售货
    /// </summary>
    internal void Sales()
    {
        if (ShelfData.remainSellTimes <= 0 || ShelfData.shelfStatus != ShelfStatus.Sell) return;
        ShelfData.remainSellTimes--;
        //售卖收入
        CreateDriedFish(CalculateDoublePurchaseRate());
        if (ShelfData.remainSellTimes <= 0)
        {
            //售卖次数消耗完
            goodsSprite.sprite = null;
            ChangeStatus(ShelfStatus.Wait);
            StartCoroutine(DelayShowBubble(3f));
        }
    }

    /// <summary>
    /// 延迟显示上货气泡按钮
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DelayShowBubble(float time)
    {
        yield return new WaitForSeconds(time);
        bubble.SetActive(true);
        ChangeStatus(ShelfStatus.Idle);
    }

    /// <summary>
    /// 计算加倍购买概率
    /// </summary>
    private int CalculateDoublePurchaseRate()
    {
        int income = goodsData.soldPrice;
        int rate = (int)(storeModule.DoublePurchaseChance * 100);
        int randomValue = UnityEngine.Random.Range(0, 101);
        //成功加倍售出
        if (randomValue <= rate)
        {
            income *= storeModule.Times;
        }
        return income;
    }

    /// <summary>
    /// 点击效果
    /// </summary>
    public override void ClickEffect()
    {
        base.ClickEffect();
        if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
        //休闲状态下才可以上货
        switch (shelfData.shelfStatus)
        {
            case ShelfStatus.Idle:
                //点击弹出货品手册
                UIManager.Instance.OpenUI<UI_Goods>();
                UI_Goods.SetLoadCallBack(OnDelivery);
                break;
            case ShelfStatus.Load:
                //商业化，通过观看视频立即上货TODO
                UIManager.Instance.OpenUI<UI_DeliveryAds>();
                UI_DeliveryAds.SetCallBack(WatchVideoSuccess, WatchVideoFail, CloseVideo);
                //WatchVideoSuccess("", "", .1f);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 设置当前货物精灵图片
    /// </summary>
    /// <param name="spriteName"></param>
    private void SetGoodsSprite(string spriteName)
    {
        Sprite goodsIcon = ResourceManager.Instance.GetSpriteResource(spriteName, ResouceType.Goods);
        goodsSprite.sprite = goodsIcon;
        goodsProgressSprite.sprite = goodsIcon;
    }

    /// <summary>
    /// 状态切换
    /// </summary>
    private void ChangeStatus(ShelfStatus status)
    {
        ShelfData.shelfStatus = status;
    }


    private void WatchVideoSuccess(string arg1, string arg2, float arg3)
    {
        ShelfData.remainLoadTime = 0;
        goodsProgressSprite.material.SetFloat("_ProgressValue", ShelfData.remainLoadTime / goodsData.loadTime);
        timer.SetActive(false);
        ChangeStatus(ShelfStatus.Sell);
    }

    private void WatchVideoFail(string arg1, string arg2) { }

    private void CloseVideo(string obj)
    {
        TDDebug.Log("关闭了激励视频!");
    }
}
