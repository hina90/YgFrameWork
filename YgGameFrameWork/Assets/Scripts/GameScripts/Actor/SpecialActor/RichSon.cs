using UnityEngine;
using DG.Tweening;

/// <summary>
/// 富二代
/// </summary>
public class RichSon : BaseSpecialActor
{
    private bool isClick = false;
    private bool isAnimation2 = false;
    private float waitime = 0f;
    private float currentTime = 0f;
    private float resetSpeedTime = 0.5f;
    private float currentResetTime = 0;
    private float originalSpeed = 0;
    private TextCom showTimeTxt;
    private GameObject textBar;

    public override void Init()
    {
        base.Init();
        hpCom.Release();
        actorText = ResourceManager.Instance.GetResourceInstantiate("ActorText", GameObject.Find("ActorSliders").transform, ResouceType.PrefabItem);
        showTimeTxt = actorText.GetComponent<TextCom>();
        signBar = actorText.transform.Find("ExclamationMark").gameObject;
        signBar.SetActive(true);
        textBar = actorText.transform.Find("ActorTextBar").gameObject;
        textBar.SetActive(false);

        waitime = ConfigData.customerType[1];
    }
    /// <summary>
    /// 初始化AI
    /// </summary>
    protected override void InitAIController()
    {
        AiController = gameObject.GetOrAddComponent<RichSonAIController>();
        AiController.Initialize(this);
        originalSpeed = AiController.MoveSpeed;    //赋值原始速度
    }
    /// <summary>
    /// 鼠标点击
    /// </summary>
    private void OnMouseDown()
    {
        if (eventComplete)
            return;

        if (!isClick) isClick = true;
        AiController.MoveSpeed = 1.5f;

        if (signBar)
            signBar.SetActive(false);
        if (textBar)
            textBar.SetActive(true);

        if (!isAnimation2)
        {
            isAnimation2 = true;
            PlaySpineAnimation(0, "animation2", true);
            TimerManager.Instance.CreateUnityTimer(3f, ()=>
            {
                isAnimation2 = false;
                PlaySpineAnimation(0, "animation", true);
            });
        }


        CreateDriedFish(99, GameObject.Find("RichSonFish").transform);
    }
    /// <summary>
    /// 帧监听事件
    /// </summary>
    protected override void Update()
    {
        CheckLeave();
        if (showTimeTxt != null)
            showTimeTxt.FollowActor(this.transform.position);
        if (isClick && !eventComplete)
        {
            currentTime += Time.deltaTime;
            showTimeTxt.UpdateText((waitime - currentTime).ToString("0"));
            if (waitime - currentTime <= 0)
            {
                eventComplete = true;
                showTimeTxt.Release();
                AiController.MoveSpeed = originalSpeed;
                AiController.SetTransition(Transition.RichSonRandomMoveOver, 0);
            }
            currentResetTime += Time.deltaTime;
            if (currentResetTime >= resetSpeedTime)
            {
                currentResetTime = 0;
                AiController.MoveSpeed = originalSpeed;
            }
        }
    }
}
