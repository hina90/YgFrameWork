using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Text))]
public class FontGradient : BaseMeshEffect
{
    /// <summary>
    /// 前的颜色
    /// </summary>
    public Color32 OneColor = Color.white;
    /// <summary>
    /// 后的颜色
    /// </summary>
    public Color32 TwoColor = Color.black;
    /// <summary>
    /// 设置颜色的方式
    /// </summary>
    public Whole m_Whole = Whole.Every;
    /// <summary>
    /// 设置文字的刷新类型
    /// </summary>
    public GradientType m_GradientType = GradientType.TopToDown;

    /// <summary>
    /// 每个字的顶点数
    /// </summary>
    private int m_Everyword = 6;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vh"></param>
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }
        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        int count = vertexList.Count;
        if (count == 0)
        {
            return;
        }
        ApplyGradient(vertexList, 0, count);
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }

    private void ApplyGradient(List<UIVertex> vertexList, int start, int end)
    {
        Vector4 temp_Vector = new Vector4
        {
            leftx = vertexList[0].position.x,
            rightx = vertexList[0].position.x,
            topy = vertexList[0].position.y,
            bottomy = vertexList[0].position.y,
        };



        int idx = 0;
        for (int i = start; i < end; ++i)
        {
            float y = vertexList[i].position.y;
            float x = vertexList[i].position.x;
            if (y > temp_Vector.bottomy)
            {
                temp_Vector.bottomy = y;
            }
            else if (y < temp_Vector.topy)
            {
                temp_Vector.topy = y;
            }
            if (x > temp_Vector.leftx)
            {
                temp_Vector.leftx = x;
            }
            else if (x < temp_Vector.rightx)
            {
                temp_Vector.rightx = x;
            }
            if (m_Whole == Whole.Every)
            {
                if ((i + 1) % m_Everyword == 0)
                {
                    SettingEveryword(vertexList, temp_Vector, idx);
                    if (i != end - 1)
                    {
                        temp_Vector.topy = vertexList[i + 1].position.y;
                        temp_Vector.bottomy = vertexList[i + 1].position.y;
                        temp_Vector.leftx = vertexList[i + 1].position.x;
                        temp_Vector.rightx = vertexList[i + 1].position.x;
                    }
                    idx = i + 1;
                }
            }
        }
        if (m_Whole == Whole.Whole)
        {
            SettingWholeWord(vertexList, temp_Vector, start, end);
        }
    }
    /// <summary>
    /// 设置每个字的颜色信息
    /// </summary>
    void SettingEveryword(List<UIVertex> vertexList, Vector4 vector, int idx)
    {
        for (int i = idx; i < idx + m_Everyword; i++)
        {
            UIVertex uiVertex = vertexList[i];
            vertexList[i] = SettingVertexColor(uiVertex, vector);
        }
    }
    /// <summary>
    /// 设置整体的颜色
    /// </summary>
    /// <param name="vertexList"></param>
    /// <param name="vector"></param>
    /// <param name="start">起点</param>
    /// <param name="end">终点</param>
    void SettingWholeWord(List<UIVertex> vertexList, Vector4 vector, int start, int end)
    {
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];
            vertexList[i] = SettingVertexColor(uiVertex, vector);
        }
    }
    /// <summary>
    /// 设置顶点颜色
    /// </summary>
    /// <param name="uiVertex"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    private UIVertex SettingVertexColor(UIVertex uiVertex, Vector4 vector)
    {
        float uiElementHeight = Mathf.Abs(vector.topy - vector.bottomy);
        float uiElementWidth = Mathf.Abs(vector.leftx - vector.rightx);
        float temp_Value = 0;
        float temp_Width = 0;
        float temp_Hight = 0;
        switch (m_GradientType)
        {
            case GradientType.TopToDown:
                temp_Hight = Mathf.Abs(vector.bottomy - uiVertex.position.y);
                temp_Value = uiElementHeight;
                break;
            case GradientType.DownToTop:
                temp_Hight = Mathf.Abs(uiVertex.position.y - vector.topy);
                temp_Value = uiElementHeight;
                break;
            case GradientType.LeftToRight:
                temp_Width = Mathf.Abs(vector.rightx - uiVertex.position.x);
                temp_Value = uiElementWidth;
                break;
            case GradientType.RightToLeft:
                temp_Width = Mathf.Abs(uiVertex.position.x - vector.leftx);
                temp_Value = uiElementWidth;
                break;
            case GradientType.LeftSlope:
                temp_Width = Mathf.Abs(vector.rightx - uiVertex.position.x);
                temp_Hight = Mathf.Abs(vector.bottomy - uiVertex.position.y);
                temp_Value = uiElementHeight + uiElementWidth;
                break;
            case GradientType.RightSlope:
                temp_Width = Mathf.Abs(uiVertex.position.x - vector.leftx);
                temp_Hight = Mathf.Abs(vector.bottomy - uiVertex.position.y);
                temp_Value = uiElementHeight + uiElementWidth;
                break;
        }
        uiVertex.color = Color32.LerpUnclamped(OneColor, TwoColor, (temp_Width + temp_Hight) / temp_Value);
        return uiVertex;
    }

    /// <summary>
    /// 整体性
    /// </summary>
    public enum Whole
    {
        /// <summary>
        /// 单个字体
        /// </summary>
        Every,
        /// <summary>
        /// 整体
        /// </summary>
        Whole,
    }
    /// <summary>
    /// 渐变方向
    /// </summary>
    public enum GradientType
    {
        /// <summary>
        /// 上至下
        /// </summary>
        TopToDown,
        /// <summary>
        /// 下至上
        /// </summary>
        DownToTop,
        /// <summary>
        /// 左至右
        /// </summary>
        LeftToRight,
        /// <summary>
        /// 右至左
        /// </summary>
        RightToLeft,
        /// <summary>
        /// 左斜角
        /// </summary>
        LeftSlope,
        /// <summary>
        /// 右斜角
        /// </summary>
        RightSlope,
    }
    public struct Vector4
    {
        public float leftx;
        public float rightx;
        public float topy;
        public float bottomy;
    }
}
