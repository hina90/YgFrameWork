using UnityEngine;

public class Com_ClearGarbage : MonoBehaviour
{
    void Start()
    {
        GuideManager.Instance.AddGuideListener(GameEvent.CLEAR_GARBAGE, CheckTaskProgress);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameManager.Instance.AddGarbageToList(transform.GetChild(i).GetComponent<GarbageItem>());
        }
    }

    /// <summary>
    /// 检测当前任务进度
    /// </summary>
    /// <param name="arg"></param>
    private void CheckTaskProgress(object[] arg)
    {
        //垃圾拾取完毕，任务完成
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_GUIDE);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GuideManager.Instance.RemoveGuideListener(GameEvent.CLEAR_GARBAGE, CheckTaskProgress);
    }
}
