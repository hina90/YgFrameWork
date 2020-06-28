using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 炉子设施实体
/// </summary>
public class StoveEntity : FacilityEntity
{
    private SpriteRenderer menuRender;
    private SpriteRenderer menuProgressRender;
    private MenuConfigData menuConfig;
    private TableEntity tableEntity;
    private Callback cookOverCallBack;
    private ParticleSystem smokingEffect;

    private bool isCooking;
    private float curTime;
    private float cookTime;
    private Transform menuTrans;
    private Vector3 initPos;

    public bool IsCooking { get => isCooking; }

    protected override void OnInit()
    {
        base.OnInit();
        menuTrans = transform.Find("foodPoint");
        menuRender = menuTrans.GetComponent<SpriteRenderer>();
        menuProgressRender = transform.Find("cookProgress").GetComponent<SpriteRenderer>();
        smokingEffect = transform.Find("Particle_Smoking").GetComponent<ParticleSystem>();
        initPos = menuTrans.position;
    }

    private void Update()
    {
        //烹饪食物
        Cooking();
    }

    /// <summary>
    /// 开始做菜
    /// </summary>
    public void StartCooking(CookData cookData)
    {
        menuConfig = ConfigDataManager.Instance.
            GetDatabase<MenuConfigDatabase>().GetDataByKey(cookData.menuId.ToString()); ;
        tableEntity = cookData.tableEntity;
        isCooking = true;
        cookTime = menuConfig.makeTime;
        if (facilityModule.FacilityAddValueCache.ContainsKey(facilitiesData.facilitiesConfigData.id))
        {
            cookTime *= (1 - (facilityModule.FacilityAddValueCache[facilitiesData.facilitiesConfigData.id] + cookData.additionRatio));
        }
        else
        {
            cookTime *= (1 - cookData.additionRatio);
        }
        //Debug.Log("菜单图片:" + menuConfig.icon);
        menuRender.sprite = ResourceManager.Instance.GetSpriteResource(menuConfig.icon, ResouceType.Icon);
        menuProgressRender.sprite = menuRender.sprite;
        menuProgressRender.material.SetFloat("_ProgressValue", 1);
        cookOverCallBack = cookData.callback;
        smokingEffect.gameObject.SetActive(true);
    }

    /// <summary>
    /// 烹饪食物
    /// </summary>
    private void Cooking()
    {
        if (!isCooking) return;
        curTime += Time.deltaTime;
        //Debug.Log($"{facilitiesData.facilitiesConfigData.name}正在烹饪食物--{menuConfig.name}");
        menuProgressRender.material.SetFloat("_ProgressValue", (cookTime - curTime) / cookTime);
        if (curTime >= cookTime)
        {
            //Debug.Log($"食物--{menuConfig.name}--烹饪完成!");
            curTime = 0;
            CookComplete();
        }
    }

    /// <summary>
    /// 烹饪完成
    /// </summary>
    private void CookComplete()
    {
        menuTrans.DOMove(new Vector2(4f, 8), 0.5f).OnComplete(() =>
        {
            menuRender.sprite = null;
            menuTrans.position = initPos;
            tableEntity.DishUp(menuConfig);
            //Debug.Log(tableEntity.transform.parent.name);
            cookOverCallBack?.Invoke();
            isCooking = false;
        });
        menuProgressRender.sprite = null;
        smokingEffect.gameObject.SetActive(false);
        //完成观看烹饪过程引导
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.AUTO_FINISH);
    }
}
