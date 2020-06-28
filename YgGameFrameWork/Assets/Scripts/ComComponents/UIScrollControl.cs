using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIScrollControl : MonoBehaviour
{

    #region property        
    //======控制组件

    /// <summary>
    /// 滑动框体 - UI组件
    /// </summary>
    private ScrollRect scroll_rect;

    /// <summary>
    /// 滑动条
    /// </summary>
    public Scrollbar scroll_bar;

    //======逻辑数据

    /// <summary>
    /// 滑动子对象 - 列表，这是一个内嵌类列表
    /// </summary>
    private List<ScrollChild> all_child_list = new List<ScrollChild>();

    /// <summary>
    /// 总数目
    /// </summary>
    private int total_count;

    /// <summary>
    /// 当前下标
    /// </summary>
    private int cur_index;

    /// <summary>
    /// 内容位置
    /// </summary>
    private float start_content_pos;

    /// <summary>
    /// 滑动框高度
    /// </summary>
    private float rect_high;

    /// <summary>
    /// 起始滑动项位置
    /// </summary>
    private Vector2 start_scrollChild_pos;

    /// <summary>
    /// content初始大小
    /// </summary>
    private Vector2 start_content_size;

    /// <summary>
    /// 是否开始标识
    /// </summary>
    private bool is_start = false;
    #endregion

    #region 外部调用
    /// <summary>
    /// 打开控制
    /// </summary>
    /// <param name="_temp_obj"></param>
    /// <param name="_total_count">数据的数量</param>
    /// <param name="_rect_high">每个格子的高</param>
    /// <param name="_action">回调</param>
    public void OpenControl(GameObject _temp_obj, int _total_count, float _rect_high,
       System.Action<int, GameObject> _action = null)
    {
        //判断是否存在scroll组件
        if (is_start == false)
        {
            scroll_rect = transform.GetComponent<ScrollRect>();
            if (scroll_rect == null || scroll_rect.content == null)
            {
                Debug.LogError("ScrollRect组件错误");
                return;
            }
            start_content_size = scroll_rect.content.sizeDelta;
        }
        Clear();

        float _show_area_high = start_content_size.y;//得到content的高
        Vector2 _startPos = Vector2.zero;//起始位置设置为00
        float _heigh = Mathf.Max(_total_count * _rect_high, start_content_size.y);//总的数据数量*每个格子的高就能得到所以格子并排的高，然后与content比较选择最大的高
        _startPos = new Vector3(0, (start_content_size.y - _heigh) / 2, 0);//修改起始位置
        scroll_rect.content.sizeDelta = new Vector2(start_content_size.x, _heigh);//修改content的大小
        scroll_rect.content.localPosition = _startPos;
        OpenControl(_temp_obj, _total_count, _rect_high, _show_area_high, new Vector2(0, (_heigh - _rect_high) / 2), _action);
    }

    /// <summary>
    /// 打开控制
    /// </summary>
    /// <param name="_temp_obj"></param>
    /// <param name="_total_count"></param>
    /// <param name="_rect_high"></param>
    /// <param name="_show_area_high"></param>
    /// <param name="_startPos"></param>
    /// <param name="_action"></param>
    private void OpenControl(GameObject _temp_obj, int _total_count, float _rect_high, float _show_area_high, Vector2 _startPos,
        System.Action<int, GameObject> _action = null)
    {

        //检查参数是否合法
        if (_temp_obj == null)
        {
            Debug.LogError("参数有误,_temp_obj == null");
            return;
        }

        if (_total_count <= 0 || _rect_high <= 0 || _show_area_high <= 0)
        {
            if (scroll_bar != null)
            {
                scroll_bar.gameObject.SetActive(false);
            }
            //        Debug.LogError(string.Format("参数有误,_totall_count:{0} _rect_high:{1} _show_area_high:{2}",_total_count, _rect_high, _show_area_high));
            return;
        }

        scroll_rect = transform.GetComponent<ScrollRect>();
        if (scroll_rect == null || scroll_rect.content == null)
        {
            Debug.LogError("ScrollRect组件错误");
            return;
        }

        //产生对象
        int _n = Mathf.CeilToInt(_show_area_high / _rect_high) + 1;
        int _new_count = Mathf.Min(_n, _total_count);
        for (int i = 0; i < _new_count; i++)
        {
            GameObject _obj;
            //if (0 == i)
            //{
            //    if (_temp_obj)
            //    {
            //        _temp_obj.gameObject.SetActive(true);
            //    }
            //    _obj = _temp_obj;
            //}
            //else
            //{
            _obj = GameObject.Instantiate(_temp_obj);
            //}
            _obj.SetActive(true);
            _obj.name = string.Format("cell_{0}", i + 1);
            _obj.transform.SetParent(scroll_rect.content.transform);
            _obj.transform.localRotation = Quaternion.identity;
            _obj.transform.localScale = Vector3.one;
            all_child_list.Add(new ScrollChild(_obj, _action));
        }

        //设置参数
        total_count = _total_count;
        rect_high = _rect_high;
        cur_index = 0;
        start_scrollChild_pos = _startPos;
        if (scroll_bar != null)
        {
            scroll_bar.gameObject.SetActive(total_count > _n);
        }

        //刷新布局
        RefreshLayout();

        scroll_rect.onValueChanged.AddListener(OnScrollRect);
        start_content_pos = scroll_rect.content.anchoredPosition.y;
        if (scroll_bar != null)
        {
            if (scroll_bar.gameObject.activeSelf)
            {
                scroll_bar.onValueChanged.AddListener(OnScrollBar);
            }
            else
            {
                scroll_bar.onValueChanged.RemoveAllListeners();
            }
        }

        //改变标识            
        is_start = true;
    }


    /// <summary>
    /// 移除一个
    /// </summary>
    public void RemoveOneChild(int _index)
    {
        total_count--;

        if (total_count <= all_child_list.Count)
        {
            cur_index = 0;
            ScrollChild child = all_child_list[all_child_list.Count - 1];
            Destroy(child.Go);
            all_child_list.Remove(child);
        }
        else
        {
            if (cur_index >= total_count - all_child_list.Count)
            {
                cur_index = total_count - all_child_list.Count - 1;
            }
        }
        float _heigh = Mathf.Max(total_count * rect_high, start_content_size.y);
        scroll_rect.content.sizeDelta = new Vector2(start_content_size.x, _heigh);
        start_content_pos = (start_content_size.y - _heigh) / 2;
        start_scrollChild_pos = new Vector2(0, (_heigh - rect_high) / 2);
        RefreshLayout();
    }

    /// <summary>
    /// 引导相关
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public GameObject GuideGetObj(int _index)
    {
        if (_index - cur_index < 0 || _index - cur_index >= all_child_list.Count)
        {
            return null;
        }
        ScrollChild _child = all_child_list[_index - cur_index];
        return _child.Go;
    }

    /// <summary>
    /// 用来给外部调用的
    /// </summary>
    public void RefreshLayoutUI()
    {
        RefreshLayout();
    }

    /// <summary>
    /// 刷新单独的一个item
    /// </summary>
    /// <param name="index"></param>
    public void RefreshIndex(int index)
    {
        int i = index - cur_index;
        if (i < 0 || i >= all_child_list.Count)
        {
            return;
        }
        ScrollChild _child = all_child_list[i];
        Vector2 _pos = start_scrollChild_pos;
        _pos.y -= index * rect_high;
        _child.Refresh(index, _pos);
    }

    /// <summary>
    /// 滑动面板移动到index去
    /// </summary>
    public void MoveToIndex(int index)
    {
        float _heigh = Mathf.Max(total_count * rect_high, start_content_size.y);
        scroll_rect.content.localPosition = new Vector3(0, (start_content_size.y - index * _heigh) / 2, 0);
        RefreshLayout();
    }
    #endregion

    #region 逻辑处理
    /// <summary>
    /// 刷新
    /// </summary>
    private void RefreshLayout()
    {
        for (int i = 0; i < all_child_list.Count; i++)
        {
            ScrollChild _child = all_child_list[i];
            int _index = cur_index + i;
            Vector2 _pos = start_scrollChild_pos;
            _pos.y -= _index * rect_high;
            _child.Refresh(_index, _pos);
        }
    }
    #endregion

    #region 滑动事件
    /// <summary>
    /// 滑动框体 - 滑动事件
    /// </summary>
    /// <param name="_offset"></param>
    private void OnScrollRect(Vector2 _offset)
    {
        if (scroll_bar != null)
        {
            if (is_scroll_bar)
            {
                is_scroll_bar = false;
                return;
            }

            is_scroll_rect = true;
            is_scroll_bar = false;

            scroll_bar.value = 1 - _offset.y;

        }

        //是否发生变化            
        float _move_value = scroll_rect.content.anchoredPosition.y - start_content_pos;
        int _index = (int)(_move_value / rect_high);
        _index = Mathf.Clamp(_index, 0, total_count - all_child_list.Count);

        if (_index == cur_index)
        {
            return;
        }
        cur_index = _index;

        //刷新布局
        RefreshLayout();
    }

    /// <summary>
    /// 滑动的是滑动框
    /// </summary>
    private bool is_scroll_rect = false;

    private bool is_scroll_bar = false;

    /// <summary>
    /// 滑动条滑动的时候执行
    /// </summary>
    /// <param name="_offset"></param>
    private void OnScrollBar(float _value)
    {

        if (is_scroll_rect)
        {
            is_scroll_rect = false;
            return;
        }

        is_scroll_bar = true;
        is_scroll_rect = false;

        scroll_rect.content.localPosition = new Vector3(scroll_rect.content.localPosition.x, start_content_pos + _value * (total_count * rect_high - start_content_size.y), 0);

        //是否发生变化            
        float _move_value = scroll_rect.content.anchoredPosition.y - start_content_pos;
        int _index = (int)(_move_value / rect_high);
        _index = Mathf.Clamp(_index, 0, total_count - all_child_list.Count);

        if (_index == cur_index)
        {
            return;
        }
        cur_index = _index;

        //刷新布局
        RefreshLayout();
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    private void Clear()
    {
        for (int i = 0; i < all_child_list.Count; i++)
        {
            all_child_list[i].Clear();
            Destroy(all_child_list[i].Go);
        }
        all_child_list.Clear();
        if (scroll_bar != null)
        {
            scroll_bar.value = 0;
        }
    }
    #endregion

    #region 内嵌类
    /// <summary>
    /// 被滑动的子物体
    /// </summary>
    public class ScrollChild
    {
        /// <summary>
        /// 对象
        /// </summary>
        private GameObject go;
        public GameObject Go { get { return go; } }
        /// <summary>
        /// 刷新回调
        /// </summary>
        private System.Action<int, GameObject> refresh_callBack;

        private RectTransform rect_tr;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_go"></param>
        /// <param name="_action"></param>
        public ScrollChild(GameObject _go, System.Action<int, GameObject> _action)
        {
            go = _go;
            rect_tr = _go.GetComponent<RectTransform>();
            refresh_callBack = _action;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_pos"></param>
        public void Refresh(int _index, Vector2 _pos)
        {
            rect_tr.anchoredPosition = _pos;
            rect_tr.localPosition = new Vector3(rect_tr.localPosition.x, rect_tr.localPosition.y, 0);
            if (refresh_callBack != null)
            {
                refresh_callBack(_index, go);
            }
        }

        public void Clear()
        {
            refresh_callBack = null;
        }
    }
    #endregion

}
