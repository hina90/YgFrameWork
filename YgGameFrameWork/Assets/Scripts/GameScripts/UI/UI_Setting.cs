using UnityEngine.UI;

public class UI_Setting : UIBase
{
    private Button bgExit;
    private Button exitBtn;
    private Button downBGMBtn;
    private Button openBGMBtn;
    private Button downAudioBtn;
    private Button openAudioBtn;
    public override void Init()
    {
        Layer = LayerMenue.UI;
        PlayAnimation(Find(gameObject, "Setting"));

        bgExit = Find<Button>(gameObject, "bg");
        exitBtn = Find<Button>(gameObject, "ExitBtn");
        downBGMBtn = Find<Button>(gameObject, "DownBGMBtn");
        openBGMBtn = Find<Button>(gameObject, "OpenBGMBtn");
        downAudioBtn = Find<Button>(gameObject, "DownAudioBtn");
        openAudioBtn = Find<Button>(gameObject, "OpenAudioBtn");

        if (AudioManager.Instance.BGMVolume == 0)
            openBGMBtn.gameObject.SetActive(true);
        else
            downBGMBtn.gameObject.SetActive(true);
        if (AudioManager.Instance.AudioVolum == 0)
            openAudioBtn.gameObject.SetActive(true);
        else
            downAudioBtn.gameObject.SetActive(true);
        RegisterButtonEvent();
    }
    private void RegisterButtonEvent()
    {
        bgExit.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
        exitBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.BackUI(Layer);
        });
        downBGMBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            openBGMBtn.gameObject.SetActive(true);
            AudioManager.Instance.BGMVolume = 0;
            downBGMBtn.gameObject.SetActive(false);
        });
        openBGMBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            downBGMBtn.gameObject.SetActive(true);
            AudioManager.Instance.BGMVolume = 1;
            openBGMBtn.gameObject.SetActive(false);
        });
        downAudioBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            openAudioBtn.gameObject.SetActive(true);
            downAudioBtn.gameObject.SetActive(false);
            AudioManager.Instance.AudioVolum = 0;
        });
        openAudioBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            downAudioBtn.gameObject.SetActive(true);
            AudioManager.Instance.AudioVolum = 1;
            openAudioBtn.gameObject.SetActive(false);
        });
    }
}
