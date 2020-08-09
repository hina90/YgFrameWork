using UnityEngine;
using System.Collections;
/// <summary>
/// 场景适配
/// </summary>
public class ScreenUnit : MonoBehaviour
{
    public float initHeight;
    public float initWidth;
    public float initSize;
    private float currentHeight;
    private float currentWidth;

    // Use this for initialization
    void Start()
    {
        currentHeight = Screen.height;
        currentWidth = Screen.width;
        GetComponent<Camera>().orthographicSize = initSize * (initWidth / initHeight) / (currentWidth / currentHeight);
    }
}