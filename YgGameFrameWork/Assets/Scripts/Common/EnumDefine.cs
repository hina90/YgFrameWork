
public enum GameEvent
{
    SCENE_LOAD_PROGRESS,
    SCENE_LOAD_COMPLETE,
    UPDATE_MONEY_BAR,
    UPDATE_WAVE,
    GAME_END,
}

/// <summary>
/// 层级
/// </summary>
public enum UILayer
{
    MapAbout = 1000,    //地图相关
    Common = 2000,      //公用层
    Fixed = 3000,       //固定层
    Effect = 4000,      //特效层
    Movie = 5000,       //电影剧情层
    Top = 6000,         //最顶层
}


