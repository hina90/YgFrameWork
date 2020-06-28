using System.Collections.Generic;

[System.Serializable]
public class StoreData
{
    public float loadRate;                       //上货效率
    public float doublePurchaseChance;           //顾客加倍购买概率
    public int times;                            //倍数
    public Dictionary<int, ShelfData> shelfDic;  //货架信息数据
}

[System.Serializable]
public class ShelfData
{
    public int goodsId;                          //货物Id
    public ShelfStatus shelfStatus;              //货架状态
    public float remainLoadTime;                 //剩余上货时间
    public int remainSellTimes;                  //剩余售卖次数
}
