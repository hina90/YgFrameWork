using UnityEngine;
using DG.Tweening;

public class FishItem : RewardItem
{
    protected override void OnInit()
    {
        base.OnInit();
        uiPrefab = "DriedFishImg";
        targetPos = new Vector2(-298, 588);
    }

    public override void ClickEffect()
    {
        //TDDebug.Log("当前小鱼干禁止触摸状态:" + GuideManager.Instance.forbidTouch + "-------------------是否可捡取:" + CanPickUp);
        if (!CanPickUp || GuideManager.Instance.forbidTouch) return;
        FishItem[] fishDatas = transform.parent.GetComponentsInChildren<FishItem>();
        for (int i = 0; i < fishDatas.Length; i++)
        {
            if (!fishDatas[i].CanPickUp) continue;
            totalValue += Value;
            GameObject fish = ResourceManager.Instance.GetResourceInstantiate(uiPrefab, uiMoney.transform, ResouceType.PrefabItem);
            Utils.World2ScreenPos(fishDatas[i].transform.position, fish.GetComponent<RectTransform>());
            fishDatas[i].gameObject.SetActive(false);
            Destroy(fishDatas[i].gameObject, 1);
            fish.transform.DOLocalMove(targetPos, 1f);
            Destroy(fish.gameObject, 1f);
            GameManager.Instance.RemoveFromSortList(fishDatas[i].gameObject.transform);
        }
        if (totalValue == 0) return;
        IncomeTipManager.Instance.ShowIncomeTip(totalValue, transform.position);
        Invoke("UpdateValue", 0.9f);
        ///移除小鱼干
        FacilityEntity facilityEntity = transform.parent.GetComponentInChildren<FacilityEntity>();
        if (facilityEntity == null) return;
        FacilitiesManager.Instance.RemoveFish(facilityEntity.FacilitiesData.itemConfigData.Id);
    }

    /// <summary>
    /// 获取小鱼干事件
    /// </summary>
    protected override void UpdateValue()
    {
        UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, Value);
        //新手引导广播事件
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.PICKUP_FISH);
    }

    /// <summary>
    /// 偷取小鱼干
    /// </summary>
    public void StealFish()
    {
        //Debug.Log("偷去了小鱼干---" + transform.parent.name);
        ///从小鱼干移除小鱼干
        FacilityEntity facilityEntity = transform.parent.parent.GetComponentInChildren<FacilityEntity>();
        FacilitiesManager.Instance.RemoveFish(facilityEntity.FacilitiesData.itemConfigData.Id);
        ///销毁该设施所有小鱼干
        FishItem[] fishDatas = transform.parent.GetComponentsInChildren<FishItem>();
        for (int i = 0; i < fishDatas.Length; i++)
        {
            GameManager.Instance.RemoveFromSortList(fishDatas[i].gameObject.transform);
            Destroy(fishDatas[i].gameObject);
        }
    }
}
