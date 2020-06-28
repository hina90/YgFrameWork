using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using Spine;
using System;

public class SystemUnlockCell : MonoBehaviour
{
    /// <summary>
    /// 设施类型
    /// </summary>
    public FacilitiesType systemType;
    /// <summary>
    /// 系统ID
    /// </summary>
    private int systemID;
    /// <summary>
    /// 系统对应数据
    /// </summary>
    private SystemData systemData;
    /// <summary>
    /// 解锁按钮上的显示价格
    /// </summary>
    private TextMeshProUGUI price;
    /// <summary>
    /// 解锁按钮下的显示条件
    /// </summary>
    private TextMeshProUGUI condition;
    /// <summary>
    /// 告示牌上的显示文字
    /// </summary>
    private TextMeshProUGUI brandText;
    /// <summary>
    /// 告示牌
    /// </summary>
    private GameObject brand;
    /// <summary>
    /// 解锁按钮
    /// </summary>
    private Button unlockBtn;
    /// <summary>
    /// 父物体NoOpen
    /// </summary>
    private GameObject fatherObj;
    private GameObject uncleObj;
    /// <summary>
    /// 玩家数据模型
    /// </summary>
    private PlayerModule playerModule;
    /// <summary>
    /// 告示牌动画
    /// </summary>
    private SkeletonAnimation brandAni;
    private bool playAni = true;
    private void Start()
    {
        systemID = int.Parse($"100{(int)systemType}");
        fatherObj = this.transform.parent.gameObject;
        uncleObj = fatherObj.transform.parent.Find("Open").gameObject;
        SetSystemState();
    }
    private void SetSystemState()
    {
        //公共逻辑
        if (!SystemManager.Instance.GetSystemIsOpen(systemID)) return;//模块未开放的(农场，咖啡店)

        if (SystemManager.Instance.GetSystemIsUnlock(systemID))//已解锁
        {
            fatherObj.SetActive(false);
            uncleObj.SetActive(true);
            return;
        }

        brand = transform.Find("Brand").gameObject;
        systemData = SystemManager.Instance.GetSystemData(systemID);
        unlockBtn = transform.Find("UnlockBtn").GetComponent<Button>();
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        condition = transform.Find("Condition").GetComponent<TextMeshProUGUI>();
        brandText = transform.Find("BrandText").GetComponent<TextMeshProUGUI>();
        brandAni = transform.parent.Find("Brand").GetComponent<SkeletonAnimation>();

        brand.SetActive(true);
        brandAni.gameObject.SetActive(false);

        //如果是花园则是另一套逻辑
        if (systemID == 1003)
        {
            brandText.text = systemData.systemConfig.name;
            condition.text = $"倒计时完成即可解锁";
            unlockBtn.interactable = false;

            Text timeRemaining = transform.Find("TimerImg/TimeRemaining").GetComponent<Text>();
            Image timerBar = transform.Find("TimerImg/bar").GetComponent<Image>();
            TimeSpan timeSpan;

            TimerManager.Instance.CreateTimer("GardenSystemTimer", 0, 1, () =>
            {
                timeSpan = TimeDifferenceManager.Instance.CountDown(playerModule.GardenUnlockTime);
                if (timeSpan.Hours > 0)
                    timeRemaining.text = $"{timeSpan.Hours}时{timeSpan.Minutes}分";
                else
                    timeRemaining.text = $"{timeSpan.Minutes}分{timeSpan.Seconds}秒";
                timerBar.fillAmount = Convert.ToSingle((1f / 5) * (5 - timeSpan.TotalMinutes));

                if (TimeDifferenceManager.Instance.CompareTime(playerModule.GardenUnlockTime))
                {
                    unlockBtn.interactable = true;
                    timeRemaining.transform.parent.gameObject.SetActive(false);
                    if (playAni)
                    {
                        brand.SetActive(false);
                        brandAni.gameObject.SetActive(true);
                        var spineEvent = brandAni.AnimationState.SetAnimation(0, "animation", false);
                        spineEvent.Complete += (TrackEntry track) => { brandAni.AnimationState.SetAnimation(1, "animation2", true); };
                        playAni = false;
                    }
                }
            });
            unlockBtn.onClick.AddListener(() =>
            {
                TipManager.Instance.ShowMsg($"解锁{systemData.systemConfig.name}");
                systemData.systemStoreData.isUnlock = true;
                SystemManager.Instance.SaveData();
                fatherObj.SetActive(false);
                uncleObj.SetActive(true);
                SDKManager.Instance.LogEvent(EventId.CC_Buy.ToString(), "UnlockSystem", "Button");
                TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.UnlockArea, systemID);
            });
            return;
        }
        //以下是非花园系统逻辑
        price = transform.Find("UnlockBtn/Price").GetComponent<TextMeshProUGUI>();
        brandText.text = systemData.systemConfig.name;
        price.text = systemData.systemConfig.price.ToString();
        condition.text = $"评价达到 <sprite=5>{systemData.systemConfig.star}";

        unlockBtn.gameObject.SetActive(true);
        condition.gameObject.SetActive(true);

        GameObject window = default;


        TimerManager.Instance.CreateTimer($"SystemTimer{systemID}", 0, 1, () =>
        {
            if (playerModule.Star >= systemData.systemConfig.star && playerModule.Fish >= systemData.systemConfig.price && playAni)
            {
                brand.SetActive(false);
                brandAni.gameObject.SetActive(true);
                var spineEvent = brandAni.AnimationState.SetAnimation(0, "animation", false);
                spineEvent.Complete += (TrackEntry track) => { brandAni.AnimationState.SetAnimation(1, "animation2", true); };
                playAni = false;
            }
        });

        unlockBtn.onClick.AddListener(() =>
        {
            if (playerModule.Star < systemData.systemConfig.star)
            {
                TipManager.Instance.ShowMsg("未达到心级评价");
                return;
            }
            if (playerModule.Fish < systemData.systemConfig.price)
            {
                TipManager.Instance.ShowMsg("小鱼干不足");
                return;
            }
            TipManager.Instance.ShowMsg($"解锁{systemData.systemConfig.name}");
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, -systemData.systemConfig.price);
            systemData.systemStoreData.isUnlock = true;
            SystemManager.Instance.SaveData();
            fatherObj.SetActive(false);
            uncleObj.SetActive(true);
            SDKManager.Instance.LogEvent(EventId.CC_Buy.ToString(), "UnlockSystem", "Button");
            if (systemID == 1004)
            {
                window = fatherObj.transform.parent.Find("pos_10047/DefaultFacility").gameObject;
                window.SetActive(true);
            }
            TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.UnlockArea, systemID);
        });
    }
}
