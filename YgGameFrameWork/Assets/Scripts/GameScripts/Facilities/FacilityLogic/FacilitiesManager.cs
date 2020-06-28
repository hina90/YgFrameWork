using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 设施管理器
/// </summary>
public class FacilitiesManager : UnitySingleton<FacilitiesManager>
{
    /// <summary>
    /// 设施建筑缓存
    /// </summary>
    private Dictionary<int, FacilitiesItemData> facilitiesBuidingDic;
    /// <summary>
    /// 炉子设施
    /// </summary>
    private List<StoveEntity> stoveCookingList;
    /// <summary>
    /// 盆栽设施
    /// </summary>
    private List<ParterreEntity> parterreList;
    /// <summary>
    /// 货架设施
    /// </summary>
    private List<ShelfEntity> shelfList;
    /// <summary>
    /// 待制作菜单队列
    /// </summary>
    private Queue<CookData> waitCookingQueue;

    private MenuModule menuModule;
    private SpriteRenderer doorEdge;      //门沿背景
    private readonly int[,] defaultFacility = new int[,]
    { { 10005, 1029 }, { 10006, 1033 }, { 10013, 1049 }, { 10014, 1053 }, { 10024, 1093 }, { 10025, 1097 }, { 10027, 1105 }, { 10028, 1109 }, };

    /// <summary>
    /// 小鱼干缓存列表
    /// </summary>
    private Dictionary<int, FishItem> fishDic;

    public Dictionary<int, FacilitiesItemData> FacilitiesBuidingDic
    {
        get { return facilitiesBuidingDic; }
    }

    protected override void Awake()
    {
        base.Awake();
        doorEdge = GameObject.Find("PathMap/Restaurant/BG/DoorDecoration").GetComponent<SpriteRenderer>();
    }

    public FacilitiesManager()
    {
        facilitiesBuidingDic = new Dictionary<int, FacilitiesItemData>();
        stoveCookingList = new List<StoveEntity>();
        parterreList = new List<ParterreEntity>();
        shelfList = new List<ShelfEntity>();
        waitCookingQueue = new Queue<CookData>();
        fishDic = new Dictionary<int, FishItem>();
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();
    }

