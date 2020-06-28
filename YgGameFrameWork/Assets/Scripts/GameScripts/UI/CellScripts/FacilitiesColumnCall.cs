using TMPro;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

public class FacilitiesColumnCall : CellBase
{
    private MenuModule menuModule;
    private Transform content;
    private LayoutElement layoutElement;
    private Image nameImg;
    private PlayerModule playerModule;
    private void Awake()
    {
        nameImg = Find<Image>(gameObject, "Name");
        content = Find(gameObject, "FacilitiesContent").transform;
    }
    void ScrollCellContent(FacilitiesConfigData facData)
    {
        nameImg.sprite = ResourceManager.Instance.GetSpriteResource(facData.nameIcon, ResouceType.FacilitiesTitle);
        nameImg.SetNativeSize();
        menuModule = GameModuleManager.Instance.GetModule<MenuModule>();
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();

        layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = facData.itemNumber > 3 ? 351 : 191;

        for (int i = 0; i < 4; i++)
        {
            int itemID = (int)typeof(FacilitiesConfigData).GetField("item_" + (i + 1)).GetValue(facData);
            if (itemID != 0)
            {
                GameObject facItemPref;
                if (i >= content.childCount)
                    facItemPref = ResourceManager.Instance.GetResourceInstantiate("FacilitiesItemPref", content, ResouceType.PrefabItem);
                else
                    facItemPref = content.GetChild(i).gameObject;
                ShowItemInfo(facItemPref, menuModule.GetFacilitiesItemData(facData, itemID));
                continue;
            }
            if (i < content.childCount)
                Destroy(content.GetChild(i).gameObject);
        }
    }
    private void ShowItemInfo(GameObject facItemPref, FacilitiesItemData fac)
    {
        Transform unlockImg = facItemPref.transform.Find("Unlock");
        Image icon = facItemPref.transform.Find("Icon").GetComponent<Image>();
        Button useImg = facItemPref.transform.Find("Use").GetComponent<Button>();
        TextMeshProUGUI priceText = facItemPref.transform.Find("Price").GetComponent<TextMeshProUGUI>();

        bool isUse = menuModule.CheckIsUseFacilityItem(fac.facilitiesConfigData.id, fac.itemId);
        bool systemIsUnlock = SystemManager.Instance.GetSystemIsUnlock(int.Parse($"100{fac.facilitiesConfigData.type}"));

        facItemPref.GetComponent<Button>().onClick.RemoveAllListeners();
        icon.sprite = ResourceManager.Instance.GetSpriteResource(fac.itemConfigData.uiIcon, ResouceType.Facility);

        if (fac.storeData.isPurchase)//已购买
        {
            priceText.text = $"已拥有";
        }
        else
        {
            //priceText.text = $"<sprite=7>{fac.itemConfigData.price}";
            if (playerModule.Fish < fac.itemConfigData.price)
                priceText.text = $"<sprite=7><color=red>{fac.itemConfigData.price}</color>";
            else
                priceText.text = $"<sprite=7>{fac.itemConfigData.price}";
        }

        if (isUse)//已使用
        {
            useImg.gameObject.SetActive(true);
        }
        else
        {
            useImg.gameObject.SetActive(false);
        }

        if (!systemIsUnlock)//系统是否解锁
        {
            unlockImg.gameObject.SetActive(true);
        }
        else
        {
            unlockImg.gameObject.SetActive(false);
        }

        facItemPref.GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            if (!systemIsUnlock)
            {
                TipManager.Instance.ShowMsg("区域未解锁");
                return;
            }
            UI_Tips.ShowFacilitiesTips(fac);
        });
    }
}
