using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 出客类型
/// </summary>
public enum GuestSpinnerType
{
    Normal,
    Click,
    Advertise
}

/// <summary>
/// 游戏主逻辑管理器
/// </summary>
public class GameManager : Singleton<GameManager>
{
    //客人列表
    private List<BaseActor> guestList = new List<BaseActor>();
    //排序列表
    private List<Transform> sortList = new List<Transform>();
    //总共已经出了多少客人
    private int TotalGuestNumber = 0;
    //普通客人的配置档
    private List<CustomerData> GuestConfigList;
    //特殊客人配置档
    private List<CustomerData> SpecialConfigList;
    //自然流客人数据
    private List<CustomerData> NormalConfigList;
    //手动宣传客人数据
    private List<CustomerData> ClickConfigList;
    //广告宣传客人数据
    private List<CustomerData> AdvertiseConfigList;
    //垃圾列表
    private List<GarbageItem> garbageList = new List<GarbageItem>();
    //菜单数据模块
    private MenuModule menuModule => GameModuleManager.Instance.GetModule<MenuModule>();
    //玩家数据模块
    private PlayerModule playerModule => GameModuleManager.Instance.GetModule<PlayerModule>();
    //顾客数据模块
    private CustomerModule customerModule => GameModuleManager.Instance.GetModule<CustomerModule>();
    // 新出现的顾客队列
    private Queue<CustomerData> customerQueue = new Queue<CustomerData>();


