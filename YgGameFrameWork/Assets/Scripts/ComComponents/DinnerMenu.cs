using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class DinnerMenu : MonoBehaviour
{
    public Callback ClickCallback { protected get; set; }
    private bool sign = true;

    /// <summary>
    /// 显示菜单
    /// </summary>
    /// <param name="sp"></param>
    public void ShowMenu(Sprite sp)
    {
        transform.Find("SpineRender").gameObject.SetActive(false);
        Transform trs = transform.Find("MenuRender");
        trs.gameObject.SetActive(true);
        Image img = trs.GetComponent<Image>();
        img.sprite = sp;
        img.SetNativeSize();

        //UUIEventListener listener = img.gameObject.GetOrAddComponent<UUIEventListener>();
        //listener.RegOnClick(ClickHander);
        Button listener = GetComponent<Button>();
        listener.onClick.AddListener(ClickHander);
    }
    /// <summary>
    /// 显示生气标志
    /// </summary>
    public void ShowAngry()
    {
        transform.Find("MenuRender").gameObject.SetActive(false);
        transform.Find("SpineRender").gameObject.SetActive(true);
        //GuideManager.Instance.EnterWeakStep(1103);
    }
    /// <summary>
    /// 菜单点击事件
    /// </summary>
    /// <param name="obj"></param>
    private void ClickHander()
    {
        ClickCallback?.Invoke();
        DestroyImmediate(gameObject);
    }
    /// <summary>
    /// 坐标跟随
    /// </summary>
    /// <param name="fellowPos"></param>
    public void FellowActor(Vector3 fellowPos, Vector3 actorSize)
    {
        transform.position = Camera.main.WorldToScreenPoint(new Vector3(fellowPos.x + actorSize.x, fellowPos.y + actorSize.y, fellowPos.z));
        gameObject.SetActive(!CameraMove.isProspect);
        if (sign)
        {
            GuideManager.Instance.BroadcastGuideEvent(GameEvent.SHOW_MASK);
            GuideManager.Instance.EnterNextStep();
            sign = false;
        }
    }

    /// <summary>
    /// 引导点菜逻辑
    /// </summary>
    public void GuideOrder()
    {
        ClickCallback?.Invoke();
        DestroyImmediate(gameObject);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Release()
    {
        ClickCallback = null;
    }
}
