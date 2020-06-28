using System.Collections.Generic;
using System.Text;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 引导数据类
/// </summary>
public class GuideModel
{
    //enum GuideModule { LoginPlot = 1000, Station = 2000, Warehouse = 3000, Observer = 4000 }

    public int GuideId { get; private set; }
    private bool isFinishGuide;
    private bool isFinishWeakGuide;
    //private GuideModule curGuideModule = GuideModule.Station;
    private List<GuideConfigData> guideList;

    public int WeakGuideId { get; private set; }
    private List<GuideConfigData> weakGuideList;

    public GuideModel()
    {
        //PlayerPrefs.DeleteAll();
        isFinishGuide = PlayerPrefs.GetInt("newGuide") == 0 ? false : true;
        InitWeakGuideData();
        if (isFinishGuide) return;
        GuideId = 1001;
        guideList = ConfigDataManager.Instance.GetDatabase<GuideConfigDatabase>().FindAll(o => o.type != 6);
        ResetGuide();
    }

    /// <summary>
    /// 初始化弱引导数据
    /// </summary>
    private void InitWeakGuideData()
    {
        WeakGuideId = PlayerPrefs.GetInt("weakGuide");
        weakGuideList = ConfigDataManager.Instance.GetDatabase<GuideConfigDatabase>().FindAll(o => o.type == 6);
    }

    /// <summary>
    /// 重置引导
    /// </summary>
    private void ResetGuide()
    {
        if (!Global.OPEN_GUIDE) return;
        StringBuilder sb = new StringBuilder();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            sb.Append(Application.streamingAssetsPath);
        }
        else
        {
            sb.Append(Application.persistentDataPath);
        }
        FileUtils.Instance.DeleteAllFile(sb.ToString());
    }

    /// <summary>
    /// 完成当前引导步骤
    /// </summary>
    public void CompleteCurGuide(int guideID)
    {
        GuideId = guideID + 1;
        if (guideList.Find(o => o.Id == GuideId) == null)
        {
            isFinishGuide = true;
            PlayerPrefs.SetInt("newGuide", 1);
            UIManager.Instance.CloseUI(LayerMenue.GUIDE);
            //开启线性引导任务
            UIManager.Instance.SendUIEvent(GameEvent.START_AIMTASK);
        }
    }

    /// <summary>
    /// 完成当前弱引导步骤
    /// </summary>
    /// <param name="weakGuideID"></param>
    public void CompleteCurWeakGuide(int weakGuideID)
    {
        PlayerPrefs.SetInt("weakGuide", weakGuideID);
        WeakGuideId = weakGuideID + 1;
        if (weakGuideList.Find(o => o.Id == WeakGuideId) == null)
        {
            isFinishWeakGuide = true;
        }
    }

    public bool IsFinishWeakGuide()
    {
        return isFinishWeakGuide;
    }

    public bool IsFinishGuide()
    {
        return isFinishGuide;
    }
}