    //是否有特殊顾客存在
    public bool isExsit = false;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {
        AudioManager.Instance.PlayBGMAudio();
        TaskManager.Instance.Init();
        OrderManager.Instance.Init();

        NormalConfigList = customerModule.GetNormalConfigList();
        ClickConfigList = customerModule.GetClickConfigList();
        AdvertiseConfigList = customerModule.GetAdvertiseConfigList();
        GuestConfigList = customerModule.GetOrdinaryCustomerData();
        SpecialConfigList = customerModule.GetEspecialCustomerData();

        if (FilterUnlockCustomer(AdvertiseConfigList).Count == 0)
            CheckUnlockActor();
    }
    /// <summary>
    /// 检测解锁角色
    /// </summary>
    public void CheckUnlockActor()
    {
        for (int i = 0; i < GuestConfigList.Count; i++)
        {
            bool isPurchase = true;
            bool isFood = true;
            bool isStar = true;
            int foodId = 0;
            int facilityId = 0;
            int lenth = GuestConfigList[i].customerConfig.visitNeedFacilities.Length;
            for (int j = 0; j < lenth; j++)
            {
                facilityId = GuestConfigList[i].customerConfig.visitNeedFacilities[j];
                if (facilityId == 0)
                    break;

                FacilitiesItemData data = menuModule.GetFacilitiesItemData(facilityId);
                if (null == data || !data.storeData.isPurchase)
                    isPurchase = false;
            }
            lenth = GuestConfigList[i].customerConfig.visitNeedFoods.Length;
            for (int n = 0; n < lenth; n++)
            {
                foodId = GuestConfigList[i].customerConfig.visitNeedFoods[n];
                if (foodId == 0)
                    break;

                MenuData foodData = menuModule.GetMenuData(foodId);
                if (null == foodData || !foodData.storeData.isStudy)
                    isFood = false;
            }
            int needStar = GuestConfigList[i].customerConfig.levelCondition;

            if (playerModule.Star < needStar)
                isStar = false;

            if (isPurchase && isFood && isStar)
            {
                //Debug.Log("存入解锁角色ID: " + GuestConfigList[i].customerConfig.Id);
                customerModule.UnlockCustomer(GuestConfigList[i].customerConfig.Id);
            }
        }
    }
    /// <summary>
    /// 自然流客人
    /// </summary>
    public void NormalGuest()
    {
        TimerManager.Instance.CreateTimer("RandomCreatGuest", Random.Range(6, 10), Random.Range(20, 35), () =>
        {
            CreateGuest(GuestSpinnerType.Normal);
        });
    }
    /// <summary>
    /// 点击宣传客人
    /// </summary>
    public void ClickGuest()
    {
        CreateGuest(GuestSpinnerType.Click);
    }
    /// <summary>
    /// 广告宣传客人
    /// </summary>
    public void AdvertiseGuest()
    {
        CreateGuest(GuestSpinnerType.Advertise);
    }
    /// <summary>
    /// 动态过滤未解锁客人
    /// </summary>
    private List<CustomerData> FilterUnlockCustomer(List<CustomerData> customerList)
    {
        List<CustomerData> filterCustomerDataList = new List<CustomerData>();
        Dictionary<int, CustomerStoreData> dataList = customerModule.CustomerStoreDataList();

        CustomerData data = null;
        foreach (KeyValuePair<int, CustomerStoreData> keyValue in dataList)
        {
            if (keyValue.Value.isActive)
            {
                data = customerList.Find(temp => temp.customerConfig.Id == keyValue.Key);
                if (null != data)
                {
                    //Debug.Log("--------解锁ID：" + data.customerConfig.Id);
                    filterCustomerDataList.Add(data);
                }
            }
        }

        return filterCustomerDataList;
    }
    /// <summary>
    /// 创建客人
    /// </summary>
    public void CreateGuest(GuestSpinnerType type)
    {
        Transform door = GameManager.Instance.GetDoor();
        if (!door) return;

        QueueUp queue = GetQueueDinner();
        if (queue.Count() >= 15)  //最大限制15个顾客
            return;

        int guestId = 0;
        List<float> rateList = default;
        List<CustomerData> dataList = null;
        Dictionary<int, int> dataDic = default;
        switch (type)
        {
            case GuestSpinnerType.Click:
                dataList = FilterUnlockCustomer(ClickConfigList);
                dataDic = AnalysisProbability(dataList, out rateList);
                break;
            case GuestSpinnerType.Normal:
                dataList = FilterUnlockCustomer(NormalConfigList);
                dataDic = AnalysisProbability(dataList, out rateList);
                break;
            case GuestSpinnerType.Advertise:
                dataList = FilterUnlockCustomer(AdvertiseConfigList);
                dataDic = AnalysisProbability(dataList, out rateList);
                break;
        }
        int rate = MakerProbabilityValue(rateList);
        foreach (KeyValuePair<int, int> kv in dataDic)
        {
            if (kv.Key == rate)
            {
                guestId = kv.Value;
                break;
            }
        }

        CustomerData guestData = GuestConfigList.Find(temp => temp.customerConfig.Id == guestId);
        //Debug.Log("-----------------guestId:" + guestId);
        //Debug.Log("-----------------出现的顾客ID:" + guestData.customerConfig.guest);

        BaseActor actor = ActorManager.Instance.CreateActor<Guest>(guestData.customerConfig);
        if (null != actor)
        {
            TotalGuestNumber++;
            if (TotalGuestNumber >= 30)
            {
                if(!isExsit)
                {
                    TotalGuestNumber = 0;
                    CreateSpecialGuest();
                }
            }

            Vector3 doorPos = door.transform.position;
            actor.transform.position = new Vector3(doorPos.x, doorPos.y, doorPos.z);
            guestList.Add(actor);
            AddToSortList(actor.transform);
            if (guestData.customerConfig.Id == 1001 || guestData.customerConfig.Id == 1002) return;
            //特殊顾客(带有特殊技能的顾客引导提示)
            //if (guestData.customerConfig.type[0] != 0)
            ShowGuestProfile(guestData);
        }
    }

