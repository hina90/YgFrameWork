public class ClickEffect : Singleton<ClickEffect>
{
    private UI_ClickEffect uiClick;
    public ClickEffect()
    {
        UIManager.Instance.OpenUI<UI_ClickEffect>();
        uiClick = UIManager.Instance.GetUI<UI_ClickEffect>(typeof(UI_ClickEffect).ToString()) as UI_ClickEffect;
    }

    public void ShowClickEffect()
    {
        uiClick.ShowClickEffect();
    }
}
