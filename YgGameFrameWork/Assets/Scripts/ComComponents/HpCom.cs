using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 血条组件
/// </summary>
public class HpCom : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    /// <summary>
    /// 跟随角色
    /// </summary>
    /// <param name="followPos"></param>
    public void FollowActor(Vector3 followPos)
    {
        this.transform.position = Camera.main.WorldToScreenPoint(new Vector3(followPos.x + 0.09f, followPos.y + 1.6f, followPos.z));
        this.gameObject.SetActive(!CameraMove.isProspect);
    }
    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHp(float currentHp, float maxHp)
    {
        slider.maxValue = maxHp;
        slider.value = currentHp;
    }
    /// <summary>
    /// 释放
    /// </summary>
    public void Release()
    {
        slider = null;
        Destroy(gameObject);
    }
}
