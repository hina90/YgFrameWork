using UnityEngine;
using UnityEngine.UI;

public class CellBase : MonoBehaviour
{
    private UI_TipsControl uiTips;
    public UI_TipsControl UI_Tips
    {
        get
        {
            UIManager.Instance.OpenUI<UI_TipsControl>();
            uiTips = UIManager.Instance.GetUI<UI_TipsControl>(typeof(UI_TipsControl).ToString()) as UI_TipsControl;
            return uiTips;
        }
    }

    #region  任务预制体提的方法
    /// <summary>
    /// 更改进度信息
    /// </summary>
    /// <param name="nowAmount"></param>
    /// <param name="targetAmount"></param>
    public void ProgressInfo(Text progressInfo, Image progressBar, int nowAmount, int targetAmount)
    {
        float addValue = 1f / targetAmount;
        if (nowAmount >= targetAmount)
            nowAmount = targetAmount;
        progressBar.fillAmount = nowAmount * addValue;
        if (targetAmount >= 1000 && targetAmount < 1000000)
        {
            progressInfo.text = $"{(nowAmount / 1000f).ToString("0.#")}k/{(targetAmount / 1000f).ToString("0.#")}k";
            return;
        }
        if (targetAmount >= 1000000)
        {
            progressInfo.text = $"{(nowAmount / 1000000f).ToString("0.#")}m/{(targetAmount / 1000000f).ToString("0.#")}m";
            return;
        }
        progressInfo.text = $"{nowAmount}/{targetAmount}";
    }
    /// <summary>
    /// 奖励图标与信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    public void RewardInfo(Image rewardIcon, Text rewardNum, int id, int amount)
    {
        if (id == 1)
        {
            rewardIcon.sprite = ResourceManager.Instance.GetSpriteResource("ui_14", ResouceType.Button);//小鱼干图标
            rewardIcon.SetNativeSize();
        }
        if (id == 2)
        {
            rewardIcon.sprite = ResourceManager.Instance.GetSpriteResource("cat-career_ui_rw_4", ResouceType.Button);//爱心图标
            rewardIcon.SetNativeSize();
        }
        rewardNum.text = amount.ToString();
    }
    /// <summary>
    /// 按钮的图标切换
    /// </summary>
    /// <param name="isFinish"></param>
    public void Finish(Image taskBtnImg, Button taskBtn, TaskState taskState)
    {
        if (taskState == TaskState.UNCLAIMED)
        {
            taskBtnImg.sprite = ResourceManager.Instance.GetSpriteResource("cat-career_ui_rw_6", ResouceType.Button);//领取
            taskBtn.enabled = true;
            taskBtnImg.SetNativeSize();
        }
        else if (taskState == TaskState.UNFINISH)
        {
            taskBtnImg.sprite = ResourceManager.Instance.GetSpriteResource("cat-career_ui_rw_16", ResouceType.Button);//进行中
            taskBtn.enabled = false;
            taskBtnImg.SetNativeSize();
        }
    }
    /// <summary>
    /// 判断按钮图标是否是已获得奖励的状态
    /// </summary>
    /// <param name="isRewaed"></param>
    public void WhetherReward(Transform complete, Transform taskBtn, TaskState taskState)
    {
        if (taskState == TaskState.FINISH)
        {
            complete.gameObject.SetActive(true);//显示打勾的图片
            taskBtn.gameObject.SetActive(false);
        }
        else
        {
            complete.gameObject.SetActive(false);
            taskBtn.gameObject.SetActive(true);//显示的是领取按钮
        }
    }
    #endregion
    public GameObject Find(GameObject obj, string name)
    {
        if (obj.name.Equals(name))
            return obj;

        int childCount = obj.transform.childCount;
        int i = 0;
        Transform child;
        GameObject childObj;
        while (i < childCount)
        {
            child = obj.transform.GetChild(i);
            childObj = Find(child.gameObject, name);
            if (childObj != null)
            {
                return childObj;
            }
            i++;
        }
        return null;
    }
    /// <summary>
    /// 查找子对象组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Find<T>(GameObject obj, string name) where T : Component
    {
        GameObject uiObj = Find(obj, name);

        return uiObj.GetOrAddComponent<T>();
    }
}
