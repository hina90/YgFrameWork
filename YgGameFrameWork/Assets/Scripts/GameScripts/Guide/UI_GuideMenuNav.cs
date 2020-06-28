using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideMenuNav : MonoBehaviour
{
    private Button btnKitchen;

    private void Awake()
    {
        btnKitchen = transform.Find("BtnKitchen").GetComponent<Button>();

        btnKitchen.onClick.AddListener(() =>
        {
            UIManager.Instance.SendUIEvent(GameEvent.TOGGLE_KITCHEN);
        });

        GuideManager.Instance.RegisterBtn(btnKitchen);
    }
}
