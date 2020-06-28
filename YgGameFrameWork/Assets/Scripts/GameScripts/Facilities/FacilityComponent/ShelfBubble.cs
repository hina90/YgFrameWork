using Tool.Database;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfBubble : MonoBehaviour
{
    private static Callback<StoreConfigData> loadCallBack;
    private SpriteRenderer goodsSprite;

    private void Awake()
    {
        goodsSprite = transform.Find("goods").GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (Utils.CheckIsClickOnUI() || CameraMove.isProspect) return;
        UIManager.Instance.OpenUI<UI_Goods>();
        UI_Goods.SetLoadCallBack(loadCallBack);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置种植花种回调
    /// </summary>
    /// <param name="callback"></param>
    internal static void SetLoadingAction(Callback<StoreConfigData> callback)
    {
        loadCallBack = callback;
    }
}
