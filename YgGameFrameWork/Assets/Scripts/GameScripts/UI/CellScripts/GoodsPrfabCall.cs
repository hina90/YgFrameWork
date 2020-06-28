using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

public class GoodsPrfabCall : CellBase
{
    void ScrollCellContent(StoreConfigData goodsData)
    {
        transform.Find("GoodsName").GetComponent<Text>().text = goodsData.name;
        Image goodIcon = transform.Find("GoodsIcon").GetComponent<Image>();
        goodIcon.sprite = ResourceManager.Instance.GetSpriteResource(goodsData.icon, ResouceType.Goods);
        goodIcon.SetNativeSize();
        TextMeshProUGUI meshPro = transform.Find("Price").GetComponent<TextMeshProUGUI>();
        meshPro.text = $"<sprite=7>{goodsData.costFish}";

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            UI_Tips.ShowGoodsTips(goodsData, LoadCallBack);
        });
    }

    /// <summary>
    /// 上货回调
    /// </summary>
    private void LoadCallBack(StoreConfigData storeData)
    {
        UI_Goods uI_Goods = UIManager.Instance.GetResUI("UI_Goods") as UI_Goods;
        uI_Goods.LoadCallBack(storeData);
    }
}
