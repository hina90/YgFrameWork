using UnityEngine;
using DG.Tweening;

/// <summary>
/// 许愿池设施实体
/// </summary>
public class WishingPoolEntity : FacilityEntity
{
    private GardenModule gardenModule;
    enum RewardType { DriedFish, Heart, }
    private GameObject bubbleObj;

    protected override void OnInit()
    {
        base.OnInit();
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
        bubbleObj = transform.Find("bubble").gameObject;
        TimerManager.Instance.CreateTimer("WishingPoolTimer", 0, 1, () =>
        {
            bubbleObj.SetActive(gardenModule.RemainResetTime <= 0);
        });
    }

    public override void ClickEffect()
    {
        base.ClickEffect();
        //if (Utils.CheckIsClickOnUI()) return;
        UIManager.Instance.OpenUI<UI_WishingPool>();
        UI_WishingPool.SetWishActin(DropReward);
    }

    /// <summary>
    /// 许愿池掉落奖励
    /// </summary>
    private void DropReward()
    {
        TaskManager.Instance.CheckTask(TaskType.VOW, 1);
        for (int i = 0; i < gardenModule.WishRewardNum + 1; i++)
        {
            int randomValue = 0;
            if (i == 0)
            {
                randomValue = Random.Range(30000, 50000);
            }
            if (i == 1)
            {
                randomValue = Random.Range(5, 10);
            }
            CreateReward((RewardType)i, randomValue);
        }
    }

    /// <summary>
    /// 创建奖励物
    /// </summary>
    private GameObject CreateReward(RewardType rewardType, int value)
    {
        GameObject reward = ResourceManager.Instance.GetResourceInstantiate(rewardType.ToString(), transform.parent, ResouceType.PrefabItem);
        reward.AddComponent<SortLayerCom>();
        reward.transform.localPosition = Vector3.up * boxColliderOffset;
        float[] randomX = new float[] { Random.Range(-1.5f, -2f), Random.Range(1.5f, 2f) };
        Vector3 targetPos = reward.transform.localPosition + new Vector3(randomX[Random.Range(0, 2)], Random.Range(-0.2f, 0.2f), 0);
        RewardItem rewardItem = null;
        switch (rewardType)
        {
            case RewardType.DriedFish:
                rewardItem = reward.GetOrAddComponent<FishItem>();
                break;
            case RewardType.Heart:
                rewardItem = reward.GetOrAddComponent<HeartItem>();
                break;
            default:
                break;
        }
        reward.transform.DOLocalJump(targetPos, 2.5f, 1, 1).OnComplete(() =>
        {
            //GameManager.Instance.AddToSortList(reward.transform);
            //reward.AddComponent<SortLayerCom>();
            //rewardItem.CanPickUp = true;
            rewardItem.Disappear();
        });
        rewardItem.Value = value;
        return reward;
    }
}
