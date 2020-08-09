using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;
using System;

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
            Utils.World2ScreenPos(transform.position, gold.GetComponent<RectTransform>());

            Vector3 endPos = gold.transform.position;
            gold.transform.DOJump(endPos, endPos.y + 200, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
             
            });
        }
    }
    /// <summary>
    /// 掉钻石
    /// </summary>
    public void OutOfDiamond(Int64 award, int type)
    {
        UIBase ui = UIManager.Instance.GetUI<UI_MainGame>("UI_MainGame");
        Transform parentTrans = GameObject.Find("PathMap/GoldLayer").transform;
        GameObject diamond = ResourceManager.Instance.GetResourceInstantiate("Diamond", parentTrans, ResouceType.PrefabItem);
        Vector3 offVec = new Vector3(3, -7, 0);
        string awardStr = award > 0 ? "+" +GameDataManager.Instance.CountToKorM(award) : GameDataManager.Instance.CountToKorM(award);
        diamond.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text = awardStr;
        Utils.World2ScreenPos(transform.position, diamond.GetComponent<RectTransform>());
        Vector3 endPos = diamond.transform.position;
        Image img = diamond.transform.Find("GFX").GetComponent<Image>();
        img.color = Color.white;

        switch (type)
        {
            case 1:
                img.sprite = ResourceManager.Instance.GetSpriteResource("ui_icon_jinbi", ResouceType.UI);
                playerData.Gold += award;
                break;
            case 3:
                img.sprite = ResourceManager.Instance.GetSpriteResource("ui_icon_zuanshi", ResouceType.UI);
                playerData.Diamond += award;
                break;
        }
        Sequence s = DOTween.Sequence();
        s.Append(diamond.transform.DOJump(endPos + new Vector3(0, 40, 0), endPos.y + 10, 1, 0.6f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(diamond);
        }));
        //s.Join(img.DOFade(0, 1.5f)).SetEase(Ease.Linear);

        s.Play();
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public override void Release()
    {
        base.Release();
    }
}
