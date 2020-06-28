using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SG;

public class UI_Menu : UIBase
{
    #region  组件
    private Button exitBtn;
    private Button bgReturn;
    private Toggle facilitiesBtn;
    private Toggle cookbookBtn;
    private Scrollbar facScrollbar;
    private Scrollbar cookScrollbar;
    private GameObject[] pans = new GameObject[2];
    private GameObject facToggleGroup;
    private LoopVerticalScrollRect facContent;
    private InitOnStart facCount;
    private LoopVerticalScrollRect cookContent;
    private InitOnStart cookCount;
    private MenuModule menuModule;

    #region 设施分类相关
    private Toggle restaurantTog;
    private Toggle kitchenTog;
    private Toggle gardenTog;
    private Toggle storeTog;
    #endregion
    #endregion

    public override void Init()
    {
        Layer = LayerMenue.UI;
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();

        //设置设施的页面数据
        facContent = Find<LoopVerticalScrollRect>(gameObject, "FacilitiesPan");
        facCount = Find<InitOnStart>(gameObject, "FacilitiesPan");
        facCount.totalCount = menuModule.GetFacilitiesListByType(FacilitiesType.Restaurant).Count;
        facContent.objectsToFill = menuModule.GetFacilitiesListByType(FacilitiesType.Restaurant).ToArray();

        //菜谱的页面数据
        cookContent = Find<LoopVerticalScrollRect>(gameObject, "CookbookPan");
        cookCount = Find<InitOnStart>(gameObject, "CookbookPan");
        cookCount.totalCount = menuModule.GetMenuListByType(MenuType.Make).Count;
        cookContent.objectsToFill = menuModule.GetMenuListByType(MenuType.Make).ToArray();
    }
    private void SetToggleIson()
    {
        TDDebug.DebugLog("====================================>:" + Global.FACILITIESTYPE);
        switch (Global.FACILITIESTYPE)
        {
            case FacilitiesType.Kitchen:
                kitchenTog.isOn = true;
                break;
            case FacilitiesType.Garden:
                gardenTog.isOn = true;
                break;
            case FacilitiesType.Store:
                storeTog.isOn = true;
                break;
        }
    }
    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        Dictionary<GameEvent, Callback<object[]>> eventDic = new Dictionary<GameEvent, Callback<object[]>>
        {
            [GameEvent.UPDATE_FOOD] = delegate (object[] param)
            {
                cookCount.totalCount = menuModule.GetMenuListByType(MenuType.Make).Count;
                cookContent.objectsToFill = menuModule.GetMenuListByType(MenuType.Make).ToArray();
                cookContent.RefreshCells();
            },
            [GameEvent.UPDATE_FACILITIES] = delegate (object[] param)
            {
                facContent.RefreshCells();
            },
            [GameEvent.SET_MENU_SCROLL] = delegate (object[] param)
            {
                facContent.enabled = (bool)param[0];
            },
            [GameEvent.TOGGLE_KITCHEN] = delegate (object[] param)
            {
                facContent.RefillCells(14);
                kitchenTog.isOn = true;
            }

        };
        return eventDic;
    }
    protected override void Enter()
    {
        base.Enter();
        bgReturn = Find<Button>(gameObject, "bg");
        exitBtn = Find<Button>(gameObject, "ExitBtn");
        facScrollbar = Find<Scrollbar>(gameObject, "Facilitiesbar");
        cookScrollbar = Find<Scrollbar>(gameObject, "Cookbookbar");
        facToggleGroup = Find(gameObject, "FacClassify");
        facilitiesBtn = Find<Toggle>(gameObject, "FacilitiesBtn");
        cookbookBtn = Find<Toggle>(gameObject, "CookbookBtn");

        restaurantTog = Find<Toggle>(gameObject, "RestaurantTog");
        kitchenTog = Find<Toggle>(gameObject, "KitchenTog");
        gardenTog = Find<Toggle>(gameObject, "GardenTog");
        storeTog = Find<Toggle>(gameObject, "StoreTog");

        pans = new GameObject[]
        {
            Find(gameObject, "FacilitiesBg"),
            Find(gameObject, "CookbookBg"),
        };

        ListenerEvent();
        facilitiesBtn.isOn = Global.ISFACILITIES;
        cookbookBtn.isOn = !facilitiesBtn.isOn;
        PlayAnimation(Find(gameObject, "Scale"));
        StartCoroutine(Enumerator());
        GuideManager.Instance.RegisterBtn(exitBtn);
        //GuideManager.Instance.EnterWeakStep(1104);
    }
    private IEnumerator Enumerator()
    {
        yield return 1;
        if (facilitiesBtn.isOn)
        {
            SetToggleIson();
        }
        else
            cookScrollbar.value = (float)Global.SCROLLBAR_VALUE;

        ///弱引导默认定位餐厅设施
        //if (GuideManager.Instance.isGuideBuyTable)
        //{
        //    restaurantTog.isOn = true;
        //    facContent.RefillCells(2);
        //    GuideManager.Instance.isGuideBuyTable = false;
        //    //GuideManager.Instance.EnterWeakStep(1102);
        //}
        //if (!GuideManager.Instance.IsFinishGuide())
        //{
        //    restaurantTog.isOn = true;
        //}

        //线性引导任务
        if (param.Length > 0)
        {
            AimTaskData aimTaskData = (AimTaskData)param[0];
            int areaType = aimTaskData.taskConfig.uiType;
            facilitiesBtn.isOn = true;
            switch (areaType)
            {
                case 1:
                    restaurantTog.isOn = true;
                    break;
                case 2:
                    kitchenTog.isOn = true;
                    break;
                case 3:
                    gardenTog.isOn = true;
                    break;
                case 4:
                    storeTog.isOn = true;
                    break;
                case 5:
                    cookbookBtn.isOn = true;
                    break;
                default:
                    break;
            }
        }
    }
    private void ListenerEvent()
    {
        exitBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.BackUI(Layer);
            GuideManager.Instance.FinishWeakGuide();
        });
        bgReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
            GuideManager.Instance.FinishWeakGuide();
        });
        facilitiesBtn.onValueChanged.AddListener((bool fyx) =>
        {
            if (!Global.ISFACILITIES) AudioManager.Instance.PlayUIAudio("button_1");
            pans[0].SetActive(true);
            pans[1].SetActive(false);
            Global.ISFACILITIES = true;
            facToggleGroup.SetActive(true);
            SetToggleIson();
        });
        cookbookBtn.onValueChanged.AddListener((bool fyx) =>
        {
            GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_WEAKGUIDE);
            if (Global.ISFACILITIES) AudioManager.Instance.PlayUIAudio("button_1");
            pans[0].SetActive(false);
            pans[1].SetActive(true);
            Global.ISFACILITIES = false;
            facToggleGroup.SetActive(false);
            //GuideManager.Instance.EnterWeakStep(1105);
        });
        restaurantTog.onValueChanged.AddListener((bool isOn) =>
        {
            //if (!isOn) return;
            AudioManager.Instance.PlayUIAudio("button_1");
            facCount.totalCount = menuModule.GetFacilitiesListByType(FacilitiesType.Restaurant).Count;
            facContent.objectsToFill = menuModule.GetFacilitiesListByType(FacilitiesType.Restaurant).ToArray();
            facCount.Start();
            Global.FACILITIESTYPE = FacilitiesType.Restaurant;
        });
        kitchenTog.onValueChanged.AddListener((bool isOn) =>
        {
            if (!isOn) return;
            AudioManager.Instance.PlayUIAudio("button_1");
            facCount.totalCount = menuModule.GetFacilitiesListByType(FacilitiesType.Kitchen).Count;
            facContent.objectsToFill = menuModule.GetFacilitiesListByType(FacilitiesType.Kitchen).ToArray();
            facCount.Start();
            Global.FACILITIESTYPE = FacilitiesType.Kitchen;
        });
        gardenTog.onValueChanged.AddListener((bool isOn) =>
        {
            if (!isOn) return;
            AudioManager.Instance.PlayUIAudio("button_1");
            facCount.totalCount = menuModule.GetFacilitiesListByType(FacilitiesType.Garden).Count;
            facContent.objectsToFill = menuModule.GetFacilitiesListByType(FacilitiesType.Garden).ToArray();
            facCount.Start();
            Global.FACILITIESTYPE = FacilitiesType.Garden;
        });
        storeTog.onValueChanged.AddListener((bool isOn) =>
        {
            if (!isOn) return;
            AudioManager.Instance.PlayUIAudio("button_1");
            facCount.totalCount = menuModule.GetFacilitiesListByType(FacilitiesType.Store).Count;
            facContent.objectsToFill = menuModule.GetFacilitiesListByType(FacilitiesType.Store).ToArray();
            facCount.Start();
            Global.FACILITIESTYPE = FacilitiesType.Store;
        });
    }
    public override void Release()
    {
        if (facilitiesBtn.isOn) Global.SCROLLBAR_VALUE = Math.Round(facScrollbar.value, 2);
        else Global.SCROLLBAR_VALUE = Math.Round(cookScrollbar.value, 2);
        CachePoolManager.Instance.DestroyPool("FacilitiesPref");
        CachePoolManager.Instance.DestroyPool("FoodPref");
        base.Release();
    }
}
