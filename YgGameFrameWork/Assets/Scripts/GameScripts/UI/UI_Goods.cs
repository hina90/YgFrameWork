using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

public class UI_Goods : UIBase
{
    private StoreModule storeModule;
    private LoopVerticalScrollRect goodsContent;
    private InitOnStart goodsCount;
    private static Callback<StoreConfigData> loadCallBack;

    private Button btnExit;

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.UI;

        storeModule = GameModuleManager.Instance.GetModule<StoreModule>();
        goodsContent = Find<LoopVerticalScrollRect>(gameObject, "GoodsPan");
        goodsCount = Find<InitOnStart>(gameObject, "GoodsPan");
        goodsCount.totalCount = storeModule.GoodsDatas.Count;
        goodsContent.objectsToFill = storeModule.GoodsDatas.ToArray();
        btnExit = Find<Button>(gameObject, "ExitBtn");
        PlayAnimation(Find(gameObject, "Scale"));
        RegisterBtnEvent();
    }

    private void RegisterBtnEvent()
    {
        btnExit.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
    }

    /// <summary>
    /// 设置上货回调
    /// </summary>
    /// <param name="callback"></param>
    internal static void SetLoadCallBack(Callback<StoreConfigData> callback)
    {
        loadCallBack = callback;
    }

    /// <summary>
    /// 上货回调
    /// </summary>
    internal void LoadCallBack(StoreConfigData storeData)
    {
        loadCallBack?.Invoke(storeData);
        TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.UpperShelf, null);
    }

    protected override void Enter()
    {
        base.Enter();
        if (param.Length > 0)
        {
            loadCallBack = FacilitiesManager.Instance.GetFreeShelf().OnDelivery;
        }
    }

    public override void MainUpdate()
    {
        base.MainUpdate();
    }

    public override void Release()
    {
        base.Release();
    }
}
