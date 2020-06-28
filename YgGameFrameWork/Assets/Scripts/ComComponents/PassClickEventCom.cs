using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassClickEventCom : MonoBehaviour, IPointerClickHandler
{
    public Callback pointCallback;
    private string stopEventComName = "";
    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (pointCallback != null)
        //    pointCallback();

        Debug.Log("------------------OnPointerClick------------------");

        //PassEventCLick(eventData, ExecuteEvents.pointerClickHandler);
    }
    /// <summary>
    /// 点击事件渗透
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="function"></param>
    public void PassEventCLick<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;

        for (int i = 0; i < results.Count; i++)
        {
            if (stopEventComName != "" && stopEventComName != null)
            {
                // Debug.Log("============stopEventComName:" + stopEventComName + "gameObjName:" + results[i].gameObject.name);
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    if (stopEventComName == results[i].gameObject.name)
                    {
                        stopEventComName = "";
                        break;
                    }
                }
            }
            else
            {
                Debug.Log(results[i].gameObject.name);
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                }
            }
        }
    }
    /// <summary>
    /// 添加穿透事件的组件名字到列表
    /// </summary>
    /// <param name="name"></param>
    public void AddPassEventComName(string name)
    {
        stopEventComName = name;
    }
}
