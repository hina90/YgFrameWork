using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteGraphic : MaskableGraphic
{
    public UGUISpriteAsset  m_spriteAsset;

    public override Texture mainTexture
    {
        get
        {
            if (m_spriteAsset == null)
                return s_WhiteTexture;

            if (m_spriteAsset.texSource == null)
                return s_WhiteTexture;
            else
                return m_spriteAsset.texSource;
        }
    }

#if UNITY_EDITOR
    //在编辑器下 
    protected override void OnValidate()
    {
        base.OnValidate();
        //Debug.Log("Texture ID is " + this.texture.GetInstanceID());
    }
#endif

    protected override void OnRectTransformDimensionsChange()
    {
        // base.OnRectTransformDimensionsChange();
    }

    /// <summary>
    /// 绘制后 需要更新材质
    /// </summary>
    public new void UpdateMaterial()
    {
        base.UpdateMaterial();
    }
}