using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariationBubble : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length <= 0) return;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.name == "variation")
                {
                    UIManager.Instance.OpenUI<UI_SeedMutationAds>();
                }
            }
        }
    }
}
