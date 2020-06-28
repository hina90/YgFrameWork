using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    /// <summary>
    /// 开始游戏
    /// </summary>
    void Start()
    {
        Main.Instance.StartGame();
    }
}
