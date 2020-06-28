/// <summary>
/// 流浪歌手
/// </summary>
public class WanderingSinger : BaseSpecialActor
{
    private float singingTime = 10;      //唱歌时长

    public override void Init()
    {
        base.Init();
        pointNum = 10;                                                                           //点击10次唱歌
        addValue = float.Parse(((decimal)1 / pointNum).ToString("0.0"));                         //每次点击增加值

        eventCallback += EventCompleteCallback;
    }
    /// <summary>
    /// 初始化AI
    /// </summary>
    protected override void InitAIController()
    {
        AiController = gameObject.GetOrAddComponent<SinggerAIController>();
        AiController.Initialize(this);
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    private void OnMouseDown()
    {
        if (AiController.CurrentStateID == FSMStateID.Leave)
            return;

        if (StartOperation)
        {
            Event();
        }
    }
    /// <summary>
    /// 回调事件开始宣传
    /// </summary>
    protected override void EventCompleteCallback()
    {
        //播放音乐动画和音乐
        PlaySpineAnimation(0, "animation2", false);
        AddSpineAnimation(0, "animation3", true);

        //宣传*15
        UIManager.Instance.SendUIEvent(GameEvent.UPDATE_PROPAGANDA, false);

        TimerManager.Instance.CreateUnityTimer(singingTime, () =>
        {
            PlaySpineAnimation(0, "animation", true);
            if(AiController)
                AiController.SetTransition(Transition.SpecialPointMoveOver);
        });
    }
}
