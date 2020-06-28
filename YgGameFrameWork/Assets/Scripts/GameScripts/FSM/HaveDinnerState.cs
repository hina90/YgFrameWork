using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用餐状态
/// </summary>
public class HaveDinnerState : FSMState
{
    //状态ID
    private int stateId = 0;

    public HaveDinnerState()
    {
        waiTime = 3f;                               //用餐时间
        stateID = FSMStateID.HaveDinner;
    }
    /// <summary>
    /// 用餐的行为转换
    /// </summary>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            //TDDebug.DebugLog("=====================HaveDinnerState   Over================ " + stateId);
            actor.AiController.SetTransition(Transition.HaveDinnerOver, stateId);
        }
    }
    /// <summary>
    /// 用餐行为
    /// </summary>
    public override void Act(BaseActor actor)
    {
        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            List<TableEntity> deskList = GameManager.Instance.GetDinnerDesk();
            TableEntity deskCom = deskList.Find(temp => temp.UseGuestID == actor.ActorID);
            if (deskCom != null)
            {
                stateId = GameManager.Instance.GetAfterDinnerState(stateID, actor);

                int times = 1;
                int[] skillList = actor.ConfigData.type;
                if (skillList.Length == 3)                //顾客技能
                {
                    if (skillList[0] == 1)                   //小费
                    {
                        times = (actor as Guest).Skill_Tipping();
                    }
                    else if (skillList[0] == 2)              //大胃王
                    {
                        //Debug.Log("----------------TakeDinnerRepeatNumber:" + actor.TakeDinnerRepeatNumber + " skillList[1]：" + skillList[1] + "   deskState:" + deskCom.HaveGuest.ToString());
                        if(actor.TakeDinnerRepeatNumber < skillList[1])
                            stateId = 4;
                    }
                    else if(skillList[0] == 3)               //好评如潮
                    {
                        Debug.Log("-------------------好评如潮技能触发");
                    }
                }

                deskCom.HaveDinnerOver(times);
                currenTime = 0;
                ChangeState = true;
                if(stateId != 4)
                    deskCom.HaveGuest = false;


                TaskManager.Instance.CheckTask(TaskType.SERVE_CUSTOMER, 1);
                TaskManager.Instance.CheckTask(TaskType.DESIGNATED_CUSTOMER, 1, actor.ConfigData.Id);
                

                //触发订单任务
                if(actor.ConfigData.order.Length > 1)
                {
                    CustomerSpanModule module = GameModuleManager.Instance.GetModule<CustomerSpanModule>();
                    module.SetHaveDinnerNumber(actor.ConfigData.Id);
                    int dinnerTime = module.GetHaveDinnerNumber(actor.ConfigData.Id);
                    if (dinnerTime == actor.ConfigData.order[1])
                    {
                        OrderManager.Instance.CreateOrder(actor.ConfigData.order[0]);
                    }
                }
            }
        }
    }
}
