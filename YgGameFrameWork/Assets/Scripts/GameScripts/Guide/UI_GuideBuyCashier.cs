using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideBuyCashier : MonoBehaviour
{
    private Button btnCashier;
    private MenuModule menuModule;
    private UI_TipsControl uiTips;
    public UI_TipsControl UI_Tips
    {
        get
        {
            UIManager.Instance.OpenUI<UI_TipsControl>();
            uiTips = UIManager.Instance.GetUI<UI_TipsControl>(typeof(UI_TipsControl).ToString()) as UI_TipsControl;
            return uiTips;
        }
    }

    void Awake()
    {
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();
        btnCashier = transform.Find("BtnCashier").GetComponent<Button>();

        UIManager.Instance.SendUIEvent(GameEvent.SET_MENU_SCROLL, false);

        btnCashier.onClick.AddListener(() =>
        {
            GuideManager.Instance.facilitiesItemData = menuModule.GetFacilitiesItemData(10001, 1001);
            UI_Tips.ShowFacilitiesTips(GuideManager.Instance.facilitiesItemData);
            UIManager.Instance.SendUIEvent(GameEvent.SET_MENU_SCROLL, true);
        });
        GuideManager.Instance.RegisterBtn(btnCashier);
    }

    private void OnDestroy()
    {
        btnCashier.onClick.RemoveAllListeners();
        GuideManager.Instance.RemoveBtn(btnCashier.name);
    }
}
