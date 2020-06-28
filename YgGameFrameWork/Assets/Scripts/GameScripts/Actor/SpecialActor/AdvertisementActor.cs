using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 广告功能角色
/// </summary>
public class AdvertisementActor : BaseSpecialActor
{
    private Transform parentTra;
    private bool isWatch = false;

    public override void Init()
    {
        base.Init();
    }
    /// <summary>
    /// 初始化AI
    /// </summary>
    protected override void InitAIController()
    {
        parentTra = GameObject.Find("RichSonFish").transform;
        AiController = gameObject.GetOrAddComponent<AdvertisementAIController>();
        AiController.Initialize(this);
    }
    /// <summary>
    /// 点击逻辑
    /// </summary> 
    private void OnMouseDown()
    {
        if (isWatch) return;
        if (AiController.CurrentStateID == FSMStateID.Leave)
            return;

        isWatch = true;
        int randomGold = Random.Range(10000, 20000);
#if UNITY_ANDROID || UNITY_IOS



        SDKManager.Instance.ShowBasedVideo((string str1, string str2, float f1) =>
        {
            //执行弹出广告，观看后弹出小鱼干 
            signBar.gameObject.SetActive(false);
            CreateDriedFish(randomGold, parentTra);
            TimerManager.Instance.CreateUnityTimer(1f, () =>
            {
                AiController.SetTransition(Transition.SpecialPointMoveOver, 0);
            });
        },
        (string str1, string str2)=>
        {

        },
        (string str1)=>
        {

        });
#endif
#if UNITY_EDITOR
        CreateDriedFish(randomGold, parentTra);
        TimerManager.Instance.CreateUnityTimer(1f, () =>
        {
            AiController.SetTransition(Transition.SpecialPointMoveOver, 0);
        });
#endif
    }
}
