using System.Collections.Generic;
using UnityEngine;

public class TipManager : Singleton<TipManager>
{
    private GameObjectPool m_MsgPool;
    private UI_TipMsg uiTip;
    private List<GameObject> msgList;
    private const string prefabName = "MsgItem";
    private const int maxShowNum = 5;
    private const float intervalTime = 0f;

    public TipManager()
    {
        msgList = new List<GameObject>();
        UIManager.Instance.OpenUI<UI_TipMsg>();
        uiTip = UIManager.Instance.GetUI<UI_TipMsg>(typeof(UI_TipMsg).ToString()) as UI_TipMsg;
        m_MsgPool = new GameObjectPool(uiTip.transform, prefabName);
    }

    /// <summary>
    /// 显示浮动信息
    /// </summary>
    /// <param name="msg">提示消息</param>
    /// <param name="colorHtml">颜色HTML值</param>
    public void ShowMsg(string msg, string colorHtml = "#4A3725")
    {
        if (msgList.Count >= maxShowNum) return;
        GameObject obj = m_MsgPool.GetPool();
        MsgItem msgItem = obj.GetOrAddComponent<MsgItem>();
        msgItem.Show(msg, msgList.Count * intervalTime, colorHtml);
        if (!msgList.Contains(obj)) msgList.Add(obj);
    }

    /// <summary>
    /// 回收消息
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(GameObject obj)
    {
        m_MsgPool.Recycle(obj);
        if (msgList.Contains(obj))
        {
            msgList.Remove(obj);
        }
    }
}
