using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
using UnityEngine.UI;

/// <summary>
/// 场景点击组件
/// </summary>
public class SceneClickCom : MonoBehaviour
{
    private bool isClick = false;
    public bool isDestory = false;

    void Start()
    {
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(()=>
        {
            if (isClick) return;
            isClick = true;
            SkeletonGraphic ani = GetComponentInChildren<SkeletonGraphic>();
            ani.AnimationState.SetAnimation(0, "tb", false).Complete += delegate
            {
                Destroy(gameObject);
            };
        });
    }
}
