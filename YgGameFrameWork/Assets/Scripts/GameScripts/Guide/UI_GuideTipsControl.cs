using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideTipsControl : MonoBehaviour
{
    private Button btnBuy;
    private MenuModule menuModule;

    void Awake()
    {
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();
        FacilitiesItemData facData = GuideManager.Instance.facilitiesItemData;
        btnBuy = transform.Find("BtnBuy").GetComponent<Button>();
        GuideManager.Instance.RegisterBtn(btnBuy);

        btnBuy.onClick.AddListener(() =>
        {
            FacilitiesManager.Instance.PurchaseFacility(facData);
            facData.storeData.isPurchase = true;
            menuModule.SetUseFacilityItem(facData.facilitiesConfigData.id, facData.itemId);//使用该设施
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, -facData.itemConfigData.price);
            ////退出所有三级二级页面
            UIManager.Instance.BackUI(LayerMenue.TIPS);
            UIManager.Instance.BackUI(LayerMenue.UI);
            TipManager.Instance.ShowMsg("购买设施道具：" + facData.itemConfigData.name);
        });
    }
}
