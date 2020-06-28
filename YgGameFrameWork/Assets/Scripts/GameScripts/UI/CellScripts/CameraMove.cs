using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CameraMove : MonoBehaviour
{
    private static bool isFocus = false;
    private Touch oldTouch1;
    private Touch oldTouch2;
    private float distance = 3500;
    private static Vector3 initPos = new Vector3(0, 0, -38);
    /// <summary>
    /// 是否是远景状态
    /// </summary>
    public static bool isProspect = false;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = initPos;
        AdapterView();
        MoveRestaurant();
        //Camera.main.fieldOfView = CameraView(Camera.main.aspect, Camera.main.fieldOfView) / 2;
    }

    private void AdapterView()
    {
        Camera.main.fieldOfView = (720f / Screen.width) * Screen.height / 1280f * 60f;
    }

    public float CameraView(float aspect, float fov)
    {
        double frustumHeight = 2.0 * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float CurrentFov = 2 * Mathf.Atan((float)frustumHeight * (1280f / 720f) / aspect * 0.5f / distance) * Mathf.Rad2Deg;
        if (CurrentFov > fov)
        {
            return CurrentFov;
        }
        return fov;
    }

    void Update()
    {
        //引导模式下禁止切换远景模式
        if (Global.OPEN_GUIDE)
        {
            if (!GuideManager.Instance.IsFinishGuide()) return;
        }

        if (!isFocus && Input.GetMouseButtonDown(0))
        {
            //UI遮挡阻断射线检测
            if (Utils.CheckIsClickOnUI()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == null) return;
                switch (hit.collider.gameObject.name)
                {
                    case "Garden":
                        MoveGarden();
                        isFocus = true;
                        break;
                    case "Restaurant":
                        MoveRestaurant();
                        isFocus = true;
                        break;
                    case "Kitchen":
                        MoveKitchen();
                        isFocus = true;
                        break;
                    case "Store":
                        MoveStore();
                        isFocus = true;
                        break;
                    case "Cafe":
                        MoveCafe();
                        isFocus = true;
                        break;
                    case "Farm":
                        MoveFarm();
                        isFocus = true;
                        break;
                    default:
                        break;
                }
            }
        }

        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    if (Input.GetMouseButtonDown(1) && isFocus)
        //    {
        //        ToggleProspect();
        //    }
        //}
        //TouchMove();
    }

    private void TouchMove()
    {
        if (Input.touchCount <= 1) return;

        //单点触摸
        //if (Input.touchCount == 1)
        //{
        //    //txt1.text = "开始了单点触摸";
        //    //Raycast(Input.GetTouch(0).position);
        //    return;
        //}

        //多点触摸
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);

        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
            return;
        }

        float oldDis = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDis = Vector2.Distance(newTouch1.position, newTouch2.position);

        //前后两次手势距离之差，正表示放大手势，负表示缩小手势
        float offset = newDis - oldDis;
        //txt1.text = "前后两次手势之差为:" + offset;

        //全景模式
        if (offset < 0 && isFocus)
        {
            ToggleProspect();
        }
        //聚焦模式
        else if (offset > 0 && !isFocus)
        {
            Vector3 centerPos = (newTouch1.position + newTouch2.position) / 2;
            Raycast(centerPos);
        }

        //刷新最新触摸点，备下次使用
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
    }

    private void Raycast(Vector3 centerPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(centerPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log("射线检测到的物体是：" + hit.collider.gameObject.name);
            switch (hit.collider.gameObject.name)
            {
                case "Garden":
                    MoveGarden();
                    break;
                case "Restaurant":
                    MoveRestaurant();
                    break;
                case "Kitchen":
                    MoveKitchen();
                    break;
                case "Store":
                    MoveStore();
                    break;
                case "Cafe":
                    MoveCafe();
                    break;
                case "Farm":
                    MoveFarm();
                    break;
                default:
                    break;
            }
            isFocus = true;
        }
    }

    internal void MoveGarden()
    {
        Camera.main.transform.DOMove(new Vector3(-7.45f, 4.21f, -10.8f), 0.5f).OnComplete(() =>
        {
            isProspect = false;
            UIManager.Instance.SendUIEvent(GameEvent.CAMERA_FOCUS, FacilitiesType.Garden);
        });
    }

    internal void MoveRestaurant()
    {
        Camera.main.transform.DOMove(new Vector3(0.06f, 4.21f, -10.8f), 0.5f).OnComplete(() =>
        {
            isProspect = false;
            UIManager.Instance.SendUIEvent(GameEvent.CAMERA_FOCUS, FacilitiesType.Restaurant);
        });
    }

    internal void MoveKitchen()
    {
        Camera.main.transform.DOMove(new Vector3(7.53f, 4.21f, -10.8f), 0.5f).OnComplete(() =>
        {
            isProspect = false;
            UIManager.Instance.SendUIEvent(GameEvent.CAMERA_FOCUS, FacilitiesType.Kitchen);
        });
    }

    internal void MoveCafe()
    {
        Camera.main.transform.DOMove(new Vector3(-7.45f, -8.9f, -10.8f), 0.5f).OnComplete(() =>
        {
            isProspect = false;
            UIManager.Instance.SendUIEvent(GameEvent.CAMERA_FOCUS, FacilitiesType.Cafe);
        });
    }

    internal void MoveStore()
    {
        Camera.main.transform.DOMove(new Vector3(0.06f, -8.9f, -10.8f), 0.5f).OnComplete(() =>
        {
            isProspect = false;
            UIManager.Instance.SendUIEvent(GameEvent.CAMERA_FOCUS, FacilitiesType.Store);
        });
    }

    internal void MoveFarm()
    {
        Camera.main.transform.DOMove(new Vector3(7.53f, -8.9f, -10.8f), 0.5f).OnComplete(() =>
        {
            isProspect = false;
            UIManager.Instance.SendUIEvent(GameEvent.CAMERA_FOCUS, FacilitiesType.Farm);
        });
    }

    /// <summary>
    /// 切换成远景模式
    /// </summary>
    public static void ToggleProspect()
    {
        if (isProspect) return;
        Camera.main.transform.DOMove(initPos, 0.5f).OnComplete(() =>
        {
            isFocus = false;
        });
        //isProspect = true;
        UIManager.Instance.SendUIEvent(GameEvent.CAMERA_PROSPECT);
    }

    /// <summary>
    /// 移动到指定区域
    /// </summary>
    /// <param name="areaType">区域类型</param>
    public void MoveDesignatedArea(FacilitiesType areaType)
    {
        switch (areaType)
        {
            case FacilitiesType.Restaurant:
                MoveRestaurant();
                break;
            case FacilitiesType.Kitchen:
                MoveKitchen();
                break;
            case FacilitiesType.Garden:
                MoveGarden();
                break;
            case FacilitiesType.Cafe:
                MoveCafe();
                break;
            case FacilitiesType.Store:
                MoveStore();
                break;
            case FacilitiesType.Farm:
                MoveFarm();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 判断是否点击在UI上
    /// </summary>
    /// <returns></returns>
    private bool CheckIsClickOnUI()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        //移动端判断如下
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#else
        //PC端判断如下
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }
}
