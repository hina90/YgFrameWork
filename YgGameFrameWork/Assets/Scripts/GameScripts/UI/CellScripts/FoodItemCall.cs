using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodItemCall : CellBase
{
    private Text foodName;
    private Image foodIcon;
    private GameObject price;
    private TextMeshProUGUI priceText;
    private PlayerModule player;

    private void Awake()
    {
        foodName = Find<Text>(gameObject, "FoodName");
        foodIcon = Find<Image>(gameObject, "FoodIcon");
        price = Find(gameObject, "Price");
        priceText = Find<TextMeshProUGUI>(gameObject, "Price");
    }
    void ScrollCellContent(MenuData menuData)
    {

        foodName.text = LanguageManager.Get(menuData.configData.name);
        foodIcon.sprite = ResourceManager.Instance.GetSpriteResource(menuData.configData.icon, ResouceType.Icon);
        foodIcon.SetNativeSize();
        player = GameModuleManager.Instance.GetModule<PlayerModule>();
        ShowPrfInfo(menuData);

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UI_Tips.ShowFoodTips(menuData);
        });
    }
    private void ShowPrfInfo(MenuData menuData)
    {
        if (menuData.storeData.isStudy)
        {
            priceText.text = $"已拥有";
        }
        else
        {
            if (menuData.configData.price == 0)
                priceText.text = "免费";
            if (player.Fish < menuData.configData.price)
                priceText.text = $"<sprite=7><color=red>{menuData.configData.price}</color>";
            else
                priceText.text = $"<sprite=7>{menuData.configData.price}";
        }
    }
}
