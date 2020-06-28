using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

/// <summary>
/// 层级类型
/// </summary>
public enum LayerSetType
{
    EQUAL,                      //赋值
    INCRASE                     //递增
}
/// <summary>
/// 角色基类
/// </summary>
public class SpineActor : MonoBehaviour
{
    //骨骼动画
    protected MeshRenderer p_render;
    private SkeletonAnimation p_anim;

    public SkeletonAnimation GetSkeletonAnimation()
    {
        return p_anim;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        p_render = GetComponentInChildren<MeshRenderer>();
        p_anim = GetComponentInChildren<SkeletonAnimation>();
    }
    /// <summary>
    /// 获取中间点XY坐标
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCenterPosXY()
    {
        return new Vector2(p_render.bounds.size.x * 0.5f, p_render.bounds.size.y * 0.5f);
    }
    /// <summary>
    /// 是否有此动画
    /// </summary>
    /// <param name="state"></param>
    public bool HaveSpineAnimation(string state)
    {
        return p_anim.AnimationState.Data.skeletonData.FindAnimation(state) != null;
    }
    /// <summary>
    /// 播放spine骨骼动画
    /// </summary>
    public void PlaySpineAnimation(int trackIndex, string state, bool loop, Action aec = null, float attachmentThreshold = 0, float mixDuration = 0)
    {
        p_anim.timeScale = 1;
        var track = p_anim.AnimationState.SetAnimation(trackIndex, state, loop);
        if (attachmentThreshold != 0)
            track.AttachmentThreshold = attachmentThreshold;
        if (mixDuration != 0)
            track.MixDuration = mixDuration;

        if (aec != null)
        {
            track.Complete += delegate
            {
                aec();
            };
        }
    }
    /// <summary>
    /// 停止播放动画
    /// </summary>
    /// <param name="trackIndex"></param>
    public void StopSpineAnimation()
    {
        p_anim.state.ClearTracks();
    }
    /// <summary>
    /// 添加空白骨骼动画
    /// </summary>
    /// <param name="trackIndex"></param>
    /// <param name="mixDuration"></param>
    /// <param name="delay"></param>
    public void AddEmptySpineAnimation(int trackIndex, float mixDuration, float delay, float attachmentThreshold)
    {
        var empty = p_anim.state.AddEmptyAnimation(trackIndex, mixDuration, delay);
        if (attachmentThreshold != 0)
            empty.AttachmentThreshold = attachmentThreshold;
    }
    /// <summary>
    /// 添加spine骨骼播放动画
    /// </summary>
    /// <param name="state"></param>
    /// <param name="loop"></param>
    public void AddSpineAnimation(int trackIndex, string state, bool loop, float delay = 0)
    {
        p_anim.timeScale = 1;
        p_anim.AnimationState.AddAnimation(trackIndex, state, loop, delay);
    }
    /// <summary>
    /// 设置动画FlipX
    /// </summary>
    /// <param name="isScale"></param>
    public void SetSpineScaleX(bool isScale)
    {
        p_anim.skeleton.scaleX = isScale ? -1 : 1;
    }
    /// <summary>
    /// 设置显示层级
    /// </summary>
    /// <param name="layerDelta"></param>
    public void SetSortLayer(int layerDelta, LayerSetType type)
    {
        if(type == LayerSetType.INCRASE) p_render.sortingOrder += layerDelta;
        else p_render.sortingOrder = layerDelta;
    }
    /// <summary>
    /// 获取层级
    /// </summary>
    /// <returns></returns>
    public int GetSortLayer()
    {
        return p_render.sortingOrder;
    }
    /// <summary>
    /// 添加动画帧事件
    /// </summary>
    /// <param name="callback"></param>
    public void AddSpineEventLisener(Callback<object[]> callback)
    {
        p_anim.AnimationState.Event += (trackEnty, e) =>
        {
            callback(new object[] { e.Data.Name, e.Int, e.Float, e.String});
        };
    }
}
