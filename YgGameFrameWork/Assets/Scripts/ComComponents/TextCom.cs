using UnityEngine;
using TMPro;

public class TextCom : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void FollowActor(Vector3 followPos)
    {
        this.transform.position = Camera.main.WorldToScreenPoint(new Vector3(followPos.x + 0.09f, followPos.y + 1.8f, followPos.z));
        this.gameObject.SetActive(!CameraMove.isProspect);
    }
    public void UpdateText(string currentValue)
    {
        text.text = currentValue;
    }
    public void Release()
    {
        text = null;
        Destroy(gameObject);
    }
}
