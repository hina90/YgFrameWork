using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideOrder : MonoBehaviour
{
    private Button btnOrder;
    private RectTransform trans;

    private void Awake()
    {
        trans = GameObject.Find("ActorSliders").transform.GetChild(0).GetComponent<RectTransform>();
        btnOrder = transform.Find("BtnOrder").GetComponent<Button>();
        btnOrder.onClick.AddListener(() =>
        {
            trans.GetComponent<DinnerMenu>().GuideOrder();
        });
        SetRectValue();
        GuideManager.Instance.RegisterBtn(btnOrder);
    }

    private void SetRectValue()
    {
        RectTransform rect = btnOrder.GetComponent<RectTransform>();
        rect.localPosition = trans.localPosition;
        rect.sizeDelta = trans.Find("MenuRender").GetComponent<RectTransform>().sizeDelta;
    }
}
