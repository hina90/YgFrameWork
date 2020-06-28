using Spine;
using Spine.Unity;
using UnityEngine;

public class UI_ClickEffect : UIBase
{
    private SkeletonGraphic m_ClickAnim;
    public override void Init()
    {
        Layer = LayerMenue.CLICK;
        base.Init();
        m_ClickAnim = Find<SkeletonGraphic>(gameObject, "ClickFeedback");
    }

    public void ShowClickEffect()
    {
        m_ClickAnim.transform.position = Input.mousePosition;
        m_ClickAnim.gameObject.SetActive(true);
        m_ClickAnim.AnimationState.Complete -= (Hide);
        m_ClickAnim.AnimationState.SetAnimation(0, "animation", false);
        m_ClickAnim.AnimationState.Complete += (Hide);
    }

    private void Hide(TrackEntry trackEntry)
    {
        m_ClickAnim.gameObject.SetActive(false);
    }

    public override void Release() { }
}
