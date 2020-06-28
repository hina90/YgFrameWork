using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 臭鼬放屁状态
/// </summary>
public class SkunkFartState : FSMState
{
    private bool waitOver = false;
    private float waitime = 5;

    public SkunkFartState()
    {
        waiTime = 3f;
        stateID = FSMStateID.SkunkFart;
    }
    /// <summary>
    /// 条件转换
    /// </summary>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if(ChangeState)
        {
            actor.AiController.SetTransition(Transition.SkunkFartOver, 0);
        }
    }
    /// <summary>
    /// 行为
    /// </summary>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if (waitOver) return;

        currenTime += Time.fixedDeltaTime;
        if(currenTime >= waiTime)
        {
            waitOver = true;
            actor.PlaySpineAnimation(0, "animation2", true, null, 0.5f, 1);
            GameObject fartObj = ResourceManager.Instance.GetResource("Fart", ResouceType.PrefabItem);
            GameObject fart = GameObject.Instantiate(fartObj);
            fart.transform.position = actor.transform.position + new Vector3(0.5f, 0, 0);
            TimerManager.Instance.CreateUnityTimer(3f, ()=>
            {
                GameManager.Instance.ExpelGuest();
            });
            TimerManager.Instance.CreateUnityTimer(7f, () =>
            {
                ChangeState = true;
                actor.PlaySpineAnimation(0, "animation", true);
                GameObject.Destroy(fart.gameObject);
            });
        }
    }
}
