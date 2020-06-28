using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 命令接口
/// </summary>
public interface ICommand 
{
    /// <summary>
    /// 执行命令
    /// </summary>
    void Execute();
}
