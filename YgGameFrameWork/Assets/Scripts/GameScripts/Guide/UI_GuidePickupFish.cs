using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuidePickupFish : MonoBehaviour
{
    private Button btnPickup;
    private Transform fishTrans;

    private void Awake()
    {
        btnPickup = transform.Find("BtnPickup").GetComponent<Button>();
        fishTrans = GameObject.Find("PathMap/Restaurant/pos_10002/fishParent").transform.GetChild(0);
        btnPickup.onClick.AddListener(() =>
        {
            fishTrans.GetComponent<FishItem>().ClickEffect();
        });
        SetRectValue();
        GuideManager.Instance.RegisterBtn(btnPickup);
    }

    private void SetRectValue()
    {
        RectTransform rect = btnPickup.GetComponent<RectTransform>();
        Utils.World2ScreenPos(fishTrans.position, rect);
        rect.sizeDelta = new Vector2(80, 80);
    }
}
