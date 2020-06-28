using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class ScrollPageTool : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Tooltip("页码父级组件")]
    public Transform pageDot;
    public GridLayoutGroup itemConnect;
    private int m_nowPage;//从0开始
    private int m_pageCount;
    /// <summary>
    /// 滑动距离超过一页的 (m_dragNum*10)% 则滑动成功
    /// </summary>
    public int m_dragNum;
    private float m_pageAreaSize;
    private const float SCROLL_SMOOTH_TIME = 0.2F;
    private const float SCROLL_MOVE_SPEED = 1F;
    private float scrollMoveSpeed = 0f;
    private bool scrollNeedMove = false;
    private float scrollTargetValue;
    public ScrollRect scrollRect;
    //页码对象池
    private GameObjectPool m_PageDotPool;
    private List<Toggle> m_Toggles;

    private void Awake()
    {
        m_Toggles = new List<Toggle>();
        m_PageDotPool = new GameObjectPool(pageDot, "PageDot");
    }

    /// <summary>
    /// 初始化页面管理器
    /// </summary>
    /// <param name="_allItemNum">总的Item数</param>
    /// <param name="pageItemSize">每页Item尺寸  比如：new vector2(2,2) 表示每页2x2大小</param>
    /// <param name="isNeedChangeSize">是否需要动态调整页面大小</param>
    /// <param name="pageNum">起始页码</param>
    public void InitManager(int _allItemNum, Vector2 pageItemSize, bool isNeedChangeSize = true, int pageNum = 0)
    {
        RectTransform conRect = itemConnect.GetComponent<RectTransform>();
        scrollRect.horizontalNormalizedPosition = 0;

        InitPageDot(_allItemNum);
        int _pageItemNum = (int)(pageItemSize.x * pageItemSize.y);
        m_pageCount = (_allItemNum / _pageItemNum) + ((_allItemNum % _pageItemNum == 0) ? 0 : 1);
        m_pageAreaSize = 1f / (m_pageCount - 1);
        ChangePage(pageNum);
        if (isNeedChangeSize)
            conRect.sizeDelta = new Vector2((itemConnect.cellSize.x * pageItemSize.x + itemConnect.spacing.x * pageItemSize.x) * m_pageCount,
            itemConnect.padding.top + itemConnect.padding.bottom + itemConnect.cellSize.y * pageItemSize.y + (pageItemSize.y - 1) * itemConnect.spacing.y);
    }

    /// <summary>
    /// 初始化页码圆点
    /// </summary>
    private void InitPageDot(int pageCount)
    {
        m_Toggles.Clear();
        m_PageDotPool.RecycleAll();
        for (int i = 0; i < pageCount; i++)
        {
            GameObject dot = m_PageDotPool.GetPool();
            dot.transform.SetAsLastSibling();
            Toggle toggle = dot.GetOrAddComponent<Toggle>();
            toggle.group = pageDot.GetComponent<ToggleGroup>();
            toggle.isOn = false;
            m_Toggles.Add(toggle);
        }
    }

    public void InitManager(int pageNum, int targetPage = 0, bool isShowAnim = false)
    {
        m_pageCount = pageNum;
        ChangePage(targetPage, isShowAnim);
    }

    private void Paging(int num)
    {
        //maxNum-1,从0开始
        num = (num < 0) ? -1 : 1;
        int temp = Mathf.Clamp(m_nowPage + num, 0, m_pageCount - 1);
        if (m_nowPage == temp)
            return;
        ChangePage(temp);
    }

    void Update()
    {
        ScrollControl();
    }

    public int GetPageNum { get { return m_nowPage; } }
    //按页翻动
    private void ScrollControl()
    {
        if (!scrollNeedMove)
            return;
        if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - scrollTargetValue) < 0.01f)
        {
            scrollRect.horizontalNormalizedPosition = scrollTargetValue;
            scrollNeedMove = false;
            return;
        }
        scrollRect.horizontalNormalizedPosition = Mathf.SmoothDamp(scrollRect.horizontalNormalizedPosition, scrollTargetValue, ref scrollMoveSpeed, SCROLL_SMOOTH_TIME);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollNeedMove = false;
        scrollTargetValue = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int tempPage = m_nowPage;
        int num = (((scrollRect.horizontalNormalizedPosition - (m_nowPage * m_pageAreaSize)) >= 0) ? 1 : -1);
        if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - (m_nowPage * m_pageAreaSize)) >= (m_pageAreaSize / 10f) * m_dragNum)
        {
            tempPage += num;
        }
        ChangePage(tempPage);
    }

    /// <summary>
    /// 切换页面
    /// </summary>
    /// <param name="pageNum"></param>
    /// <param name="isShowAnim"></param>
    public void ChangePage(int pageNum, bool isShowAnim = true)
    {
        if (pageNum >= m_pageCount)
            pageNum = m_pageCount - 1;
        if (pageNum < 0)
            pageNum = 0;

        m_nowPage = pageNum;
        ChangePageDot(pageNum);
        if (isShowAnim)
        {
            scrollTargetValue = m_nowPage * m_pageAreaSize;
            scrollNeedMove = true;
            scrollMoveSpeed = 0;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = m_nowPage * m_pageAreaSize;
        }
        ChangePageDot(m_nowPage);
    }

    /// <summary>
    /// 更新页码
    /// </summary>
    /// <param name="num"></param>
    public void ChangePageDot(int num)
    {
        int maxPageTo0Start = m_pageCount - 1;
        m_nowPage = Mathf.Clamp(num, 0, maxPageTo0Start);
        m_Toggles[m_nowPage].isOn = true;
        //only one page
        if (maxPageTo0Start == 0)
        {
            scrollRect.enabled = false;
            return;
        }
        else
        {
            scrollRect.enabled = true;
        }
    }

    private void OnDestroy()
    {
        m_PageDotPool.Release();
        m_PageDotPool = null;
    }
}
