using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerData()
    {
       
    }


    

    #region ExhibitFragment
    /// <summary>
    /// 拥有展品展品
    /// </summary>
    public Dictionary<int, int> DicOwnExhibitIDs = new Dictionary<int,int>();
    /// <summary>
    ///获得展品碎片
    /// </summary>
    public void AddOwnExhibit(int ExhibitID)
    {
        if (!DicOwnExhibitIDs.ContainsKey(ExhibitID))
            DicOwnExhibitIDs[ExhibitID] = 1;
        DicOwnExhibitIDs[ExhibitID] += 1;
    }
    /// <summary>
    ///消耗展品
    /// </summary>
    /// <param name="ExhibitID">展品ID</param>
    /// <param name="inCount">消耗数量</param>
    public bool consumeOwnExhibit(int ExhibitID,int inCount)
    {
        if (inCount < 0)
            return false;
        var ownCount = getOwnExhibitCount(ExhibitID);
        if (ownCount < inCount)
            return false;
        DicOwnExhibitIDs[ExhibitID] -= inCount;
        return true;
    }

    /// <summary>
    ///是否拥有了某个展品碎片
    /// </summary>
    /// <param name="ExhibitID">展品ID</param>
    public bool IsOwnExhibit(int ExhibitID)
    {
        if (!DicOwnExhibitIDs.ContainsKey(ExhibitID))
            return false;
        return true;
    }
    /// <summary>
    ///是否拥有了某个展品数量
    /// </summary>
    /// <param name="ExhibitID">展品ID</param>
    public int getOwnExhibitCount(int ExhibitID)
    {
        if (!IsOwnExhibit(ExhibitID))
            return 0;
        return DicOwnExhibitIDs[ExhibitID];
    }
    #endregion

    #region cultivate
    /// <summary>
    /// 养成碎片
    /// </summary>
    Dictionary<int, int> DicCultivate = new Dictionary<int, int>();
    /// <summary>
    /// 养成星级
    /// </summary>
    Dictionary<int, int> DicStar = new Dictionary<int, int>();
    /// <summary>
    /// 装饰品是否解锁
    /// </summary>
    /// <param name="cullId"></param>
    /// <returns></returns>
    public bool isDerationLock(int cullId)
    {
        if (DicCultivate.ContainsKey(cullId))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    ///升级养成等级
    /// </summary>
    public void AddCultivate(int CulID)
    {
        if (!DicCultivate.ContainsKey(CulID)&&!DicStar.ContainsKey(CulID))
        {
            DicStar[CulID] = 1;
            DicCultivate[CulID] = 1;
        }        
        else
        {
            DicCultivate[CulID] += 1;
            if (DicCultivate[CulID]>= getDecorationSliderMaxVule(CulID))
            {
                DicCultivate[CulID] -= getDecorationSliderMaxVule(CulID);
                if (DicStar[CulID]< getDecorationMaxLv(CulID))
                {
                    DicStar[CulID] += 1;
                }
            }
        }
    }

    public int getDecorationSliderVule(int CulID)
    {
        if (!DicCultivate.ContainsKey(CulID) || !DicStar.ContainsKey(CulID))
        {
            return 0;
        }
        return DicCultivate[CulID];
    }
    public int getDecorationSliderMaxVule(int CulID)
    {
        return getUpDecoration(CulID);
    }

    public string GetSliderText(int CulID)
    {
        return getDecorationSliderVule(CulID) + "/" + getDecorationSliderMaxVule(CulID);
    }
    /// <summary>
    ///获取养成星级
    /// </summary>
    public int getCultivateLvl(int CulID)
    {
        if (!DicStar.ContainsKey(CulID))
            return 0;
        return DicStar[CulID];
    }
   
    public int getUpDecoration(int culID)
    {
        var templist= ConfigDataManager.Instance.GetDatabase<CultivateConfigDatabase>().FindAll();
        if (DicStar.ContainsKey(culID))
        {
            var ss= templist.Find(temp => temp.CultivteID == culID && temp.CultivteLvl == DicStar[culID]).UpCostCount;
            return templist.Find(temp => temp.CultivteID == culID && temp.CultivteLvl == DicStar[culID]).UpCostCount;
        }
        else
        {
            return 1;
        }
    }
    /// <summary>
    /// 是否最大等级
    /// </summary>
    /// <param name="cullID"></param>
    /// <returns></returns>
    public bool isDecorationMax(int cullID)
    {
        if (DicStar.ContainsKey(cullID))
        {
            if (DicStar[cullID] >= getDecorationMaxLv(cullID))
            {
                return true;
            }
        }      
        return false;
    }
    /// <summary>
    /// 获取最大等级
    /// </summary>
    /// <param name="cullID"></param>
    /// <returns></returns>
    public int getDecorationMaxLv(int cullID)
    {
        var list = ConfigDataManager.Instance.GetDatabase<CultivateConfigDatabase>().FindAll();
        List<CultivateConfigData> culList = new List<CultivateConfigData>();
        culList.Clear();
        list.ForEach((temp) =>
        {
            if (temp.CultivteID==cullID)
            {
                culList.Add(temp);
            }
       });
        return culList.Count;
    }
    #endregion
    public SystemLanguage mCurrentLanguage = SystemLanguage.Chinese;
    /// <summary>
    /// 背景音乐关闭
    /// </summary>
    public bool mIsGameBGMOpen ;
    /// <summary>
    /// 点击音效关闭
    /// </summary>
    public bool mIsGameSoundOpen ;
    /// <summary>
    /// 新添加 图鉴ID,星级
    /// </summary>
    public Dictionary<int, int> handBookNewDict = new Dictionary<int, int>();
    /// <summary>
    /// 离线时间 实际执行中是使用最后一次存档时间代替
    /// </summary>
    public DateTime mOffLineTime;
}
