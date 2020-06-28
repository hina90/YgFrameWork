using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 点餐状态
/// </summary>
public class TakeOrderState : FSMState
{
    //当前要去的空闲餐桌
    private TableEntity currentDest = null;
    //是否点餐
    private bool isTakeOrder = false;
    //偏移量
    private Vector3 offSetVec = new Vector3(0f, 0.7f, 0);
    //菜单列表
    private List<MenuConfigData> menuList;
    //顾客数据
    private MenuModule menuModule;
    //状态下标
    private int stateIndex = 0;

    public TakeOrderState()
    {
        waiTime = 2f;
        stateID = FSMStateID.TakeOrder;

        menuModule = GameModuleManager.Instance.GetModule<MenuModule>(); 
        menuList = ConfigDataManager.Instance.GetDatabase<MenuConfigDatabase>().FindAll(); 
    } 
    /// <summary>
    /// 点餐状态转换
    /// </summary>
    public override void Reason(BaseActor actor)
    {
        if(ChangeState)
        {
            actor.AiController.SetTransition(Transition.TakeOrderOver, stateIndex);
        }
    }
    /// <summary>
    /// 点餐行为
    /// </summary>
    public override void Act(BaseActor actor)
    {
        if (currentDest == null)
            currentDest = GetFreeDesk();
        if (currentDest == null)
            return;

        actor.AiController.FindPath(currentDest.transform.position + offSetVec);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                currentDest.UseGuestID = actor.ActorID;
                if(!isTakeOrder)
                {
                    isTakeOrder = true;
                    int menuIndex = Random.Range(0, actor.ConfigData.correspondingDish.Length);
                    int menuId = actor.ConfigData.correspondingDish[menuIndex];
                    MenuConfigData data = menuList.Find(temp => temp.Id == menuId);
                    (actor as Guest).ShowDinnerMenu(data.icon, ()=>
                    {
                        MenuData foodData = menuModule.GetMenuData(menuId);
                        if (null != foodData && foodData.storeData.isStudy)   //学习了这个菜
                        {
                            FacilitiesManager.Instance.Cooking(menuId, currentDest, () =>
                            {
                                stateIndex = 0; 
                                ChangeState = true;
                            });
                        }
                        else                                                 //没有学习这个菜 生气走开
                        {
                            currentDest.HaveGuest = false;
                            (actor as Guest).ShowAngerSign();
                            stateIndex = 1;
                            ChangeState = true;
                        }
                    });
                }
            }
        }
    }
    /// <summary>
    /// 获取当前空余桌子
    /// </summary>
    private TableEntity GetFreeDesk()
    {
        TableEntity curDesk = null;
        List<TableEntity> deskList = GameManager.Instance.GetDinnerDesk();
        for (int i = 0; i < deskList.Count; i++)
        {
            if (!deskList[i].HaveGuest)
            {
                curDesk = deskList[i];
                curDesk.HaveGuest = true;
                break;
            }
        }
        return curDesk;
    }
}
