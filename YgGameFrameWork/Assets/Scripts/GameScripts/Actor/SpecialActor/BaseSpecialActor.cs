using UnityEngine;
using DG.Tweening;

/// <summary>
/// 特殊客人基类
/// </summary>
public class BaseSpecialActor : BaseActor
{
    protected GameObject slider;
    protected HpCom hpCom;
    protected bool eventComplete;                                                                        //事件是否已经完成
    protected int pointNum = 10;                                                                         //需要点击的次数
    protected float addValue = 0;                                                                        //进度每次增长值
    protected event Callback eventCallback;                                                              //能量条满了后的事件回调

    private float current = 0;                                                                           //当前值
    protected GameObject signBar;
    protected GameObject sliderBar;
    protected GameObject actorText;

    private float currentTime = 0;
    private bool isLeave = false;
    private const float WAITIME = 30;
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        base.Init();
        slider = ResourceManager.Instance.GetResourceInstantiate("ActorSlider", GameObject.Find("ActorSliders").transform, ResouceType.PrefabItem);
        hpCom = slider.GetComponent<HpCom>();

        signBar = slider.transform.Find("ExclamationMark").gameObject;
        sliderBar = slider.transform.Find("Slider").gameObject;

        if (ConfigData.customerType.Length >= 2)
            pointNum = ConfigData.customerType[1];                                               //需要点击的次数

        addValue = float.Parse(((decimal)1 / pointNum).ToString("0.0"));                         //每次点击增加值 
        eventCallback += EventCompleteCallback;
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    protected virtual void Event()
    {
        if(signBar)
            signBar.SetActive(false);

        if(sliderBar)
            sliderBar.SetActive(true);

        if (eventComplete)
            return;
        if (hpCom == null)
            return;
        hpCom.UpdateHp(current += addValue, 1);
        if (current >= 1)
        {
            eventComplete = true;
            eventCallback?.Invoke();
            hpCom.Release();
        }
    }
    protected virtual void EventCompleteCallback()
    {

    }
    /// <summary>
    /// 帧事件
    /// </summary>
    protected override void Update()
    {
        CheckLeave();
        if (hpCom == null)
            return;
        hpCom.FollowActor(this.transform.position);
    }
    protected void CheckLeave()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= WAITIME)
        {
            currentTime = 0;
            if (!isLeave)
            {
                isLeave = true;
                TimeToLeave();
                GameManager.Instance.isExsit = false;
            }
        }
    }
    /// <summary>
    /// 离开
    /// </summary>
    private void TimeToLeave()
    {
        //小偷
        if (AiController.CurrentStateID == FSMStateID.ThiefRandomMove)
            AiController.SetTransition(Transition.ThiefRandomMoveOver, 0);
        else if (AiController.CurrentStateID == FSMStateID.Steal)
            AiController.SetTransition(Transition.StealOver, 0);
        //富二代
        else if (AiController.CurrentStateID == FSMStateID.RichSonRandomMove)
            AiController.SetTransition(Transition.RichSonRandomMoveOver, 0);
        //特殊點
        else if (AiController.CurrentStateID == FSMStateID.SpecialPointMove)
            AiController.SetTransition(Transition.SpecialPointMoveOver, 0);
        //臭鼬
        else if (AiController.CurrentStateID == FSMStateID.SkunkMove)
            AiController.SetTransition(Transition.SkunkMoveOver, 0);
        else if (AiController.CurrentStateID == FSMStateID.SkunkFart)
            AiController.SetTransition(Transition.SkunkFartOver, 0);
        //垃圾大王
        else if (AiController.CurrentStateID == FSMStateID.ThrowerRandomMove)
            AiController.SetTransition(Transition.ThiefRandomMoveOver, 0);
    }

    /// <summary>
    /// 创建小鱼干
    /// </summary>
    /// <param name="value"></param>
    /// <param name="parents"></param>
    /// <returns></returns>
    protected GameObject CreateDriedFish(int value, Transform parents)
    {
        GameObject fishPrefab = ResourceManager.Instance.GetResource("DriedFish", ResouceType.PrefabItem);

        GameObject fish = Object.Instantiate(fishPrefab, parents);
        fish.transform.localPosition = transform.position;
        Vector3 targetPos = fish.transform.localPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 1.5f), 0);
        FishItem fishItem = fish.GetOrAddComponent<FishItem>();
        fishItem.Value = value;
        fish.transform.DOLocalJump(targetPos, targetPos.y, 1, 1f).SetEase(Ease.Linear).OnComplete(() => 
        {
            /*fishItem.CanPickUp = true;*/
            fishItem.Disappear();
        });



        return fish;
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public override void Release()
    {
        if(null != hpCom)
            hpCom.Release();

        if (actorText)
            Destroy(actorText);

        Destroy(slider);
        base.Release();
    }
}
