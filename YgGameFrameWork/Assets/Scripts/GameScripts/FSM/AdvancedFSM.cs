using System.Collections.Generic;

/// <summary>
/// 状态转换枚举
/// </summary>
public enum Transition
{
    TakeOrderOver,
    TakeOrderRepeatOver,
    QueueUpDinnerOver,
    QueueUpWineOver,
    QueueUpDessertOver,
    QueueUpCoffeeOver,
    HaveDinnerOver,
    WineConsumptionOver,
    DessertConsumptionOver,
    CoffeeConsumptionOver,
    Consume,
    GardenFlowersOver,
    RandomMoveOver,
    ThiefRandomMoveOver,
    RichSonRandomMoveOver,
    SpecialPointMoveOver,
    GardenRandomMoveOver,
    NotionRandomMoveOver,
    NotionBuyOver,
    SkunkMoveOver,
    SkunkFartOver,
    StealOver,
}
/// <summary>
/// 状态ID
/// </summary>
public enum FSMStateID
{
    QueueUpCoffee = 0,          //咖啡排队
    QueueUpWine,                //酒柜排队
    QueueUpDessert,             //甜品排队
    Leave,                      //离开
    CoffeeConsumption,          //咖啡消费
    WineConsumption,        //消费酒水
    DessertConsumption,     //甜品消费
    TakeOrder,              //点餐
    TakeDinnerRepeat,       //大胃王重复点餐
    HaveDinner,             //用餐
    QueueUpDinner,          //用餐排队
    GuestRandomMove,        //客人随机移动
    ThiefRandomMove,        //小偷的随机移动
    RichSonRandomMove,      //富二代随机移动
    SpecialPointMove,       //特殊点移动
    SkunkMove,              //臭鼬移动
    SkunkFart,              //臭鼬放屁
    ThrowerRandomMove,      //垃圾大王随机移动
    GardenRandomMove,       //花园随机移动
    GardenFlowers,          //花园赏花
    NotionStoreRandomMove,  //便利店随机移动
    NotionStoreBuy,         //便利店购买
    Steal,                  //偷小鱼干
    None,                   //无
}
/// <summary>
/// 状态管理器
/// </summary>
public class AdvancedFSM : FSM
{
    //状态列表
    protected List<FSMState> fsmStates;
    //当前状态编号
    private FSMStateID currentStateID;
    //当前状态
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; }  }
    public FSMStateID CurrentStateID { get { return currentStateID; } }

    public AdvancedFSM()
    {
        fsmStates = new List<FSMState>();
    }
    /// <summary>
    /// 向状态列表中加入一个新状态
    /// </summary>
    /// <param name="fsmState"></param>
    public void AddFSMState(FSMState fsmState)
    {
        if(fsmState == null)
        {
            TDDebug.DebugLogError("FSM ERROR: Null reference is not allowed");
        }
        if(fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return; 
        }
        foreach(FSMState state in fsmStates)
        {
            if(state.ID == fsmState.ID)
            {
                TDDebug.DebugLogError("FSM ERROR: Trying to add a state that was already inside the list:" + state.ID );
                return;
            }
        }
        fsmStates.Add(fsmState);
    }
    /// <summary>
    /// 从状态列表中删除一个状态
    /// </summary>
    /// <param name="fsmState"></param>
    public void DeleteState(FSMStateID fsmState)
    {
        FSMState state = fsmStates.Find(temp => temp.ID == fsmState);
        if (state != null) fsmStates.Remove(state);
        TDDebug.DebugLogError("FSM ERROR: The state passed was not on the list. Impossible to delete it");
    }
    /// <summary>
    /// 根据当前状态和参数中传递的转换
    /// 转移到新状态
    /// </summary>
    /// <param name="trans">转换条件</param>
    /// <param name="stateIndex">要转换的状态索引</param>>
    public void PerformTransition(Transition trans, int stateIndex)
    {
        List<FSMStateID> idList = currentState.GetOutputState(trans);
        if(idList == null || idList.Count == 0)
        {
            TDDebug.Log("FSM ERROR: The transation was not on the list");
            return;
        }
        currentStateID = idList[stateIndex];
        FSMState state = fsmStates.Find(temp => temp.ID == currentStateID);
        if(state != null)
        {
            state.ResetFSMState();
            currentState = state;
        }
    }
}
