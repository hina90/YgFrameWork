using System.Collections.Generic;
using Tool.Database;
using UnityEngine.UI;

public class UI_Parterre : UIBase
{
    private Button m_BtnMask;
    private Text m_TxtName;
    private Image m_ImgItem;
    private Text m_TxtDes;
    private Button m_BtnSowing;
    private Text m_TxtSowing;

    private FacilitiesItemData facilitiesData;
    private static Callback plantingAction;
    private GardenConfigData flowerData;
    private PlayerModule playerModule;

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.UI;
        m_BtnMask = Find<Button>(gameObject, "Mask");
        m_TxtName = Find<Text>(gameObject, "TxtName");
        m_ImgItem = Find<Image>(gameObject, "ImgItem");
        m_TxtDes = Find<Text>(gameObject, "TxtDes");
        m_BtnSowing = Find<Button>(gameObject, "BtnSowing");
        m_TxtSowing = Find<Text>(m_BtnSowing.gameObject, "TxtPrice");

        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        RegisterBtnEvent();
        PlayAnimation(Find(gameObject, "msgPanel"));
    }

    private void RegisterBtnEvent()
    {
        m_BtnMask.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseUI(Layer);
        });

        m_BtnSowing.onClick.AddListener(() =>
        {
            if (playerModule.Fish < flowerData.price)
            {
                TipManager.Instance.ShowMsg("小鱼干不足，请购买!");
                return;
            }
            plantingAction?.Invoke();
            TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.Planting, null);
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, -flowerData.price);
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.CloseUI(Layer);
        });
    }

    protected override void Enter()
    {
        base.Enter();
        facilitiesData = (FacilitiesItemData)param[0];
        ShowInfo();
    }

    private void ShowInfo()
    {
        m_TxtName.text = facilitiesData.itemConfigData.name;
        flowerData = GetFlowerInfo((int)facilitiesData.itemConfigData.funParam_1[0]);
        m_ImgItem.overrideSprite = ResourceManager.Instance.GetSpriteResource(flowerData.icon, ResouceType.Icon);
        m_ImgItem.SetNativeSize();
        m_TxtDes.text = $"{flowerData.name}种子需要{flowerData.GrowthTime[0] + flowerData.GrowthTime[1]}秒开花需要播种吗？";
        m_TxtSowing.text = flowerData.price.ToString();
    }

    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        return base.CtorEvent();
    }

    public static void SetPlantingAction(Callback callback)
    {
        plantingAction = callback;
    }

    private GardenConfigData GetFlowerInfo(int Id)
    {
        return ConfigDataManager.Instance.GetDatabase<GardenConfigDatabase>().GetDataByKey(Id.ToString());
    }

    public override void Release()
    {
        base.Release();
    }
}