    /// <summary>
    /// 创建特殊顾客
    /// </summary>
    public void CreateSpecialGuest()
    {
        Transform door = GameManager.Instance.GetDoor();
        if (!door) return;
        int actorType = Random.Range(10, 16);
        //actorType = 12;
        BaseActor actor = null;
        CustomerData guestData = SpecialConfigList.Find(temp => temp.customerConfig.customerType[0] == actorType);
        switch (actorType)
        {
            case 11:
                actor = ActorManager.Instance.CreateActor<GarbageThrower>(guestData.customerConfig);             //垃圾大王
                break;
            case 10:
                actor = ActorManager.Instance.CreateActor<Thief>(guestData.customerConfig);                      //小偷
                break;
            case 14:
                actor = ActorManager.Instance.CreateActor<RichSon>(guestData.customerConfig);                    //富二代
                break;
            case 12:
                actor = ActorManager.Instance.CreateActor<WanderingSinger>(guestData.customerConfig);            //流浪歌手
                break;
            case 13:
                actor = ActorManager.Instance.CreateActor<SkunkActor>(guestData.customerConfig);                 //放屁臭鼬
                break;
            case 15:
                actor = ActorManager.Instance.CreateActor<AdvertisementActor>(guestData.customerConfig);         //广告商人
                break;
        }
        if (null != actor)
        {
            actor.transform.position = door.transform.position;
            guestList.Add(actor);
            AddToSortList(actor.transform);
            TaskManager.Instance.CheckTask(TaskType.SPECIAL_CUSTOMERS, 1);

            customerModule.UnlockCustomer(actor.ConfigData.Id);
            ShowGuestProfile(guestData);

            isExsit = true;
        }
    }

