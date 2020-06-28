using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace UnityEngine.UI
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]   //当附加该组件的时候。会强制自动添加组件typeof(RectTransform)
    public abstract class LoopScrollRect : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement, ILayoutElement, ILayoutGroup
    {
        //==========LoopScrollRect==========
        [Tooltip("Prefab Source/预制体信息")]      //鼠标放在上面的时候会显示提示
        public LoopScrollPrefabSource prefabSource; //预制体信息

        [Tooltip("Total count, negative means INFINITE mode/总计数，负数表示“无限模式”")]
        public int totalCount;      //内容总计数，即一共多少条内容


        [HideInInspector]           //将这个变量在编辑器中隐藏
        [NonSerialized]             //标记这个变量为不可序列化
        /// <summary>
        /// 数据传递，传给预制体
        /// </summary>
        public LoopScrollDataSource dataSource = LoopScrollSendIndexSource.Instance;
        /// <summary>
        /// 接收数据的属性，如果没有传递数据的话那预制体默认接收到的是下标值
        /// </summary>
        /// <value></value>
        public object[] objectsToFill
        {
            // wrapper for forward compatbility   前向兼容性包装
            set
            {
                if (value != null)
                    dataSource = new LoopScrollArraySource<object>(value);//如果有传递数据则发送给预制体的是泛型消息
                else
                    dataSource = LoopScrollSendIndexSource.Instance;
            }
        }
        [Tooltip("两端预留出来的缓存量(像素数)")]
        protected float threshold = 0;          //阈值
        [Tooltip("Reverse direction for dragging/反向拖动")]
        /// <summary>
        /// 反向拖动
        /// </summary>
        public bool reverseDirection = false;
        [Tooltip("Rubber scale for outside/外部比例")]
        public float rubberScale = 1;
        /// <summary>
        /// 每个单元格的下标值，从0开始，当前页第一个元素的下标
        /// </summary>
        protected int itemTypeStart = 0;
        /// <summary>
        /// 每个单元格的下标值，从0开始，当前页最后一个元素的下标
        /// 值与itemTypeStart是相同的，只不过是一个向上滑向下滑使用的时候的不同
        /// </summary>
        protected int itemTypeEnd = 0;
        /// <summary>
        /// 获得物体大小
        /// </summary>
        protected abstract float GetSize(RectTransform item);
        /// <summary>
        /// 获得LayoutGroup中的间隔spacing
        /// </summary>
        protected abstract float GetDimension(Vector2 vector);
        /// <summary>
        /// 获得位置的偏移量
        /// </summary>
        protected abstract Vector2 GetVector(float value);
        /// <summary>
        /// 方向标志
        /// </summary>
        protected int directionSign = 0;
        /// <summary>
        /// Content间距
        /// </summary>
        private float m_ContentSpacing = -1;
        /// <summary>
        /// 排版组件
        /// </summary>
        protected GridLayoutGroup m_GridLayout = null;
        /// <summary>
        /// 若m_ContentSpacing有值则直接返回间距值，若没有则在组件中去获取预制体之间的间距值
        /// </summary>
        /// <value>返回Content间距</value>
        protected float contentSpacing
        {
            get
            {
                if (m_ContentSpacing >= 0)
                {
                    return m_ContentSpacing;
                }
                m_ContentSpacing = 0;
                if (content != null)
                {
                    HorizontalOrVerticalLayoutGroup layout1 = content.GetComponent<HorizontalOrVerticalLayoutGroup>();
                    if (layout1 != null)
                    {
                        m_ContentSpacing = layout1.spacing;
                    }
                    m_GridLayout = content.GetComponent<GridLayoutGroup>();
                    if (m_GridLayout != null)
                    {
                        //获取绝对值
                        m_ContentSpacing = Mathf.Abs(GetDimension(m_GridLayout.spacing));
                    }
                }
                return m_ContentSpacing;
            }
        }
        /// <summary>
        /// 内容约束计数
        /// </summary>
        private int m_ContentConstraintCount = 0;
        /// <summary>
        /// 一行中有多少个物体
        /// </summary>
        protected int contentConstraintCount
        {
            get
            {
                if (m_ContentConstraintCount > 0)
                {
                    return m_ContentConstraintCount;
                }
                m_ContentConstraintCount = 1;
                if (content != null)
                {
                    GridLayoutGroup layout2 = content.GetComponent<GridLayoutGroup>();
                    if (layout2 != null)
                    {
                        //Constraint的模式必须是Fixed Column Count
                        if (layout2.constraint == GridLayoutGroup.Constraint.Flexible)
                        {
                            Debug.LogWarning("[LoopScrollRect] Flexible not supported yet");
                        }
                        m_ContentConstraintCount = layout2.constraintCount;
                    }
                }
                return m_ContentConstraintCount;
            }
        }

        // the first line
        /// <summary>
        /// 当前页第一行是总共元素的第几行
        /// </summary>
        int StartLine
        {
            get
            {
                //返回整数大于或等于Value值
                return Mathf.CeilToInt((float)(itemTypeStart) / contentConstraintCount);
            }
        }

        // how many lines we have for now
        /// <summary>
        /// 当前页面有几行
        /// </summary>
        int CurrentLines
        {
            get
            {
                return Mathf.CeilToInt((float)(itemTypeEnd - itemTypeStart) / contentConstraintCount);
            }
        }

        // how many lines we have in total
        /// <summary>
        /// 我们总共有多少行
        /// </summary>
        int TotalLines
        {
            get
            {
                return Mathf.CeilToInt((float)(totalCount) / contentConstraintCount);
            }
        }
        /// <summary>
        /// 更新控件
        /// </summary>
        /// <param name="viewBounds">显示控件的边界盒</param>
        /// <param name="contentBounds">content控件的边界盒</param>
        protected virtual bool UpdateItems(Bounds viewBounds, Bounds contentBounds) { return false; }
        //==========LoopScrollRect==========

        /// <summary>
        /// 移动类型
        /// </summary>
        public enum MovementType
        {
            /// <summary>
            /// 无限滚动
            /// </summary>
            Unrestricted, // Unrestricted movement -- can scroll forever
            /// <summary>
            /// 有弹力的滚动，到边缘后会回弹
            /// </summary>
            Elastic, // Restricted but flexible -- can go past the edges, but springs back in place
            /// <summary>
            /// 没有弹力的滚动，无法越过边界
            /// </summary>
            Clamped, // Restricted movement where it's not possible to go past the edges
        }
        /// <summary>
        /// 滚动条可见性
        /// </summary>
        public enum ScrollbarVisibility
        {
            /// <summary>
            /// 始终可见
            /// </summary>
            Permanent,
            /// <summary>
            /// 自动隐藏
            /// </summary>
            AutoHide,
            /// <summary>
            /// 自动隐藏和展开视图
            /// </summary>
            AutoHideAndExpandViewport,
        }

        [Serializable]  //表示变量可被序列化
        /// <summary>
        /// UI的事件回调
        /// </summary>
        public class ScrollRectEvent : UnityEvent<Vector2> { }

        [SerializeField]
        private RectTransform m_Content;
        public RectTransform content { get { return m_Content; } set { m_Content = value; } }

        [SerializeField]
        private bool m_Horizontal = true;
        public bool horizontal { get { return m_Horizontal; } set { m_Horizontal = value; } }

        [SerializeField]
        private bool m_Vertical = true;
        public bool vertical { get { return m_Vertical; } set { m_Vertical = value; } }

        [Tooltip("滚动类型")]
        [SerializeField]
        private MovementType m_MovementType = MovementType.Elastic;
        public MovementType movementType { get { return m_MovementType; } set { m_MovementType = value; } }

        [Tooltip("Elastic模式下的弹力值")]
        [SerializeField]
        /// <summary>
        /// Elastic模式下的弹力值
        /// </summary>
        private float m_Elasticity = 0.1f; // Only used for MovementType.Elastic
        /// <summary>
        /// 弹力值
        /// </summary>
        public float elasticity { get { return m_Elasticity; } set { m_Elasticity = value; } }

        [Tooltip("拖动后的惯性")]
        [SerializeField]
        /// <summary>
        /// 拖动后的惯性
        /// </summary>
        private bool m_Inertia = true;
        /// <summary>
        /// 拖动后的惯性
        /// </summary>
        public bool inertia { get { return m_Inertia; } set { m_Inertia = value; } }

        [Tooltip("减速率，只在inertia（惯性）激活状态下有用")]
        [SerializeField]
        /// <summary>
        /// 减速率，只在inertia（惯性）激活状态下有用
        /// </summary>
        private float m_DecelerationRate = 0.135f; // Only used when inertia is enabled
        public float decelerationRate { get { return m_DecelerationRate; } set { m_DecelerationRate = value; } }

        [Tooltip("滚动灵敏度")]
        [SerializeField]
        /// <summary>
        /// 滚动灵敏度
        /// </summary>
        private float m_ScrollSensitivity = 1.0f;
        public float scrollSensitivity { get { return m_ScrollSensitivity; } set { m_ScrollSensitivity = value; } }

        [SerializeField]
        /// <summary>
        /// 视区（viewport）的RectTransform
        /// </summary>
        private RectTransform m_Viewport;
        /// <summary>
        /// 视区的RectTransform
        /// </summary>
        public RectTransform viewport { get { return m_Viewport; } set { m_Viewport = value; SetDirtyCaching(); } }

        [SerializeField]
        /// <summary>
        /// 水平Scrollbar
        /// </summary>
        private Scrollbar m_HorizontalScrollbar;
        /// <summary>
        /// 水平Scrollbar
        /// </summary>
        public Scrollbar horizontalScrollbar
        {
            get
            {
                return m_HorizontalScrollbar;
            }
            set
            {
                if (m_HorizontalScrollbar)
                    m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
                m_HorizontalScrollbar = value;
                if (m_HorizontalScrollbar)
                    m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
                SetDirtyCaching();
            }
        }

        [SerializeField]
        /// <summary>
        /// 垂直Scrollbar
        /// </summary>
        private Scrollbar m_VerticalScrollbar;
        /// <summary>
        /// 垂直Scrollbar
        /// </summary>
        public Scrollbar verticalScrollbar
        {
            get
            {
                return m_VerticalScrollbar;
            }
            set
            {
                if (m_VerticalScrollbar)
                    m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
                m_VerticalScrollbar = value;
                if (m_VerticalScrollbar)
                    m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
                SetDirtyCaching();
            }
        }

        [Tooltip("滚动条可见性")]
        [SerializeField]
        private ScrollbarVisibility m_HorizontalScrollbarVisibility;
        public ScrollbarVisibility horizontalScrollbarVisibility { get { return m_HorizontalScrollbarVisibility; } set { m_HorizontalScrollbarVisibility = value; SetDirtyCaching(); } }

        [SerializeField]
        private ScrollbarVisibility m_VerticalScrollbarVisibility;
        public ScrollbarVisibility verticalScrollbarVisibility { get { return m_VerticalScrollbarVisibility; } set { m_VerticalScrollbarVisibility = value; SetDirtyCaching(); } }

        [SerializeField]
        /// <summary>
        /// 滚动条和视口之间的空间
        /// </summary>
        private float m_HorizontalScrollbarSpacing;
        /// <summary>
        /// 滚动条和视口之间的空间
        /// </summary>
        public float horizontalScrollbarSpacing { get { return m_HorizontalScrollbarSpacing; } set { m_HorizontalScrollbarSpacing = value; SetDirty(); } }

        [SerializeField]
        /// <summary>
        /// 滚动条和视口之间的空间
        /// </summary>
        private float m_VerticalScrollbarSpacing;
        /// <summary>
        /// 滚动条和视口之间的空间
        /// </summary>
        public float verticalScrollbarSpacing { get { return m_VerticalScrollbarSpacing; } set { m_VerticalScrollbarSpacing = value; SetDirty(); } }

        [Tooltip("事件回调")]
        [SerializeField]
        private ScrollRectEvent m_OnValueChanged = new ScrollRectEvent();
        /// <summary>
        /// 事件回调
        /// </summary>
        public ScrollRectEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

        // The offset from handle position to mouse down position
        /// <summary>
        /// 从手柄位置到鼠标向下位置的偏移量
        /// </summary>
        private Vector2 m_PointerStartLocalCursor = Vector2.zero;
        private Vector2 m_ContentStartPosition = Vector2.zero;

        /// <summary>
        /// 视区的rectTransform组件
        /// </summary>
        private RectTransform m_ViewRect;
        /// <summary>
        /// 视区的rectTransform组件，若为空则设置为m_Viewport
        /// </summary>
        protected RectTransform viewRect
        {
            get
            {
                if (m_ViewRect == null)
                    m_ViewRect = m_Viewport;
                if (m_ViewRect == null)
                    m_ViewRect = (RectTransform)transform;
                return m_ViewRect;
            }
        }
        /// <summary>
        /// Content的边界盒
        /// </summary>
        private Bounds m_ContentBounds;
        /// <summary>
        /// 视区的边界盒
        /// </summary>
        private Bounds m_ViewBounds;
        /// <summary>
        /// 速度
        /// </summary>
        private Vector2 m_Velocity;
        public Vector2 velocity { get { return m_Velocity; } set { m_Velocity = value; } }
        /// <summary>
        /// 是否能拖拽
        /// </summary>
        private bool m_Dragging;
        /// <summary>
        /// 上一个位置
        /// </summary>
        private Vector2 m_PrevPosition = Vector2.zero;
        /// <summary>
        /// 上一个位置时候的content的边界盒
        /// </summary>
        private Bounds m_PrevContentBounds;
        /// <summary>
        /// 上一个位置时候的视区的边界盒
        /// </summary>
        private Bounds m_PrevViewBounds;
        [NonSerialized]//标记为不能序列化
        /// <summary>
        /// 是否重建布局
        /// </summary>
        private bool m_HasRebuiltLayout = false;
        /// <summary>
        /// 水平滑块是否展开
        /// </summary>
        private bool m_HSliderExpand;
        /// <summary>
        /// 垂直滑块是否展开
        /// </summary>
        private bool m_VSliderExpand;
        /// <summary>
        /// 水平滑块高度
        /// </summary>
        private float m_HSliderHeight;
        /// <summary>
        /// 垂直滑块宽度
        /// </summary>
        private float m_VSliderWidth;

        [System.NonSerialized]
        /// <summary>
        /// 自己的rectTransform组件
        /// </summary>
        private RectTransform m_Rect;
        /// <summary>
        /// 自己的rectTransform组件
        /// </summary>
        /// <value></value>
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }
        /// <summary>
        /// 水平scrollbar的rect组件
        /// </summary>
        private RectTransform m_HorizontalScrollbarRect;
        /// <summary>
        /// 垂直scrollbar的rect组件
        /// </summary>
        private RectTransform m_VerticalScrollbarRect;
        /// <summary>
        /// 驱动矩形变换跟踪器
        /// </summary>
        private DrivenRectTransformTracker m_Tracker;

        protected LoopScrollRect()
        {
            flexibleWidth = -1;
        }

        //==========LoopScrollRect==========

        /// <summary>
        /// 清除所有单元格
        /// </summary>
        public void ClearCells()
        {
            //当在任何种类的播放器时，返回真（只读）。在Unity编辑器中，如果处于播放模式时返回真。
            if (Application.isPlaying)
            {
                itemTypeStart = 0;
                itemTypeEnd = 0;
                totalCount = 0;
                objectsToFill = null;
                for (int i = content.childCount - 1; i >= 0; i--)
                {
                    prefabSource.ReturnObject(content.GetChild(i));
                }
            }
        }
        /// <summary>
        /// 滚动单元格到指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="speed"></param>
        public void SrollToCell(int index, float speed)
        {
            if (totalCount >= 0 && (index < 0 || index >= totalCount))
            {
                Debug.LogWarningFormat("invalid index {0}", index);
                return;
            }
            if (speed <= 0)
            {
                Debug.LogWarningFormat("invalid speed {0}", speed);
                return;
            }
            StopAllCoroutines();//停止此行为上运行的所有协同程序。
            StartCoroutine(ScrollToCellCoroutine(index, speed));//开启协同
        }
        /// <summary>
        /// 滚动单元格
        /// </summary>
        /// <param name="index"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        IEnumerator ScrollToCellCoroutine(int index, float speed)
        {
            bool needMoving = true;
            while (needMoving)
            {
                yield return null;//等一帧
                if (!m_Dragging)
                {
                    float move = 0;
                    if (index < itemTypeStart)
                    {
                        move = -Time.deltaTime * speed;
                    }
                    else if (index >= itemTypeEnd)
                    {
                        move = Time.deltaTime * speed;
                    }
                    else
                    {
                        m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
                        var m_ItemBounds = GetBounds4Item(index);
                        var offset = 0.0f;
                        if (directionSign == -1)
                            offset = reverseDirection ? (m_ViewBounds.min.y - m_ItemBounds.min.y) : (m_ViewBounds.max.y - m_ItemBounds.max.y);
                        else if (directionSign == 1)
                            offset = reverseDirection ? (m_ItemBounds.max.x - m_ViewBounds.max.x) : (m_ItemBounds.min.x - m_ViewBounds.min.x);
                        // check if we cannot move on
                        if (totalCount >= 0)
                        {
                            if (offset > 0 && itemTypeEnd == totalCount && !reverseDirection)
                            {
                                m_ItemBounds = GetBounds4Item(totalCount - 1);
                                // reach bottom
                                if ((directionSign == -1 && m_ItemBounds.min.y > m_ViewBounds.min.y) ||
                                    (directionSign == 1 && m_ItemBounds.max.x < m_ViewBounds.max.x))
                                {
                                    needMoving = false;
                                    break;
                                }
                            }
                            else if (offset < 0 && itemTypeStart == 0 && reverseDirection)
                            {
                                m_ItemBounds = GetBounds4Item(0);
                                if ((directionSign == -1 && m_ItemBounds.max.y < m_ViewBounds.max.y) ||
                                    (directionSign == 1 && m_ItemBounds.min.x > m_ViewBounds.min.x))
                                {
                                    needMoving = false;
                                    break;
                                }
                            }
                        }

                        float maxMove = Time.deltaTime * speed;
                        if (Mathf.Abs(offset) < maxMove)
                        {
                            needMoving = false;
                            move = offset;
                        }
                        else
                            move = Mathf.Sign(offset) * maxMove;
                    }
                    if (move != 0)
                    {
                        Vector2 offset = GetVector(move);
                        content.anchoredPosition += offset;
                        m_PrevPosition += offset;
                        m_ContentStartPosition += offset;
                    }
                }
            }
            StopMovement();
            UpdatePrevData();
        }
        /// <summary>
        /// 刷新单元格数据
        /// </summary>
        public void RefreshCells()
        {
            if (Application.isPlaying)
            {
                itemTypeEnd = itemTypeStart;
                // recycle items if we can
                for (int i = 0; i < content.childCount; i++)
                {
                    if (itemTypeEnd < totalCount)
                    {
                        dataSource.ProvideData(content.GetChild(i), itemTypeEnd);
                        itemTypeEnd++;
                    }
                    else
                    {
                        prefabSource.ReturnObject(content.GetChild(i));
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// 从末端重新填充单元格
        /// </summary>
        /// <param name="offset"></param>
        public void RefillCellsFromEnd(int offset = 0)
        {
            if (!Application.isPlaying || prefabSource == null)
                return;

            StopMovement();
            itemTypeEnd = reverseDirection ? offset : totalCount - offset;
            itemTypeStart = itemTypeEnd;

            if (totalCount >= 0 && itemTypeStart % contentConstraintCount != 0)
                Debug.LogWarning("Grid will become strange since we can't fill items in the last line");

            for (int i = m_Content.childCount - 1; i >= 0; i--)
            {
                prefabSource.ReturnObject(m_Content.GetChild(i));
            }

            float sizeToFill = 0, sizeFilled = 0;
            if (directionSign == -1)
                sizeToFill = viewRect.rect.size.y;
            else
                sizeToFill = viewRect.rect.size.x;

            while (sizeToFill > sizeFilled)
            {
                float size = reverseDirection ? NewItemAtEnd() : NewItemAtStart();
                if (size <= 0) break;
                sizeFilled += size;
            }

            Vector2 pos = m_Content.anchoredPosition;
            float dist = Mathf.Max(0, sizeFilled - sizeToFill);
            if (reverseDirection)
                dist = -dist;
            if (directionSign == -1)
                pos.y = dist;
            else if (directionSign == 1)
                pos.x = -dist;
            m_Content.anchoredPosition = pos;
        }
        /// <summary>
        /// 重新填充单元格
        /// </summary>
        public void RefillCells(int offset = 0)
        {
            if (!Application.isPlaying || prefabSource == null)
                return;

            StopMovement();
            itemTypeStart = reverseDirection ? totalCount - offset : offset;
            itemTypeEnd = itemTypeStart;

            if (totalCount >= 0 && itemTypeStart % contentConstraintCount != 0)
                Debug.LogWarning("Grid will become strange since we can't fill items in the first line");

            // Don't `Canvas.ForceUpdateCanvases();` here, or it will new/delete cells to change itemTypeStart/End
            for (int i = m_Content.childCount - 1; i >= 0; i--)
            {
                prefabSource.ReturnObject(m_Content.GetChild(i));
            }

            float sizeToFill = 0, sizeFilled = 0;
            // m_ViewBounds may be not ready when RefillCells on Start
            if (directionSign == -1)
                sizeToFill = viewRect.rect.size.y;
            else
                sizeToFill = viewRect.rect.size.x;

            while (sizeToFill > sizeFilled)
            {
                float size = reverseDirection ? NewItemAtStart() : NewItemAtEnd();
                if (size <= 0) break;
                sizeFilled += size;
            }

            Vector2 pos = m_Content.anchoredPosition;
            if (directionSign == -1)
                pos.y = 0;
            else if (directionSign == 1)
                pos.x = 0;
            m_Content.anchoredPosition = pos;
        }
        /// <summary>
        /// 往上滑的时候，增加元素,实现了在头部增加一个(或一行，针对GridLayout)元素，并返回这些元素的高度
        /// </summary>
        /// <returns></returns>
        protected float NewItemAtStart()
        {     
            if (totalCount >= 0 && itemTypeStart - contentConstraintCount < 0)//到头了就返回0
            {
                return 0;
            }
            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                itemTypeStart--;
                RectTransform newItem = InstantiateNextItem(itemTypeStart);
                newItem.SetAsFirstSibling();//将转换移动到本地转换列表的开头。
                size = Mathf.Max(GetSize(newItem), size);
            }
            threshold = Mathf.Max(threshold, size * 1.5f);

            if (!reverseDirection)
            {
                Vector2 offset = GetVector(size);
                content.anchoredPosition += offset;
                m_PrevPosition += offset;
                m_ContentStartPosition += offset;
            }
            return size;
        }
        /// <summary>
        /// 往下滑的时候，删除元素,代表删除头部的一个元素
        /// </summary>
        /// <returns></returns>
        protected float DeleteItemAtStart()
        {
            // special case: when moving or dragging, we cannot simply delete start when we've reached the end
            //特殊情况：当移动或拖动时，我们不能在到达终点时简单地删除start
            if (((m_Dragging || m_Velocity != Vector2.zero) && totalCount >= 0 && itemTypeEnd >= totalCount - 1) || content.childCount == 0)
            {
                return 0;
            }

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                RectTransform oldItem = content.GetChild(0) as RectTransform;
                size = Mathf.Max(GetSize(oldItem), size);
                prefabSource.ReturnObject(oldItem);

                itemTypeStart++;

                if (content.childCount == 0)
                {
                    break;
                }
            }

            if (!reverseDirection)
            {
                Vector2 offset = GetVector(size);
                content.anchoredPosition -= offset;
                m_PrevPosition -= offset;
                m_ContentStartPosition -= offset;
            }
            return size;
        }

        /// <summary>
        /// 往下滑的时候，在底部增加元素
        /// </summary>
        /// <returns></returns>
        protected float NewItemAtEnd()
        {
            if (totalCount >= 0 && itemTypeEnd >= totalCount)
            {
                return 0;
            }
            float size = 0;
            // issue 4: fill lines to end first
            int count = contentConstraintCount - (content.childCount % contentConstraintCount);//底部每行要增加几个元素
            for (int i = 0; i < count; i++)
            {
                RectTransform newItem = InstantiateNextItem(itemTypeEnd);
                size = Mathf.Max(GetSize(newItem), size);
                itemTypeEnd++;
                if (totalCount >= 0 && itemTypeEnd >= totalCount)
                {
                    break;
                }
            }
            threshold = Mathf.Max(threshold, size * 1.5f);

            if (reverseDirection)
            {
                Vector2 offset = GetVector(size);
                content.anchoredPosition -= offset;
                m_PrevPosition -= offset;
                m_ContentStartPosition -= offset;
            }

            return size;
        }
        /// <summary>
        /// 往上滑的时候，删除底部的元素
        /// </summary>
        /// <returns></returns>
        protected float DeleteItemAtEnd()
        {
            if (((m_Dragging || m_Velocity != Vector2.zero) && totalCount >= 0 && itemTypeStart < contentConstraintCount)
                || content.childCount == 0)
            {
                return 0;
            }

            float size = 0;
            for (int i = 0; i < contentConstraintCount; i++)
            {
                RectTransform oldItem = content.GetChild(content.childCount - 1) as RectTransform;
                size = Mathf.Max(GetSize(oldItem), size);
                prefabSource.ReturnObject(oldItem);

                itemTypeEnd--;
                if (itemTypeEnd % contentConstraintCount == 0 || content.childCount == 0)
                {
                    break;  //只需删除整行
                }
            }

            if (reverseDirection)
            {
                Vector2 offset = GetVector(size);
                content.anchoredPosition += offset;
                m_PrevPosition += offset;
                m_ContentStartPosition += offset;
            }
            return size;
        }
        /// <summary>
        /// 实例化一个或者在缓存池中返回下一个单元格
        /// </summary>
        /// <param name="itemIdx"></param>
        /// <returns></returns>
        private RectTransform InstantiateNextItem(int itemIdx)
        {
            RectTransform nextItem = prefabSource.GetObject().transform as RectTransform;
            nextItem.transform.SetParent(content, false);
            nextItem.gameObject.SetActive(true);
            dataSource.ProvideData(nextItem, itemIdx);
            return nextItem;
        }
        //==========LoopScrollRect==========

        public virtual void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout)
            {
                UpdateCachedData();
            }

            if (executing == CanvasUpdate.PostLayout)
            {
                UpdateBounds();
                UpdateScrollbars(Vector2.zero);
                UpdatePrevData();

                m_HasRebuiltLayout = true;
            }
        }

        public virtual void LayoutComplete()
        { }

        public virtual void GraphicUpdateComplete()
        { }

        void UpdateCachedData()
        {
            Transform transform = this.transform;
            m_HorizontalScrollbarRect = m_HorizontalScrollbar == null ? null : m_HorizontalScrollbar.transform as RectTransform;
            m_VerticalScrollbarRect = m_VerticalScrollbar == null ? null : m_VerticalScrollbar.transform as RectTransform;

            // These are true if either the elements are children, or they don't exist at all.
            bool viewIsChild = (viewRect.parent == transform);
            bool hScrollbarIsChild = (!m_HorizontalScrollbarRect || m_HorizontalScrollbarRect.parent == transform);
            bool vScrollbarIsChild = (!m_VerticalScrollbarRect || m_VerticalScrollbarRect.parent == transform);
            bool allAreChildren = (viewIsChild && hScrollbarIsChild && vScrollbarIsChild);

            m_HSliderExpand = allAreChildren && m_HorizontalScrollbarRect && horizontalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport;
            m_VSliderExpand = allAreChildren && m_VerticalScrollbarRect && verticalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport;
            m_HSliderHeight = (m_HorizontalScrollbarRect == null ? 0 : m_HorizontalScrollbarRect.rect.height);
            m_VSliderWidth = (m_VerticalScrollbarRect == null ? 0 : m_VerticalScrollbarRect.rect.width);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_HorizontalScrollbar)
                m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
            if (m_VerticalScrollbar)
                m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);

            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

        protected override void OnDisable()
        {
            CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);

            if (m_HorizontalScrollbar)
                m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
            if (m_VerticalScrollbar)
                m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);

            m_HasRebuiltLayout = false;
            m_Tracker.Clear();
            m_Velocity = Vector2.zero;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }
        /// <summary>
        /// 如果gameobject和组件处于活动状态且Content不为空，则返回true。
        /// </summary>
        /// <returns></returns>
        public override bool IsActive()
        {
            return base.IsActive() && m_Content != null;
        }
        /// <summary>
        /// 确保已重建布局
        /// </summary>
        private void EnsureLayoutHasRebuilt()
        {
            if (!m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
                Canvas.ForceUpdateCanvases();
        }
        /// <summary>
        /// 停止运动
        /// </summary>
        public virtual void StopMovement()
        {
            m_Velocity = Vector2.zero;
        }

        public virtual void OnScroll(PointerEventData data)
        {
            if (!IsActive())
                return;

            EnsureLayoutHasRebuilt();
            UpdateBounds();

            Vector2 delta = data.scrollDelta;
            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            if (vertical && !horizontal)
            {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    delta.y = delta.x;
                delta.x = 0;
            }
            if (horizontal && !vertical)
            {
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                    delta.x = delta.y;
                delta.y = 0;
            }

            Vector2 position = m_Content.anchoredPosition;
            position += delta * m_ScrollSensitivity;
            if (m_MovementType == MovementType.Clamped)
                position += CalculateOffset(position - m_Content.anchoredPosition);

            SetContentAnchoredPosition(position);
            UpdateBounds();
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_Velocity = Vector2.zero;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            UpdateBounds();

            m_PointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
            m_ContentStartPosition = m_Content.anchoredPosition;
            m_Dragging = true;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_Dragging = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            UpdateBounds();

            var pointerDelta = localCursor - m_PointerStartLocalCursor;
            Vector2 position = m_ContentStartPosition + pointerDelta;

            // Offset to get content into place in the view.
            Vector2 offset = CalculateOffset(position - m_Content.anchoredPosition);
            position += offset;
            if (m_MovementType == MovementType.Elastic)
            {
                //==========LoopScrollRect==========
                if (offset.x != 0)
                    position.x = position.x - RubberDelta(offset.x, m_ViewBounds.size.x) * rubberScale;
                if (offset.y != 0)
                    position.y = position.y - RubberDelta(offset.y, m_ViewBounds.size.y) * rubberScale;
                //==========LoopScrollRect==========
            }

            SetContentAnchoredPosition(position);
        }

        protected virtual void SetContentAnchoredPosition(Vector2 position)
        {
            if (!m_Horizontal)
                position.x = m_Content.anchoredPosition.x;
            if (!m_Vertical)
                position.y = m_Content.anchoredPosition.y;

            if (position != m_Content.anchoredPosition)
            {
                m_Content.anchoredPosition = position;
                UpdateBounds(true);
            }
        }

        protected virtual void LateUpdate()
        {
            if (!m_Content)
                return;

            EnsureLayoutHasRebuilt();
            UpdateScrollbarVisibility();
            UpdateBounds();
            float deltaTime = Time.unscaledDeltaTime;
            Vector2 offset = CalculateOffset(Vector2.zero);
            if (!m_Dragging && (offset != Vector2.zero || m_Velocity != Vector2.zero))
            {
                Vector2 position = m_Content.anchoredPosition;
                for (int axis = 0; axis < 2; axis++)
                {
                    // Apply spring physics if movement is elastic and content has an offset from the view.
                    if (m_MovementType == MovementType.Elastic && offset[axis] != 0)
                    {
                        float speed = m_Velocity[axis];
                        position[axis] = Mathf.SmoothDamp(m_Content.anchoredPosition[axis], m_Content.anchoredPosition[axis] + offset[axis], ref speed, m_Elasticity, Mathf.Infinity, deltaTime);
                        m_Velocity[axis] = speed;
                    }
                    // Else move content according to velocity with deceleration applied.
                    else if (m_Inertia)
                    {
                        m_Velocity[axis] *= Mathf.Pow(m_DecelerationRate, deltaTime);
                        if (Mathf.Abs(m_Velocity[axis]) < 1)
                            m_Velocity[axis] = 0;
                        position[axis] += m_Velocity[axis] * deltaTime;
                    }
                    // If we have neither elaticity or friction, there shouldn't be any velocity.
                    else
                    {
                        m_Velocity[axis] = 0;
                    }
                }

                if (m_Velocity != Vector2.zero)
                {
                    if (m_MovementType == MovementType.Clamped)
                    {
                        offset = CalculateOffset(position - m_Content.anchoredPosition);
                        position += offset;
                    }

                    SetContentAnchoredPosition(position);
                }
            }

            if (m_Dragging && m_Inertia)
            {
                Vector3 newVelocity = (m_Content.anchoredPosition - m_PrevPosition) / deltaTime;
                m_Velocity = Vector3.Lerp(m_Velocity, newVelocity, deltaTime * 10);
            }

            if (m_ViewBounds != m_PrevViewBounds || m_ContentBounds != m_PrevContentBounds || m_Content.anchoredPosition != m_PrevPosition)
            {
                UpdateScrollbars(offset);
                m_OnValueChanged.Invoke(normalizedPosition);
                UpdatePrevData();
            }
        }

        /// <summary>
        /// 更新content边界盒与位置与视区相同
        /// </summary>
        private void UpdatePrevData()
        {
            if (m_Content == null)
                m_PrevPosition = Vector2.zero;
            else
                m_PrevPosition = m_Content.anchoredPosition;
            m_PrevViewBounds = m_ViewBounds;
            m_PrevContentBounds = m_ContentBounds;
        }
        //=====AddCount=====
        int tmpTotalCount;
        int addCount = 0;
        void AddCount()
        {
            addCount = 0;
            tmpTotalCount = totalCount;
            if (totalCount % contentConstraintCount != 0)
            {
                tmpTotalCount = totalCount + contentConstraintCount - (totalCount) % contentConstraintCount;
            }
            if ((itemTypeEnd - itemTypeStart) % contentConstraintCount != 0)
            {
                addCount = contentConstraintCount - (itemTypeEnd - itemTypeStart) % contentConstraintCount;
            }
        }
        //=====AddCount=====
        private void UpdateScrollbars(Vector2 offset)
        {
            if (m_HorizontalScrollbar)
            {
                //==========LoopScrollRect==========
                if (m_ContentBounds.size.x > 0 && totalCount > 0)
                {
                    //=====AddCount=====
                    AddCount();
                    m_HorizontalScrollbar.size = Mathf.Clamp01(
                        (m_ViewBounds.size.x - Mathf.Abs(offset.x))
                        / m_ContentBounds.size.x
                        * (itemTypeEnd - itemTypeStart + addCount)
                        / tmpTotalCount
                        );
                    //=====AddCount=====
                }
                //==========LoopScrollRect==========
                else
                    m_HorizontalScrollbar.size = 1;

                m_HorizontalScrollbar.value = horizontalNormalizedPosition;
            }

            if (m_VerticalScrollbar)
            {
                //==========LoopScrollRect==========
                if (m_ContentBounds.size.y > 0 && totalCount > 0)
                {
                    //=====AddCount=====
                    AddCount();
                    m_VerticalScrollbar.size = Mathf.Clamp01(
                        (m_ViewBounds.size.y - Mathf.Abs(offset.y))
                        / m_ContentBounds.size.y
                        * (itemTypeEnd - itemTypeStart + addCount)
                        / tmpTotalCount
                        );
                    //=====AddCount=====
                }
                //==========LoopScrollRect==========
                else
                    m_VerticalScrollbar.size = 1;

                m_VerticalScrollbar.value = verticalNormalizedPosition;
            }
        }

        public Vector2 normalizedPosition
        {
            get
            {
                return new Vector2(horizontalNormalizedPosition, verticalNormalizedPosition);
            }
            set
            {
                SetNormalizedPosition(value.x, 0);
                SetNormalizedPosition(value.y, 1);
            }
        }

        public float horizontalNormalizedPosition
        {
            get
            {
                UpdateBounds();
                //==========LoopScrollRect==========
                if (totalCount > 0 && itemTypeEnd > itemTypeStart)
                {
                    //TODO: consider contentSpacing
                    //=====AddCount=====
                    float elementSize = m_ContentBounds.size.x / (itemTypeEnd - itemTypeStart + addCount);
                    float totalSize = elementSize * tmpTotalCount;
                    float offset = m_ContentBounds.min.x - elementSize * itemTypeStart;
                    //=====AddCount=====
                    if (totalSize <= m_ViewBounds.size.x)
                        return (m_ViewBounds.min.x > offset) ? 1 : 0;
                    return (m_ViewBounds.min.x - offset) / (totalSize - m_ViewBounds.size.x);
                }
                else
                    return 0.5f;
                //==========LoopScrollRect==========
            }
            set
            {
                SetNormalizedPosition(value, 0);
            }
        }

        public float verticalNormalizedPosition
        {
            get
            {
                UpdateBounds();
                //==========LoopScrollRect==========
                if (totalCount > 0 && itemTypeEnd > itemTypeStart)
                {
                    //TODO: consider contentSpacinge
                    //=====AddCount=====
                    float elementSize = m_ContentBounds.size.y / (itemTypeEnd - itemTypeStart + addCount);
                    float totalSize = elementSize * (tmpTotalCount);
                    float offset = m_ContentBounds.max.y + elementSize * itemTypeStart;
                    //=====AddCount=====

                    if (totalSize <= m_ViewBounds.size.y)
                    {
                        return (offset > m_ViewBounds.max.y) ? 1 : 0;
                    }
                    return (offset - m_ViewBounds.max.y) / (totalSize - m_ViewBounds.size.y);
                }
                else
                {
                    return 0.5f;
                }
                //==========LoopScrollRect==========
            }
            set
            {
                SetNormalizedPosition(value, 1);
            }
        }
        /// <summary>
        /// 调整水平scrollbar的值
        /// </summary>
        /// <param name="value"></param>
        private void SetHorizontalNormalizedPosition(float value) { SetNormalizedPosition(value, 0); }
        /// <summary>
        /// 调整垂直scrollbar的值
        /// </summary>
        /// <param name="value"></param>
        private void SetVerticalNormalizedPosition(float value) { SetNormalizedPosition(value, 1); }

        /// <summary>
        /// 调整scrollbar的值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="axis">0为水平，1为垂直</param>
        private void SetNormalizedPosition(float value, int axis)
        {
            //==========LoopScrollRect==========
            if (totalCount <= 0 || itemTypeEnd <= itemTypeStart)
                return;
            //==========LoopScrollRect==========

            EnsureLayoutHasRebuilt();
            UpdateBounds();

            //==========LoopScrollRect==========
            Vector3 localPosition = m_Content.localPosition;
            float newLocalPosition = localPosition[axis];
            if (axis == 0)
            {
                //=====AddCount=====
                float elementSize = m_ContentBounds.size.x / (itemTypeEnd - itemTypeStart + addCount);
                float totalSize = elementSize * tmpTotalCount;
                float offset = m_ContentBounds.min.x - elementSize * itemTypeStart;
                //=====AddCount=====

                newLocalPosition += m_ViewBounds.min.x - value * (totalSize - m_ViewBounds.size[axis]) - offset;
            }
            else if (axis == 1)
            {
                //=====AddCount=====
                float elementSize = m_ContentBounds.size.y / (itemTypeEnd - itemTypeStart + addCount);
                float totalSize = elementSize * tmpTotalCount;
                float offset = m_ContentBounds.max.y + elementSize * itemTypeStart;
                //=====AddCount=====

                newLocalPosition -= offset - value * (totalSize - m_ViewBounds.size.y) - m_ViewBounds.max.y;
            }
            //==========LoopScrollRect==========

            if (Mathf.Abs(localPosition[axis] - newLocalPosition) > 0.01f)
            {
                localPosition[axis] = newLocalPosition;
                m_Content.localPosition = localPosition;
                m_Velocity[axis] = 0;
                UpdateBounds(true);
            }
        }

        private static float RubberDelta(float overStretching, float viewSize)
        {
            return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        private bool hScrollingNeeded
        {
            get
            {
                if (Application.isPlaying)
                    return m_ContentBounds.size.x > m_ViewBounds.size.x + 0.01f;
                return true;
            }
        }
        private bool vScrollingNeeded
        {
            get
            {
                if (Application.isPlaying)
                    return m_ContentBounds.size.y > m_ViewBounds.size.y + 0.01f;
                return true;
            }
        }

        public virtual void CalculateLayoutInputHorizontal() { }
        public virtual void CalculateLayoutInputVertical() { }

        public virtual float minWidth { get { return -1; } }
        public virtual float preferredWidth { get { return -1; } }
        public virtual float flexibleWidth { get; private set; }

        public virtual float minHeight { get { return -1; } }
        public virtual float preferredHeight { get { return -1; } }
        public virtual float flexibleHeight { get { return -1; } }

        public virtual int layoutPriority { get { return -1; } }

        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();

            if (m_HSliderExpand || m_VSliderExpand)
            {
                m_Tracker.Add(this, viewRect,
                    DrivenTransformProperties.Anchors |
                    DrivenTransformProperties.SizeDelta |
                    DrivenTransformProperties.AnchoredPosition);

                // Make view full size to see if content fits.
                viewRect.anchorMin = Vector2.zero;
                viewRect.anchorMax = Vector2.one;
                viewRect.sizeDelta = Vector2.zero;
                viewRect.anchoredPosition = Vector2.zero;

                // Recalculate content layout with this size to see if it fits when there are no scrollbars.
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
                m_ContentBounds = GetBounds();
            }

            // If it doesn't fit vertically, enable vertical scrollbar and shrink view horizontally to make room for it.
            if (m_VSliderExpand && vScrollingNeeded)
            {
                viewRect.sizeDelta = new Vector2(-(m_VSliderWidth + m_VerticalScrollbarSpacing), viewRect.sizeDelta.y);

                // Recalculate content layout with this size to see if it fits vertically
                // when there is a vertical scrollbar (which may reflowed the content to make it taller).
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
                m_ContentBounds = GetBounds();
            }

            // If it doesn't fit horizontally, enable horizontal scrollbar and shrink view vertically to make room for it.
            if (m_HSliderExpand && hScrollingNeeded)
            {
                viewRect.sizeDelta = new Vector2(viewRect.sizeDelta.x, -(m_HSliderHeight + m_HorizontalScrollbarSpacing));
                m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
                m_ContentBounds = GetBounds();
            }

            // If the vertical slider didn't kick in the first time, and the horizontal one did,
            // we need to check again if the vertical slider now needs to kick in.
            // If it doesn't fit vertically, enable vertical scrollbar and shrink view horizontally to make room for it.
            if (m_VSliderExpand && vScrollingNeeded && viewRect.sizeDelta.x == 0 && viewRect.sizeDelta.y < 0)
            {
                viewRect.sizeDelta = new Vector2(-(m_VSliderWidth + m_VerticalScrollbarSpacing), viewRect.sizeDelta.y);
            }
        }

        public virtual void SetLayoutVertical()
        {
            UpdateScrollbarLayout();
            m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            m_ContentBounds = GetBounds();
        }

        void UpdateScrollbarVisibility()
        {
            if (m_VerticalScrollbar && m_VerticalScrollbarVisibility != ScrollbarVisibility.Permanent && m_VerticalScrollbar.gameObject.activeSelf != vScrollingNeeded)
                m_VerticalScrollbar.gameObject.SetActive(vScrollingNeeded);

            if (m_HorizontalScrollbar && m_HorizontalScrollbarVisibility != ScrollbarVisibility.Permanent && m_HorizontalScrollbar.gameObject.activeSelf != hScrollingNeeded)
                m_HorizontalScrollbar.gameObject.SetActive(hScrollingNeeded);
        }

        void UpdateScrollbarLayout()
        {
            if (m_VSliderExpand && m_HorizontalScrollbar)
            {
                m_Tracker.Add(this, m_HorizontalScrollbarRect,
                              DrivenTransformProperties.AnchorMinX |
                              DrivenTransformProperties.AnchorMaxX |
                              DrivenTransformProperties.SizeDeltaX |
                              DrivenTransformProperties.AnchoredPositionX);
                m_HorizontalScrollbarRect.anchorMin = new Vector2(0, m_HorizontalScrollbarRect.anchorMin.y);
                m_HorizontalScrollbarRect.anchorMax = new Vector2(1, m_HorizontalScrollbarRect.anchorMax.y);
                m_HorizontalScrollbarRect.anchoredPosition = new Vector2(0, m_HorizontalScrollbarRect.anchoredPosition.y);
                if (vScrollingNeeded)
                    m_HorizontalScrollbarRect.sizeDelta = new Vector2(-(m_VSliderWidth + m_VerticalScrollbarSpacing), m_HorizontalScrollbarRect.sizeDelta.y);
                else
                    m_HorizontalScrollbarRect.sizeDelta = new Vector2(0, m_HorizontalScrollbarRect.sizeDelta.y);
            }

            if (m_HSliderExpand && m_VerticalScrollbar)
            {
                m_Tracker.Add(this, m_VerticalScrollbarRect,
                              DrivenTransformProperties.AnchorMinY |
                              DrivenTransformProperties.AnchorMaxY |
                              DrivenTransformProperties.SizeDeltaY |
                              DrivenTransformProperties.AnchoredPositionY);
                m_VerticalScrollbarRect.anchorMin = new Vector2(m_VerticalScrollbarRect.anchorMin.x, 0);
                m_VerticalScrollbarRect.anchorMax = new Vector2(m_VerticalScrollbarRect.anchorMax.x, 1);
                m_VerticalScrollbarRect.anchoredPosition = new Vector2(m_VerticalScrollbarRect.anchoredPosition.x, 0);
                if (hScrollingNeeded)
                    m_VerticalScrollbarRect.sizeDelta = new Vector2(m_VerticalScrollbarRect.sizeDelta.x, -(m_HSliderHeight + m_HorizontalScrollbarSpacing));
                else
                    m_VerticalScrollbarRect.sizeDelta = new Vector2(m_VerticalScrollbarRect.sizeDelta.x, 0);
            }
        }

        private void UpdateBounds(bool updateItems = false)
        {
            m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            m_ContentBounds = GetBounds();

            if (m_Content == null)
                return;

            // ============LoopScrollRect============
            // Don't do this in Rebuild
            if (Application.isPlaying && updateItems && UpdateItems(m_ViewBounds, m_ContentBounds))
            {
                Canvas.ForceUpdateCanvases();
                m_ContentBounds = GetBounds();
            }
            // ============LoopScrollRect============

            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 contentSize = m_ContentBounds.size;
            Vector3 contentPos = m_ContentBounds.center;
            Vector3 excess = m_ViewBounds.size - contentSize;
            if (excess.x > 0)
            {
                contentPos.x -= excess.x * (m_Content.pivot.x - 0.5f);
                contentSize.x = m_ViewBounds.size.x;
            }
            if (excess.y > 0)
            {
                contentPos.y -= excess.y * (m_Content.pivot.y - 0.5f);
                contentSize.y = m_ViewBounds.size.y;
            }

            m_ContentBounds.size = contentSize;
            m_ContentBounds.center = contentPos;
        }

        private readonly Vector3[] m_Corners = new Vector3[4];
        private Bounds GetBounds()
        {
            if (m_Content == null)
                return new Bounds();

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = viewRect.worldToLocalMatrix;
            m_Content.GetWorldCorners(m_Corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        private Bounds GetBounds4Item(int index)
        {
            if (m_Content == null)
                return new Bounds();

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = viewRect.worldToLocalMatrix;
            int offset = index - itemTypeStart;
            if (offset < 0 || offset >= m_Content.childCount)
                return new Bounds();
            var rt = m_Content.GetChild(offset) as RectTransform;
            if (rt == null)
                return new Bounds();
            rt.GetWorldCorners(m_Corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        private Vector2 CalculateOffset(Vector2 delta)
        {
            Vector2 offset = Vector2.zero;
            if (m_MovementType == MovementType.Unrestricted)
                return offset;

            Vector2 min = m_ContentBounds.min;
            Vector2 max = m_ContentBounds.max;

            if (m_Horizontal)
            {
                min.x += delta.x;
                max.x += delta.x;
                if (min.x > m_ViewBounds.min.x)
                    offset.x = m_ViewBounds.min.x - min.x;
                else if (max.x < m_ViewBounds.max.x)
                    offset.x = m_ViewBounds.max.x - max.x;
            }

            if (m_Vertical)
            {
                min.y += delta.y;
                max.y += delta.y;
                if (max.y < m_ViewBounds.max.y)
                    offset.y = m_ViewBounds.max.y - max.y;
                else if (min.y > m_ViewBounds.min.y)
                    offset.y = m_ViewBounds.min.y - min.y;
            }

            return offset;
        }
        /// <summary>
        /// 若组件是失活或者canvas为空的状态下，将自己的rectTransform组件标记为需要在下一个布局过程中重新计算其布局
        /// </summary>
        protected void SetDirty()
        {
            if (!IsActive())
                return;
            //LayoutRebuilder用于管理CanvasElement的布局重建的包装器类
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        /// <summary>
        /// 若组件是失活或者canvas为空的状态下，重新生成给定元素的布局
        /// </summary>
        protected void SetDirtyCaching()
        {
            if (!IsActive())
                return;
            //重新生成给定元素的布局。
            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            //将给定的rectTransform标记为需要在下一个布局过程中重新计算其布局
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirtyCaching();
        }
#endif
    }
}