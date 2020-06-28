using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SortLayerCom : MonoBehaviour
{
    private Renderer m_render;

    private void Start()
    {
        Transform trans = transform.Find("guest_gfx");
        if(null != trans)
            m_render = trans.GetComponent<MeshRenderer>();

        if (null == m_render)
            m_render = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 设置显示层级
    /// </summary>
    /// <param name="layerDelta"></param>
    public void SetSortLayer(int layerDelta, LayerSetType type)
    {
        if (null == m_render) return;

        if (type == LayerSetType.INCRASE)
                m_render.sortingOrder += layerDelta;
        else m_render.sortingOrder = layerDelta;
    }
}
