using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 状态抽象类
/// </summary>
public abstract class FSMState
{
    protected float waiTime = 0f;
    protected float currenTime = 0;
    protected bool moveOver = false;
    protected bool ChangeState = false;
    protected Transform target = null;//参观点
    protected Dictionary<Transition, List<FSMStateID>> map = new Dictionary<Transition, List<FSMStateID>>();

    //状态编号ID
    protected FSMStateID stateID;
    public FSMStateID ID { get { return stateID; } }

    /// <summary>
    /// 添加转换条件
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="id"></param>
    public void AddTransition(Transition transition, FSMStateID id)
    {
        if(!map.ContainsKey(transition))
            map.Add(transition, new List<FSMStateID>());

        List<FSMStateID> stateList = null;
        map.TryGetValue(transition, out stateList);
        stateList.Add(id);
    }
    /// <summary>
    /// 删除转换条件
    /// </summary>
    /// <param name="trans"></param>
    public void DeleteTransition(Transition trans)
    {
        if(map.ContainsKey(trans))
        {
            map.Remove(trans);
        }
    }
    /// <summary>
    /// 获取当前状态下发生转换时新状态的编号
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public List<FSMStateID> GetOutputState(Transition trans)
    {
        if(map.ContainsKey(trans))
            return map[trans];

        return null;
    }
    /// <summary>
    /// 重置状态数据
    /// </summary>
    public void ResetFSMState()
    {
        currenTime = 0;
        moveOver = false;
        ChangeState = false;
    }

    /// <summary>
    /// 是否转换状态 
    /// 以及发生哪种转换
    /// </summary>
    /// <param name="player"></param>
    /// <param name="npc"></param>
    public abstract void Reason(BaseActor actor);
    /// <summary>
    /// AI角色行为
    /// </summary>
    /// <param name="player"></param>
    /// <param name="npc"></param>
    public abstract void Act(BaseActor actor);
}
