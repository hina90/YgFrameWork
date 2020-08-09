using System.Collections.Generic;

/// <summary>
/// 状态转换枚举
/// </summary>
public enum Transition
{
    VisitOver,
    RestOver,
}
/// <summary>
/// 状态ID
/// </summary>
public enum FSMStateID
{
    BuyTicket,                   //买票
    VisitStart,                  //访问开始
    VisitEnd,                    //访问结束
    Rest,                        //休息
    Drink,                       //喝水
    Photo,                       //照相
    TriggerEvent,                //事件触发（have a rest , drink , take pictures and so on....）
    Leave,                       //离开
    None,                        //无
    //特殊
    SantaClausStay,              //圣诞老人停留
    SantaClausLeave,             //圣诞老人离开
    BusStay,                     //大巴停留
    BusLeave,                    //大巴离开
    TourGuideStay,               //导游停留
    TourGuideLeave,              //导游离开
    LittleCarMove,               //小汽车移动
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
    /// <summary>
    /// 状态状态
    /// </summary>
    /// <param name="stateId"></param>
    public void PerformTransition(BaseActor actor, FSMStateID stateId)
    {
        currentStateID = stateId;
        FSMState state = fsmStates.Find(temp => temp.ID == currentStateID);
        if(state != null)
        {
            state.ResetFSMState();
            currentState = state;
        }
    }
}
