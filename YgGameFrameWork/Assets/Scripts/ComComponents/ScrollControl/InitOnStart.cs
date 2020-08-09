using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
[DisallowMultipleComponent]
public class InitOnStart : MonoBehaviour
{
    public int totalCount = -1;
    public void Start()
    {
        var ls = GetComponent<LoopScrollRect>();
        ls.totalCount = totalCount;
        ls.RefillCells();
    }
}
