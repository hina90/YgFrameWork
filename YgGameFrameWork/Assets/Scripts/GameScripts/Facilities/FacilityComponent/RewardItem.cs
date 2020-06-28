using UnityEngine;
using UnityEngine.EventSystems;

public class RewardItem : MonoBehaviour
{
    private int value;
    public int Value { get => value; set => this.value = value; }
    private bool canPickUp;
    public bool CanPickUp { get => canPickUp; set => canPickUp = value; }

    protected int totalValue = 0;
    protected Vector2 targetPos;
    protected UI_Money uiMoney;
    protected string uiPrefab;

    void Awake()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        uiMoney = UIManager.Instance.GetUI<UI_Money>("UI_Money");
    }

    //private void OnMouseDown()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject()) return;
    //    ClickEffect();
    //}

    public virtual void ClickEffect() { }

    protected virtual void UpdateValue() { }

    /// <summary>
    /// 奖励物消失
    /// </summary>
    public void Disappear()
    {
        IncomeTipManager.Instance.ShowIncomeTip(Value, transform.position);
        Invoke("UpdateValue", 0.1f);
        Destroy(gameObject, 0.2f);
    }
}