    /// <summary>
    /// 删除客人
    /// </summary>
    public void RemoveGuest(BaseActor actor)
    {
        BaseActor actorGuest = guestList.Find(temp => temp.ActorID == actor.ActorID);
        if (actorGuest != null)
        {
            guestList.Remove(actorGuest);
            RemoveFromSortList(actorGuest.transform);
            ActorManager.Instance.DestroyActor(actorGuest.gameObject);
        }
    }
    /// <summary>
    /// 客人生成概率
    /// </summary>  
    /// <returns></returns>
    private int MakerProbabilityValue(List<float> probabilityValues)
    {
        float total = 0;
        for (int i = 0; i < probabilityValues.Count; i++)
        {
            total += probabilityValues[i];
        }
        float nob = Random.Range(0, total);
        for (int i = 0; i < probabilityValues.Count; i++)
        {
            if (nob < probabilityValues[i])
                return i;

            else nob -= probabilityValues[i];
        }

        return probabilityValues.Count - 1;
    }
    /// <summary>
    /// 解析要生成概率
    /// </summary>
    /// <param name="itemList"></param>
    private Dictionary<int, int> AnalysisProbability(List<CustomerData> itemList, out List<float> rateList)
    {
        float totalValue = 0;
        Dictionary<int, int> rateDic = new Dictionary<int, int>();
        for (int i = 0; i < itemList.Count; i++)
        {
            totalValue += itemList[i].customerConfig.refreshWeight;
        }
        float f2;
        string rateValue;
        List<float> valueList = new List<float>();

        for (int i = 0; i < itemList.Count; i++)
        {
            rateValue = (itemList[i].customerConfig.refreshWeight / totalValue).ToString("0.00");
            if (float.TryParse(rateValue, out f2))
            {
                valueList.Add(f2);
                rateDic.Add(i, itemList[i].customerConfig.Id);
            }
        }

        rateList = valueList;

        return rateDic;
    }
    /// <summary>
    /// 主循环
    /// </summary>
    public void MainUpdate()
    {
        SortRunning();
    }
    /// <summary>
    /// 获取餐桌设施
    /// </summary>
    /// <returns></returns>
    public List<TableEntity> GetDinnerDesk()
    {
        List<TableEntity> deskList = new List<TableEntity>();
        AddDinnerDesk("pos_10002", deskList);
        AddDinnerDesk("pos_10003", deskList);
        AddDinnerDesk("pos_10004", deskList);
        AddDinnerDesk("pos_10007", deskList);
        AddDinnerDesk("pos_10008", deskList);
        AddDinnerDesk("pos_10009", deskList);

        return deskList;
    }
    private void AddDinnerDesk(string name, List<TableEntity> list)
    {
        TableEntity desk = GameObject.Find(name).GetComponentInChildren<TableEntity>();
        if (null != desk)
            list.Add(desk);
    }
    /// <summary>
    /// 获取餐桌是否有空余状态
    /// </summary>
    /// <returns>haveGuest</returns>
    /// true 
    public bool IsDinnerDeskBusy()
    {
        List<TableEntity> deskList = new List<TableEntity>();
        for (int i = 2; i < 5; i++)
        {
            TableEntity desk = GameObject.Find("pos_1000" + i).GetComponentInChildren<TableEntity>();
            if (desk != null)
            {
                deskList.Add(desk);
            }
        }
        for (int i = 7; i < 10; i++)
        {
            TableEntity desk = GameObject.Find("pos_1000" + i).GetComponentInChildren<TableEntity>();
            if (desk != null)
            {
                deskList.Add(desk);
            }
        }

        bool isBusy = true;
        for (int i = 0; i < deskList.Count; i++)
        {
            if (!deskList[i].HaveGuest)
            {
                isBusy = false;
                break;
            }
        }

        return isBusy;
    }
    /// <summary>
    /// 获取咖啡台是否空闲
    /// </summary>
    /// <returns></returns>
    public bool IsCoffeeMakerBusy()
    {
        ConsumeEntity facility = GameObject.Find("pos_10013").GetComponentInChildren<ConsumeEntity>();

        return facility == null ? true : facility.HaveGuest;
    }
    /// <summary>
    /// 获取甜品台是否空闲
    /// </summary>
    /// <returns></returns>
    public bool IsDessertTableBusy()
    {
        ConsumeEntity facility = GameObject.Find("pos_10006").GetComponentInChildren<ConsumeEntity>();

        return facility == null ? true : facility.HaveGuest;
    }
    /// <summary>
    /// 获取吧台是否空闲
    /// </summary>
    /// <returns></returns>
    public bool IsWineCabinetBusy()
    {
        ConsumeEntity facility = GameObject.Find("pos_10014").GetComponentInChildren<ConsumeEntity>();

        return facility == null ? true : facility.HaveGuest;
    }
    /// <summary>
    /// 获取排队点  用餐
    /// </summary>
    /// <returns></returns>
    public QueueUp GetQueueDinner()
    {
        QueueUp queue = GameObject.Find("queueDinner").GetComponent<QueueUp>();

        return queue;
    }
    /// <summary>
    /// 获取排队点  甜品
    /// </summary>
    /// <returns></returns>
    public QueueUp GetQueueDessert()
    {
        QueueUp queue = GameObject.Find("queueDessert").GetComponent<QueueUp>();

        return queue;
    }
    /// <summary>
    /// 获取排队点  咖啡机
    /// </summary>
    /// <returns></returns>
    public QueueUp GetQueueCoffee()
    {
        QueueUp queue = GameObject.Find("queueCoffee").GetComponent<QueueUp>();

        return queue;
    }
    /// <summary>
    /// 获取排队点  酒柜
    /// </summary>
    /// <returns></returns>
    public QueueUp GetQueueWine()
    {
        QueueUp queue = GameObject.Find("queueWine").GetComponent<QueueUp>();

        return queue;
    }
    /// <summary>
    /// 获取甜品台
    /// </summary>
    /// <returns></returns>
    public ConsumeEntity GetConsumeDessertTable()
    {
        ConsumeEntity coffeeMaker = GameObject.Find("pos_10006").GetComponentInChildren<ConsumeEntity>();

        return coffeeMaker;
    }
    /// <summary>
    /// 获取咖啡台
    /// </summary>
    /// <returns></returns>
    public ConsumeEntity GetConsumeCoffeeMaker()
    {
        ConsumeEntity wineCabinet = GameObject.Find("pos_10013").GetComponentInChildren<ConsumeEntity>();

        return wineCabinet;
    }
    /// <summary>
    /// 获取吧台
    /// </summary>
    /// <returns></returns>
    public ConsumeEntity GetConsumeWineCabinet()
    {
        ConsumeEntity dessertTable = GameObject.Find("pos_10014").GetComponentInChildren<ConsumeEntity>();

        return dessertTable;
    }
    /// <summary>
    /// 获取随机移动点
    /// </summary>
    /// <returns></returns>
    public Transform GetRandomMovePoint()
    {
        int randomIndex = Random.Range(1, 9);
        GameObject moveObj = GameObject.Find("randomPoint_" + randomIndex);

        return moveObj.transform;
    }
    /// <summary>
    /// 获取花园随机移动点
    /// </summary>
    /// <returns></returns>
    public Transform GetGardenRandomMovePoint()
    {
        int randomIndex = Random.Range(9, 14);
        GameObject moveObj = GameObject.Find("randomPoint_" + randomIndex);

        return moveObj.transform;
    }
    /// <summary>
    /// 获取杂货店随机移动点
    /// </summary>
    /// <returns></returns>
    public Transform GetNotionStoreRandomMovePoint()
    {
        int randomIndex = Random.Range(1, 5);
        GameObject moveObj = GameObject.Find("notion_store_randomPoint_" + randomIndex);

        return moveObj.transform;
    }
    /// <summary>
    /// 获取门
    /// </summary>
    /// <returns></returns>
    public Transform GetDoor()
    {
        Transform door = GameObject.Find("pos_10011").transform;

        return door;
    }
    /// <summary>
    /// 获取特殊点
    /// </summary>
    /// <returns></returns>
    public Transform GetSpecialPoint()
    {
        GameObject specialPoint = GameObject.Find("SpecialPoint");

        return specialPoint.transform;
    }
    /// <summary>
    /// 获取臭鼬移动点
    /// </summary>
    /// <returns></returns>
    public Transform GetSkunkMovePoint()
    {
        GameObject movePoint = GameObject.Find("randomPoint_2");

        return movePoint.transform;
    }
    /// <summary>
    /// 获取用餐后的行为状态
    /// </summary>
    /// <returns></returns>
    public int GetAfterDinnerState(FSMStateID stateId, BaseActor actor)
    {
        ConsumeEntity entity = null;
        FSMStateID nextStateId = FSMStateID.None;
        int rate = Random.Range(0, 3);

        if (rate == 0)
        {
            entity = GetConsumeCoffeeMaker();
            nextStateId = FSMStateID.QueueUpCoffee;
            if (null == entity)
            {
                entity = GetConsumeDessertTable();
                nextStateId = FSMStateID.QueueUpDessert;
            }
            if (null == entity)
            {
                entity = GetConsumeWineCabinet();
                nextStateId = FSMStateID.QueueUpWine;
            }
        }
        else if (rate == 1)
        {
            entity = GetConsumeWineCabinet();
            nextStateId = FSMStateID.QueueUpWine;
            if (null == entity)
            {
                entity = GetConsumeDessertTable();
                nextStateId = FSMStateID.QueueUpDessert;
            }
            if (null == entity)
            {
                entity = GetConsumeCoffeeMaker();
                nextStateId = FSMStateID.QueueUpCoffee;
            }
        }
        else if (rate == 2)
        {
            entity = GetConsumeDessertTable();
            nextStateId = FSMStateID.QueueUpDessert;
            if (null == entity)
            {
                entity = GetConsumeCoffeeMaker();
                nextStateId = FSMStateID.QueueUpCoffee;
            }
            if (null == entity)
            {
                entity = GetConsumeWineCabinet();
                nextStateId = FSMStateID.QueueUpWine;
            }
        }
        if (entity == null)
        {
            if (actor.RandomMoveNumber >= BaseActor.MAX_RANDOMMOVE)
                nextStateId = FSMStateID.Leave;
            else
            {
                actor.RandomMoveNumber++;
                nextStateId = FSMStateID.WineConsumption;  //占位（随机移动）
            }
        }
        else
            actor.ConsumptionNumber++;

        return (int)nextStateId;
    }
    /// <summary>
    /// 获取消费后的行为状态
    /// </summary>
    /// <param name="stateId"></param>
    /// <param name="actor"></param>
    /// <returns></returns>
    public int GetAfterConsumptionState(FSMStateID stateId, BaseActor actor)
    {
        if (actor.ConsumptionNumber >= BaseActor.MAX_CONSUPTION)
        {
            if (CheckAreaUnLock(1003))    //去花园状态
            {
                int rate = Random.Range(0, 101);
                if (rate <= 70)
                {
                    return 3;
                }
                return 4;
            }
            else                         //设置离开状态
            {
                return 4;
            }
        }

        Transform trans = null;
        ConsumeEntity entity = null;
        int nextStateId = 0;

        if (stateId == FSMStateID.GuestRandomMove)
        {
            int random = Random.Range(0, 3);

            if (random == 0)
            {
                entity = GetConsumeWineCabinet();
                nextStateId = 0;
                if (entity == null)
                {
                    entity = GetConsumeCoffeeMaker();
                    nextStateId = 1;
                }
                if (entity == null)
                {
                    entity = GetConsumeDessertTable();
                    nextStateId = 2;
                }
            }
            else if (random == 1)
            {
                entity = GetConsumeCoffeeMaker();
                nextStateId = 1;
                if (entity == null)
                {
                    entity = GetConsumeWineCabinet();
                    nextStateId = 0;
                }
                if (entity == null)
                {
                    entity = GetConsumeDessertTable();
                    nextStateId = 2;
                }
            }
            else if (random == 2)
            {
                entity = GetConsumeDessertTable();
                nextStateId = 2;
                if (entity == null)
                {
                    entity = GetConsumeWineCabinet();
                    nextStateId = 0;
                }
                if (entity == null)
                {
                    entity = GetConsumeCoffeeMaker();
                    nextStateId = 1;
                }
            }
            if (entity != null)
                actor.ConsumptionNumber++;
        }
        else
        {
            trans = GetRandomMovePoint();
            nextStateId = 2;                    //随机移动状态
        }

        if (entity == null && trans == null)
            nextStateId = 3;

        return nextStateId;
    }

