using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 语言类型
/// </summary>
public enum LANGUAGETYPE
{
    CNJ,                     //中文简体
    CNF,                     //中文繁体
    EN                       //英文
}

/// <summary>
/// 多语言包管理器
/// </summary>
public class LanguageManager
{
    public static LANGUAGETYPE LanguageType = LANGUAGETYPE.EN;
    //语言配置档内容列表
    private static List<LanguageData> languageDataList;

    /// <summary>
    /// 初始化语言包
    /// </summary>
    public static void Init()
    {
        languageDataList = ConfigDataManager.Instance.GetDatabase<LanguageDatabase>().FindAll();
    }
    /// <summary>
    /// 获取内容
    /// </summary>
    public static string Get(int id)
    {
        string content = "";
        LanguageData data = languageDataList.Find(temp => temp.ID == id);
        if (LanguageType == LANGUAGETYPE.CNJ)
            content = data.cnj;
        else if (LanguageType == LANGUAGETYPE.CNF)
            content = data.cnf;
        else if (LanguageType == LANGUAGETYPE.EN)
            content = data.en;

        return content;

    }
}
