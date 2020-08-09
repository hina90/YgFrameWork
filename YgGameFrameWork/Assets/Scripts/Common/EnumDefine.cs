/// <summary>
/// UI层级类型
/// </summary>
public enum LayerMenue
{
    UI,
    SECONDUI,
    THREEUI,
    PUBLIC,
    TIPS,
    LOADING,
    MAIN,
    WARNINGTIP,
    TIPMSG,
    MONEY_BAR,
    MASK,
    TEACH,
    START
}

public enum GameEvent
{
    SCENE_LOAD_PROGRESS,
    SCENE_LOAD_COMPLETE,
    EQUIPMENT_UPDATE,
    UPDATE_MONEY_BAR,
    UPDATE_WAVE,
    GAME_END,
    DIG,
    UPDATE_BUILDPANEL,
    UPDATE_BUILDPANEL_LEVEL,
    UPDATE_ACHIEVE,
    OPEN_CUL_DETAIL,
    UPDATE_CULTIVATE_STATUS,
    UPDATE_CULTIVATE_CELL,
    UPDATE_MAINGAME_EFFECT,
    UP_BTN,
    CULTIVATE_CAN_UPGRADE,
}

public enum DigItemType
{
    Null,
    Exhibit,
    Decoration,
    MoneyDiamond,
    MoneyGold,
}
public enum DigCellType
{
    Null,
    Soft_3,
    Top_6
}

public enum ShowcaseType
{
    Null,
    Small,
    Middle,
    Large
}
public enum ePlayerItem
{
    Null,
    Gold,
    Diamond,
    DigCostItem,
    Max
}
public enum eMoneyBarStyle
{
    Null,
    Diamond_Gold
}
public enum eDecorationType
{
    Null,
    Taizi,
    Zhiwu,
    Fire,
    ChairPoint,
    Zhiwutai,
    China,
    Kuijia,
    Water,
    Qiubite,
    Xinxilan,
    DrinkingPoint,
    Max
}
public enum eCulEffectType
{
    Null,
    Exhibit_1_Income_Up,
    Exhibit_2_Income_Up,
    Exhibit_3_Income_Up,
    Exhibit_4_Income_Up,
    Exhibit_5_Income_Up,
    Exhibit_6_Income_Up,
    Offline_Income_Up,//离线收益增加		
    Offline_Time_Up,//离线收益上限时间增加	
    Super_Npc_Rate_Up1,//圣诞老人出现几率增加
    Super_Npc_Rate_Up2,//旅游大巴出现几率增加
    Super_Npc_Rate_Up3,//圣诞老人奖励增加
}
public enum eRedPointType
{
    Null,
    Cultivate,//主界面养成入口界面的红点
    Max
}
public enum eUIAnimation
{
    Null,
    PopUp
}
public enum eTeachType
{
    lockstand,
    npc,
    awardGold,
    uplv,
    toDig,
    ad,
    npcAward,
    over,
    previewDesc,
    Max
}
public enum eQuality
{
    Null,
    Blue,
    Purple,
    Yellow,
    Max
}
