using UnityEngine;
using UnityEngine.UI;

public class StaffPrefCall : CellBase
{
    private Text staffName;
    private Image cusIcon;
    private GameObject whatGame;
    private void Awake()
    {
        staffName = Find<Text>(gameObject, "StaffName");
        cusIcon = Find<Image>(gameObject, "StaffIcon");
        whatGame = Find(gameObject, "What");
    }
    void ScrollCellContent(CustomerData customer)
    {
        staffName.text = customer.customerConfig.name;


        if (!customer.storeData.isActive)
        {
            cusIcon.sprite = ResourceManager.Instance.GetSpriteResource("wz", ResouceType.Icon);
            staffName.gameObject.SetActive(false);
            whatGame.SetActive(true);
        }
        else
        {
            cusIcon.sprite = ResourceManager.Instance.GetSpriteResource(customer.customerConfig.icon, ResouceType.Icon);
            staffName.gameObject.SetActive(true);
            whatGame.SetActive(false);
        }
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UI_Tips.ShowCustomerTips(customer);
        });
    }
}
