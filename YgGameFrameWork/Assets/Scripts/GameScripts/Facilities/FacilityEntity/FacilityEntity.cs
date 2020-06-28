using DG.Tweening;
using Spine.Unity;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 基本设施实体
/// </summary>
public class FacilityEntity : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    protected FacilitiesItemData facilitiesData;
    protected UI_Money uiMoney;
    protected const string UI_FISH_PREFAB = "DriedFishImg";
    protected const string FISH_NAME = "DriedFish";
    protected readonly Vector2 targetPos = new Vector2(-296, 585);
    protected float boxColliderOffset;
    private Transform fishParents;
    //protected const string HEART_NAME = "Heart";
    //private Transform heartParents;

    protected FacilityModule facilityModule;

    public FacilitiesItemData FacilitiesData { get => facilitiesData; }

    void Awake()
    {
        OnInit();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void OnInit()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        facilityModule = GameModuleManager.Instance.GetModule<FacilityModule>();
        uiMoney = UIManager.Instance.GetUI<UI_Money>("UI_Money");
    }

    /// <summary>
    /// 绑定设施数据
    /// </summary>
    /// <param name="facilitiesData"></param>
    public void SetData(FacilitiesItemData facilitiesData)
    {
        this.facilitiesData = facilitiesData;
        facilitiesData.gameObject = gameObject;
        if (facilitiesData.facilitiesConfigData.id == 10012)
        {
            spriteRenderer.sortingOrder = 500;
        }
        //if (facilitiesData.facilitiesConfigData.id == 10014)
        //{
        //    spriteRenderer.sortingOrder = 1;
        //}
        if (facilitiesData.facilitiesConfigData.id == 10046)
        {
            spriteRenderer.sortingOrder = 600;
        }
        if (facilitiesData.facilitiesConfigData.id == 10047)
        {
            spriteRenderer.sortingOrder = 600;
        }
        RefeshFacilityLogic();
    }

    /// <summary>
    /// 更换设施替换设施显示精灵图片
    /// </summary>
    /// <param name="facilitiesData"></param>
    public void ReplaceFacility(FacilitiesItemData facilitiesData, SpriteRenderer doorEdge)
    {
        this.facilitiesData = facilitiesData;
        facilitiesData.gameObject = gameObject;
        spriteRenderer.sprite = ResourceManager.Instance.GetSpriteResource(facilitiesData.itemConfigData.icon, ResouceType.Facility);
        boxColliderOffset = spriteRenderer.sprite.rect.height / 200;
        //Debug.Log($"当前设施名称为：{facilitiesData.itemConfigData.name}");
        if (facilitiesData.facilitiesConfigData.isInteractive == 1)
        {
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D != null)
            {
                DestroyImmediate(boxCollider2D);
            }
            BoxCollider2D newBoxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            newBoxCollider2D.offset = new Vector2(0, boxColliderOffset);
        }
        RefeshFacilityLogic();
        if (facilitiesData.facilitiesConfigData.id == 10011)
        {
            //当建造的设施为门时，替换门装饰背景
            ItemConfigData itemData = ConfigDataManager.Instance.GetDatabase<ItemConfigDatabase>().GetDataByKey(facilitiesData.itemId.ToString());
            doorEdge.sprite = ResourceManager.Instance.GetSpriteResource("ct_m2_" + itemData.level, ResouceType.Facility);
        }
        //显示设施特效
        if (facilitiesData.facilitiesConfigData.type == 1) return;
        Transform trans = facilitiesData.gameObject.transform.Find("anim");
        if (trans == null) return;
        if (facilitiesData.itemConfigData.effect == 1)
        {
            spriteRenderer.sprite = null;
            trans.gameObject.SetActive(true);
            SkeletonAnimation skeAnim = trans.GetComponent<SkeletonAnimation>();
            FacilityEffectList effects = trans.GetComponent<FacilityEffectList>();
            skeAnim.skeletonDataAsset = effects.animAssets[facilitiesData.itemConfigData.effectIndex];
            skeAnim.Initialize(true);
            float[] pos = facilitiesData.itemConfigData.effectPos;
            if (pos.Length > 0)
            {
                trans.localPosition = new Vector3(pos[0], pos[1], 0);
            }
        }
        else
        {
            trans.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 替换设施刷新设施逻辑
    /// </summary>
    protected virtual void RefeshFacilityLogic() { }

    /// <summary>
    /// 设施点击触发效果
    /// </summary>
    public void OnMouseDown()
    {
        if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
        //防止UI界面穿透
        //if (UIManager.Instance.transform.Find("UI_TipMsg") == null)
        //{
        //    if (UIManager.Instance.transform.childCount > 3) return;
        //}
        //else
        //{
        //    if (UIManager.Instance.transform.childCount > 4) return;
        //}
        ClickEffect();
    }

    /// <summary>
    /// 点击功能效果
    /// </summary>
    public virtual void ClickEffect() { }

    /// <summary>
    /// 创建一个小鱼干
    /// </summary>
    protected GameObject CreateDriedFish(int value)
    {
        if (fishParents == null)
        {
            fishParents = new GameObject("fishParent").transform;
            fishParents.SetParent(transform.parent);
            fishParents.localPosition = Vector3.up * boxColliderOffset;
        }

        GameObject fishPrefab = ResourceManager.Instance.GetResource(FISH_NAME, ResouceType.PrefabItem);
        int randomTemp = transform.parent.localPosition.y < -4 ? 1 : -1;
        GameObject fish = Instantiate(fishPrefab, fishParents);
        Vector3 targetPos = fish.transform.localPosition + new Vector3(Random.Range(-0.5f, 0.5f),
            Random.Range(randomTemp * 1f, randomTemp * 1.5f), 0);
        FishItem fishItem = fish.GetOrAddComponent<FishItem>();
        fishItem.Value = value;
        fish.transform.DOLocalJump(targetPos, 0.5f, 2, 1f).SetEase(Ease.Linear).OnComplete(() =>
         {
             //GameManager.Instance.AddToSortList(fish.transform);
             //fish.AddComponent<SortLayerCom>();
             //fishItem.CanPickUp = true;
             fishItem.Disappear();

         });
        //FacilitiesManager.Instance.AddFish(facilitiesData.facilitiesConfigData.id, fishItem);
        return fish;
    }

    /// <summary>
    /// 创建一个心心
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>

    //protected GameObject CreateHeart(int value)
    //{
    //    if (heartParents == null)
    //    {
    //        heartParents = new GameObject("heartParent").transform;
    //        heartParents.SetParent(transform.parent);
    //        heartParents.localPosition = Vector3.up * boxColliderOffset;
    //    }

    //    GameObject heartPrefab = ResourceManager.Instance.GetResource(HEART_NAME, ResouceType.PrefabItem);
    //    int randomTemp = transform.parent.localPosition.y < -4 ? 1 : -1;
    //    GameObject heart = Instantiate(heartPrefab, heartParents);
    //    Vector3 targetPos = heart.transform.localPosition + new Vector3(Random.Range(-0.5f, 0.5f),
    //        Random.Range(randomTemp * 1f, randomTemp * 1.5f), 0);
    //    HeartItem heartItem = heart.GetOrAddComponent<HeartItem>();
    //    heart.transform.DOLocalJump(targetPos, 0.5f, 2, 1f).SetEase(Ease.Linear).OnComplete(() =>
    //    {
    //        GameManager.Instance.AddToSortList(heart.transform);
    //        heart.AddComponent<SortLayerCom>();
    //        heartItem.CanPickUp = true;
    //    });
    //    heartItem.Value = value;
    //    return heart;
    //}
}
