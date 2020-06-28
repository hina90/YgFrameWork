using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Tool.Database;
using TMPro;

public class UI_TipsControl : UIBase
{
    private PlayerModule playerModule;
    private MenuModule menuModule;
    private GardenModule gardenModule;

    #region 公共组件
    private Button bgReturn;
    private Canvas canvas;
    private Image icon;
    private Text tipName;
    private Text info;
    private Transform tipsContent;
    private Transform myTips;
    private RectTransform tipsRect;
    #endregion
    public override void Init()
    {
        Layer = LayerMenue.TIPS;
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();

        canvas = GetComponent<Canvas>();
        bgReturn = Find<Button>(gameObject, "bg");
        icon = Find<Image>(gameObject, "Icon");
        tipName = Find<Text>(gameObject, "Name");
        info = Find<Text>(gameObject, "Info");
        myTips = Find(gameObject, "Tips").transform;
        tipsContent = Find(gameObject, "Content").transform;
        tipsRect = myTips.GetComponent<RectTransform>();

        PlayAnimation(myTips.gameObject);
    }

    protected override void Enter()
    {
        bgReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
    }
    /// <summary>
    /// 显示设施信息的提示框
    /// </summary>
    public void ShowFacilitiesTips(FacilitiesItemData facData)
    {
        //设置层级和提示框高度和位置
        canvas.sortingOrder = 2;
        tipsRect.sizeDelta = new Vector2(531, 600);
        //设置基础信息
        icon.sprite = ResourceManager.Instance.GetSpriteResource(facData.itemConfigData.uiIcon, ResouceType.Facility);

        tipName.text = facData.itemConfigData.name;
        info.text = facData.itemConfigData.des;
        //显示需要的组件
        Button purchaseBtn = Find<Button>(gameObject, "FacPurchaseBtn");
        Button useBtn = Find<Button>(gameObject, "FacUseBtn");
        List<string> conditions = new List<string>();

        #region 添加功能和条件
        bool purchaseBtnState = true;
        string difference = "";

        if (playerModule.Fish < facData.itemConfigData.price)
        {
            purchaseBtnState = false;
            difference = $"小鱼干不足，还差{facData.itemConfigData.price - playerModule.Fish}小鱼干";
        }
        if (playerModule.Star < facData.itemConfigData.unlockHeart)
        {
            purchaseBtnState = false;
            difference = "星级不足";
        }
        if (facData.itemConfigData.unlockHeart > 0)
            conditions.Add($"<sprite=0>需要达到{facData.itemConfigData.unlockHeart}<sprite=5>");

        for (int i = 0; i < facData.itemConfigData.funType.Length; i++)
        {
            float[] args = (float[])typeof(ItemConfigData).GetField("funParam_" + (i + 1)).GetValue(facData.itemConfigData);
            switch (facData.itemConfigData.funType[i])
            {
                case 1:
                    conditions.Add($"<sprite=1>增加<sprite=5>{args[0]}点");
                    break;
                case 2:
                    conditions.Add($"<sprite=1>增加小费收入{args[0]}<sprite=7>/分钟");
                    break;
                case 3:
                    conditions.Add($"<sprite=1>增加小费收入上限到{args[0]}点");
                    break;
                case 4:
                    conditions.Add($"<sprite=1>客人每次消费后获得{args[0]}<sprite=7>");
                    break;
                case 5:
                    conditions.Add($"<sprite=1>提高做菜效率{args[0] * 100}%");
                    break;
                case 6:
                    conditions.Add($"<sprite=1>每隔{args[0]}秒额外获得<sprite=7>{args[1]}");
                    break;
                case 7:
                    conditions.Add($"<sprite=1>免费许愿次数+{args[0]}");
                    break;
                case 8:
                    conditions.Add($"<sprite=1>奖励物品掉落个数+{args[0]}");
                    break;
                case 9:
                    conditions.Add($"<sprite=1>可播种{gardenModule.GardenConfig((int)args[0]).name}种子");
                    break;
                case 10:
                    conditions.Add($"<sprite=1>可提供客人观赏+{args[0]}，顾客观赏后可额外获得{args[1]}<sprite=7>");
                    break;
                case 11:
                    conditions.Add($"<sprite=1>提高植物的养成效率{args[0] * 100}%");
                    break;
                case 12:
                    conditions.Add($"<sprite=1>提高上货效率{args[0] * 100}%");
                    break;
                case 13:
                    conditions.Add($"<sprite=1>提高顾客{args[0]}倍购买的概率{args[1] * 100}%");
                    break;
                case 14:
                    conditions.Add($"<sprite=1>提高货物可售卖次数{args[0]}");
                    break;
            }
        }
        for (int i = 0; i < conditions.Count; i++)
        {
            GameObject condition = ResourceManager.Instance.GetResourceInstantiate("Condition", tipsContent, ResouceType.PrefabItem);
            condition.GetComponent<TextMeshProUGUI>().text = conditions[i];
        }
        #endregion

        //判断按钮状态
        if (!facData.storeData.isPurchase)//未购买时会呈现可购买状态和不可购买状态
        {
            //设置按钮图标的形态
            purchaseBtn.gameObject.SetActive(true);
            purchaseBtn.transform.Find("StudyNum").GetComponent<Text>().text = facData.itemConfigData.price.ToString();
            if (!purchaseBtnState)//条件不足无法购买
            {
                purchaseBtn.interactable = false;
                purchaseBtn.transform.Find("DifferenceTxt").gameObject.SetActive(true);
                purchaseBtn.transform.Find("DifferenceTxt").GetComponent<Text>().text = difference;
            }
            else//可以购买
            {
                purchaseBtn.interactable = true;
                purchaseBtn.onClick.AddListener(() =>
                {
                    AudioManager.Instance.PlayUIAudio("button_1");
                    FacilitiesManager.Instance.PurchaseFacility(facData);
                    facData.storeData.isPurchase = true;
                    menuModule.SetUseFacilityItem(facData.facilitiesConfigData.id, facData.itemId);//使用该设施
                    UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, -facData.itemConfigData.price);
                    //退出所有三级二级页面
                    UIManager.Instance.BackUI(Layer);
                    //UIManager.Instance.BackUI(LayerMenue.UI);
                    UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FACILITIES);
                    TipManager.Instance.ShowMsg("购买设施道具：" + facData.itemConfigData.name);
                    TaskManager.Instance.CheckTask(TaskType.UNLOCK_FACILITIES, 1);
                    GameManager.Instance.CheckUnlockActor();
                    SDKManager.Instance.LogEvent(EventId.CC_Buy.ToString(), "PurchaseFac", "Button");
                });
            }
            GuideManager.Instance.RegisterBtn(purchaseBtn);
        }
        else//已购买时会呈现使用和使用中两种状态
        {
            useBtn.gameObject.SetActive(true);
            if (menuModule.CheckIsUseFacilityItem(facData.facilitiesConfigData.id, facData.itemId))//当前设施是使用中
            {
                useBtn.interactable = false;
            }
            else//当前设施已购买但未使用
            {
                useBtn.interactable = true;
                useBtn.onClick.AddListener(() =>
                {
                    AudioManager.Instance.PlayUIAudio("button_1");
                    FacilitiesManager.Instance.BuildFacilities(facData);
                    menuModule.SetUseFacilityItem(facData.facilitiesConfigData.id, facData.itemId);
                    //退出所有三级二级页面
                    UIManager.Instance.BackUI(Layer);
                    //UIManager.Instance.BackUI(LayerMenue.UI);
                    UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FACILITIES);
                    TipManager.Instance.ShowMsg("使用设施道具：" + facData.itemConfigData.name);
                });
            }
        }
    }
    /// <summary>
    /// 显示食物信息的提示框
    /// </summary>
    public void ShowFoodTips(MenuData foodData)
    {
        canvas.sortingOrder = 2;

        icon.sprite = ResourceManager.Instance.GetSpriteResource(foodData.configData.icon, ResouceType.Icon);
        icon.SetNativeSize();
        tipName.text = LanguageManager.Get(foodData.configData.name);
        info.text = foodData.configData.des.ToString();

        Button studyBtn = Find<Button>(gameObject, "FoodStudyBtn");
        Button upPrice = Find<Button>(gameObject, "FoodUpPriceBtn");
        List<string> conditions = new List<string>();

        #region 设置功能条件
        bool studyBtnState = true;
        string difference = "";

        if (playerModule.Fish < foodData.configData.price)
        {
            studyBtnState = false;
            difference = $"小鱼干不足，还差{foodData.configData.price - playerModule.Fish}小鱼干";
        }
        for (int i = 0; i < foodData.configData.UnlockCondition.Length; i++)
        {
            switch (foodData.configData.UnlockCondition[i])
            {
                case 1:
                    if (playerModule.Star < foodData.configData.unluckValue[i])
                    {
                        studyBtnState = false;
                        difference = "星级不足";
                    }
                    if (foodData.configData.unluckValue[i] > 0)
                        conditions.Add($"<sprite=0>需要达到{foodData.configData.unluckValue[i]}<sprite=5>");
                    break;
            }
        }

        conditions.Add($"<sprite=1>制作一份需要{foodData.configData.makeTime}s");
        conditions.Add($"<sprite=1>每卖出一份能收入<sprite=7>+{foodData.configData.soldPrice}");
        for (int i = 0; i < conditions.Count; i++)
        {
            GameObject condition = ResourceManager.Instance.GetResourceInstantiate("Condition", tipsContent, ResouceType.PrefabItem);
            condition.GetComponent<TextMeshProUGUI>().text = conditions[i];
        }
        #endregion

        //提示框按钮状态0
        if (!foodData.storeData.isStudy)//未学习
        {
            studyBtn.gameObject.SetActive(true);
            studyBtn.transform.Find("StudyNum").GetComponent<Text>().text = foodData.configData.price.ToString();
            if (!studyBtnState)//学习条件不足时
            {
                studyBtn.interactable = false;
                studyBtn.transform.Find("DifferenceTxt").gameObject.SetActive(true);
                studyBtn.transform.Find("DifferenceTxt").GetComponent<Text>().text = difference;
            }
            else//条件满足时
            {
                studyBtn.interactable = true;
                if (foodData.configData.price == 0)
                {
                    studyBtn.transform.Find("StudyNum").gameObject.SetActive(false);
                    studyBtn.transform.Find("PriceIcon").gameObject.SetActive(false);
                    studyBtn.transform.Find("StudyImg").gameObject.SetActive(false);
                    studyBtn.transform.Find("Free").gameObject.SetActive(true);
                }
                studyBtn.onClick.AddListener(() =>
                {
                    AudioManager.Instance.PlayUIAudio("button_1");
                    foodData.storeData.isStudy = true;
                    foodData.storeData.isUnluck = true;
                    UIManager.Instance.BackUI(Layer);
                    //UIManager.Instance.BackUI(LayerMenue.UI);
                    UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FOOD);
                    UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, -foodData.configData.price);
                    TipManager.Instance.ShowMsg("学习菜谱：" + foodData.configData.name);
                    menuModule.SaveMenuData();
                    TaskManager.Instance.CheckTask(TaskType.UNLOCK_COOK, 1);
                    GameManager.Instance.CheckUnlockActor();
                    SDKManager.Instance.LogEvent(EventId.CC_Buy.ToString(), "StudyFood", "Button");
                    TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.LearnMenu, foodData.configData.Id);
                });
            }
        }
    }
    /// <summary>
    /// 显示顾客信息提示框
    /// </summary>
    public void ShowCustomerTips(CustomerData coustomerData)
    {
        canvas.sortingOrder = 5;
        tipsRect.sizeDelta = new Vector2(531, 600);
        if (!coustomerData.storeData.isActive)//是否解锁
        {
            icon.sprite = ResourceManager.Instance.GetSpriteResource("wz", ResouceType.Icon);
            icon.SetNativeSize();
            tipName.text = "???";
            info.text = "???";
        }
        else
        {
            icon.sprite = ResourceManager.Instance.GetSpriteResource(coustomerData.customerConfig.icon, ResouceType.Icon);
            icon.SetNativeSize();
            tipName.text = coustomerData.customerConfig.name;
            info.text = coustomerData.customerConfig.introduction;
        }

        #region 设置条件
        List<string> conditions = new List<string>();
        string names = "";

        if (coustomerData.customerConfig.levelCondition != 0)
            conditions.Add($"<sprite=0>挑剔：评价必须达到<sprite=5>{coustomerData.customerConfig.levelCondition}");

        for (int i = 0; i < coustomerData.customerConfig.visitNeedFacilities.Length; i++)//来访需要设施
        {
            names = "";
            if (coustomerData.customerConfig.visitNeedFacilities[i] != 0)
            {
                names += ConfigDataManager.Instance.GetDatabase<ItemConfigDatabase>().GetDataByKey(coustomerData.customerConfig.visitNeedFacilities[i].ToString()).name + ",";
            }
        }
        if (names != "")
        {
            names = DelComma(names);
            conditions.Add($"<sprite=1>来访需要设施：{names}");
        }

        for (int i = 0; i < coustomerData.customerConfig.visitNeedFoods.Length; i++)//来访需要食物
        {
            names = "";
            MenuData foodData = menuModule.GetMenuData(coustomerData.customerConfig.visitNeedFoods[i]);
            if (foodData.configData != null)
                names += foodData.configData.name + ",";
        }
        if (names != "")
        {
            names = DelComma(names);
            conditions.Add($"<sprite=1>来访需要的食物：{names}");
        }

        for (int i = 0; i < coustomerData.customerConfig.visitNeedFlower.Length; i++)//来访需要花朵
        {
            names = "";
            if (coustomerData.customerConfig.visitNeedFlower[i] != 0)
            {
                names += ConfigDataManager.Instance.GetDatabase<ItemConfigDatabase>().GetDataByKey(coustomerData.customerConfig.visitNeedFlower[i].ToString()).name + ",";
            }
        }
        if (names != "")
        {
            names = DelComma(names);
            conditions.Add($"<sprite=1>来访需要的花：{names}");
        }

        //来访的宣传条件
        switch (coustomerData.customerConfig.promotionalConditions)
        {
            case 0:
                conditions.Add($"<sprite=1>解锁后随时都会出现的顾客");
                break;
            case 1:
                conditions.Add($"<sprite=1>需要手动点击宣传出现的顾客");
                break;
            case 2:
                conditions.Add($"<sprite=1>需要观看视频才会出现的顾客");
                break;
        }

        //技能条件显示
        if (coustomerData.customerConfig.type[0] != 0)
        {
            switch (coustomerData.customerConfig.type[0])
            {
                case 1:
                    conditions.Add($"<sprite=1>土豪：每次付费，都会有{coustomerData.customerConfig.type[1]}%的概率给予{coustomerData.customerConfig.type[2]}倍<sprite=7>");
                    break;
                case 2:
                    conditions.Add($"<sprite=1>大胃王：会连续吃{coustomerData.customerConfig.type[1]}道菜，受大胃王鼓舞，厨房做菜的速度增加{coustomerData.customerConfig.type[2]}%");
                    break;
            }
        }

        //特殊顾客条件显示
        switch (coustomerData.customerConfig.Id)
        {
            case 1201:
                conditions.Add($"<sprite=0>会偷取餐厅或厨房地上的小鱼干，多次点击可将他驱赶离开");
                break;
            case 1202:
                conditions.Add($"<sprite=0>会在餐厅或厨房地上乱扔垃圾，多次点击可将他驱赶离开");
                break;
            case 1203:
                conditions.Add($"<sprite=0>会唱出优美的歌声来吸引招揽众多顾客，多次点击可邀请她献歌一首");
                break;
            case 1204:
                conditions.Add($"<sprite=0>会散发臭臭的气体将排队的顾客熏跑，多次点击可将他驱赶离开");
                break;
            case 1205:
                conditions.Add($"<sprite=0>在规定时间内点击他会扔出许多小鱼干");
                break;
            case 1206:
                conditions.Add($"<sprite=0>可以选择观看他所提供的广告，观看完成后会获得一笔价格不菲的小鱼干");
                break;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            GameObject condition = ResourceManager.Instance.GetResourceInstantiate("Condition", tipsContent, ResouceType.PrefabItem);
            condition.GetComponent<TextMeshProUGUI>().text = conditions[i];
        }
        #endregion
    }

    internal void ShowGoodsTips(StoreConfigData goodsData, Callback<StoreConfigData> loadCallBack)
    {
        canvas.sortingOrder = 2;
        tipsRect.sizeDelta = new Vector2(531, 665);

        icon.sprite = ResourceManager.Instance.GetSpriteResource(goodsData.icon, ResouceType.Goods);
        icon.SetNativeSize();
        tipName.text = goodsData.name;
        info.text = goodsData.des;

        List<string> conditions = new List<string>();
        Button loadBtn = Find<Button>(gameObject, "GoodsLoadBtn");

        #region 设置功能条件
        bool btnState = true;
        string difference = "";

        if (playerModule.Star < goodsData.needStar)
        {
            btnState = false;
            difference = "星级不足";
        }
        if (goodsData.needStar > 0)
        {
            conditions.Add($"<sprite=0>需要达到{goodsData.needStar}<sprite=5>");
        }
        if (playerModule.Fish < goodsData.costFish)
        {
            btnState = false;
            difference = $"小鱼干不足，还差{goodsData.costFish - playerModule.Fish}小鱼干";
        }
        if (goodsData.costFish > 0)
        {
            conditions.Add($"<sprite=0>需要{goodsData.costFish}<sprite=7>");
        }
        for (int i = 0; i < conditions.Count; i++)
        {
            GameObject condition = ResourceManager.Instance.GetResourceInstantiate("Condition", tipsContent, ResouceType.PrefabItem);
            condition.GetComponent<TextMeshProUGUI>().text = conditions[i];
        }
        #endregion

        //提示框按钮状态
        loadBtn.transform.Find("PriceNum").GetComponent<Text>().text = goodsData.costFish.ToString();
        if (!string.IsNullOrEmpty(difference))
        {
            Text tip = loadBtn.transform.Find("DifferenceTxt").GetComponent<Text>();
            tip.gameObject.SetActive(true);
            tip.text = difference;
        }
        loadBtn.gameObject.SetActive(true);
        loadBtn.interactable = btnState;
        if (btnState)
        {
            loadBtn.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayUIAudio("button_1");
                //便利店上货
                loadCallBack?.Invoke(goodsData);
                //关闭2、3级UI界面
                UIManager.Instance.BackUI(Layer);
                UIManager.Instance.BackUI(LayerMenue.UI);
                UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, -goodsData.costFish);
            });
        }
    }

    private string DelComma(string str)
    {
        char[] chars = str.ToCharArray();
        chars[chars.Length - 1] = ' ';
        return new string(chars);
    }
}