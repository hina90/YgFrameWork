using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

public class Guest : BaseActor
{
    Vector3 actorSize;
    DinnerMenu dinnerMenu;

    protected override void InitAIController()
    {
        AiController = gameObject.GetOrAddComponent<GuestAIController>();
        AiController.Initialize(this);
    }

    /// <summary>
    /// 显示菜单
    /// </summary>
    public void ShowDinnerMenu(string icon, Callback callback)
    {
        GameObject prefab = ResourceManager.Instance.GetResource("DinnerMenu", ResouceType.PrefabItem);
        GameObject menuObj = Instantiate(prefab, GameObject.Find("ActorSliders").transform);
        actorSize = transform.GetComponent<BoxCollider2D>().size;

        dinnerMenu = menuObj.GetComponent<DinnerMenu>();
        dinnerMenu.ClickCallback = callback;
        Sprite sprite = ResourceManager.Instance.GetSpriteResource(icon, ResouceType.Icon);
        dinnerMenu.ShowMenu(sprite);
    }
    /// <summary>
    /// 显示生气标志
    /// </summary>
    public void ShowAngerSign()
    {
        GameObject prefab = ResourceManager.Instance.GetResource("DinnerMenu", ResouceType.PrefabItem);
        GameObject menuObj = Instantiate(prefab, GameObject.Find("ActorSliders").transform);
        actorSize = transform.GetComponent<BoxCollider2D>().size;

        dinnerMenu = menuObj.GetComponent<DinnerMenu>();
        dinnerMenu.ShowAngry();
    }
    /// <summary>
    /// 帧事件
    /// </summary>
    protected override void Update()
    {
        if (null != dinnerMenu)
            dinnerMenu.FellowActor(transform.position, actorSize);
    }
    /// <summary>
    /// 技能-小费
    /// </summary>
    public int Skill_Tipping()
    {
        int rate = ConfigData.type[1];
        int random = Random.Range(0, 101);
        int times = ConfigData.type[2];
        if(random <= rate)   //倍数
        {
            return times;
        }

        return 1;
    }
    /// <summary>
    /// 技能-好评如潮
    /// </summary>
    private int Skill_HotSell()
    {
        return 0;
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public override void Release()
    {
        if (dinnerMenu != null)
        {
            dinnerMenu.Release();
            GameObject.Destroy(dinnerMenu.gameObject);
        }
        base.Release();
    }
}
