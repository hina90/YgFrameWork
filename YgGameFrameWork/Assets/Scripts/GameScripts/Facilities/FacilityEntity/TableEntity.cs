using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 饭桌设施实体
/// </summary>
public class TableEntity : FacilityEntity
{
    private SpriteRenderer menuRender;
    private MenuConfigData menuConfig;
    private GameObject animObj;
    private bool haveGuest;
    /// <summary>
    /// 当前使用设施的客人Id
    /// </summary>
    public int UseGuestID { get; set; }
    /// <summary>
    /// 是否有客人
    /// </summary>
    public bool HaveGuest
    {
        get { return haveGuest; }
        set
        {
            haveGuest = value;
            if (haveGuest)
            {
                animObj.SetActive(false);
            }
            else
            {
                animObj.SetActive(true);
            }
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        menuRender = transform.Find("foodPoint").GetComponent<SpriteRenderer>();
        animObj = transform.Find("anim").gameObject;
    }

    /// <summary>
    /// 炉子烹饪上菜
    /// </summary>
    public void DishUp(MenuConfigData menuConfig)
    {
        this.menuConfig = menuConfig;
        menuRender.sprite = ResourceManager.Instance.GetSpriteResource(menuConfig.icon, ResouceType.Icon);
    }

    /// <summary>
    /// 餐桌用餐完成
    /// </summary>
    public void HaveDinnerOver(int payTimes)
    {
        menuRender.sprite = null;
        Payment(payTimes);
    }

    /// <summary>
    /// 付费
    /// </summary>
    /// <param name="value"></param>
    private void Payment(int payTimes)
    {
        CreateDriedFish(payTimes * menuConfig.soldPrice);
        //if (GuideManager.Instance.IsFinishGuide())
        //{
        //    GuideManager.Instance.EnterWeakStep(1101);
        //}
    }
}
