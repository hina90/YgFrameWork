using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Com_PiackupFish : MonoBehaviour
{
    protected const string UI_FISH_PREFAB = "DriedFishImg";
    protected const string FISH_NAME = "DriedFish";

    void Start()
    {
        StartCoroutine(IEnumCreateFish());
        GuideManager.Instance.AddGuideListener(GameEvent.PICKUP_FISH, FinishPickup);
    }

    IEnumerator IEnumCreateFish()
    {
        for (int i = 0; i < 3; i++)
        {
            CreateFish(150);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void CreateFish(int value)
    {
        GameObject fishPrefab = ResourceManager.Instance.GetResource(FISH_NAME, ResouceType.PrefabItem);
        int randomTemp = transform.parent.localPosition.y < -4 ? 1 : -1;
        GameObject fish = Instantiate(fishPrefab, transform);
        Vector3 targetPos = fish.transform.localPosition + new Vector3(UnityEngine.Random.Range(0.5f, 1f), 0, 0);
        FishItem fishItem = fish.GetOrAddComponent<FishItem>();
        fish.transform.DOLocalJump(targetPos, 0.5f, 2, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            GameManager.Instance.AddToSortList(fish.transform);
            fish.AddComponent<SortLayerCom>();
            fishItem.CanPickUp = true;
        });
        fishItem.Value = value;
    }

    private void FinishPickup(object[] arg)
    {
        UIManager.Instance.SendUIEvent(GameEvent.SHOW_MAINUI);
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_GUIDE);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GuideManager.Instance.RemoveGuideListener(GameEvent.PICKUP_FISH, FinishPickup);
    }
}
