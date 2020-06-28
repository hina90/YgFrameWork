using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public enum MoveDir { Horizontal, Vertical, }
    public Image m_ImgFill;
    public Text m_TxtValue;
    public bool IsShowTxt;
    public MoveDir m_MoveDir;
    private float m_Value;
    public float Value
    {
        get
        {
            return m_Value;
        }
        set
        {
            m_Value = value;
            switch (m_MoveDir)
            {
                case MoveDir.Horizontal:
                    m_ImgFill.transform.localScale = new Vector3(Mathf.Clamp(value, 0, 1), 1, 1);
                    break;
                case MoveDir.Vertical:
                    m_ImgFill.transform.localScale = new Vector3(1, Mathf.Clamp(value, 0, 1), 1);
                    break;
                default:
                    break;
            }
            m_TxtValue.text = ((int)(value * 100)).ToString() + "%";
            m_TxtValue.gameObject.SetActive(IsShowTxt);
        }
    }
}
