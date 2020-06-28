using UnityEngine.UI;

public class UI_Progress : UIBase
{
    private Button maskBtn;
    private Button watchBtn;
    public override void Init()
    {
        Layer = LayerMenue.TIPS;
        maskBtn = Find<Button>(gameObject, "Mask");
        watchBtn = Find<Button>(gameObject, "WatchBtn");
        RegisterBtnEvent();
        PlayAnimation(Find(gameObject, "Bg"));
    }
    private void RegisterBtnEvent()
    {
        maskBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
        watchBtn.onClick.AddListener(() =>
        {
#if UNITY_ANDROID || UNITY_IOS
            SDKManager.Instance.ShowBasedVideo((string str1, string str2, float f1) =>
            {
                //看完广告后宣传X15
                TaskManager.Instance.CheckTask(TaskType.WATCH_AD, 1);           //刷新看广告任务的次数
                UIManager.Instance.SendUIEvent(GameEvent.UPDATE_PROPAGANDA);
            },
            (string str1, string str2) =>
            {

            },
            (string str1) =>
            {

            });
#endif
#if UNITY_EDITOR
            TaskManager.Instance.CheckTask(TaskType.WATCH_AD, 1);           //刷新看广告任务的次数
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_PROPAGANDA);
#endif
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.BackUI(Layer);
            SDKManager.Instance.LogEvent(EventId.CC_PropagandaYes.ToString(), "ProgressWatchAdYes", "Button");
            TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.ClickBtn, watchBtn.name);
        });
        GuideManager.Instance.RegisterBtn(watchBtn);
    }
}
