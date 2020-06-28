/// <summary>
/// 事件枚举
/// </summary>
public enum GameEvent
{
    SCENE_LOAD_PROGRESS,
    SCENE_LOAD_COMPLETE,
    //更新鱼干
    UPDATE_FISH,
    //更新星星
    UPDATE_STAR,
    UPDATE_PROPAGANDA,
    //更新许愿池重置冷却时间
    UPDATE_WISHPOOL_RESETTIME,
    //更新许愿池下一次许愿间隔时间
    UPDATE_WISHPOOL_INTERVALTTIME,
    OPEN_TASKTIP,
    CLOSE_TASKTIP,
    SET_MENU_SCROLL,
    CAMERA_FOCUS,
    CAMERA_PROSPECT,
    //开始引导任务
    START_AIMTASK,
    // 刷新引导任务
    REFRESH_AIMTASK,
    //完成引导任务
    FINISH_AIMTASK,

    #region 新手引导
    ENTER_GUIDE,
    FINISH_GUIDE,
    CLEAR_GARBAGE,
    PICKUP_FISH,
    TOGGLE_KITCHEN,
    AUTO_FINISH,
    SHOW_MASK,
    SHOW_MAINUI,
    UPDATE_FOOD,
    UPDATE_FACILITIES,
    FINISH_WEAKGUIDE,
    #endregion
}

/// <summary>
/// 顾客类型
/// </summary>
public enum ActorType
{
    //消费客人
    GUEST = 0,
    //小偷
    THIFE,
    //歌唱者
    SINGER,
    //富二代
    RICH_SECOND_GENERATION,
}

