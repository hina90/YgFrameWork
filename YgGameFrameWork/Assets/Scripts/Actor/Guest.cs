using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor;

/// <summary>
/// 参观客人
/// </summary>
public class Guest : BaseActor
{
    PlayerData playerData;

    protected override void InitAIController()
    {
        AiController = gameObject.GetOrAddComponent<GuestAIController>();
        AiController.Initialize(this);
    }
    /// <summary>
    /// 掉金幣
    /// </summary>
    public void OutOfGold(string name)
    {
        string[] nameArray = name.Split('_');
        string id = nameArray[1];
        string i = id.Substring(id.Length - 1);
        //bool isLock = MuseumManager.Instance.IsPlace(int.Parse(i));
        bool isLock = true;
        if (isLock)
        {
            Transform parentTrans = GameObject.Find("PathMap/GoldLayer").transform;
            UIBase ui = UIManager.Instance.GetUI<UI_MainGame>("UI_MainGame");
            GameObject gold = ResourceManager.Instance.GetResourceInstantiate("gold", parentTrans, ResouceType.PrefabItem);
            Helper.World2ScreenPos(transform.position, gold.GetComponent<RectTransform>());

            Vector3 endPos = gold.transform.position;
            gold.transform.DOJump(endPos, endPos.y + 200, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
             
            });
        }
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public override void Release()
    {
        base.Release();
    }
}
