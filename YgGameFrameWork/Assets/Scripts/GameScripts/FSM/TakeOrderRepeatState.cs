using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;


/// <summary>
/// 大胃王状态
/// </summary>
public class TakeOrderRepeatState : FSMState
{
    private bool isTakeOrder = false;           //是否点单

    public TakeOrderRepeatState()
    {
        waiTime = 1;
        stateID = FSMStateID.TakeDinnerRepeat;
    }
    /// <summary>
    /// 状态转换
    /// </summary>
    public override void Reason(BaseActor actor)
    {
        if(ChangeState)
        {
            currenTime = 0;
            isTakeOrder = false;
            actor.AiController.SetTransition(Transition.TakeOrderRepeatOver, 0);
        }
    }
    /// <summary>
    /// 重复点餐行为
    /// </summary>
    public override void Act(BaseActor actor)
    {
        currenTime += Time.fixedDeltaTime;
        if(currenTime >= waiTime)
        {
            if(!isTakeOrder)
            {
                isTakeOrder = true;
                List<TableEntity> deskList = GameManager.Instance.GetDinnerDesk();
                TableEntity deskCom = deskList.Find(temp => temp.UseGuestID == actor.ActorID);

                MenuModule menuModule = GameModuleManager.Instance.GetModule<MenuModule>();
                List<MenuConfigData> menuList = ConfigDataManager.Instance.GetDatabase<MenuConfigDatabase>().FindAll();

                float cookBuffSpeed = actor.ConfigData.type[2] != 0 ? actor.ConfigData.type[2] * 0.01f : 0;
                int menuIndex = Random.Range(0, actor.ConfigData.correspondingDish.Length);
                int menuId = actor.ConfigData.correspondingDish[menuIndex];
                MenuConfigData data = menuList.Find(temp => temp.Id == menuId);
                (actor as Guest).ShowDinnerMenu(data.icon, () =>
                {
                    MenuData foodData = menuModule.GetMenuData(menuId);
                    if (null != foodData && foodData.storeData.isStudy)   //学习了这个菜
                    {
                        FacilitiesManager.Instance.Cooking(menuId, deskCom, () =>
                        {
                            actor.TakeDinnerRepeatNumber++;
                            ChangeState = true;
                        }, cookBuffSpeed);
                    }
                });
            }
        }
    }
}
