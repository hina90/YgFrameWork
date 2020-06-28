using UnityEngine;
using UnityEngine.UI;

public class CustomerCallback : CellBase
{
    private Text cusName;
    private Image cusIcon;
    private GameObject whatGame;
    private void Awake()
    {
        cusName = Find<Text>(gameObject, "CustomerName");
        cusIcon = Find<Image>(gameObject, "CustomerIcon");
        whatGame = Find(gameObject, "What");
    }
    void ScrollCellContent(CustomerData customer)
    {
        cusName.text = customer.customerConfig.name;


        if (!customer.storeData.isActive)
        {
            cusIcon.sprite = ResourceManager.Instance.GetSpriteResource("wz", ResouceType.Icon);
            cusName.gameObject.SetActive(false);
            whatGame.SetActive(true);
        }
        else
        {
            cusIcon.sprite = ResourceManager.Instance.GetSpriteResource(customer.customerConfig.icon, ResouceType.Icon);
            cusName.gameObject.SetActive(true);
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
