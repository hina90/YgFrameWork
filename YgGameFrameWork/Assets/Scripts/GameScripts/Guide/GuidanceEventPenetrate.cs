using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidanceEventPenetrate : MonoBehaviour, ICanvasRaycastFilter
{
    private Image _targetImage;
    private bool isPass;

    public void SetTargetImage(Image target)
    {
        _targetImage = target;
        Invoke("ClickPass", 1f);
    }

    private void ClickPass()
    {
        isPass = true;
        if (GuideManager.Instance.forbidTouch)
        {
            GuideManager.Instance.forbidTouch = false;
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (_targetImage == null || isPass == false)
            return true;

        return !RectTransformUtility.RectangleContainsScreenPoint(_targetImage.rectTransform, sp, eventCamera);
    }

    private void OnDisable()
    {
        isPass = false;
    }
}