    /// <summary>
    /// 初始化所有建筑
    /// </summary>
    public void InitAllBuilding()
    {
        List<FacilitiesItemData> facItemList = menuModule.GetAllUseFacilityItem();
        if (facItemList.Count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                FacilitiesItemData facData = menuModule.GetFacilitiesItemData(defaultFacility[i, 0], defaultFacility[i, 1]);
                PurchaseFacility(facData, true);
                facData.storeData.isPurchase = true;
                menuModule.SetUseFacilityItem(facData.facilitiesConfigData.id, facData.itemId);//使用该设施
            }
        }
        facItemList.ForEach((facilitiesData) =>
        {
            BuildFacilities(facilitiesData);
        });
    }

    private float timeHit = 0f;         //用于点击的时间间隔,每次点击时间间隔应大于0.2秒  

    private void Update()
    {
        if (waitCookingQueue.Count > 0)
        {
            StoveEntity stoveEntity = stoveCookingList.Find(o => o.IsCooking == false);
            if (stoveEntity != null)
            {
                Cooking(waitCookingQueue.Dequeue(), stoveEntity);
            }
        }

        #region 手动点击收集小鱼干逻辑

        timeHit += Time.deltaTime;
        if (timeHit > 0.2f)
        {
            //拾取小鱼干行为
            if (Input.GetMouseButtonDown(0))
            {
                timeHit = 0f;
                //TDDebug.Log("当前点击是否在UI上:" + Utils.CheckIsClickOnUI() + "----------是否远景:" + CameraMove.isProspect);
                if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
                if (hits.Length <= 0) return;
                for (int i = 0; i < hits.Length; i++)
                {
                    //if (hits[i].collider.tag == "fish" || hits[i].collider.tag == "heart")
                    //{
                    //    //Debug.Log("点击到了小鱼干");
                    //    hits[i].collider.GetComponent<RewardItem>().ClickEffect();
                    //    break;
                    //}
                    if (hits[i].collider.tag == "wishPool")
                    {
                        hits[i].collider.GetComponent<WishingPoolEntity>().ClickEffect();
                    }
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 购买设施
    /// </summary>
    /// <param name="facilitiesData"></param>
    public void PurchaseFacility(FacilitiesItemData facilitiesData, bool defaultFacility = false)
    {
        facilitiesData = BuildFacilities(facilitiesData);
        Facilities facilities = null;
        for (int i = 0; i < facilitiesData.itemConfigData.funType.Length; i++)
        {
            float[] args = (float[])typeof(ItemConfigData).GetField("funParam_" + (i + 1)).GetValue(facilitiesData.itemConfigData);

            int type = facilitiesData.itemConfigData.funType[i];
            switch (facilitiesData.itemConfigData.funType[i])
            {
                case 1:
                    if (defaultFacility) continue;
                    facilities = new FacIncreaseStar(facilitiesData.facilitiesConfigData.id);
                    break;
                case 2:
                    facilities = new FacIncreaseIncomePerminute(facilitiesData.facilitiesConfigData.id);
                    break;
                case 3:
                    facilities = new FacIncreaseToplimit(facilitiesData.facilitiesConfigData.id);
                    break;
                case 4:
                    facilities = new FacIncomeAfterConsume(facilitiesData.facilitiesConfigData.id);
                    break;
                case 5:
                    facilities = new FacIncreaseCookRatio(facilitiesData.facilitiesConfigData.id);
                    break;
                case 6:
                    facilities = new FacIncreaseIncomeFixedtime(facilitiesData.facilitiesConfigData.id);
                    break;
                case 7:
                    facilities = new FacFreeWish(facilitiesData.facilitiesConfigData.id);
                    break;
                case 8:
                    facilities = new FacRewardDrop(facilitiesData.facilitiesConfigData.id);
                    break;
                case 10:
                    facilities = new FacVisitIncome(facilitiesData.facilitiesConfigData.id);
                    break;
                case 11:
                    facilities = new FacIncreaseFlowerGrowth(facilitiesData.facilitiesConfigData.id);
                    break;
                case 12:
                    facilities = new FacIncreaseLoadRate(facilitiesData.facilitiesConfigData.id);
                    break;
                case 13:
                    facilities = new FacIncreaseDoublePurchaseChance(facilitiesData.facilitiesConfigData.id);
                    break;
                case 14:
                    facilities = new FacIncreaseGoodsSoldTimes(facilitiesData.facilitiesConfigData.id);
                    break;
                default:
                    continue;
            }
            if (facilities == null) return;
            facilities.PutIntoUse(facilitiesData, args);
        }
        TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.BuyFacility, facilitiesData);
    }

    /// <summary>
    /// 建造设施
    /// </summary>
    public FacilitiesItemData BuildFacilities(FacilitiesItemData facilitiesData)
    {
        //1、未创建过当前设施，创建
        //播放建造特效TODO
        FacilityEntity entity = null;
        if (!facilitiesBuidingDic.TryGetValue(facilitiesData.facilitiesConfigData.id, out FacilitiesItemData data))
        {
            string facPos = ((FacilitiesType)facilitiesData.facilitiesConfigData.type).ToString() + "/" + facilitiesData.pos;
            Transform parentTrans = GameObject.Find(facPos).transform;
            if (parentTrans.childCount > 0)
            {
                DestroyImmediate(parentTrans.GetChild(0).gameObject);
            }
            GameObject obj = ResourceManager.Instance.GetResourceInstantiate(facilitiesData.facilitiesConfigData.prefabName, parentTrans, ResouceType.PrefabItem);
            entity = obj.GetComponent<FacilityEntity>();
            entity.SetData(facilitiesData);
            facilitiesBuidingDic.Add(facilitiesData.facilitiesConfigData.id, facilitiesData);
            data = facilitiesData;

            if (facilitiesData.facilitiesConfigData.id != 10012
                && facilitiesData.facilitiesConfigData.id != 10010
                && facilitiesData.facilitiesConfigData.id != 10046
                && facilitiesData.facilitiesConfigData.id != 10038
                && facilitiesData.facilitiesConfigData.id != 10047)          //&& facilitiesData.facilitiesConfigData.id != 10014
            {
                //添加排序组件
                data.gameObject.AddComponent<SortLayerCom>();
                //添加排序
                GameManager.Instance.AddToSortList(data.gameObject.transform);
            }

            //缓存炉子设施
            if (entity is StoveEntity)
            {
                stoveCookingList.Add(entity as StoveEntity);
            }
            //缓存盆栽设施
            if (entity is ParterreEntity)
            {
                parterreList.Add(entity as ParterreEntity);
            }
            //缓存货架设施
            if (entity is ShelfEntity)
            {
                shelfList.Add(entity as ShelfEntity);
            }
        }
        else
        {
            //2、已存在当前设施，替换创建
            data.itemConfigData = facilitiesData.itemConfigData;
            data.itemId = facilitiesData.itemId;
        }
        FacilityEntity facilityEntity = data.gameObject.GetComponent<FacilityEntity>();
        facilityEntity.ReplaceFacility(facilitiesData, doorEdge);
        return data;
    }

    /// <summary>
    /// 烹饪菜单入队列
    /// </summary>
    /// <param name="menuId">菜品Id</param>
    /// <param name="tableEntity">餐桌设施</param>
    /// <param name="callback">点餐回调</param>
    /// <param name="additionRatio">做菜加成效率，默认无加成</param>
    public void Cooking(int menuId, TableEntity tableEntity, Callback callback, float additionRatio = 0)
    {
        waitCookingQueue.Enqueue(new CookData()
        {
            menuId = menuId,
            tableEntity = tableEntity,
            callback = callback,
            additionRatio = additionRatio,
        });
    }

    /// <summary>
    /// 烹饪
    /// </summary>
    /// <param name="cookData"></param>
    public void Cooking(CookData cookData, StoveEntity stoveEntity)
    {
        stoveEntity.StartCooking(cookData);
    }

    /// <summary>
    /// 添加小鱼干
    /// </summary>
    public void AddFish(int facilityId, FishItem fishItem)
    {
        if (fishDic.ContainsKey(facilityId)) return;
        fishDic.Add(facilityId, fishItem);
    }

    /// <summary>
    /// 移除小鱼干
    /// </summary>
    public void RemoveFish(int facilityId)
    {
        if (!fishDic.ContainsKey(facilityId)) return;
        fishDic.Remove(facilityId);
    }

    /// <summary>
    /// 获取最近的小鱼干
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public FishItem GetNearestFish(Transform trans)
    {
        float tempDis = 0;
        FishItem targetFish = null;
        bool isFirst = true;
        foreach (var fish in fishDic.Values)
        {
            if (fish == null) continue;
            float dis = Vector3.Distance(trans.position, fish.transform.position);
            if (isFirst)
            {
                tempDis = dis;
                targetFish = fish;
                isFirst = false;
            }
            else
            {
                if (dis < tempDis)
                {
                    tempDis = dis;
                    targetFish = fish;
                }
            }
        }
        return targetFish;
    }

    /// <summary>
    /// 获取所有观赏期的盆栽
    /// </summary>
    /// <returns></returns>
    public List<ParterreEntity> GetAllShowingParterre()
    {
        if (parterreList.Count <= 0) return null;
        return parterreList.FindAll(o => o.ParterreStoreData.parterreStatus == ParterreStatus.Show);
    }

    /// <summary>
    /// 获取第一个空闲的盆栽
    /// </summary>
    /// <returns></returns>
    public ParterreEntity GetFreeParterre()
    {
        if (parterreList.Count <= 0) return null;
        return parterreList.Find(o => o.ParterreStoreData.parterreStatus == ParterreStatus.Idle);
    }

    /// <summary>
    /// 获取所有售卖货物阶段的货架
    /// </summary>
    /// <returns></returns>
    public List<ShelfEntity> GetAllSellingShelf()
    {
        if (shelfList.Count <= 0) return null;
        return shelfList.FindAll(o => o.ShelfData.shelfStatus == ShelfStatus.Sell);
    }

    /// <summary>
    /// 获取第一个空闲的货架
    /// </summary>
    /// <returns></returns>
    public ShelfEntity GetFreeShelf()
    {
        if (shelfList.Count <= 0) return null;
        return shelfList.Find(o => o.ShelfData.shelfStatus == ShelfStatus.Idle);
    }

    /// <summary>
    /// 获取小费台设施
    /// </summary>
    /// <returns></returns>
    public CashierEntity GetCashier()
    {
        if (!FacilitiesBuidingDic.ContainsKey(10001))
        {
            return null;
        }
        return FacilitiesBuidingDic[10001].gameObject.GetComponent<CashierEntity>();
    }
}