    public void IntroMessageTip(string content)
    {
        TextMesh text = GameObject.Find("introText").GetComponent<TextMesh>();
        text.text = content;
    }
    /// <summary>
    /// 检测区域是否开启
    /// </summary>
    /// <param name="id">区域ID</param>
    public bool CheckAreaUnLock(int id)
    {
        return SystemManager.Instance.GetSystemIsUnlock(id);
    }

    /// <summary>
    /// 添加到排序列表
    /// </summary>
    /// <param name="obj"></param>
    public void AddToSortList(Transform trans)
    {
        if (!sortList.Contains(trans))
            sortList.Add(trans);
    }
    /// <summary>
    /// 从排序列表删除
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveFromSortList(Transform trans)
    {
        if (sortList.Contains(trans))
            sortList.Remove(trans);
    }
    /// <summary>
    /// 驱赶客人 
    /// </summary>
    public void ExpelGuest()
    {
        int randomNumber = 0;
        for (int i = 0; i < guestList.Count; i++)
        {
            if (guestList[i].ConfigData.customerType[0] == 1)
            {
                randomNumber = Random.Range(0, 100);
                if (randomNumber <= 40)
                {
                    GuestLeave(guestList[i]);
                }
            }
        }
    }
    /// <summary>
    /// 客人离开
    /// </summary>
    /// <param name="actor"></param>
    private void GuestLeave(BaseActor actor)
    {
        if (actor.AiController.CurrentStateID == FSMStateID.QueueUpDinner)
        {
            actor.AiController.SetTransition(Transition.QueueUpDinnerOver, 1);
            (actor as Guest).ShowAngerSign();
        }
        //if (actor.AiController.CurrentStateID == FSMStateID.TakeOrder)
        //    actor.AiController.SetTransition(Transition.TakeOrderOver, 1);
        //else if(actor.AiController.CurrentStateID == FSMStateID.HaveDinner)
        //    actor.AiController.SetTransition(Transition.HaveDinnerOver, 3);
        else if (actor.AiController.CurrentStateID == FSMStateID.QueueUpWine)
        {
            actor.AiController.SetTransition(Transition.QueueUpWineOver, 1);
            (actor as Guest).ShowAngerSign();
        }
        else if (actor.AiController.CurrentStateID == FSMStateID.QueueUpCoffee)
        {
            actor.AiController.SetTransition(Transition.QueueUpCoffeeOver, 1);
            (actor as Guest).ShowAngerSign();
        }
        else if (actor.AiController.CurrentStateID == FSMStateID.QueueUpDessert)
        {
            actor.AiController.SetTransition(Transition.QueueUpDessertOver, 1);
            (actor as Guest).ShowAngerSign();
        }
        //else if (actor.AiController.CurrentStateID == FSMStateID.WineConsumption)
        //    actor.AiController.SetTransition(Transition.WineConsumptionOver, 3);
        //else if (actor.AiController.CurrentStateID == FSMStateID.CoffeeConsumption)
        //    actor.AiController.SetTransition(Transition.CoffeeConsumptionOver, 3);
        //else if (actor.AiController.CurrentStateID == FSMStateID.DessertConsumption)
        //    actor.AiController.SetTransition(Transition.DessertConsumptionOver, 3);
        else if (actor.AiController.CurrentStateID == FSMStateID.GuestRandomMove)
        {
            actor.AiController.SetTransition(Transition.RandomMoveOver, 3);
            (actor as Guest).ShowAngerSign();
        }


    }

