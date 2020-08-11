using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置档数据管理器
/// </summary>
public class TableManager : BaseObject
{
    private static TableManager instance;

    public static TableManager Create()
    {
        if (instance == null)
        {
            instance = new TableManager();
        }
        return instance;
    }

    public override void Initialize()
    {

    }


    private void LoadTables()
    {

    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnDispose()
    {

    }
}
