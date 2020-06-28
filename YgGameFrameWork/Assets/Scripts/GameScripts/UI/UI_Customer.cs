using SG;
using UnityEngine;
using UnityEngine.UI;

public class UI_Customer : UIBase
{
    private Button exitBtn;
    private Button bgReturn;
    private Toggle especialCustomerBtn;
    private Toggle ordinaryCustomerBtn;
    private GameObject[] pans = new GameObject[2];
    private LoopVerticalScrollRect especialContent;
    private InitOnStart especialCount;
    private LoopVerticalScrollRect customerContent;
    private InitOnStart customerCount;
    private Text customerNum;

    private CustomerModule customerModule;
    public override void Init()
    {
        Layer = LayerMenue.UI;
        customerModule = GameModuleManager.Instance.GetModule<CustomerModule>();
        PlayAnimation(Find(gameObject, "Scale"));

        //顾客页面数据
        customerContent = Find<LoopVerticalScrollRect>(gameObject, "CustomerPan");
        customerCount = Find<InitOnStart>(gameObject, "CustomerPan");

        //特殊顾客的页面数据
        especialContent = Find<LoopVerticalScrollRect>(gameObject, "EspecialCustomerPan");
        especialCount = Find<InitOnStart>(gameObject, "EspecialCustomerPan");
    }
    protected override void Enter()
    {
        exitBtn = Find<Button>(gameObject, "ExitBtn");
        bgReturn = Find<Button>(gameObject, "bg");
        especialCustomerBtn = Find<Toggle>(gameObject, "EspecialCustomerBtn");
        ordinaryCustomerBtn = Find<Toggle>(gameObject, "CustomerBtn");
        customerNum = Find<Text>(gameObject, "CustomerNum");
        pans = new GameObject[]
        {
            Find(gameObject, "EspecialCustomerBg"),
            Find(gameObject, "CustomerBg"),
        };
        ordinaryCustomerBtn.image.alphaHitTestMinimumThreshold = 0.1f;
        AddListener();
        ordinaryCustomerBtn.isOn = true;
    }
    private bool playAudio = false;
    private void AddListener()
    {
        exitBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.BackUI(Layer);
        });
        bgReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
        //特殊顾客
        especialCustomerBtn.onValueChanged.AddListener((bool fyx) =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            pans[0].SetActive(true);
            pans[1].SetActive(false);
            especialCount.totalCount = customerModule.GetEspecialCustomerData().Count;
            especialContent.objectsToFill = customerModule.GetEspecialCustomerData().ToArray();
        });
        //普通顾客
        ordinaryCustomerBtn.onValueChanged.AddListener((bool fyx) =>
        {
            if (playAudio)
                AudioManager.Instance.PlayUIAudio("button_1");
            pans[0].SetActive(false);
            pans[1].SetActive(true);
            customerCount.totalCount = customerModule.GetOrdinaryCustomerData().Count;
            customerContent.objectsToFill = customerModule.GetOrdinaryCustomerData().ToArray();
            customerNum.text = $"{customerModule.GetUnlockedCustomer()}/{customerModule.GetAllCustomer().Count}";
            playAudio = true;
        });
    }
    public override void Release()
    {
        CachePoolManager.Instance.DestroyPool("CustomerPref");
        CachePoolManager.Instance.DestroyPool("EspecialCusPref");
        base.Release();
    }
}
