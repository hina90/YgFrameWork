[System.Serializable]
public class ExhibitsStoreData
{
    /// <summary>
    /// 是否解锁
    /// </summary>
    public bool isUnlock;
    /// <summary>
    /// 是否被放置
    /// </summary>
    public bool isPlace;
    /// <summary>
    /// 当前展品放置在哪个展台
    /// </summary>
    public int standID;
    /// <summary>
    /// 展品当前等级
    /// </summary>
    public int level;
    /// <summary>
    /// 升级消耗，根据公式成长
    /// </summary>
    public System.Int64 costBase;
    /// <summary>
    /// 当前展品收益，根据公式成长
    /// </summary>
    public System.Int64 currentCoin;
    /// <summary>
    /// 当前展品的加成值
    /// </summary>
    public float currentAddition;
    /// <summary>
    /// 展品升级后提供给展馆的经验值，根据公式成长
    /// </summary>
    public int provideMuseumExp;
    /// <summary>
    /// 展品当前星级
    /// </summary>
    public int starLevel;
    /// <summary>
    /// 展品当前经验值
    /// </summary>
    public int exp;
    /// <summary>
    /// 升星许可证
    /// </summary>
    public bool upStarPermit;
}
