using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideGardenTimer : MonoBehaviour
{
    private Image timerArea;
    private RectGuidanceController rectController;

    void Start()
    {
        timerArea = transform.Find("Image").GetComponent<Image>();
        rectController = transform.Find("Rect").GetComponent<RectGuidanceController>();
        rectController.gameObject.SetActive(true);

        rectController.Target = timerArea;
    }
}
