using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/CustomItalic")]
public class CustomItalic : BaseMeshEffect
{
    [SerializeField]
    private float slope;

    List<UIVertex> mVertexList = null;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) { return; }
        int count = vh.currentVertCount;
        if (count == 0) { return; }
        if (mVertexList == null) { mVertexList = new List<UIVertex>(); }
        vh.GetUIVertexStream(mVertexList);
        //Debug.LogError(JsonConvert.SerializeObject(mVertexList.Select(v => new { v.position.x, v.position.y }), Formatting.Indented));
        ApplyGradient(mVertexList);
        vh.Clear();
        vh.AddUIVertexTriangleStream(mVertexList);
    }
    private void ApplyGradient(List<UIVertex> vertexList)
    {
        float max_y = 0;
        float min_y = 0;
        for (int i = 0; i < vertexList.Count;)
        {
            List<UIVertex> sub_vertexs = vertexList.GetRange(i, 6);

            foreach(var item in sub_vertexs)
            {
                if (max_y < item.position.y)
                    max_y = item.position.y;
                if (min_y > item.position.y)
                    min_y = item.position.y;
            }
            ChangePos(vertexList, i, min_y, max_y);
            ChangePos(vertexList, i + 1, min_y, max_y);
            ChangePos(vertexList, i + 2, min_y, max_y);
            ChangePos(vertexList, i + 3, min_y, max_y);
            ChangePos(vertexList, i + 4, min_y, max_y);
            ChangePos(vertexList, i + 5, min_y, max_y);
            i += 6;
        }
    }

    private void ChangePos(List<UIVertex> verList, int index, float min_y, float max_y)
    {
        UIVertex temp = verList[index];
        Vector3 pos = temp.position;
        pos.x += pos.y == min_y ? 0 : slope;
        temp.position = pos;
        verList[index] = temp;
    }
}
