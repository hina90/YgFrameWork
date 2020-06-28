/// <summary>
/// 玩家属性数据
/// </summary>
[System.Serializable]
public struct PlayerData
{
    /// <summary>
    /// 体力
    /// </summary>
    public int PHY { get; set; }
    /// <summary>
    /// 最大体力值
    /// </summary>
    public int MaxPHY { get; set; }
    /// <summary>
    /// 精神值
    /// </summary>
    public int SPR { get; set; }
    /// <summary>
    /// 最大精神值
    /// </summary>
    public int MaxSPR { get; set; }
    /// <summary>
    /// 耐力值
    /// </summary>
    public int STA { get; set; }
    /// <summary>
    /// 最大耐力值
    /// </summary>
    public int MaxSTA { get; set; }
    /// <summary>
    /// 金币
    /// </summary>
    public int Gold { get; set; }
    /// <summary>
    /// 宝石
    /// </summary>
    public int Gem { get; set; }
    /// <summary>
    /// 当前探索关卡
    /// </summary>
    public int Level { get; set; }
    /// <summary>
    /// 存活天数
    /// </summary>
    public int SurvivalDays { get; set; }
    /// <summary>
    /// 上次离线日期
    /// </summary>
    public string OfflineDate { get; set; }
}
