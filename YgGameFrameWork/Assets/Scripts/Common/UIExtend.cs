using UnityEngine;
using UnityEngine.UI;

public static class UIExtend
{
    static public void setGray(this Image image, bool isGray)
    {
        Material mat;
        if (isGray)
        {
            mat = new Material(Shader.Find("Sprites/Gray"));
            image.material = mat;
        }
        else
        {
            mat = new Material(Shader.Find("Sprites/Default"));
        }
        image.material = mat;
    }
    static public void setGray(this Text text, bool isGray)
    {
        Material mat;
        if (isGray)
        {
            mat = new Material(Shader.Find("Sprites/Gray"));
        }
        else
        {
            //mat = new Material(Shader.Find("Sprites/Default"));
            mat = null;
        }
        text.material = mat;
    }
}
