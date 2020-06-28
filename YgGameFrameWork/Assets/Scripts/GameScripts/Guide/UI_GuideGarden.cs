using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuideGarden : MonoBehaviour
{
    private Button btnGarden;

    private void Awake()
    {
        btnGarden = transform.Find("BtnGarden").GetComponent<Button>();

        btnGarden.onClick.AddListener(() =>
        {
            Camera.main.GetComponent<CameraMove>().MoveGarden();
        });

        GuideManager.Instance.RegisterBtn(btnGarden);
    }
}
