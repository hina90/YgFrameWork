using UnityEngine;
using UnityEngine.UI;

public class UI_Exit : UIBase
{
    private Button yesBtn;
    private Button noBtn;
    private Button bgReturn;

    public override void Init()
    {
        Layer = LayerMenue.UI;
        PlayAnimation(Find(gameObject, "Exit"));
    }
    protected override void Enter()
    {
        yesBtn = Find<Button>(gameObject, "Yes");
        noBtn = Find<Button>(gameObject, "No");
        bgReturn = Find<Button>(gameObject, "bg");

        yesBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            Application.Quit();
        });
        noBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.BackUI(Layer);
        });
        bgReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
    }

}
