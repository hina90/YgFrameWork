using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 餐厅客人引导
/// </summary>
public class UI_GuestProfile : UIBase
{
    private CustomerModule customerModule;
    private MenuModule menuModule;

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
        base.Init();
        Layer = LayerMenue.TIPS;
        customerModule = GameModuleManager.Instance.GetModule<CustomerModule>();
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();

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
        base.Enter();
        bgReturn.onClick.AddListener(() =>
        {
            StartCoroutine(DelayCloseUI(0));
        });
        ShowCustomerTips((CustomerData)param[0]);
        StartCoroutine(DelayCloseUI(5));
    }

    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        return base.CtorEvent();
    }

    public override void Release()
    {
        base.Release();
    }

    /// <summary>
    /// 延迟关闭UI
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayCloseUI(float time)
    {
        yield return new WaitForSeconds(time);
        UIManager.Instance.BackUI(Layer);
        GameManager.Instance.ShowGuestProfile();
    }

    /// <summary>
    /// 显示顾客信息提示框
    /// </summary>
    public void ShowCustomerTips(CustomerData coustomerData)
    {
        canvas.sortingOrder = 5;
        tipsRect.sizeDelta = new Vector2(531, 600);
        //if (!coustomerData.storeData.isActive)//是否解锁
        //{
        //    icon.sprite = ResourceManager.Instance.GetSpriteResource("wz", ResouceType.Icon);
        //    icon.SetNativeSize();
        //    tipName.text = "???";
        //    info.text = "???";
        //}
        //else
        //{
        icon.sprite = ResourceManager.Instance.GetSpriteResource(coustomerData.customerConfig.icon, ResouceType.Icon);
        icon.SetNativeSize();
        tipName.text = coustomerData.customerConfig.name;
        info.text = coustomerData.customerConfig.introduction;
        //}

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
                conditions.Add($"<sprite=0>浣熊大盗出现了，他会偷取餐厅或厨房地上的小鱼干，多次点击该顾客可将他驱赶离开");
                break;
            case 1202:
                conditions.Add($"<sprite=0>破烂大王出现了，他会在餐厅或厨房地上乱扔垃圾，多次点击该顾客可将他驱赶离开");
                break;
            case 1203:
                conditions.Add($"<sprite=0>歌王栗栗出现了，他会唱出优美的歌声来招揽众多顾客，多次点击该顾客可邀请她献歌一首");
                break;
            case 1204:
                conditions.Add($"<sprite=0>臭臭小满出现了，他会散发臭臭的气体将排队的顾客熏跑，多次点击该顾客可将他驱赶离开");
                break;
            case 1205:
                conditions.Add($"<sprite=0>石油国王出现了，规定时间内连续点击该顾客会扔出丰厚的小鱼干");
                break;
            case 1206:
                conditions.Add($"<sprite=0>王牌销售员出现了，点击该顾客后可选择观看广告，观看完成可获得一笔价格不菲的小鱼干");
                break;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            GameObject condition = ResourceManager.Instance.GetResourceInstantiate("Condition", tipsContent, ResouceType.PrefabItem);
            condition.GetComponent<TextMeshProUGUI>().text = conditions[i];
        }
        #endregion
    }

    private string DelComma(string str)
    {
        char[] chars = str.ToCharArray();
        chars[chars.Length - 1] = ' ';
        return new string(chars);
    }
}
