using DG.Tweening;
using UnityEngine;

public class HeartItem : RewardItem
{
    protected override void OnInit()
    {
        base.OnInit();
        uiPrefab = "HeartImg";
        targetPos = new Vector2(-300, 525);
    }

    public override void ClickEffect()
    {
        if (!CanPickUp) return;
        HeartItem[] heartDatas = transform.parent.GetComponentsInChildren<HeartItem>();
        for (int i = 0; i < heartDatas.Length; i++)
        {
            if (!heartDatas[i].CanPickUp) continue;
            totalValue += Value;
            GameObject reward = ResourceManager.Instance.GetResourceInstantiate(uiPrefab, uiMoney.transform, ResouceType.PrefabItem);
            Utils.World2ScreenPos(heartDatas[i].transform.position, reward.GetComponent<RectTransform>());
            heartDatas[i].gameObject.SetActive(false);
            Destroy(heartDatas[i].gameObject, 1);
            reward.transform.DOLocalMove(targetPos, 1f);
            Destroy(reward.gameObject, 1f);
            GameManager.Instance.RemoveFromSortList(heartDatas[i].gameObject.transform);
        }
        if (totalValue == 0) return;
        IncomeTipManager.Instance.ShowIncomeTip(totalValue, transform.position);
        Invoke("UpdateValue", 0.9f);
    }

    /// <summary>
    /// 获取心心事件
    /// </summary>
    protected override void UpdateValue()
    {
        UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, Value);
    }
}
