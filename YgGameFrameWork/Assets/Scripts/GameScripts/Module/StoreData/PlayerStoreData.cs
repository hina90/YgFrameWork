using System;
[System.Serializable]
public class PlayerStoreData
{
    /// <summary>
    /// 星级
    /// </summary>
    public int star;
    /// <summary>
    /// 鱼干
    /// </summary>
    public int fish;
    /// <summary>
    /// 打理天数
    /// </summary>
    public int survivalDays;
    /// <summary>
    /// UI_money界面所需
    /// </summary>
    public bool moneyIsOne;
    /// <summary>
    /// 上线离线时间
    /// </summary>
    public DateTime offlineTime;
    /// <summary>
    /// 登录的时间
    /// </summary>
    public DateTime loadingTime;
    /// <summary>
    /// 花园解锁的目标时间
    /// </summary>
    public DateTime gardenUnlockTime;
    /// <summary>
    /// BGM音量
    /// </summary>
    public float bgmVolume;
    /// <summary>
    /// 音效音量
    /// </summary>
    public float audioVolume;
    /// <summary>
    /// 是否免费2倍小费台
    /// </summary>
    public int freeCashier;
}
