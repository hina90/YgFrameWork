using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 收银台设施实体
/// </summary>
public class CashierEntity : FacilityEntity
{
    private GameObject gratuityObj;
    private TextMeshPro txtGratuity;
    private float curTime = 0;

    protected override void OnInit()
    {
        base.OnInit();
        gratuityObj = transform.Find("gratuity").gameObject;
        txtGratuity = gratuityObj.transform.Find("TxtGratuity").GetComponent<TextMeshPro>();
        if (facilityModule.CurGratuity > 0) UpdateGratuity();
    }

    private void Update()
    {
        //更新收银台收益效果
        Cashier();
    }

    /// <summary>
    /// 收银台点击收益效果
    /// </summary>
    public override void ClickEffect()
    {
        base.ClickEffect();
        if (facilityModule.CurGratuity <= 0) return;
        UIManager.Instance.OpenUI<UI_Cashier>(facilityModule.CurGratuity);
        UI_Cashier.SetGainFishActin(GainFish);
    }

    /// <summary>
    /// 收获小鱼干行为
    /// </summary>
    /// <param name="multiple">倍数</param>
    private void GainFish(int multiple = 1)
    {
        GameObject fish = ResourceManager.Instance.GetResourceInstantiate(UI_FISH_PREFAB, uiMoney.transform, ResouceType.PrefabItem);
        Utils.World2ScreenPos(gratuityObj.transform.position, fish.GetComponent<RectTransform>());
        int gratuity = facilityModule.CurGratuity;
        fish.transform.DOLocalMove(targetPos, 0.8f).OnComplete(() =>
        {
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, gratuity * multiple);
            Destroy(fish.gameObject);
        });
        ResetGratuity();
    }

    /// <summary>
    /// 重置小费台小费
    /// </summary>
    public void ResetGratuity()
    {
        facilityModule.CurGratuity = 0;
        gratuityObj.SetActive(false);
    }

    /// <summary>
    /// 小费台小费显示
    /// </summary>
    public void UpdateGratuity()
    {
        if (gratuityObj.activeInHierarchy == false)
        {
            gratuityObj.SetActive(true);
        }
        txtGratuity.text = $"小费<sprite=7>{facilityModule.CurGratuity}";
    }

    /// <summary>
    /// 餐厅小费收入
    /// </summary>
    private void Cashier()
    {
        if (facilityModule.GratuityPerMinute == 0) return;
        if (facilityModule.CurGratuity >= facilityModule.MaxGratuity) return;
        curTime += Time.deltaTime;
        if (curTime >= 60)
        {
            facilityModule.CurGratuity = Mathf.Clamp(facilityModule.CurGratuity + facilityModule.GratuityPerMinute, 0, facilityModule.MaxGratuity);
            //Debug.Log("收银台每分钟收益:" + facilityModule.GratuityPerMinute);
            Vector3 pos = transform.position;
            pos.y = pos.y + 1f;
            IncomeTipManager.Instance.ShowIncomeTip(facilityModule.GratuityPerMinute, pos);
            UpdateGratuity();
            curTime = 0;
        }
    }
}
