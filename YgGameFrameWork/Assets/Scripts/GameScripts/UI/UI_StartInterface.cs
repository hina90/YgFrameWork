using UnityEngine.UI;

public class UI_StartInterface : UIBase
{
    private Button playBtn;
    private void Awake()
    {
        playBtn = Find<Button>(gameObject, "playBtn");
    }
    private void Start()
    {
        playBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            CSceneManager.Instance.ChangeScene("MainScene");
            SDKManager.Instance.LogEvent(EventId.CC_PlayButton.ToString(), "StarGamePlay", "Button");
        });   
    }
}
