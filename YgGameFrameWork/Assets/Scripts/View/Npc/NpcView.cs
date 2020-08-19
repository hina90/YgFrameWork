using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UObject = UnityEngine.Object;

/// <summary>
/// NPC
/// </summary>
public class NpcView : NpcPather, INpcView
{
    public GameObject gameObject;
    public ViewObject viewObject;
    protected GameObject roleObject;


    private NpcData npcData;
    public NpcData NpcData { get => npcData; set => npcData = value; }

    public virtual void Initialize(NpcData npcData, Vector3 pos, Action initOK = null)
    {
        var id = npcData.roleid.ToString();
        CreateNpcObject(id, pos, new Vector2(1, 1), delegate (GameObject prefab)
        {
            gameObject.transform.position = pos;

            base.Initialize(gameObject);

            if (initOK != null) initOK();
        });
    }
    public virtual void OnAwake()
    {
    }

    public virtual void OnDispose()
    {
    }
    public virtual void OnUpdate()
    {
    }

    /// <summary>
    /// 加载NPC预制体
    /// </summary>
    protected void CreateNpcObject(string roleid, Vector3 pos, Vector2 scale, Action<GameObject> loadOK)
    {
        var path = "Prefabs/Character/Npc/" + roleid;
        resMgr.LoadAssetAsync<GameObject>(path, new[] { roleid }, delegate (UObject[] prefabs)
        {
            if (prefabs[0] == null) return;
            var prefab = prefabs[0] as GameObject;
            if (prefab != null)
            {
                CreateNpcObject(prefab, pos, scale, loadOK);
            }
        });
    }

    /// <summary>
    /// 创建NPC对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <param name="scale"></param>
    void CreateNpcObject(GameObject prefab, Vector3 pos, Vector2 scale, Action<GameObject> loadOK)
    {
        gameObject.name = "user_" + NpcData.npcid;
        gameObject.transform.position = pos;

        roleObject = Instantiate<GameObject>(prefab);
        roleObject.name = "roleObject";
        roleObject.transform.SetParent(gameObject.transform);
        roleObject.transform.localPosition = Vector2.zero;
        roleObject.transform.localScale = scale;
        roleObject.transform.localEulerAngles = Vector3.zero;

        if (loadOK != null) loadOK(gameObject);
    }
    /// <summary>
    /// NPC消失
    /// </summary>
    protected void OnNpcDeath()
    {
        if (roleObject != null)
        {
            Destroy(roleObject);
        }
    }
}
