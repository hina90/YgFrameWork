using UnityEngine;

/// <summary>
/// 垃圾大王
/// </summary>
public class GarbageThrower : BaseSpecialActor
{
    public int ThrowNumber { get; set; }                //扔垃圾次数

    public override void Init()
    {
        base.Init();
        ThrowNumber = ConfigData.customerType[1];
    }
    /// <summary>
    /// 初始化AI
    /// </summary>
    protected override void InitAIController()
    {
        AiController = gameObject.GetOrAddComponent<GarbageThrowerAIController>();
        AiController.Initialize(this);
    }

    private void OnMouseDown()
    {
        Event();
    }
    /// <summary>
    /// 扔垃圾
    /// </summary>
    public void ThrowGarbage()
    {
        ThrowNumber--;
        GameObject prefab = ResourceManager.Instance.GetResource("Garbage", ResouceType.PrefabItem);
        GameObject garbageObj = Instantiate(prefab);

        int resIndex = Random.Range(1, 5);
        Sprite sprite = ResourceManager.Instance.GetSpriteResource("ui_lj_" + resIndex, ResouceType.Icon);
        GarbageItem garbageItem = garbageObj.GetComponent<GarbageItem>();
        garbageItem.ShowGarbageItem(sprite);
        garbageObj.transform.position = transform.position;

        GameManager.Instance.AddGarbageToList(garbageItem);
    }
    /// <summary>
    /// 事件完成回调
    /// </summary>
    protected override void EventCompleteCallback()
    {
        if (AiController.CurrentStateID == FSMStateID.ThrowerRandomMove)
            AiController.SetTransition(Transition.RandomMoveOver, 0);
    }
}