    private float SortCurrenTime = 0;
    private float SortWaitInterval = .4f;
    /// <summary>
    /// 执行排序
    /// </summary>
    private void SortRunning()
    {
        SortCurrenTime += Time.deltaTime;
        if (SortCurrenTime >= SortWaitInterval)
        {
            SortCurrenTime = 0;
            sortList.Sort(delegate (Transform a, Transform b)
            {
                return a.position.y.CompareTo(b.position.y);
            });

            for (int i = 0; i < sortList.Count; i++)
            {
                sortList[i].GetComponent<SortLayerCom>().SetSortLayer(sortList.Count - i, LayerSetType.EQUAL);
            }
        }
    }
    /// <summary>
    /// 检测用餐人员是否超载
    /// </summary>
    public bool CheckDinnerLimited()
    {
        QueueUp queue = GetQueueDinner();
        if (queue.Count() >= 15)
            return true;

        return false;
    }

    /// <summary>
    /// 添加垃圾到列表
    /// </summary>
    /// <param name="garbage"></param>
    public void AddGarbageToList(GarbageItem garbage)
    {
        garbageList.Add(garbage);
    }
    /// <summary>
    /// 从列表中删除垃圾
    /// </summary>
    /// <param name="garbage"></param>
    public void RemoveGarbageFromList(GarbageItem garbage)
    {
        if (garbageList.Contains(garbage))
        {
            garbageList.Remove(garbage);
            GameObject.DestroyImmediate(garbage.gameObject);
            TaskManager.Instance.CheckTask(TaskType.GARBAGE_CLEANING, 1);
        }
    }

    /// <summary>
    /// 显示特殊顾客提示面板
    /// </summary>
    /// <param name="guestData"></param>
    private void ShowGuestProfile(CustomerData guestData)
    {
        if (customerModule.GetCustomerIsProfile(guestData.customerConfig.Id)) return;

        ///添加到已显示顾客缓存中
        customerModule.AddHaveProfileCustomer(guestData.customerConfig.Id);
        customerQueue.Enqueue(guestData);
        TimerManager.Instance.CreateUnityTimer(2.5f, () =>
        {
            ShowGuestProfile();
        });
    }

    public void ShowGuestProfile()
    {
        if (customerQueue.Count > 0)
        {
            if (UIManager.Instance.GetResUI("UI_GuestProfile") != null)
            {
                UIManager.Instance.CloseUI(LayerMenue.TIPS);
            }
            UIManager.Instance.OpenUI<UI_GuestProfile>(customerQueue.Dequeue());
        }
    }
}
