using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuidePropaganda : MonoBehaviour
{
    private Button btnPropaganda;

    private void Awake()
    {
        btnPropaganda = transform.Find("BtnPropaganda").GetComponent<Button>();

        btnPropaganda.onClick.AddListener(() =>
        {
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_PROPAGANDA);
            UIManager.Instance.BackUI(LayerMenue.TIPS);
        });
        GuideManager.Instance.RegisterBtn(btnPropaganda);
    }

    private void OnDestroy()
    {
        btnPropaganda.onClick.RemoveAllListeners();
    }
}
