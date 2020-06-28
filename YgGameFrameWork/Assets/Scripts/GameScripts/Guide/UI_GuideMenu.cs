using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideMenu : MonoBehaviour
{
    private Button btnTable;
    private Button btnStove;
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
        btnTable = transform.Find("BtnTable").GetComponent<Button>();
        btnStove = transform.Find("BtnStove").GetComponent<Button>();

        UIManager.Instance.SendUIEvent(GameEvent.SET_MENU_SCROLL, false);
        btnTable.onClick.AddListener(() =>
        {
            GuideManager.Instance.facilitiesItemData = menuModule.GetFacilitiesItemData(10002, 1005);
            UI_Tips.ShowFacilitiesTips(GuideManager.Instance.facilitiesItemData);
            UIManager.Instance.SendUIEvent(GameEvent.SET_MENU_SCROLL, true);
        });

        btnStove.onClick.AddListener(() =>
        {
            GuideManager.Instance.facilitiesItemData = menuModule.GetFacilitiesItemData(10015, 1057);
            UI_Tips.ShowFacilitiesTips(GuideManager.Instance.facilitiesItemData);
            UIManager.Instance.SendUIEvent(GameEvent.SET_MENU_SCROLL, true);
        });

        GuideManager.Instance.RegisterBtn(btnTable);
        GuideManager.Instance.RegisterBtn(btnStove);
    }

    private void OnDestroy()
    {
        btnTable.onClick.RemoveAllListeners();
        btnStove.onClick.RemoveAllListeners();
        GuideManager.Instance.RemoveBtn(btnTable.name);
        GuideManager.Instance.RemoveBtn(btnStove.name);
    }
}
