using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private Stack<GameObject> m_ItemStack;
    private List<GameObject> m_ItemList;
    private readonly string m_ItemName;
    private readonly Transform m_Parent;

    public GameObjectPool(Transform parentTrans, string prefabName)
    {
        m_ItemList = new List<GameObject>();
        m_ItemStack = new Stack<GameObject>();
        m_Parent = parentTrans;
        m_ItemName = prefabName;
    }

    /// <summary>
    /// 从池子获取对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetPool()
    {
        GameObject item;
        if (m_ItemStack.Count == 0)
        {
            item = CreateObj();
        }
        else
        {
            item = m_ItemStack.Pop();
            item.SetActive(true);
        }
        return item;
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <returns></returns>
    private GameObject CreateObj()
    {
        GameObject obj = ResourceManager.Instance.GetResource(m_ItemName, ResouceType.PrefabItem);
        GameObject item = Object.Instantiate(obj, m_Parent);
        m_ItemList.Add(item);
        return item;
    }

    /// <summary>
    /// 回收指定对象
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(GameObject obj)
    {
        //if (!obj.activeInHierarchy) return;
        if (m_ItemStack.Contains(obj)) return;
        obj.SetActive(false);
        m_ItemStack.Push(obj);
    }

    /// <summary>
    /// 回收所有对象
    /// </summary>
    public void RecycleAll()
    {
        m_ItemList.ForEach((item) =>
        {
            Recycle(item);
        });
    }

    /// <summary>
    /// 释放数据
    /// </summary>
    public void Release()
    {
        m_ItemList.ForEach((item) =>
        {
            Object.Destroy(item);
        });
        m_ItemList = null;
        m_ItemStack = null;
    }

    /// <summary>
    /// 移除所有实例物体
    /// </summary>
    public void DestoryAll()
    {
        m_ItemList.ForEach((item) =>
        {
            Object.Destroy(item);
        });
    }
}
