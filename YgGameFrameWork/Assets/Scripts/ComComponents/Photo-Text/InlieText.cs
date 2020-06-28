using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InlieText : Text
{

    /// <summary>
    /// 用正则取标签属性 名称-大小-宽度比例
    /// </summary>
    //private static readonly Regex m_spriteTagRegex = new Regex(@"<quad name=(.+?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?) />", RegexOptions.Singleline);
    private static readonly Regex m_spriteTagRegex = new Regex(@"<quad name=(\d*\.?\d+%?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?)/>", RegexOptions.Singleline);
    /// <summary>
    /// 需要渲染的图片信息列表
    /// </summary>
    private List<InlineSpriteInfor> listSprite;
    /// <summary>
    /// 图片资源
    /// </summary>
    private UGUISpriteAsset m_spriteAsset;
    /// <summary>
    /// 标签的信息列表
    /// </summary>
    private List<SpriteTagInfor> listTagInfor;
    /// <summary>
    /// 图片渲染组件
    /// </summary>
    private SpriteGraphic m_spriteGraphic;
    /// <summary>
    /// CanvasRenderer
    /// </summary>
    private CanvasRenderer m_spriteCanvasRenderer;

    /// <summary>
    /// 初始化 
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_spriteGraphic == null)
            m_spriteGraphic = GetComponentInChildren<SpriteGraphic>();
        if (m_spriteCanvasRenderer == null)
            m_spriteCanvasRenderer = m_spriteGraphic.GetComponentInChildren<CanvasRenderer>();
        m_spriteAsset = m_spriteGraphic.m_spriteAsset;
    }

    /// <summary>
    /// 在设置顶点时调用
    /// </summary>
    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        //解析标签属性
        listTagInfor = new List<SpriteTagInfor>();
        foreach (Match match in m_spriteTagRegex.Matches(text))
        {
            SpriteTagInfor tempSpriteTag = new SpriteTagInfor();
            //tempSpriteTag.index = int.Parse(match.Groups[1].Value);
            tempSpriteTag.name = match.Groups[1].Value;
            tempSpriteTag.index = match.Index;
            tempSpriteTag.size = new Vector2(float.Parse(match.Groups[2].Value) * float.Parse(match.Groups[3].Value), float.Parse(match.Groups[2].Value));
            listTagInfor.Add(tempSpriteTag);
        }
    }

    /// <summary>
    /// 绘制模型
    /// </summary>
    /// <param name="toFill"></param>
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
        //获取所有的UIVertex,绘制一个字符对应6个UIVertex，绘制顺序为012 203
        List<UIVertex> listUIVertex = new List<UIVertex>();
        toFill.GetUIVertexStream(listUIVertex);

        //通过标签信息来设置需要绘制的图片的信息
        listSprite = new List<InlineSpriteInfor>();
        for (int i = 0; i < listTagInfor.Count; i++)
        {
            //UGUIText不支持<quad/>标签，表现为乱码，我这里将他的uv全设置为0,清除乱码
            for (int m = listTagInfor[i].index * 6; m < listTagInfor[i].index * 6 + 6; m++)
            {
                UIVertex tempVertex = listUIVertex[m];
                tempVertex.uv0 = Vector2.zero;
                listUIVertex[m] = tempVertex;
            }

            InlineSpriteInfor tempSprite = new InlineSpriteInfor();
            //如果图片在第一个位置,则计算他的位置为文本的初始点位置
            //否,则返回上一个字符的第三个UIVertex的position,这是根据他的顶点的绘制顺序所获得的
            if (listTagInfor[i].index == 0)
            {
                Vector2 anchorPivot = GetTextAnchorPivot(alignment);
                Vector2 rectSize = rectTransform.sizeDelta;

                tempSprite.textpos = -rectSize / 2.0f + new Vector2(rectSize.x * anchorPivot.x, rectSize.y * anchorPivot.y - listTagInfor[i].size.y);

            }
            else
                tempSprite.textpos = listUIVertex[listTagInfor[i].index * 6 - 4].position;

            //设置图片的位置
            tempSprite.vertices = new Vector3[4];
            tempSprite.vertices[0] = new Vector3(0, 0, 0) + tempSprite.textpos;
            tempSprite.vertices[1] = new Vector3(listTagInfor[i].size.x, listTagInfor[i].size.y, 0) + tempSprite.textpos;
            tempSprite.vertices[2] = new Vector3(listTagInfor[i].size.x, 0, 0) + tempSprite.textpos;
            tempSprite.vertices[3] = new Vector3(0, listTagInfor[i].size.y, 0) + tempSprite.textpos;

            //计算其uv
            Rect spriteRect = m_spriteAsset.listSpriteAssetInfor[0].rect;
            for (int j = 0; j < m_spriteAsset.listSpriteAssetInfor.Count; j++)
            {
                //通过标签的名称去索引spriteAsset里所对应的sprite的名称
                if (listTagInfor[i].name == m_spriteAsset.listSpriteAssetInfor[j].ID.ToString())
                    spriteRect = m_spriteAsset.listSpriteAssetInfor[j].rect;
            }
            Vector2 texSize = new Vector2(m_spriteAsset.texSource.width, m_spriteAsset.texSource.height);

            tempSprite.uv = new Vector2[4];
            tempSprite.uv[0] = new Vector2(spriteRect.x / texSize.x, spriteRect.y / texSize.y);
            tempSprite.uv[1] = new Vector2((spriteRect.x + spriteRect.width) / texSize.x, (spriteRect.y + spriteRect.height) / texSize.y);
            tempSprite.uv[2] = new Vector2((spriteRect.x + spriteRect.width) / texSize.x, spriteRect.y / texSize.y);
            tempSprite.uv[3] = new Vector2(spriteRect.x / texSize.x, (spriteRect.y + spriteRect.height) / texSize.y);

            //声明三角顶点所需要的数组
            tempSprite.triangles = new int[6];
            listSprite.Add(tempSprite);
        }
        //清除<quad />标签的乱码 重新绘制
        toFill.Clear();
        toFill.AddUIVertexTriangleStream(listUIVertex);
        DrawSprite();
    }

    /// <summary>
    /// 绘制图片
    /// </summary>
    void DrawSprite()
    {
        Mesh m_spriteMesh = new Mesh();

        List<Vector3> tempVertices = new List<Vector3>();
        List<Vector2> tempUv = new List<Vector2>();
        List<int> tempTriangles = new List<int>();

        for (int i = 0; i < listSprite.Count; i++)
        {
            for (int j = 0; j < listSprite[i].vertices.Length; j++)
            {
                tempVertices.Add(listSprite[i].vertices[j]);
            }
            for (int j = 0; j < listSprite[i].uv.Length; j++)
            {
                tempUv.Add(listSprite[i].uv[j]);
            }
            for (int j = 0; j < listSprite[i].triangles.Length; j++)
            {
                tempTriangles.Add(listSprite[i].triangles[j]);
            }
        }
        //计算顶点绘制顺序
        for (int i = 0; i < tempTriangles.Count; i++)
        {
            if (i % 6 == 0)
            {
                int num = i / 6;
                tempTriangles[i] = 0 + 4 * num;
                tempTriangles[i + 1] = 1 + 4 * num;
                tempTriangles[i + 2] = 2 + 4 * num;

                tempTriangles[i + 3] = 1 + 4 * num;
                tempTriangles[i + 4] = 0 + 4 * num;
                tempTriangles[i + 5] = 3 + 4 * num;
            }
        }

        m_spriteMesh.vertices = tempVertices.ToArray();
        m_spriteMesh.uv = tempUv.ToArray();
        m_spriteMesh.triangles = tempTriangles.ToArray();

        if (m_spriteMesh == null)
            return;

        m_spriteCanvasRenderer.SetMesh(m_spriteMesh);
        m_spriteGraphic.UpdateMaterial();
    }

}

[System.Serializable]
public class SpriteTagInfor
{
    /// <summary>
    /// sprite名称
    /// </summary>
    public string name;
    /// <summary>
    /// 对应的字符索引
    /// </summary>
    public int index;
    /// <summary>
    /// 大小
    /// </summary>
    public Vector2 size;
}


[System.Serializable]
public class InlineSpriteInfor
{
    // 文字的最后的位置
    public Vector3 textpos;
    // 4 顶点 
    public Vector3[] vertices;
    //4 uv
    public Vector2[] uv;
    //6 三角顶点顺序
    public int[] triangles;
}
