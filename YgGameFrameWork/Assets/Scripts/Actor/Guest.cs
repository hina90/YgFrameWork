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
      
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public override void Release()
    {
        base.Release();
    }
}
