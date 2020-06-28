using Tool.Database;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParterreBubble : MonoBehaviour
{
    private FacilitiesItemData facilitiesData;
    private Callback plantingAction;
    private SpriteRenderer seedSprite;
    private GardenConfigData flowerConfig;

    private void Awake()
    {
        seedSprite = transform.Find("seed").GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
        UIManager.Instance.OpenUI<UI_Parterre>(facilitiesData);
        UI_Parterre.SetPlantingAction(plantingAction);
        transform.localScale = Vector2.one * 1.1f;
    }

    private void OnMouseUp()
    {
        transform.localScale = Vector2.one;
    }

    /// <summary>
    /// 绑定设施数据
    /// </summary>
    /// <param name="facilitiesData"></param>
    internal void SetData(FacilitiesItemData facilitiesData)
    {
        this.facilitiesData = facilitiesData;
        int flowerId = (int)(facilitiesData.itemConfigData.funParam_1[0]);
        ShowSeedSprite(flowerId);
    }

    /// <summary>
    /// 显示花圃可种植花类
    /// </summary>
    /// <param name="flowerId"></param>
    private void ShowSeedSprite(int flowerId)
    {
        flowerConfig = ConfigDataManager.Instance.GetDatabase<GardenConfigDatabase>().GetDataByKey(flowerId.ToString());
        //seedSprite.sprite = null; //ResourceManager.Instance.GetSpriteResource(flowerConfig.icon, ResouceType.Icon);
    }

    /// <summary>
    /// 设置种植花种回调
    /// </summary>
    /// <param name="callback"></param>
    internal void SetPlantingAction(Callback callback)
    {
        plantingAction = callback;
    }
}
